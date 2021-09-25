using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.AccessoriesMisc.DustboundRing
{
	public class Dustbound_RingTile : GlobalTile
	{
		public override void DrawEffects (int i, int j, int type, SpriteBatch spriteBatch, ref Color drawColor, ref int nextSpecialDrawIndex)
		{
			Player player = Main.LocalPlayer;

			if (Main.tileValue[type] > 0)
				for (int b = 0; b < 8 + player.extraAccessorySlots; b++)
					if (player.armor[b].type == ModContent.ItemType<Dustbound_Ring>())
						drawColor = Color.White * .6f;
		}
	}
}