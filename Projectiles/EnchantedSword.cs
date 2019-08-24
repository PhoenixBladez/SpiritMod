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
	public class EnchantedSword : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Enchanted Sword");
		}

		public override void SetDefaults()
		{
			projectile.width = 16;
			projectile.height = 16;
			projectile.friendly = true;
			projectile.thrown = true;
			projectile.penetrate = 3;
			projectile.tileCollide = false;
			projectile.alpha = 255;
			projectile.timeLeft = 10000;
			projectile.light = 0;
			projectile.extraUpdates = 1;
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 9;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		Vector2 offset = new Vector2(100, 100);
		public override void AI()
		{
			Player player = Main.player[projectile.owner];
			projectile.ai[0] += .03f;
			projectile.Center = player.Center + offset.RotatedBy(projectile.ai[0] + projectile.ai[1] * (Math.PI * 10 / 1));

			int dust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 58, 0f, 0f);
			int dust1 = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 187, 0f, 0f);
			Main.dust[dust].noGravity = true;
			Main.dust[dust1].noGravity = true;
			Main.dust[dust].scale = 0.9f;
			Main.dust[dust1].scale = 0.9f;

			projectile.rotation = projectile.velocity.ToRotation() + (float)(Math.PI / 2);
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 5; i++)
			{
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 58);
			}
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