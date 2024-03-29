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

			dustType = DustID.Demonite;
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
			TileObjectData.newTile.AnchorValidTiles = new int[] { TileID.CorruptGrass, TileID.Ebonstone };
			TileObjectData.addTile(Type);

			AddMapEntry(new Color(124, 91, 133));
		}

		public virtual int FrameDelay => 12;

		public override void NumDust(int i, int j, bool fail, ref int num) => num = 2;

		public bool animate = false;
		public override void AnimateTile(ref int frame, ref int frameCounter)
		{
			if (animate && !Main.gamePaused)
			{
				frameCounter++;
				if (frameCounter >= 12) {
					frameCounter = 0;
					frame++;
					frame %= 3;
				}
			}
		}

		public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height) => offsetY = 2;

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			for(int num = 1; num <= 6; num++)
				Gore.NewGore(new Vector2(i * 16 + Main.rand.Next(-10, 10), j * 16 + Main.rand.Next(-20, -10)), new Vector2(Main.rand.Next(-2, 2), Main.rand.Next(-2, -1)), mod.GetGoreSlot("Gores/CorpseBloom/CorpseBloom" + num.ToString()), 1f);
		}

		public int cloudtimer;
		public override void NearbyEffects(int i, int j, bool closer)
		{
			Player player = Main.LocalPlayer;
			if (closer && !Main.gamePaused) {
				int distance1 = (int)Vector2.Distance(new Vector2(i * 16, j * 16), player.Center);
				if (distance1 < 260) {
					animate = true;
					if (Main.rand.Next(10) == 0) {
						int d = Dust.NewDust(new Vector2(i * 16, j * 16 - 20), 16, 16, DustID.Vile, 0.0f, -1, 0, Color.Purple, 0.65f);

						Main.dust[d].velocity *= .8f;
						Main.dust[d].noGravity = true;
					}

					if (++cloudtimer > 500) {
						if(!Main.dedServ)
							Main.PlaySound(SoundID.Item, new Vector2(i * 16, j * 16), 95);

						cloudtimer = 0;
						for(int num = 1; num <= 3; num++)
							Gore.NewGore(new Vector2(num * 16 + Main.rand.Next(-10, 10), j * 16 + Main.rand.Next(-20, -10)), new Vector2(Main.rand.Next(-2, 2), Main.rand.Next(-2, -1)), mod.GetGoreSlot("Gores/CorpseBloom/Belch" + num.ToString()), 1f);

						Projectile.NewProjectile(new Vector2(i * 16, j * 16), Vector2.Zero, ModContent.ProjectileType<CorpsebloomExplosion>(), 0, 0f);
					}
				}
			}
		}
	}

	public class Corpsebloom1 : Corpsebloom
	{
		public override int FrameDelay => 15;
	}

	public class Corpsebloom2 : Corpsebloom
	{

	}
}