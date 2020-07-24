using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Summon
{
	public class HappySoul : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Soul of Happiness");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 13;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			projectile.hostile = false;
			projectile.minion = true;
			projectile.width = 20;
			projectile.timeLeft = 3600;
			projectile.friendly = false;
			projectile.penetrate = -1;
			projectile.height = 20;
			projectile.aiStyle = -1;
		}
		public override void AI()
		{
			projectile.alpha += 5;
			if (projectile.alpha >= 200) {
				projectile.alpha = 200;
			}
			if (projectile.localAI[0] == 0f) {
				projectile.localAI[0] = projectile.Center.Y;
				projectile.netUpdate = true; //localAI probably isnt affected by this... buuuut might as well play it safe
			}
			if (projectile.Center.Y >= projectile.localAI[0]) {
				projectile.localAI[1] = -1f;
				projectile.netUpdate = true;
			}
			if (projectile.Center.Y <= projectile.localAI[0] - 2f) {
				projectile.localAI[1] = 1f;
				projectile.netUpdate = true;
			}
			projectile.velocity.Y = MathHelper.Clamp(projectile.velocity.Y + 0.009f * projectile.localAI[1], -.75f, .75f);

			projectile.frameCounter++;
			if (projectile.frameCounter >= 45) {
				projectile.frameCounter = 0;
				float num = 1000f;
				int num2 = -1;
				for (int i = 0; i < 200; i++) {
					float num3 = Vector2.Distance(projectile.Center, Main.npc[i].Center);
					if (num3 < num && num3 < 1640f && Main.npc[i].CanBeChasedBy(projectile, false)) {
						num2 = i;
						num = num3;
					}
				}
				if (num2 != -1) {
					bool flag = Collision.CanHit(projectile.position, projectile.width, projectile.height, Main.npc[num2].position, Main.npc[num2].width, Main.npc[num2].height);
					if (flag) {
						Vector2 value = Main.npc[num2].Center - projectile.Center;
						float num4 = 9f;
						float num5 = (float)Math.Sqrt((double)(value.X * value.X + value.Y * value.Y));
						if (num5 > num4)
							num5 = num4 / num5;

						value *= num5;
						int p = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, value.X, value.Y, mod.ProjectileType("NovaBeam1"), projectile.damage, projectile.knockBack / 2f, projectile.owner, 0f, 0f);
						Main.projectile[p].minion = true;
						Main.projectile[p].magic = false;
					}
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
		public override Color? GetAlpha(Color lightColor)
		{
			return new Color(252, 252, 252, projectile.alpha);
		}
	}
}
