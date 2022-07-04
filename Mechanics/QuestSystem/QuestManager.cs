using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using SpiritMod.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Chat;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SpiritMod.Mechanics.QuestSystem
{
    public static class QuestManager
    {
		/* Boffin's TODO list
		
		 * NPC text override and quest buttons. (a helper class with a bunch of queued up quests to give for town npcs!) (I think this is done, but I'll keep this here just in case I'm wrong - Gabe)
		 * New clients joining servers need to sync their quest managers up.

		*/
		public const int MAX_QUESTS_ACTIVE = 5;

		public static List<Quest> Quests { get; private set; }
		public static List<Quest> ActiveQuests { get; private set; }
		public static bool QuestBookUnlocked { get; set; }
		public static Dictionary<string, QuestCategory> Categories { get; private set; }
		public static Dictionary<string, StoredQuestData> UnloadedQuests { get; private set; }

		private static Dictionary<Type, Quest> _questDict;
		private static Dictionary<string, QuestTask> _tasksDict;
		private static int _serverSyncCounter;

		public static event Action<Quest> OnQuestActivate;
		public static event Action<Quest> OnQuestDeactivate;

		public static bool Quiet = false;

        public static void Load()
        {
			_questDict = new Dictionary<Type, Quest>();
			_tasksDict = new Dictionary<string, QuestTask>();
			Categories = new Dictionary<string, QuestCategory>();

			Quests = new List<Quest>();
            ActiveQuests = new List<Quest>();
			UnloadedQuests = new Dictionary<string, StoredQuestData>();

			// register our categories]
			if (!Main.dedServ)
			{
				var iconTexture = ModContent.Request<Texture2D>("SpiritMod/UI/QuestUI/Textures/Icons", AssetRequestMode.ImmediateLoad);
				RegisterCategory("Main", new Color(234, 194, 107), iconTexture, new Rectangle(0, 0, 18, 18));
				RegisterCategory("Explorer", new Color(186, 141, 117), iconTexture, new Rectangle(54, 0, 18, 18));
				RegisterCategory("Forager", new Color(153, 196, 102), iconTexture, new Rectangle(36, 0, 18, 18));
				RegisterCategory("Slayer", new Color(196, 66, 77), iconTexture, new Rectangle(18, 0, 18, 18));
				RegisterCategory("Designer", new Color(125, 183, 224), iconTexture, new Rectangle(72, 0, 18, 18));
				RegisterCategory("Other", new Color(173, 117, 198), iconTexture, new Rectangle(90, 0, 18, 18));
			}

			// add all quests from the assembly
			IEnumerable<Type> questTypes = typeof(QuestManager).Assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(Quest)) && t != typeof(InstancedQuest) && !Attribute.IsDefined(t, typeof(ObsoleteAttribute)));
            foreach (Type type in questTypes)
            {
				Quest q = (Quest)Activator.CreateInstance(type, true);

				// load related quest image
				string tex = "SpiritMod/UI/QuestUI/Textures/Quests/" + type.Name;
				if (SpiritMod.Instance.HasAsset(tex) && !Main.dedServ)
				
					q.QuestImage = ModContent.Request<Texture2D>(tex, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

				q.WhoAmI = Quests.Count;

				// if it contains a unique category name, just add it to the list of categories.
				if (!Categories.ContainsKey(q.QuestCategory))
					RegisterCategory(q.QuestCategory, Color.White, null, null);

				Quests.Add(q);
				_questDict[type] = q;
			}

			// store an instance of all quest tasks for cross-mod compatibility purposes.
			IEnumerable<Type> taskTypes = typeof(QuestManager).Assembly.GetTypes().Where(t => typeof(QuestTask).IsAssignableFrom(t));
			foreach (Type type in taskTypes)
			{
				if (type.IsInterface) continue;

				var task = (QuestTask)Activator.CreateInstance(type);
				_tasksDict[task.ModCallName] = task;
			}

			Main.OnTickForThirdPartySoftwareOnly += Update;
		}

        public static void Unload()
        {
			Main.OnTickForThirdPartySoftwareOnly -= Update;

            Quests = null;
            ActiveQuests = null;
			Categories = null;
			_questDict = null;
			_tasksDict = null;
		}

		public static bool ActivateQuest(int index) => ActivateQuest(Quests[index]);

		public static bool ActivateQuest(Quest quest)
		{
			if (quest.IsActive || ActiveQuests.Count >= MAX_QUESTS_ACTIVE)
				return false;

			quest.IsActive = true;

			ActiveQuests.Add(quest);
			OnQuestActivate?.Invoke(quest);

			if (!Quiet && Main.netMode == NetmodeID.MultiplayerClient) //Tell server to activate the current quest. This only runs on host.
			{
				ModPacket packet = SpiritMod.Instance.GetPacket(MessageType.Quest, 4);
				packet.Write((byte)QuestMessageType.Activate);
				packet.Write(true);
				packet.Write(quest.QuestName);
				packet.Write((byte)Main.myPlayer);
				packet.Send();
			}
			return true;
		}

		public static void DeactivateQuest(int index) => DeactivateQuest(Quests[index]);

		public static void DeactivateQuest(Quest quest)
		{
			if (!quest.IsActive || !ActiveQuests.Contains(quest))
				return;

			ActiveQuests.Remove(quest);
			OnQuestDeactivate?.Invoke(quest);

			quest.ResetAllProgress();
			quest.IsActive = false;

			if (!Quiet && Main.netMode == NetmodeID.MultiplayerClient) //Tell server to deactivate the current quest. This only runs on host.
			{
				ModPacket packet = SpiritMod.Instance.GetPacket(MessageType.Quest, 4);
				packet.Write((byte)QuestMessageType.Deactivate);
				packet.Write(true);
				packet.Write(quest.QuestName);
				packet.Write((byte)Main.myPlayer);
				packet.Send();
			}
		}

		public static void RestartEverything()
		{
			foreach (Quest quest in Quests)
			{
				DeactivateQuest(quest);
				quest.ResetEverything();
			}
		}

		public static void GiveRewards(Quest quest)
		{
			if (quest.RewardsGiven)
				return;

			quest.GiveRewards();
			quest.RewardsGiven = true;
		}

		private static void RegisterCategory(string category, Color colour, Asset<Texture2D> iconTexture, Rectangle? frame)
		{
			Categories[category] = new QuestCategory() { Index = Categories.Count, Name = category, Color = colour, Texture = iconTexture, Frame = frame };
		}

		public static QuestCategory GetCategoryInfo(string category)
		{
			if (Categories.TryGetValue(category, out var data)) return data;
			// if it doesn't exist, register
			RegisterCategory(category, Color.White, null, null);
			return Categories[category];
		}

		public static void Update()
		{
			if (Main.gameMenu) return;

			// test if we need to sync
			bool syncMP = false;
			if (Main.netMode != NetmodeID.SinglePlayer)
			{
				_serverSyncCounter++;
				if (_serverSyncCounter >= 20)
				{
					_serverSyncCounter = 0;
					syncMP = true;
				}
			}

			// handle limited time unlock quests
			foreach (Quest quest in Quests)
			{
				if (quest.IsUnlocked && quest.LimitedUnlock)
				{
					quest.UnlockTime--;
					if (quest.UnlockTime <= 0)
					{
						quest.IsUnlocked = false;
						if (quest.AnnounceRelocking)
							SayInChat("[[sQ/" + quest.WhoAmI + ":" + quest.QuestName + "]] has been locked again.", Color.White);
					}
				}
			}

			// update quests and sync if we're a mp client or server
			var shallowCopy = new List<Quest>(ActiveQuests);
			foreach (Quest quest in shallowCopy)
			{
				quest.Update();
				if (syncMP)
				{
					if (Main.netMode == NetmodeID.MultiplayerClient)
						quest.OnMPSync();
				}
			}
		}

		public static Quest GetQuest<T>()
        {
			if (_questDict.TryGetValue(typeof(T), out Quest q))
				return q;
            return null;
		}

		public static Quest GetQuest(Quest q)
		{
			if (_questDict.TryGetValue(q.GetType(), out Quest quest))
				return quest;
			return null;
		}

		public static void UnlockQuest<T>(bool showInChat = true)
		{
			Quest q = GetQuest<T>();
			if (q == null)
				return;

			UnlockQuest(q, showInChat);
		}

		public static void UnlockQuest(Quest quest, bool showInChat = true)
		{
			if (quest.IsUnlocked)
				return;

			quest.IsUnlocked = true;
			quest.OnUnlock();

			if (showInChat && quest.IsQuestPossible() && Main.netMode != NetmodeID.Server)
				SayInChat("You have unlocked a new quest! [[sQ/" + quest.WhoAmI + ":" + quest.QuestName + "]]", Color.White);

			if (!Quiet && Main.netMode == NetmodeID.MultiplayerClient)
			{
				ModPacket packet = SpiritMod.Instance.GetPacket(MessageType.Quest, 4);
				packet.Write((byte)QuestMessageType.Unlock);
				packet.Write(true);
				packet.Write(quest.QuestName);
				packet.Write((byte)Main.myPlayer);
				packet.Send();
			}
		}

		public static void SetBookState(bool open) => SpiritMod.Instance.BookUserInterface.SetState(open ? SpiritMod.QuestBookUIState : null);

		public static int ModCallAddQuest(object[] args)
		{
			if (args.Length < 9)
			{
				SpiritMod.Instance.Logger.Warn("Error adding custom quest! Less than 9 arguments means something must be missing, check you've added everything you need to add!");
				return -1;
			}

			int index = 1;
			// quest name
			if (!QuestUtils.TryUnbox(args[index++], out string questName, "Quest name")) return -1;

			// quest category
			if (!QuestUtils.TryUnbox(args[index++], out string questCategory, "Quest category")) return -1;
			// TODO: check if quest category exists. if it doesn't, add it, make the colour white.

			// quest client
			if (!QuestUtils.TryUnbox(args[index++], out string questClient, "Quest client")) return -1;

			// quest description
			if (!QuestUtils.TryUnbox(args[index++], out string questDesc, "Quest description")) return -1;

			// quest rewards
			if (!QuestUtils.TryUnbox(args[index++], out (int, int)[] questRewards, "Quest rewards")) return -1;

			// quest difficulty
			if (!QuestUtils.TryUnbox(args[index++], out int questDifficulty, "Quest difficulty")) return -1;

			// quest image
			if (!QuestUtils.TryUnbox(args[index++], out Texture2D questImage, "Quest image")) return -1;

			// quest objectives
			List<QuestTask> tasks = new List<QuestTask>();
			for (int i = index; i < args.Length; i++)
			{
				if (!QuestUtils.TryUnbox(args[i], out object[] objectiveArgs, "Quest objective " + (i - index + 1))) return -1;

				QuestTask task = ParseTaskFromArguments(objectiveArgs);
				if (task == null)
				{
					SpiritMod.Instance.Logger.Warn("Task returned null.");
					return -1;
				}

				tasks.Add(task);
			}

			// add the quest
			Quest q = new InstancedQuest(questName, questCategory, questDifficulty, questClient, questDesc, questRewards, questImage, tasks);
			q.WhoAmI = Quests.Count;
			Quests.Add(q);

			SpiritMod.Instance.Logger.Info("Added a cross-mod quest! Called: " + questName);

			return q.WhoAmI;
		}

		public static void SyncToClient(int toClient)
		{
			var log = ModContent.GetInstance<SpiritMod>().Logger;
			log.Debug("Sending manager...");

			ModPacket packet = SpiritMod.Instance.GetPacket(MessageType.RecieveQuestManager, 1);
			packet.Write(QuestBookUnlocked);
			packet.Write((short)Quests.Count);
			Quests.ForEach(x => QuestMultiplayer.WriteQuestToPacket(packet, x));
			packet.Send(toClient, -1);
		}

		public static void ModCallUnlockQuest(object[] args)
		{
			if (!QuestUtils.TryUnbox(args[1], out int questIndex, "Quest ID"))
				return;

			if (questIndex < 0 || questIndex >= Quests.Count) return;

			UnlockQuest(Quests[questIndex]);
		}

		public static bool ModCallGetQuestValueFromContext(object[] args, int context)
		{
			if (!QuestUtils.TryUnbox(args[1], out int questIndex, "Quest ID"))
				return false;

			if (questIndex < 0 || questIndex >= Quests.Count) return false;

			switch (context)
			{
				default:
				case 0:
					return Quests[questIndex].IsUnlocked;
				case 1:
					return Quests[questIndex].IsActive;
				case 2:
					return Quests[questIndex].IsCompleted;
				case 3:
					return Quests[questIndex].RewardsGiven;
			}
		}

		public static QuestTask ParseTaskFromArguments(object[] args)
		{
			if (!QuestUtils.TryUnbox(args[0], out string name, "Quest Objective Type"))
				return null;

			if (!_tasksDict.TryGetValue(name, out QuestTask task))
			{
				SpiritMod.Instance.Logger.Warn("Quest task " + name + " does not exist.");
				return null;
			}

			return task.Parse(args);
		}

		public static void SayInChat(string text, Color colour, bool noServer = false)
		{
			if (Main.netMode == NetmodeID.SinglePlayer || Main.netMode == NetmodeID.MultiplayerClient)
				Main.NewText(text, colour.R, colour.G, colour.B);
			else if (Main.netMode == NetmodeID.Server && !noServer)
				ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(text), colour, -1);
		}

		public static void UnlockQuestBook(bool openBook = true)
		{
			if (!QuestBookUnlocked)
			{
				QuestBookUnlocked = true;
				UnlockQuest<Quests.FirstAdventure>(true);

				SayInChat("Press 'C' to open the Quest Journal!", Color.White, true);
				SayInChat("Press 'V' to keep track of your progress wih the HUD!", Color.White, true);

				if (openBook)
					SetBookState(true);
			}
		}

		public struct QuestCategory
		{
			public int Index;
			public string Name;
			public Color Color;
			public Asset<Texture2D> Texture;
			public Rectangle? Frame;
		}
	}

	public struct StoredQuestData
	{
		public bool IsUnlocked;
		public bool IsActive;
		public bool IsCompleted;
		public bool RewardsGiven;
		public short TimeLeftUnlocked;
		public short TimeLeftActive;
		public byte[] Buffer;

		public override string ToString()
		{
			string buffer = string.Empty;
			if (Buffer != null && Buffer.Length > 0)
				for (int i = 0; i < Buffer.Length; ++i)
					buffer += Buffer[i] + " ";
			return $"Unlocked: {IsUnlocked}\nActive: {IsActive}\nCompleted: {IsCompleted}\nRewarded: {RewardsGiven}\nTime Left U/A {TimeLeftUnlocked} / {TimeLeftActive}" + (buffer != string.Empty ? $"\nBuffer: {buffer}\n" : "\n");
		}
	}
}
