using Microsoft.Xna.Framework;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using SpiritMod.Mechanics.QuestSystem;
using SpiritMod.Mechanics.QuestSystem.Tasks;
using System;
using System.Linq;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Utilities.ILEdits
{
	class LifeformQuestEdit : ILEdit
	{
		private static bool _changeInfoAccColor = false;

		public override void Load(Mod mod) => IL.Terraria.Main.DrawInfoAccs += Main_DrawInfoAccs;

		private static void Main_DrawInfoAccs(ILContext il)
		{
			ILCursor c = new ILCursor(il);

			if (!c.TryGotoNext(x => x.MatchLdstr("GameUI.NoRareCreatures"))) //Get to the rare creatures line
				return;

			if (!c.TryGotoNext(MoveType.After, x => x.MatchStloc(19))) //Match the stloc
				return;

			c.Emit(OpCodes.Ldloc, 19); //Load up the original for comparison / defaulting
			c.EmitDelegate<Func<string, string>>(InfoAccQuestCreatures);
			c.Emit(OpCodes.Stloc, 19); //Set possibly new value

			if (!c.TryGotoNext(MoveType.After, x => x.MatchStloc(67))) //Match the color2 stloc
				return;

			c.Emit(OpCodes.Ldloc, 67); //Same as prior emit/delegate/emit triplet, but for color
			c.Emit(OpCodes.Ldloc, 64); //Apart from this, which loads n (for loop iteration variable)
			c.EmitDelegate<Func<Color, int, Color>>(GetModifiedInfoAccColor);
			c.Emit(OpCodes.Stloc, 67);
		}

		private static Color GetModifiedInfoAccColor(Color c, int n)
		{
			if (_changeInfoAccColor)
			{
				if (n == 4) //Reset this value only if the loop is done
					_changeInfoAccColor = false;
				return new Color(140, 69, 0);
			}
			return c;
		}

		private static string InfoAccQuestCreatures(string orig)
		{
			_changeInfoAccColor = false;

			for (int i = 0; i < Main.maxNPCs; ++i)
			{
				NPC npc = Main.npc[i];

				if (npc.CanBeChasedBy()) //This NPC is valid
				{
					foreach (var quest in QuestManager.ActiveQuests)
					{
						if (quest.CurrentTask is SlayTask slay && slay.MonsterIDs.Contains(npc.type)) //This NPC is part of a slay quest
						{
							_changeInfoAccColor = true;
							return npc.GivenOrTypeName;
						}
						else if (quest.CurrentTask is BranchingTask branch && branch.Tasks.Any(x => x is SlayTask slayTask && slayTask.MonsterIDs.Contains(npc.type)))
						{
							_changeInfoAccColor = true;
							return npc.GivenOrTypeName;
						}
					}
				}
			}
			return orig;
		}
	}
}
