using SpiritMod.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ID;

namespace SpiritMod.Mechanics.QuestSystem.Tasks
{
	public class GiveNPCTask : QuestTask
	{
		public override string ModCallName => "TalkNPC";

		/// <summary>If true, all items must be fulfilled in order to have this quest completed. Defaults to true.</summary>
		public readonly bool RequireAllItems = true;
		public readonly bool TakeItems = true;
		public readonly string NPCText = "Hey thanks for the stuff!";

		private readonly int _npcType;
		private readonly string _objective;
		private readonly int[] _itemIDs;
		private readonly int[] _itemStacks;
		private readonly int _optionalReward;

		private bool _takenItems;
		private bool _givenToNPC;

		public GiveNPCTask() { }

		public GiveNPCTask(int npcType, int[] giveItem, int[] stack, string text, string objective = null, bool requireAll = true, bool takeItems = true, int? optionalReward = null)
		{
			_npcType = npcType;
			_objective = objective;
			_itemIDs = giveItem;
			_itemStacks = stack;

			if (giveItem.Length <= 0 || stack.Length <= 0) //Error about empty arrays
				SpiritMod.Instance.Logger.Error($"A Give NPC Task with an empty _itemIDs or _itemStacks list has been added.\nLengths: _itemID: {giveItem.Length}\t_itemStacks: {stack.Length}.",
					new Exception("GiveNPCTask with either no itemIDs or stack sizes has been added. Report to mod devs."));
			if (giveItem.Length != stack.Length)
				SpiritMod.Instance.Logger.Error($"A Give NPC Task with mismatched _itemIDs and _itemStacks sizes has been added.",
					new Exception("Mismatched GiveNPCTask quest item list/stack added. Report to mod devs."));

			RequireAllItems = requireAll;
			TakeItems = takeItems;
			_optionalReward = optionalReward.GetValueOrDefault();
			NPCText = text;

			_takenItems = false;
			_givenToNPC = false;
		}

		public GiveNPCTask(int npcType, int giveItem, int stack, string text, string objective, bool requireAll = true, bool takeItems = true, int? optionalReward = null) : this(npcType, new int[] { giveItem }, new int[] { stack }, text, objective, requireAll, takeItems, optionalReward)
		{ }

		public override QuestTask Parse(object[] args)
		{
			//NPC type
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
				if (!QuestUtils.TryUnbox(args[2], out objective, "Give NPC Objective"))
					return null;
			}

			// TODO: Make this parsing work for int arrays, not sure how to best do that.
			return new GiveNPCTask(npcID, new int[] { 1 }, new int[] { 1 }, objective);
		}

		public override bool CheckCompletion()
		{
			if (_givenToNPC)
				return true;

			if (Main.netMode == NetmodeID.SinglePlayer)
			{
				if (Main.LocalPlayer.talkNPC != -1 && Main.npc[Main.LocalPlayer.talkNPC].type == _npcType)
				{
					if (ScanForItems(Main.LocalPlayer))
					{
						if (TakeItems && !_takenItems)
							RemoveItems(Main.LocalPlayer);

						Main.LocalPlayer.QuickSpawnItem(Main.LocalPlayer.GetSource_GiftOrReward(), _optionalReward);
						Main.npcChatText = NPCText;
						return true;
					}
				}
			}
			else if (Main.netMode == NetmodeID.MultiplayerClient)
			{
				for (int i = 0; i < Main.player.Length; i++)
				{
					Player player = Main.player[i];
					if (player.active && player.talkNPC >= 0 && Main.npc[player.talkNPC].netID == _npcType)
					{
						if (ScanForItems(player))
						{
							if (TakeItems && !_takenItems)
								RemoveItems(player);

							player.QuickSpawnItem(Main.LocalPlayer.GetSource_GiftOrReward(), _optionalReward);
							Main.npcChatText = NPCText;

							_givenToNPC = true;
							return true;
						}
					}
				}
			}
			return false;
		}

		private void RemoveItems(Player p)
		{
			for (int i = 0; i < p.inventory.Length; ++i) //scan through inventory
			{
				Item item = p.inventory[i];
				if (!item.IsAir) //if item exists
				{
					for (int j = 0; j < _itemIDs.Length; ++j) //look through all IDs
					{
						if (item.type == _itemIDs[j] && item.stack >= _itemStacks[j]) //if this item is a related quest item and it has a high enough stack
						{
							if (item.stack == _itemStacks[j])
								item.TurnToAir();
							else
								item.stack -= _itemStacks[j];
							break;
						}
					}
				}
			}
			_takenItems = true;
		}

		private bool ScanForItems(Player p)
		{
			bool[] requirements = new bool[_itemIDs.Length]; //cache all conditions
			for (int i = 0; i < p.inventory.Length; ++i) //scan through inventory
			{
				if (!requirements.Contains(false))
				{
					return true;
				}
				Item item = p.inventory[i];
				if (!item.IsAir) //if item exists
				{
					for (int j = 0; j < _itemIDs.Length; ++j) //look through all IDs
					{
						if (item.type == _itemIDs[j] && item.stack >= _itemStacks[j]) //if this item is a related quest item and it has a high enough stack
						{
							if (!RequireAllItems) //We have an item that fulfills the requirement, we are good
								return true;
							requirements[j] = true; //set the condition to true and continue
							break;
						}
					}
				}
			}
			return false;
		}

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