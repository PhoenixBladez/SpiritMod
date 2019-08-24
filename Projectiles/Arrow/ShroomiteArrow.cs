using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Arrow
{
	class ShroomiteArrow : ModProjectile
	{
		private int lastFrame = 0;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shroomite Arrow");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 6;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.WoodenArrowFriendly);
			projectile.penetrate = -1;
			projectile.aiStyle = 0;
			aiType = 0;
			projectile.extraUpdates = 5;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.StrikeNPC(projectile.damage, 0f, 0, crit);
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 5; i++)
			{
				Dust.NewDust(projectile.position, projectile.width, projectile.height, 224);
			}
			Main.PlaySound(0, (int)projectile.position.X, (int)projectile.position.Y);
		}

		public override void AI()
		{
			if (projectile.ai[0] == 0)
			{
				projectile.ai[0] = 1;
				ProjectileExtras.LookAlongVelocity(this);
			}
			else if (lastFrame > 0)
			{
				lastFrame++;
				if (lastFrame > 2)
				{
					projectile.Kill();
				}
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
