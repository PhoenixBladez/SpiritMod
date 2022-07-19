using MonoMod.Cil;
using Terraria.GameContent.Drawing;
using Terraria.ModLoader;
using Mono.Cecil.Cil;
using Terraria;
using SpiritMod.GlobalClasses.Tiles;
using SpiritMod.Utilities.Helpers;
using Terraria.ID;

namespace SpiritMod.Utilities.ILEdits
{
	class TileSwayEdit : ILEdit
	{
		public override void Load(Mod mod) => IL.Terraria.GameContent.Drawing.TileDrawing.Draw += AddSwayTiles;

		private static void AddSwayTiles(ILContext il)
		{
			ILCursor c = new ILCursor(il);

			ILHelper.CompleteLog(c, true);

			if (!c.TryGotoNext(MoveType.After, i => i.MatchCall(typeof(TileLoader), "PreDraw"))) //Move to PreDraw call
				return;

			if (!c.TryGotoNext(MoveType.After, i => i.MatchLdcI4(52))) //Move to 52 condition to grab the label
				return;

			var goTo = c.Next;

			if (!c.TryGotoPrev(MoveType.Before, i => i.MatchLdcI4(271))) //Move to 217 condition
				return;

			c.Index--;

			foreach (int item in ModContent.GetInstance<SwingGlobalTile>().Vines)
			{
				c.Emit(OpCodes.Ldloc_S, (byte)15);
				c.Emit(OpCodes.Ldc_I4, item);
				c.Emit(goTo.OpCode, goTo.Operand); //Really ugly thing that just spams switch conditions
			}

			ILHelper.CompleteLog(c, false);
		}
	}
}
