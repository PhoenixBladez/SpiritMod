using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.ReachBoss
{
	public class BossRedSpike : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Needle Spike");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			Projectile.hostile = true;
			Projectile.height = 16;
			Projectile.width = 16;
			Projectile.friendly = false;
			Projectile.aiStyle = 1;
			AIType = ProjectileID.Bullet;
			Projectile.timeLeft = 1000;
		}

		public override void AI()
		{
			Projectile.rotation = Projectile.velocity.ToRotation() + 1.57f;

			Projectile.tileCollide = Projectile.timeLeft < 940;

			const int maxvel = 18;
			if (Projectile.velocity.Length() < maxvel)
				Projectile.velocity *= 1.03f;
			else
				Projectile.velocity = Vector2.Normalize(Projectile.velocity) * maxvel;
		}

		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Grass, Projectile.Center);
			SoundEngine.PlaySound(SoundID.NPCHit3, Projectile.Center);
			for (int index = 0; index < 15; ++index)
			{
				int i = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.LifeDrain, 0.0f, 0.0f, 0, default, Main.rand.NextFloat(0.9f, 1.3f));
				Main.dust[i].noGravity = true;
				Main.dust[i].velocity = Projectile.velocity.RotatedByRandom(MathHelper.Pi / 6) * 0.5f;
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++) {
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			}
			return false;
		}

		public override void PostDraw(Color lightColor)
		{
			float num108 = 4;
			float num107 = (float)Math.Cos((double)(Main.GlobalTimeWrappedHourly % 2.4f / 1.2f * 6.28318548f)) / 4f + 0.75f;
			float num106 = 0f;
			Vector2 vector33 = new Vector2(Projectile.Center.X, Projectile.Center.Y) - Main.screenPosition - Projectile.velocity;
			Color color29 = new Color(127, 127, 127, 0).MultiplyRGBA(Color.Tomato);
			for (int num103 = 0; num103 < 4; num103++) {
				Color color28 = color29;
				color28 = Projectile.GetAlpha(color28);
				color28 *= 1f - num107;
				Vector2 vector29 = Projectile.Center + ((float)num103 / (float)num108 * 6.28318548f + Projectile.rotation + num106).ToRotationVector2() * (4f * num107 + 2f) - Main.screenPosition - Projectile.velocity * (float)num103;
				Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, vector29, null, color28, Projectile.rotation, TextureAssets.Projectile[Projectile.type].Value.Size() / 2f, Projectile.scale, SpriteEffects.None, 0f);
			}
		}
		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if (Main.rand.Next(15) == 1)
				target.AddBuff(BuffID.Bleeding, 200);
		}
	}
}