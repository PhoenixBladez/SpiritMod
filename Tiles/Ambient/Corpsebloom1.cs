using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;
using SpiritMod.Projectiles.Hostile;

namespace SpiritMod.Tiles.Ambient
{
	public class Corpsebloom1 : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoFail[Type] = true;

			dustType = 14;
			soundType = 6;
            animationFrameHeight = 54;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
            TileObjectData.newTile.Height = 3;
            TileObjectData.newTile.Width = 2;
            TileObjectData.newTile.CoordinateHeights = new int[]
            {
            16,
            16,
            16
            };
            TileObjectData.addTile(Type);

			AddMapEntry(new Color(124, 91, 133));
		}

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = 2;
		}

        public override void AnimateTile(ref int frame, ref int frameCounter)
        {
            {
                frameCounter++;
                if (frameCounter >= 15)
                {
                    frameCounter = 0;
                    frame++;
                    frame %= 3;
                }
            }
        }
        public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height)
        {
            offsetY = 2;
        }
        public int cloudtimer;
        public override void NearbyEffects(int i, int j, bool closer)
        {
            if (closer)
            {
                if (Main.rand.Next(14) == 0)
                {
                    int d = Dust.NewDust(new Vector2(i * 16, j * 16 - 20), 16, 16, 18, 0.0f, -1, 0, Color.Purple, 0.65f);

                    Main.dust[d].velocity *= .8f;
                    Main.dust[d].noGravity = true;
                }
                cloudtimer++;
                if (cloudtimer >= Main.rand.Next(300, 510))
                {
                    Main.PlaySound(2, new Vector2(i * 16, j * 16), 95);
                    cloudtimer = 0;
                    Projectile.NewProjectile(new Vector2(i * 16, j * 16), Vector2.Zero, ModContent.ProjectileType<CorpsebloomExplosion>(), 0, 0f);
                    int ing1 = Gore.NewGore(new Vector2(i * 16 + Main.rand.Next(-10, 10), j * 16 + Main.rand.Next(-20, -10)), new Vector2(Main.rand.Next(-2, 2), Main.rand.Next(-2, -1)), mod.GetGoreSlot("Gores/CorpseBloom/Belch1"), 1f);
                    int ing2 = Gore.NewGore(new Vector2(i * 16 + Main.rand.Next(-10, 10), j * 16 + Main.rand.Next(-20, -10)), new Vector2(Main.rand.Next(-2, 2), Main.rand.Next(-2, -1)), mod.GetGoreSlot("Gores/CorpseBloom/Belch2"), 1f);
                    int ing3 = Gore.NewGore(new Vector2(i * 16 + Main.rand.Next(-10, 10), j * 16 + Main.rand.Next(-20, -10)), new Vector2(Main.rand.Next(-2, 2), Main.rand.Next(-2, -1)), mod.GetGoreSlot("Gores/CorpseBloom/Belch3"), 1f);
                }
            }
        }
        /*float alphaCounter = 0;
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            alphaCounter += 0.04f;
            float sineAdd = (float)Math.Sin(alphaCounter) + 3;
            Tile tile = Main.tile[i, j];
            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }
            int height = tile.frameY == 36 ? 18 : 16;
            Main.spriteBatch.Draw(SpiritMod.instance.GetTexture("Tiles/Ambient/Corpsebloom1"), new Vector2(i * 16 - (int)Main.screenPosition.X + 8, j * 16 - (int)Main.screenPosition.Y + 16) + zero, null, new Color(60, 60, 60, 100), 0f, new Vector2(50, 50), 0.2f * (sineAdd + 1), SpriteEffects.None, 0f);
        }*/
    }
}