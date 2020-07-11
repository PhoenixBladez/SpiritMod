
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using System;

namespace SpiritMod.Projectiles.Flail
{
	public class TentacleChainProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tentacle Chain");
		}

		public override void SetDefaults()
		{
            projectile.width = 10;
			projectile.height = 10;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.timeLeft = 900;
			projectile.melee = true;
            projectile.ignoreWater = true;
        }
		//  bool comingHome = false;
		public override bool PreAI()
		{
            projectile.velocity.Y += .0625f;
			if(projectile.Hitbox.Intersects(Main.player[projectile.owner].Hitbox) && projectile.timeLeft < 870) {
				projectile.active = false;
			}
			if(projectile.timeLeft < 869) {
				Vector2 direction9 = Main.player[projectile.owner].Center - projectile.position;
				projectile.velocity = projectile.velocity.RotatedBy(direction9.ToRotation() - projectile.velocity.ToRotation());
				if (Math.Sqrt((projectile.velocity.X * projectile.velocity.X) + (projectile.velocity.Y * projectile.velocity.Y)) < 18)
				{
					projectile.velocity *= 1.075f;
				}
				projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
				projectile.tileCollide = false;
			} else {
				projectile.velocity -= new Vector2(projectile.ai[0], projectile.ai[1]) / 30f;
				projectile.rotation = new Vector2(projectile.ai[0], projectile.ai[1]).ToRotation() - 1.57f;
			}
			return false;
		}
        //projectile.ai[0]: X speed initial
        //projectile.ai[1]: y speed initial
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            for (int i = 0; i < 20; i++)
            {
                int num = Dust.NewDust(target.position, target.width, target.height, 199, 0f, -2f, 0, default(Color), 2f);
                Main.dust[num].noGravity = true;
                Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
                Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
                Main.dust[num].scale *= .85f;
                if (Main.dust[num].position != target.Center)
                    Main.dust[num].velocity = target.DirectionTo(Main.dust[num].position) * 5f;
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            for (int i = 0; i < 20; i++)
            {
                int num = Dust.NewDust(projectile.position, projectile.width, projectile.height, 199, 0f, -2f, 0, default(Color), 2f);
                Main.dust[num].noGravity = true;
                Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
                Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
                Main.dust[num].scale *= .825f;
                if (Main.dust[num].position != projectile.Center)
                    Main.dust[num].velocity = projectile.DirectionTo(Main.dust[num].position) * 5f;
            }
            return true;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			ProjectileExtras.DrawChain(projectile.whoAmI, Main.player[projectile.owner].MountedCenter,
			"SpiritMod/Projectiles/Flail/TentacleChain_Chain");
			ProjectileExtras.DrawAroundOrigin(projectile.whoAmI, lightColor);
			return false;
		}
		public override void Kill(int timeLeft)
        {
            Main.PlaySound(0, projectile.Center);
        }
	}
}
