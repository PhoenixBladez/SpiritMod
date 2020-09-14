using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Terraria.Graphics.Shaders;
using SpiritMod.Projectiles;

namespace SpiritMod.NPCs.DarkfeatherMage.Projectiles
{
	public class DarkfeatherBomb : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Darkfeather Bomb");
        }

		public override void SetDefaults()
		{
			projectile.width = 4;
			projectile.height = 4;
            projectile.hostile = true;
            projectile.penetrate = 2;
            projectile.timeLeft = 90;
		}
        float alphaCounter;
        float sineAdd;
        bool chooseFrame;
		public override void AI()
		{
            projectile.velocity.Y += .185f;
            Vector2 currentSpeed = new Vector2(projectile.velocity.X, projectile.velocity.Y);
            projectile.velocity = currentSpeed.RotatedBy(Main.rand.Next(-1, 2) * (Math.PI / 40));
            projectile.ai[0] += .1135f;
            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver2;
            int num623 = Dust.NewDust(projectile.Center, 4, 4,
                157, 0f, 0f, 0, default(Color), 1f);
            Main.dust[num623].shader = GameShaders.Armor.GetSecondaryShader(67, Main.LocalPlayer);
            if (projectile.scale > .5f)
            {
                Main.dust[num623].noGravity = true;
            }
            else
            {
                Main.dust[num623].noGravity = false;
            }
            Main.dust[num623].velocity = projectile.velocity;
            Main.dust[num623].scale = MathHelper.Clamp(1.6f, .9f, 10 / projectile.ai[0]);
        }
        public override void Kill(int timeLeft)
        {
            int num = 0;
			if (projectile.friendly)
            {
                num = 120;
            }
			else
            {
                num = 80;
            }
            ProjectileExtras.Explode(projectile.whoAmI, num, num,
            delegate
            {
                if (projectile.friendly)
                {
                    Main.PlaySound(3, (int)projectile.position.X, (int)projectile.position.Y, 3);
                }
                else
                {
                    Main.PlaySound(SoundID.Item, (int)projectile.position.X, (int)projectile.position.Y, 74);
                }

                for (int k = 0; k < 35; k++)
                {
                    Dust d = Dust.NewDustPerfect(projectile.Center, 159, Vector2.One.RotatedByRandom(6.28f) * Main.rand.NextFloat(5), 0, default, Main.rand.NextFloat(.8f, 1.2f));
                    d.noGravity = false;
                    d.shader = GameShaders.Armor.GetSecondaryShader(67, Main.LocalPlayer);
                }
            });
        }
	}
}
