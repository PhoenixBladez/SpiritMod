using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Graphics.Shaders;
using SpiritMod.Mechanics.Trails;

namespace SpiritMod.Projectiles.Arrow
{
	public class MorningtideProjectile : ModProjectile, IDrawAdditive, ITrailProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dawnstrike Shafts");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 3;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.penetrate = 1;
			projectile.alpha = 100;
			projectile.timeLeft = 200;
			projectile.height = 50;
			projectile.width = 20;
			projectile.ranged = true;
			projectile.aiStyle = 1;
			aiType = ProjectileID.WoodenArrowFriendly;
		}

		public void DoTrailCreation(TrailManager tM)
		{
			tM.CreateTrail(projectile, new GradientTrail(new Color(255, 225, 117), new Color(91, 21, 150)), new RoundCap(), new SleepingStarTrailPosition(), 100f, 180f, new ImageShader(mod.GetTexture("Textures/Trails/Trail_4"), 0.01f, 1f, 1f));
			tM.CreateTrail(projectile, new GradientTrail(new Color(255, 225, 117), new Color(91, 21, 150)), new RoundCap(), new SleepingStarTrailPosition(), 90f, 180f, new ImageShader(mod.GetTexture("Textures/Trails/Trail_1"), 0.01f, 1f, 1f));
			tM.CreateTrail(projectile, new StandardColorTrail(Color.White * 0.3f), new RoundCap(), new SleepingStarTrailPosition(), 12f, 80f, new DefaultShader());
		}

		float num;

		public override void AI()
		{
			if (projectile.timeLeft >= 199)
			{
				projectile.alpha = 255;
				projectile.height = 20;
				projectile.width = 10;
			}
			else
			{
				Lighting.AddLight(projectile.position, 0.205f * 1.85f, 0.135f * 1.85f, 0.255f * 1.85f);
				projectile.alpha = 0;
				projectile.height = 50;
				projectile.width = 26;
			}
			num += .4f;
			projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Vector2 mouse = new Vector2(Main.mouseX, Main.mouseY) + Main.screenPosition;
			int amount = Main.rand.Next(1, 3);
			for (int i = 0; i < amount; ++i)
			{
				Vector2 pos = new Vector2(mouse.X + target.width * 0.5f + Main.rand.Next(-35, 36), mouse.Y - 220f);
				pos.X = (pos.X * 10f + mouse.X) / 11f - 350;
				pos.Y -= Main.rand.Next(-20, 20);
				float spX = mouse.X + target.width * 0.5f + Main.rand.Next(-100, 101) - mouse.X;
				float spY = mouse.Y - pos.Y;
				if (spY < 0f)
					spY *= -1f;
				if (spY < 20f)
					spY = 20f;

				float length = (float)Math.Sqrt((double)(spX * spX + spY * spY));
				length = 12 / length;
				spX *= length;
				spY *= length;
				spX = spX + (float)Main.rand.Next(-40, 41) * 0.02f;
				spY = spY + (float)Main.rand.Next(-40, 41) * 0.12f;
				spX *= (float)Main.rand.Next(75, 150) * 0.006f;
				pos.X += (float)Main.rand.Next(-20, 21);
				DustHelper.DrawCircle(new Vector2(pos.X, pos.Y), 222, 1, 1f, 1.35f, .85f, .85f);

				Projectile.NewProjectile(pos.X, pos.Y, spX * projectile.direction, spY, ModContent.ProjectileType<MorningtideProjectile2>(), projectile.damage / 3 * 2, 2, Main.player[projectile.owner].whoAmI);
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (projectile.timeLeft < 196)
			{
				Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
				for (int k = 0; k < projectile.oldPos.Length; k++)
				{
					Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
					Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
					spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
				}
			}
			else
				return false;
			return true;
		}
		public void AdditiveCall(SpriteBatch spriteBatch)
		{
			if (projectile.timeLeft < 196)
			{
				for (int k = 0; k < projectile.oldPos.Length; k++)
				{
					Color color = new Color(255, 255, 200) * 0.75f * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);

					float scale = projectile.scale;
					Texture2D tex = ModContent.GetTexture("SpiritMod/Projectiles/Arrow/Morningtide_Glow");

					spriteBatch.Draw(tex, projectile.oldPos[k] + projectile.Size / 2 - Main.screenPosition, null, color, projectile.rotation, tex.Size() / 2, scale, default, default);

					Color color1 = new Color(255, 186, 252) * 0.45475f * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
					spriteBatch.Draw(tex, projectile.oldPos[k] + projectile.Size / 2 - Main.screenPosition + new Vector2(Main.rand.Next(-3, 3), Main.rand.Next(-3, 3)), null, color1, projectile.rotation, tex.Size() / 2, scale * 1.5425f, default, default);
				}
			}
		}

		public override Color? GetAlpha(Color lightColor) => Color.Orange;

		public override void Kill(int timLeft)
		{
			Main.PlaySound(SoundID.NPCHit, (int)projectile.position.X, (int)projectile.position.Y, 3);

			for (int k = 0; k < 18; k++)
			{
				var d = Dust.NewDustPerfect(projectile.Center, 222, Vector2.One.RotatedByRandom(6.28f) * Main.rand.NextFloat(5), 0, default, 0.75f);
				d.noGravity = true;

				var d1 = Dust.NewDustPerfect(projectile.Center, 222, Vector2.One.RotatedByRandom(6.28f) * Main.rand.NextFloat(5), 0, default, 0.75f);
				d1.shader = GameShaders.Armor.GetSecondaryShader(100, Main.LocalPlayer);
			}
		}
	}
}
