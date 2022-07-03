using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using SpiritMod.Mechanics.QuestSystem.Quests;
using Terraria.ID;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace SpiritMod.Mechanics.QuestSystem
{
	public class QuestPlayer : ModPlayer
	{
		public override void CatchFish(FishingAttempt attempt, ref int itemDrop, ref int npcSpawn, ref AdvancedPopupRequest sonar, ref Vector2 sonarPosition)
		{
			if (Player.ZoneJungle && QuestManager.GetQuest<ItsNoSalmon>().IsActive && Main.rand.NextBool(10) && !Player.HasItem(ModContent.ItemType<Items.Consumable.Quest.HornetfishQuest>()))
				itemDrop = ModContent.ItemType<Items.Consumable.Quest.HornetfishQuest>();
		}

		public override void OnEnterWorld(Player player)
		{
			if (Main.netMode == NetmodeID.MultiplayerClient)
			{
				ModPacket packet = SpiritMod.Instance.GetPacket(MessageType.RequestQuestManager, 1);
				packet.Write((byte)player.whoAmI);
				packet.Send();
			}
		}

		/// <summary>Handles syncing the QuestManager from server.</summary>
		internal static void RecieveManager(BinaryReader reader)
		{
			QuestManager.QuestBookUnlocked = reader.ReadBoolean();
			int questCount = reader.ReadInt16();

			if (questCount != QuestManager.Quests.Count)
				throw new Exception("Inconsistent quest sizes. Network error?");

			var datas = new List<Tuple<StoredQuestData, string>>();

			for (int i = 0; i < questCount; ++i)
			{
				string name = reader.ReadString();
				long flags = reader.ReadInt64();

				var data = QuestMultiplayer.ReadQuestFromLong(flags);

				byte bufferLength = reader.ReadByte();
				byte[] buffer = null;
				if (bufferLength > 0)
				{
					buffer = new byte[bufferLength];
					for (int j = 0; j < bufferLength; ++j)
						buffer[j] = reader.ReadByte();
				}

				data.Buffer = buffer;
				datas.Add(new Tuple<StoredQuestData, string>(data, name));
			}

			foreach (var item in datas) //Might be ordered properly already but I don't care
			{
				Quest q = QuestManager.Quests.FirstOrDefault(x => x.QuestName == item.Item2);

				if (q is null)
					ModContent.GetInstance<SpiritMod>().Logger.Debug($"No quest of name {item.Item2} exists on client.");
				else
				{
					StoredQuestData data = item.Item1;
					q.IsUnlocked = data.IsUnlocked;
					q.IsCompleted = data.IsCompleted;
					q.RewardsGiven = data.RewardsGiven;

					if (data.IsActive)
					{
						QuestManager.ActivateQuest(q);
						q.ReadFromDataBuffer(data.Buffer ?? new byte[0]); //Extra security so I don't pass null
					}

					q.ActiveTime = data.TimeLeftActive;
					q.UnlockTime = data.TimeLeftUnlocked;
				}
			}
		}
	}
}
