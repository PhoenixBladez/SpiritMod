using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Summon
{
	public class HappySoul : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Soul of Happiness");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 13;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			Projectile.hostile = false;
			Projectile.sentry = true;
			Projectile.width = 20;
			Projectile.timeLeft = Projectile.SentryLifeTime;
			Projectile.friendly = false;
			Projectile.penetrate = -1;
			Projectile.height = 20;
			Projectile.aiStyle = -1;
		}
		public override void AI()
		{
			Projectile.alpha += 5;
			if (Projectile.alpha >= 200) {
				Projectile.alpha = 200;
			}
			if (Projectile.localAI[0] == 0f) {
				Projectile.localAI[0] = Projectile.Center.Y;
				Projectile.netUpdate = true; //localAI probably isnt affected by this... buuuut might as well play it safe
			}
			if (Projectile.Center.Y >= Projectile.localAI[0]) {
				Projectile.localAI[1] = -1f;
				Projectile.netUpdate = true;
			}
			if (Projectile.Center.Y <= Projectile.localAI[0] - 2f) {
				Projectile.localAI[1] = 1f;
				Projectile.netUpdate = true;
			}
			Projectile.velocity.Y = MathHelper.Clamp(Projectile.velocity.Y + 0.009f * Projectile.localAI[1], -.75f, .75f);

			Projectile.frameCounter++;
			if (Projectile.frameCounter >= 45) {
				Projectile.frameCounter = 0;
				float num = 1000f;
				int num2 = -1;
				for (int i = 0; i < 200; i++) {
					float num3 = Vector2.Distance(Projectile.Center, Main.npc[i].Center);
					if (num3 < num && num3 < 1640f && Main.npc[i].CanBeChasedBy(Projectile, false)) {
						num2 = i;
						num = num3;
					}
				}
				if (num2 != -1) {
					bool flag = Collision.CanHit(Projectile.position, Projectile.width, Projectile.height, Main.npc[num2].position, Main.npc[num2].width, Main.npc[num2].height);
					if (flag) {
						Vector2 value = Main.npc[num2].Center - Projectile.Center;
						float num4 = 9f;
						float num5 = (float)Math.Sqrt((double)(value.X * value.X + value.Y * value.Y));
						if (num5 > num4)
							num5 = num4 / num5;

						value *= num5;
						int p = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center.X, Projectile.Center.Y, value.X, value.Y, ModContent.ProjectileType<NovaBeam1>(), Projectile.damage, Projectile.knockBack / 2f, Projectile.owner, 0f, 0f);
						Main.projectile[p].DamageType = DamageClass.Summon;
					}
				}
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

		public override Color? GetAlpha(Color lightColor) => new Color(252, 252, 252, Projectile.alpha);
	}
}
