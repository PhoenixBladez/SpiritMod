using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Thrown
{
	class MechKnifeProj : ModProjectile
	{
		private int lastFrame = 0;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mechanical Scrap");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 6;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.ThrowingKnife);
			projectile.penetrate = 1;
			projectile.aiStyle = 0;
			aiType = 0;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.StrikeNPC(projectile.damage, 0f, 0, crit);

			if (Main.rand.Next(2) == 0)
				target.AddBuff(BuffID.OnFire, 300, true);
		}

		public override void Kill(int timeLeft)
		{
			if (Main.rand.Next(0, 4) == 0)
				Item.NewItem((int)projectile.position.X, (int)projectile.position.Y, projectile.width, projectile.height, mod.ItemType("MechKnife"), 1, false, 0, false, false);

			for (int i = 0; i < 5; i++)
			{
				Dust.NewDust(projectile.position, projectile.width, projectile.height, 6);
			}
			Main.PlaySound(0, (int)projectile.position.X, (int)projectile.position.Y);
		}

		public override void AI()
		{
			if (projectile.ai[0] == 0)
			{
				projectile.ai[0] = 1;
				projectile.rotation = (float)Math.Atan2(projectile.velocity.X, -projectile.velocity.Y);
			}
			else if (lastFrame > 0)
			{
				lastFrame++;
				if (lastFrame > 4)
					projectile.Kill();
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			//Pre-compute some values to improve performance.
			Texture2D texture = Main.projectileTexture[projectile.type];
			float divisor = 1f / (float)projectile.oldPos.Length;
			Color preColor = projectile.GetAlpha(lightColor) * divisor;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, projectile.height * 0.5f);
			drawOrigin += new Vector2(0f, projectile.gfxOffY);

			for (int k = 0; k < projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = projectile.oldPos[k] + drawOrigin;
				drawPos -= Main.screenPosition;
				Color color = preColor * (float)(projectile.oldPos.Length - k);
				spriteBatch.Draw(texture, drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			}
			return true;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			lastFrame = 1;
			projectile.tileCollide = false;
			return false;
		}

	}
}
