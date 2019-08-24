using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	public class QuicksilverKunai : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Quicksilver Kunai");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 9;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			projectile.width = 7;
			projectile.height = 14;
			projectile.aiStyle = 113;
			projectile.friendly = true;
			projectile.thrown = true;
			projectile.penetrate = 4;
			projectile.timeLeft = 200;
			projectile.alpha = 255;
			projectile.extraUpdates = 1;
			projectile.light = 0;
			aiType = ProjectileID.Shuriken;
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

		public override void AI()
		{
			projectile.rotation += 0.2f;
		}

		public override void Kill(int timeLeft)
		{
			if (Main.rand.Next(5) == 1)
			{
				for (int h = 0; h < 2; h++)
				{
					Vector2 vel = new Vector2(0, -1);
					float rand = Main.rand.NextFloat() * 6.283f;
					vel = vel.RotatedBy(rand);
					vel *= 8f;
					int p = Projectile.NewProjectile(projectile.position.X, projectile.position.Y, vel.X, vel.Y, mod.ProjectileType("QuicksilverBolt"), projectile.damage, 1, projectile.owner, 0f, 0f);
					Main.projectile[p].thrown = true;
					Main.projectile[p].melee = false;
				}
			}
			if (Main.rand.Next(0, 12) == 0)
				Item.NewItem((int)projectile.position.X, (int)projectile.position.Y, projectile.width, projectile.height, mod.ItemType("QuicksilverKunai"), 1, false, 0, false, false);

			for (int i = 0; i < 5; i++)
			{
				Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.SilverCoin);
			}
			Main.PlaySound(0, (int)projectile.position.X, (int)projectile.position.Y);
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