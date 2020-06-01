using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class SandWall : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sand Wall");
		}

		public override void SetDefaults()
		{
			projectile.hostile = false;
			projectile.magic = true;
			projectile.width = 30;
			projectile.height = 30;
			projectile.aiStyle = -1;
			projectile.friendly = true;
			projectile.penetrate = 3;
			projectile.alpha = 255;
			projectile.timeLeft = 40;
			projectile.tileCollide = true; //Tells the game whether or not it can collide with a tile
		}
        public override bool PreAI()
		{
            Vector2 vector33;
            Player player = Main.player[projectile.owner];
            Vector2 vector5 = player.RotatedRelativePoint(player.MountedCenter, true);
            float num16 = 1f;
            float num17 = projectile.velocity.ToRotation();
            float num18 = projectile.velocity.Length();
            float num19 = 22f;
            Vector2 spinningpoint = new Vector2(1f, 0f).RotatedBy((double)(3.14159274f + num16 * 6.28318548f), default(Vector2)) * new Vector2(num18, projectile.ai[0]);
            Vector2 destination3 = vector5 + spinningpoint.RotatedBy((double)num17, default(Vector2)) + new Vector2(num18 + num19 + 40f, 0f).RotatedBy((double)num17, default(Vector2));

            Vector2 value2 = player.DirectionTo(destination3);
            Vector2 vector9 = projectile.velocity.SafeNormalize(Vector2.UnitY);
            float num25 = 2f;
            for (int num26 = 0; (float)num26 < num25; num26++)
            {
                Dust dust4 = Dust.NewDustDirect(projectile.Center, 14, 14, 173, 0f, 0f, 110, default(Color), 1f);
                dust4.velocity = player.DirectionTo(dust4.position) * 2f;
                dust4.position = projectile.Center + vector9.RotatedBy((double)(num16 * 6.28318548f * 2f + (float)num26 / num25 * 6.28318548f), default(Vector2)) * 10f;
                dust4.scale = 1f + 0.6f * Main.rand.NextFloat();
                Dust dust6 = dust4;
                dust6.velocity += vector9 * 3f;
                dust4.noGravity = true;
            }
            for (int j = 0; j < 1; j++)
            {
                if (Main.rand.Next(3) == 0)
                {
                    Dust dust5 = Dust.NewDustDirect(projectile.Center, 20, 20, 173, 0f, 0f, 110, default(Color), 1f);
                    dust5.velocity = player.DirectionTo(dust5.position) * 2f;
                    dust5.position = projectile.Center + value2 * -110f;
                    dust5.scale = 0.45f + 0.4f * Main.rand.NextFloat();
                    dust5.fadeIn = 0.7f + 0.4f * Main.rand.NextFloat();
                    dust5.noGravity = true;
                    dust5.noLight = true;
                }
            }
            return false;
		}
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            projectile.penetrate--;
            if (projectile.penetrate <= 0)
                projectile.Kill();
            else
            {
                projectile.ai[0] += 0.1f;
                if (projectile.velocity.X != oldVelocity.X)
                    projectile.velocity.X = -oldVelocity.X;

                if (projectile.velocity.Y != oldVelocity.Y)
                    projectile.velocity.Y = -oldVelocity.Y;

                projectile.velocity *= 0.75f;
            }
            return false;
        }
    }
}
