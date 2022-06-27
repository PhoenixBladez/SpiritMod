using Mono.Cecil.Cil;
using MonoMod.Cil;
using SpiritMod.Items.Weapon.Summon.StardustBomb;
using SpiritMod.Mechanics.CollideableNPC;
using SpiritMod.NPCs.Hydra;
using System;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Utilities.ILEdits
{
	class NPCHoverHealthEdit : ILEdit
	{
		public override void Load(Mod mod) => IL.Terraria.Main.DrawMouseOver += Main_DrawMouseOver;

		private static void Main_DrawMouseOver(ILContext il)
		{
			ILCursor c = new ILCursor(il);

			for (int i = 0; i < 3; ++i)
				if (!c.TryGotoNext(x => x.MatchStfld<Player>("showItemIcon"))) //match stfld for showItemIcon 3 times for the right call
					return;

			c.Index -= 5;
			c.Emit(OpCodes.Ldsfld, typeof(Main).GetField("npc")); //Push the current NPC to the stack -->
			c.Emit(OpCodes.Ldloc_S, (byte)8);
			c.Emit(OpCodes.Ldelem_Ref); // <--
			c.EmitDelegate<Func<bool, NPC, bool>>(CheckDrawLife); //And make a hook for overriding if the life stat draws
		}

		private static bool CheckDrawLife(bool flag, NPC self)
		{
			if (self.type == ModContent.NPCType<StardustBombNPC>() || self.type == ModContent.NPCType<Hydra>())
				return false;

			if (self.ModNPC != null && self.ModNPC is ISolidTopNPC) //Platforms dont show names
				return false;
			return flag;
		}
	}
}
