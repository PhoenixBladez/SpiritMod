using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	public class Wheeze : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wheeze Gas");
            Main.projFrames[projectile.type] = 8;
        }

		public override void SetDefaults()
		{
			projectile.hostile = false;
			projectile.width = 22;
			projectile.height = 22;
			projectile.aiStyle = 1;
			projectile.melee = true;
			aiType = ProjectileID.Bullet;
			projectile.friendly = true;
			projectile.penetrate = 5;
            projectile.alpha = 60;
			projectile.timeLeft = 180;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(20, 150);
		}

		public override void OnHitPvp(Player target, int damage, bool crit)
		{
			target.AddBuff(20, 150);
		}

		public override void AI()
		{
            projectile.alpha+= 3;
			projectile.velocity *= 0.92f;
            projectile.spriteDirection = projectile.direction;
            projectile.frameCounter++;
            if (projectile.frameCounter >= 6)
            {
                projectile.frame++;
                projectile.frameCounter = 0;
                if (projectile.frame >= 8)
                    projectile.frame = 0;
            }
        }
	}
}
