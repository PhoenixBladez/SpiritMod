using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.AccessoriesMisc.DustboundRing
{
	public class Dustbound_RingTile : GlobalTile
	{
		public override void DrawEffects (int i, int j, int type, SpriteBatch spriteBatch, ref TileDrawInfo drawData)
		{
			Player player = Main.LocalPlayer;

			if (Main.tileOreFinderPriority[type] > 0)
				for (int b = 0; b < 8 + player.extraAccessorySlots; b++)
					if (player.armor[b].type == ModContent.ItemType<Dustbound_Ring>())
						drawColor = Color.White * .6f;
		}
	}
}