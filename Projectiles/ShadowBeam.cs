using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	public class ShadowBeam : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Solar Beam");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 9;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			projectile.width = 8;
			projectile.height = 8;
			projectile.friendly = true;
			projectile.minion = true;
			projectile.penetrate = 1;
			projectile.tileCollide = false;
			projectile.alpha = 255;
			projectile.timeLeft = 500;
			projectile.light = 0;
			projectile.extraUpdates = 60;
		}

		public override void AI()
		{
			for (int num447 = 0; num447 < 2; num447++)
			{
				Vector2 vector33 = projectile.position;
				vector33 -= projectile.velocity * ((float)num447 * 0.25f);
				projectile.alpha = 255;
				int num448 = Dust.NewDust(vector33, 1, 1, 244, 0f, 0f, 0, new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB), 0.25f);
				Main.dust[num448].noGravity = true;
				Main.dust[num448].position = vector33;
				Main.dust[num448].scale = (float)Main.rand.Next(70, 110) * 0.013f;
				Main.dust[num448].velocity *= 0.2f;

				int num449 = Dust.NewDust(vector33, 1, 1, 173, 0f, 0f, 0, new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB), 0.25f);
				Main.dust[num449].noGravity = true;
				Main.dust[num449].position = vector33;
				Main.dust[num449].scale = (float)Main.rand.Next(70, 110) * 0.013f;
				Main.dust[num449].velocity *= 0.2f;
			}
			return;
		}

		public override void Kill(int timeLeft)
		{
			for (int num621 = 0; num621 < 5; num621++)
			{
				Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 242, 0f, 0f, 100, default(Color), 2f);
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.StrikeNPC(projectile.damage, 0f, 0, crit);
			target.StrikeNPC(projectile.damage, 0f, 0, crit);

			if (Main.rand.Next(5) == 2)
				target.AddBuff(BuffID.ShadowFlame, 180);
		}

		//public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		//{
		//    Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
		//    for (int k = 0; k < projectile.oldPos.Length; k++)
		//    {
		//        Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
		//        Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
		//        spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
		//    }
		//    return true;
		//}
	}
}