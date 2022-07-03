using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Placeable.Tiles;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System;

namespace SpiritMod.Tiles.Block
{
	public class NeonBlockGreen : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;

            TileID.Sets.NeedsGrassFraming[Type] = true;

			AddMapEntry(new Color(53, 59, 74));
			DustType = -1;
            HitSound = SoundID.Tink;
            ItemDrop = ModContent.ItemType<NeonBlockGreenItem>();
		}

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = .051f*3;
            g = .255f*3;
            b = .112f*3;
        }

        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Texture2D glow = ModContent.Request<Texture2D>("SpiritMod/Tiles/Block/NeonBlockGreen_Glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			Color colour = Color.White * MathHelper.Lerp(0.3f, 1f, (float)((Math.Sin(SpiritMod.GlobalNoise.Noise(i * 0.2f, j * 0.2f) * 3f + Main.GlobalTimeWrappedHourly * 1.3f) + 1f) * 0.5f));
			GTile.DrawSlopedGlowMask(i, j, glow, colour, Vector2.Zero);
		}
	}
}

