using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Hostile;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SpiritMod.Tiles.Ambient
{
	public class Corpsebloom : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoFail[Type] = true;

			dustType = 14;
			soundType = SoundID.Grass;
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
				if(frameCounter >= 12) {
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
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Gore.NewGore(new Vector2(i * 16 + Main.rand.Next(-10, 10), j * 16 + Main.rand.Next(-20, -10)), new Vector2(Main.rand.Next(-2, 2), Main.rand.Next(-2, -1)), mod.GetGoreSlot("Gores/CorpseBloom/CorpseBloom1"), 1f);
            Gore.NewGore(new Vector2(i * 16 + Main.rand.Next(-10, 10), j * 16 + Main.rand.Next(-20, -10)), new Vector2(Main.rand.Next(-2, 2), Main.rand.Next(-2, -1)), mod.GetGoreSlot("Gores/CorpseBloom/CorpseBloom2"), 1f);
            Gore.NewGore(new Vector2(i * 16 + Main.rand.Next(-10, 10), j * 16 + Main.rand.Next(-20, -10)), new Vector2(Main.rand.Next(-2, 2), Main.rand.Next(-2, -1)), mod.GetGoreSlot("Gores/CorpseBloom/CorpseBloom3"), 1f);
            Gore.NewGore(new Vector2(i * 16 + Main.rand.Next(-10, 10), j * 16 + Main.rand.Next(-20, -10)), new Vector2(Main.rand.Next(-2, 2), Main.rand.Next(-2, -1)), mod.GetGoreSlot("Gores/CorpseBloom/CorpseBloom4"), 1f);
            Gore.NewGore(new Vector2(i * 16 + Main.rand.Next(-10, 10), j * 16 + Main.rand.Next(-20, -10)), new Vector2(Main.rand.Next(-2, 2), Main.rand.Next(-2, -1)), mod.GetGoreSlot("Gores/CorpseBloom/CorpseBloom5"), 1f);
            Gore.NewGore(new Vector2(i * 16 + Main.rand.Next(-10, 10), j * 16 + Main.rand.Next(-20, -10)), new Vector2(Main.rand.Next(-2, 2), Main.rand.Next(-2, -1)), mod.GetGoreSlot("Gores/CorpseBloom/CorpseBloom6"), 1f);

        }
        public int cloudtimer;
		public override void NearbyEffects(int i, int j, bool closer)
		{
			if(closer) {
				if(Main.rand.Next(14) == 0) {
					int d = Dust.NewDust(new Vector2(i * 16, j * 16 - 20), 16, 16, 18, 0.0f, -1, 0, Color.Purple, 0.65f);

					Main.dust[d].velocity *= .8f;
					Main.dust[d].noGravity = true;
				}
				cloudtimer++;
				if(cloudtimer >= Main.rand.Next(300, 510)) {
					Main.PlaySound(SoundID.Item, new Vector2(i * 16, j * 16), 95);
					cloudtimer = 0;
					Projectile.NewProjectile(new Vector2(i * 16, j * 16), Vector2.Zero, ModContent.ProjectileType<CorpsebloomExplosion>(), 0, 0f);
					int ing1 = Gore.NewGore(new Vector2(i * 16 + Main.rand.Next(-10, 10), j * 16 + Main.rand.Next(-20, -10)), new Vector2(Main.rand.Next(-2, 2), Main.rand.Next(-2, -1)), mod.GetGoreSlot("Gores/CorpseBloom/Belch1"), 1f);
					int ing2 = Gore.NewGore(new Vector2(i * 16 + Main.rand.Next(-10, 10), j * 16 + Main.rand.Next(-20, -10)), new Vector2(Main.rand.Next(-2, 2), Main.rand.Next(-2, -1)), mod.GetGoreSlot("Gores/CorpseBloom/Belch2"), 1f);
					int ing3 = Gore.NewGore(new Vector2(i * 16 + Main.rand.Next(-10, 10), j * 16 + Main.rand.Next(-20, -10)), new Vector2(Main.rand.Next(-2, 2), Main.rand.Next(-2, -1)), mod.GetGoreSlot("Gores/CorpseBloom/Belch3"), 1f);
				}
			}
		}
	}
}