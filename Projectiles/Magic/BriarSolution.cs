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
			projectile.width = 6;
			projectile.height = 6;
			projectile.friendly = true;
			projectile.alpha = 255;
			projectile.penetrate = -1;
			projectile.extraUpdates = 2;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
		}
		public override bool? CanCutTiles() {
               return false;
         }
		public override void AI()
		{
			const int dustType = 163;

			if (projectile.owner == Main.myPlayer)
				Convert((int) (projectile.position.X + projectile.width / 2f) / 16,
					(int) (projectile.position.Y + projectile.height / 2f) / 16, 2);

			if (projectile.timeLeft > 133)
				projectile.timeLeft = 133;

			if (projectile.ai[0] > 7f) {
				float dustScale = 1f;
				switch (projectile.ai[0]) {
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

				projectile.ai[0] += 1f;
				for (int i = 0; i < 1; i++) {
					Dust dust = Dust.NewDustDirect(new Vector2(projectile.position.X, projectile.position.Y),
						projectile.width, projectile.height, dustType, projectile.velocity.X * 0.2f,
						projectile.velocity.Y * 0.2f, 100);
					dust.noGravity = true;
					dust.scale *= 1.75f;
					dust.velocity.X *= 2f;
					dust.velocity.Y *= 2f;
					dust.scale *= dustScale;
				}
			}
			else
				projectile.ai[0] += 1f;

			projectile.rotation += 0.3f * projectile.direction;
		}

		public void Convert(int i, int j, int size = 4)
		{
			for (int k = i - size; k <= i + size; k++)
			for (int l = j - size; l <= j + size; l++) {
				if (!WorldGen.InWorld(k, l, 1) ||
				    !(Math.Abs(k - i) + Math.Abs(l - j) < Math.Sqrt(size * size + size * size)))
					continue;

				int type = Main.tile[k, l].type;
				int wall = Main.tile[k, l].wall;

				if (WallID.Sets.Conversion.Grass[wall]) {
					Main.tile[k, l].wall = (ushort) ModContent.WallType<ReachWallNatural>();
					WorldGen.SquareWallFrame(k, l);
					NetMessage.SendTileSquare(-1, k, l, 1);
				}

				if (!TileID.Sets.Conversion.Grass[type]) 
					continue;

				Main.tile[k, l].type = (ushort) ModContent.TileType<BriarGrass>();
				WorldGen.SquareTileFrame(k, l);
				NetMessage.SendTileSquare(-1, k, l, 1);
			}
		}
	}
}
