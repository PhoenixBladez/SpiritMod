using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Placeable.Tiles;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System;

namespace SpiritMod.Tiles.Block
{
	public class NeonBlockRed : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			//	Main.tileMerge[Type][ModContent.TileType<SpiritDirt>()] = true;
			//Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
			AddMapEntry(new Color(53, 59, 74));
			dustType = -1;
			soundType = SoundID.Tink;
			TileID.Sets.NeedsGrassFraming[Type] = true;
			drop = ModContent.ItemType<NeonBlockRedItem>();
		}

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			r = .255f * 3;
			g = .065f * 3;
			b = .051f * 3;
		}

		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Texture2D glow = ModContent.GetTexture("SpiritMod/Tiles/Block/NeonBlockRed_Glow");
			Color colour = Color.White * MathHelper.Lerp(0.3f, 1f, (float)((Math.Sin(SpiritMod.GlobalNoise.Noise(i * 0.2f, j * 0.2f) * 3f + Main.GlobalTime * 1.3f) + 1f) * 0.5f));
			GTile.DrawSlopedGlowMask(i, j, glow, colour, Vector2.Zero);
		}
	}
}

