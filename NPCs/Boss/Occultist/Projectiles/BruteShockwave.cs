using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.Occultist.Projectiles
{
	public class BruteShockwave : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Skeleton Brute");
			Main.projFrames[projectile.type] = 4;
		}

		public override void SetDefaults()
		{
			projectile.timeLeft = MAXTIME;
			projectile.hostile = true;
			projectile.height = 32;
			projectile.width = 14;
			projectile.tileCollide = false;
			projectile.penetrate = -1;
			projectile.alpha = 255;
			projectile.hide = true;
			projectile.extraUpdates = 2;
		}

		public const int MAXTIME = 100;

		private ref float NumProjsLeft => ref projectile.ai[0];

		public override void AI()
		{
			projectile.UpdateFrame((int)(4 * 60f / MAXTIME));
			projectile.position -= projectile.velocity;
			projectile.direction = projectile.spriteDirection = -Math.Sign(projectile.velocity.X);
			if ((MAXTIME - projectile.timeLeft) == MAXTIME / 6 && NumProjsLeft > 0)
				MakeShockwave(projectile.Center, -projectile.direction, projectile.damage, (int)NumProjsLeft - 1);
		}

		public static void MakeShockwave(Vector2 center, int direction, int damage, int projsLeft)
		{
			Projectile proj = Projectile.NewProjectileDirect(center + new Vector2(direction * 100, 0), direction * Vector2.UnitX,
				ModContent.ProjectileType<BruteShockwave>(), damage, 1, Main.myPlayer, projsLeft - 1);
			if (proj.modProjectile is BruteShockwave shockwave)
			{
				if (!shockwave.SetTilePos())
					proj.active = false;
			}

			if (Main.netMode != NetmodeID.SinglePlayer)
				NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, proj.whoAmI);

			if (!Main.dedServ)
				Main.PlaySound(new LegacySoundStyle(SoundID.Item, 14).WithVolume(0.5f).WithPitchVariance(0.2f), center);
		}

		public bool SetTilePos()
		{
			Point tilePos = projectile.Center.ToTileCoordinates();

			int tilesMoved = 0;
			int maxTilesToMove = 15;

			while (tilePos.Y < 0 && ProjectileExtensions.CheckSolidTilesAndPlatforms(new Rectangle(tilePos.X, tilePos.Y, 0, 0)) && tilesMoved < maxTilesToMove)
			{
				tilesMoved++;
				tilePos.Y--;
			}

			while (tilePos.Y < Main.maxTilesY && !ProjectileExtensions.CheckSolidTilesAndPlatforms(new Rectangle(tilePos.X, tilePos.Y + 1, 1, 0)) && tilesMoved < maxTilesToMove)
			{
				tilesMoved++;
				tilePos.Y++;
			}
			if (tilesMoved >= maxTilesToMove)
				return false;

			tilePos.Y--;
			projectile.position.Y = tilePos.ToWorldCoordinates().Y - 8;
			return true;
		}

		public override void DrawBehind(int index, List<int> drawCacheProjsBehindNPCsAndTiles, List<int> drawCacheProjsBehindNPCs, List<int> drawCacheProjsBehindProjectiles, List<int> drawCacheProjsOverWiresUI) => drawCacheProjsBehindNPCs.Add(index);

		public override Color? GetAlpha(Color lightColor) => Color.White;
	}
}