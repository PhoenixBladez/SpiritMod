using Microsoft.Xna.Framework;
using SpiritMod.Tiles.Block;
using SpiritMod.Tiles.Walls.Natural;
using System;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class SoullessSolution : ModProjectile
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Soulless Spray");
		}

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

		public override void AI()
		{
			int dustType = 4;
			if (Projectile.owner == Main.myPlayer)
				Convert((int)(Projectile.position.X + (float)(Projectile.width / 2)) / 16, (int)(Projectile.position.Y + (float)(Projectile.height / 2)) / 16, 2);

			if (Projectile.timeLeft > 133)
				Projectile.timeLeft = 133;

			if (Projectile.ai[0] > 7f) {
				float dustScale = 1f;
				if (Projectile.ai[0] == 8f)
					dustScale = 0.2f;
				else if (Projectile.ai[0] == 9f)
					dustScale = 0.4f;
				else if (Projectile.ai[0] == 10f)
					dustScale = 0.6f;
				else if (Projectile.ai[0] == 11f)
					dustScale = 0.8f;

				Projectile.ai[0] += 1f;
				for (int i = 0; i < 1; i++) {
					int dustIndex = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, dustType, Projectile.velocity.X * 0.2f, Projectile.velocity.Y * 0.2f, 100, default, 1f);
					Dust dust = Main.dust[dustIndex];
					dust.noGravity = true;
					dust.scale *= 1.75f;
					dust.velocity.X = dust.velocity.X * 2f;
					dust.velocity.Y = dust.velocity.Y * 2f;
					dust.scale *= dustScale;
				}
			}
			else {
				Projectile.ai[0] += 1f;
			}
			Projectile.rotation += 0.3f * (float)Projectile.direction;
		}

		public void Convert(int i, int j, int size = 4)
		{
			for (int k = i - size; k <= i + size; k++) {
				for (int l = j - size; l <= j + size; l++) {
					if (WorldGen.InWorld(k, l, 1) && Math.Abs(k - i) + Math.Abs(l - j) < Math.Sqrt(size * size + size * size)) {
						int type = (int)Main.tile[k, l].TileType;
						int wall = (int)Main.tile[k, l].WallType;
						if (type == (int)ModContent.WallType<SpiritWall>() || type == (int)ModContent.WallType<ReachWallNatural>()) {
							Main.tile[k, l].WallType = (ushort)2;
							WorldGen.SquareWallFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						if (type == ModContent.TileType<SpiritStone>()) {
							Main.tile[k, l].TileType = (ushort)1;
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (type == ModContent.TileType<SpiritDirt>()) {
							Main.tile[k, l].TileType = 0;
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (type == ModContent.TileType<SpiritGrass>() || type == ModContent.TileType<BriarGrass>()) {
							Main.tile[k, l].TileType = (ushort)2;
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (type == ModContent.TileType<Spiritsand>()) {
							Main.tile[k, l].TileType = (ushort)53;
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}
						else if (type == ModContent.TileType<SpiritIce>()) {
							Main.tile[k, l].TileType = (ushort)161;
							WorldGen.SquareTileFrame(k, l, true);
							NetMessage.SendTileSquare(-1, k, l, 1);
						}

					}
				}
			}
		}

	}
}