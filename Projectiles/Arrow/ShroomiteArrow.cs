
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
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
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.WoodenArrowFriendly);
			Projectile.penetrate = -1;
			Projectile.aiStyle = 0;
			AIType = 0;
			Projectile.extraUpdates = 5;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.StrikeNPC(Projectile.damage, 0f, 0, crit);
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 5; i++) {
				Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Shiverthorn);
			}
			SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
		}

		public override void AI()
		{
			if (Projectile.ai[0] == 0) {
				Projectile.ai[0] = 1;
				ProjectileExtras.LookAlongVelocity(this);
			}
			else if (lastFrame > 0) {
				lastFrame++;
				if (lastFrame > 2) {
					Projectile.Kill();
				}
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			//Pre-compute some values to improve performance.
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
			float divisor = 1f / (float)Projectile.oldPos.Length;
			Color preColor = Projectile.GetAlpha(lightColor) * divisor;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
			drawOrigin += new Vector2(0f, Projectile.gfxOffY);

			for (int k = 0; k < Projectile.oldPos.Length; k++) {
				Vector2 drawPos = Projectile.oldPos[k] + drawOrigin;
				drawPos -= Main.screenPosition;
				Color color = preColor * (float)(Projectile.oldPos.Length - k);
				spriteBatch.Draw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			}
			return true;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			lastFrame = 1;
			Projectile.tileCollide = false;
			return false;
		}

	}
}
