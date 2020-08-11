using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.MoonWizard.Projectiles
{
	public class LightningNode : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lightning Node");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 3;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}
		public override void SetDefaults()
		{
			projectile.penetrate = 1;
			projectile.tileCollide = false;
			projectile.hostile = true;
			projectile.friendly = false;
			projectile.timeLeft = 300;
			projectile.damage = 13;
			//projectile.extraUpdates = 1;
			projectile.width = projectile.height = 32;
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 9;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;

		}
		public override void AI()
		{
			int rightValue = (int)projectile.ai[0] - 1;
			if (rightValue < (double)Main.npc.Length && rightValue != -1) {
				NPC other = Main.npc[rightValue];
				Vector2 direction9 = other.Center - projectile.Center;
				int distance = (int)Math.Sqrt((direction9.X * direction9.X) + (direction9.Y * direction9.Y));
				direction9.Normalize();
				if (projectile.timeLeft % 4 == 0 && distance < 600 && other.active) {
					int proj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, (float)direction9.X * 15, (float)direction9.Y * 15, mod.ProjectileType("MoonLightning"), 30, 0);
					Main.projectile[proj].timeLeft = (int)(distance / 15);
					DustHelper.DrawElectricity(projectile.Center + (projectile.velocity * 4), other.Center + (other.velocity * 4), 226, 0.3f, 60);
				}
			}
		}
	}
}