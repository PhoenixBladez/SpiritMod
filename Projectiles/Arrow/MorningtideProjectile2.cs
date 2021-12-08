using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Utilities;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Graphics.Shaders;
using SpiritMod.Mechanics.Trails;

namespace SpiritMod.Projectiles.Arrow
{
	public class MorningtideProjectile2 : ModProjectile, IDrawAdditive, ITrailProjectile
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sunbeam Bolt");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 4;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			projectile.penetrate = 1;
			projectile.friendly = true;
			projectile.ignoreWater = true;
            projectile.tileCollide = false;
			projectile.ranged = true;
            projectile.timeLeft = 300;
            projectile.width = 26;
            projectile.height = 54;
		}

		public void DoTrailCreation(TrailManager tM)
		{
			tM.CreateTrail(projectile, new GradientTrail(new Color(255, 225, 117), new Color(91, 21, 150)), new RoundCap(), new SleepingStarTrailPosition(), 80f, 180f, new ImageShader(mod.GetTexture("Textures/Trails/Trail_4"), 0.01f, 1f, 1f));
			tM.CreateTrail(projectile, new GradientTrail(new Color(255, 225, 117), new Color(91, 21, 150)), new RoundCap(), new SleepingStarTrailPosition(), 60f, 180f, new ImageShader(mod.GetTexture("Textures/Trails/Trail_1"), 0.01f, 1f, 1f));
		}

		public override void Kill(int timLeft)
        {
            Main.PlaySound(SoundID.NPCHit, (int)projectile.position.X, (int)projectile.position.Y, 3);

            for (int k = 0; k < 10; k++)
            {

                Dust d = Dust.NewDustPerfect(projectile.Center, 222, Vector2.One.RotatedByRandom(6.28f) * Main.rand.NextFloat(3), 0, default, 0.55f);
                d.noGravity = true;

                Dust d1 = Dust.NewDustPerfect(projectile.Center, 222, Vector2.One.RotatedByRandom(6.28f) * Main.rand.NextFloat(3), 0, default, 0.55f);
                d1.shader = GameShaders.Armor.GetSecondaryShader(100, Main.LocalPlayer);
            }
        }

		public override void AI()
		{
			Lighting.AddLight((int)(projectile.position.X / 16f), (int)(projectile.position.Y / 16f), 0.396f, 0.170588235f, 0.564705882f);
			projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
            Vector2 currentSpeed = new Vector2(projectile.velocity.X, projectile.velocity.Y);
            projectile.velocity = currentSpeed.RotatedBy(Main.rand.Next(-1, 1) * (Math.PI / 50));
        }
		public void AdditiveCall(SpriteBatch spriteBatch)
		{
			for (int k = 0; k < projectile.oldPos.Length; k++)
			{
				Color color = new Color(255, 255, 200) * 0.75f * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);

				float scale = projectile.scale;
				Texture2D tex = ModContent.GetTexture("SpiritMod/Projectiles/Arrow/MorningtideProjectile2");

				spriteBatch.Draw(tex, projectile.oldPos[k] + projectile.Size / 2 - Main.screenPosition, null, color, projectile.rotation, tex.Size() / 2, scale, default, default);

				Color color1 = new Color(255, 186, 252) * 0.45475f * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
				for (int j = 0; j < 2; j++)
				{
					spriteBatch.Draw(tex, projectile.oldPos[k] + projectile.Size / 2 - Main.screenPosition + new Vector2(Main.rand.Next(-3, 3), Main.rand.Next(-3, 3)), null, color1, projectile.rotation, tex.Size() / 2, scale, default, default);
				}
			}
		}

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
			for (int k = 0; k < projectile.oldPos.Length; k++) {
				Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
				Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
				spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			}
			return false;
		}

		public override Color? GetAlpha(Color lightColor) => Color.Orange;
	}
}
