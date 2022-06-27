using System;
using Microsoft.Xna.Framework;
using SpiritMod.Tiles.Block;
using SpiritMod.Tiles.Walls.Natural;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class BriarSolution : ModProjectile
	{

		public override void SetStaticDefaults() => DisplayName.SetDefault("Briar Spray");

		public override void SetDefaults()
		{
			Projectile.width = 6;
			Projectile.height = 6;
			Projectile.friendly = true;
			Projectile.alpha = 255;
			Projectile.penetrate = -1;
			Projectile.extraUpdates = 2;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
		}
		public override bool? CanCutTiles() {
               return false;
         }
		public override void AI()
		{
			const int dustType = 163;

			if (Projectile.owner == Main.myPlayer)
				Convert((int) (Projectile.position.X + Projectile.width / 2f) / 16,
					(int) (Projectile.position.Y + Projectile.height / 2f) / 16, 2);

			if (Projectile.timeLeft > 133)
				Projectile.timeLeft = 133;

			if (Projectile.ai[0] > 7f) {
				float dustScale = 1f;
				switch (Projectile.ai[0]) {
					case 8f:
						dustScale = 0.2f;
						break;
					case 9f:
						dustScale = 0.4f;
						break;
					case 10f:
						dustScale = 0.6f;
						break;
					case 11f:
						dustScale = 0.8f;
						break;
				}

				Projectile.ai[0] += 1f;
				for (int i = 0; i < 1; i++) {
					Dust dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y),
						Projectile.width, Projectile.height, dustType, Projectile.velocity.X * 0.2f,
						Projectile.velocity.Y * 0.2f, 100);
					dust.noGravity = true;
					dust.scale *= 1.75f;
					dust.velocity.X *= 2f;
					dust.velocity.Y *= 2f;
					dust.scale *= dustScale;
				}
			}
			else
				Projectile.ai[0] += 1f;

			Projectile.rotation += 0.3f * Projectile.direction;
		}

		public void Convert(int i, int j, int size = 4)
		{
			for (int k = i - size; k <= i + size; k++)
			for (int l = j - size; l <= j + size; l++) {
				if (!WorldGen.InWorld(k, l, 1) ||
				    !(Math.Abs(k - i) + Math.Abs(l - j) < Math.Sqrt(size * size + size * size)))
					continue;

				int type = Main.tile[k, l].TileType;
				int wall = Main.tile[k, l].WallType;

				if (WallID.Sets.Conversion.Grass[wall]) {
					Main.tile[k, l].WallType = (ushort) ModContent.WallType<ReachWallNatural>();
					WorldGen.SquareWallFrame(k, l);
					NetMessage.SendTileSquare(-1, k, l, 1);
				}

				if (!TileID.Sets.Conversion.Grass[type]) 
					continue;

				Main.tile[k, l].TileType = (ushort) ModContent.TileType<BriarGrass>();
				WorldGen.SquareTileFrame(k, l);
				NetMessage.SendTileSquare(-1, k, l, 1);
			}
		}
	}
}
