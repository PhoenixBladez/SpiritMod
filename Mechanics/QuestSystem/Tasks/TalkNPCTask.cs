﻿using SpiritMod.Utilities;
using System.Collections.Generic;
using System.Text;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Mechanics.QuestSystem.Tasks
{
	public class TalkNPCTask : QuestTask
	{
		public override string ModCallName => "TalkNPC";

		private int _npcType;
		private QuestPoolData? _poolData;
		private int _itemReceived;
		private string _objective;
		public readonly string NPCText = "Have a great day!";
		private bool hasTakenItems;

		public TalkNPCTask() { }

		public TalkNPCTask(int npcType, string text, string objective = null, QuestPoolData? poolData = null, int? itemReceived = null)
		{
			_npcType = npcType;
			NPCText = text;
			_objective = objective;
			_poolData = poolData;
			_itemReceived = itemReceived.GetValueOrDefault();
			hasTakenItems = false;
		}

		public override QuestTask Parse(object[] args)
		{
			// get the npc type
			if (!QuestUtils.TryUnbox(args[1], out int npcID))
			{
				if (QuestUtils.TryUnbox(args[1], out short IDasShort, "NPC Type"))
					npcID = IDasShort;
				else
					return null;
			}

			// get the name override, if there is one
			string objective = null;
			if (args.Length > 2)
			{
				if (!QuestUtils.TryUnbox(args[2], out objective, "Talk NPC Objective"))
					return null;
			}

			return new TalkNPCTask(npcID, objective);
		}

		public override bool CheckCompletion()
		{
			if (Main.netMode == NetmodeID.SinglePlayer)
			{
				if (Main.LocalPlayer.talkNPC != -1 && Main.npc[Main.LocalPlayer.talkNPC].type == _npcType)
				{
					Main.npcChatText = NPCText;
					if (!hasTakenItems)
					{
						Main.LocalPlayer.QuickSpawnItem(_itemReceived);
						hasTakenItems = true;
					}
					return Main.npc[Main.LocalPlayer.talkNPC].type == _npcType;
				}
			}
			else if (Main.netMode == NetmodeID.MultiplayerClient)
			{
				for (int i = 0; i < Main.player.Length; i++)
				{
					if (Main.player[i].active && Main.player[i].talkNPC >= 0 && Main.npc[Main.player[i].talkNPC].netID == _npcType)
					{
						Main.npcChatText = NPCText;
						if (!hasTakenItems)
						{
							Main.player[i].QuickSpawnItem(_itemReceived);
							hasTakenItems = true;
						}
						return true;
					}
				}
			}
			return false;
		}

		public override void Activate(Quest fromQuest)
		{
			if (_poolData != null)
				QuestGlobalNPC.AddToPool(_npcType, _poolData.Value);
		}

		public override void Deactivate() => QuestGlobalNPC.RemoveFromPool(_npcType);
		public override void AutogeneratedBookText(List<string> lines) => lines.Add(GetObjectives(false));
		public override void AutogeneratedHUDText(List<string> lines) => lines.Add(GetObjectives(true));

		public string GetObjectives(bool showProgress)
		{
			StringBuilder builder = new StringBuilder();

			if (_objective != null)
			{
				builder.Append(_objective);
				return builder.ToString();
			}

			builder.Append("Talk to the ").Append(Lang.GetNPCNameValue(_npcType));
			return builder.ToString();
		}
	}
}
