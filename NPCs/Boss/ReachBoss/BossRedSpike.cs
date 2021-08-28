using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.ReachBoss
{
	public class BossRedSpike : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Needle Spike");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 3;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			projectile.hostile = true;
			projectile.height = 16;
			projectile.width = 16;
			projectile.friendly = false;
			projectile.aiStyle = 1;
			aiType = ProjectileID.Bullet;
			projectile.timeLeft = 1000;
		}

		public override void AI()
		{
			projectile.rotation = projectile.velocity.ToRotation() + 1.57f;

			projectile.tileCollide = projectile.timeLeft < 940;

			const int maxvel = 18;
			if (projectile.velocity.Length() < maxvel)
				projectile.velocity *= 1.03f;
			else
				projectile.velocity = Vector2.Normalize(projectile.velocity) * maxvel;
		}

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(SoundID.Grass, (int)projectile.position.X, (int)projectile.position.Y);
			Main.PlaySound(SoundID.NPCHit, (int)projectile.position.X, (int)projectile.position.Y, 3);
			for (int index = 0; index < 15; ++index)
			{
				int i = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustID.LifeDrain, 0.0f, 0.0f, 0, default, Main.rand.NextFloat(0.9f, 1.3f));
				Main.dust[i].noGravity = true;
				Main.dust[i].velocity = projectile.velocity.RotatedByRandom(MathHelper.Pi / 6) * 0.5f;
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

		public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			float num108 = 4;
			float num107 = (float)Math.Cos((double)(Main.GlobalTime % 2.4f / 1.2f * 6.28318548f)) / 4f + 0.75f;
			float num106 = 0f;
			Vector2 vector33 = new Vector2(projectile.Center.X, projectile.Center.Y) - Main.screenPosition - projectile.velocity;
			Color color29 = new Color(127, 127, 127, 0).MultiplyRGBA(Color.Tomato);
			for (int num103 = 0; num103 < 4; num103++) {
				Color color28 = color29;
				color28 = projectile.GetAlpha(color28);
				color28 *= 1f - num107;
				Vector2 vector29 = projectile.Center + ((float)num103 / (float)num108 * 6.28318548f + projectile.rotation + num106).ToRotationVector2() * (4f * num107 + 2f) - Main.screenPosition - projectile.velocity * (float)num103;
				Main.spriteBatch.Draw(Main.projectileTexture[projectile.type], vector29, null, color28, projectile.rotation, Main.projectileTexture[projectile.type].Size() / 2f, projectile.scale, SpriteEffects.None, 0f);
			}
		}
		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if (Main.rand.Next(15) == 1)
				target.AddBuff(BuffID.Bleeding, 200);
		}
	}
}