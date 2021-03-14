using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.ReachBoss
{   
	public class ReachBossBouncingProjectile : ModProjectile, IDrawAdditive
	{
		int target;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bouncing Spore");
		}

		public override void SetDefaults()
		{
			projectile.hostile = true;
			projectile.friendly = false;
			projectile.penetrate = 5;
            projectile.aiStyle = -1;
			projectile.timeLeft = 600;
			projectile.alpha = 100;
			projectile.width = 64;
			projectile.height = 64;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			projectile.penetrate--;
			if (projectile.penetrate <= 0)
				projectile.Kill();


			if (projectile.velocity.X != oldVelocity.X)
				projectile.velocity.X = oldVelocity.X * .5f;

			if (projectile.velocity.Y != oldVelocity.Y)
				projectile.velocity.Y = oldVelocity.Y * -1.3f;

			Main.PlaySound(SoundID.Item, (int)projectile.position.X, (int)projectile.position.Y, 10);
			return false;
		}
        public void DrawAdditive(SpriteBatch spriteBatch)
        {
            {
                for (int k = 0; k < projectile.oldPos.Length; k++)
                {
                    Color color = new Color(255, 255, 200) * 0.75f * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);

                    float scale = projectile.scale;
                    Texture2D tex = ModContent.GetTexture("SpiritMod/NPCs/Boss/ReachBoss/ReachBossBouncingProjectile_Glow");

                    spriteBatch.Draw(tex, projectile.oldPos[k] + projectile.Size / 2 - Main.screenPosition, null, color, projectile.rotation, tex.Size() / 2, scale, default, default);
                }
            }
        }
		public override void AI()
		{
			projectile.ai[0]++;
			projectile.rotation += 0.4f;
			Lighting.AddLight(projectile.position, 0.201f, .01f, .042f);
			for (int k = 0; k < 4; k++) {
				Vector2 center = projectile.Center;
				Vector2 vector2 = Vector2.UnitY.RotatedByRandom(6.28318548202515) * new Vector2((float)projectile.height, (float)projectile.height) * projectile.scale * 1.45f / 2f;
				float num8 = (float)projectile.ai[0] / 60f;
				float num7 = 2.09439516f;
				for (int i = 0; i < 5; i++) {
					int num6 = Dust.NewDust(center + vector2, 0, 0, 235, 0f, 0f, 100, default(Color), Main.rand.NextFloat(.5f, 1.1f));
					Main.dust[num6].noGravity = true;
					Main.dust[num6].velocity = Vector2.Zero;
					Main.dust[num6].noLight = true;
					Main.dust[num6].position = center + (num8 * 6.28318548f + num7 * (float)i).ToRotationVector2() * 12f;
				}
			}
		}
      
	}
}
