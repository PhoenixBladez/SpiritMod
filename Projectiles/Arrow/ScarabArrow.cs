using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Arrow
{
	public class ScarabArrow : ModProjectile
	{
		static readonly int traillength = 10;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Scarab Arrow");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = traillength;
			ProjectileID.Sets.TrailingMode[projectile.type] = 2;
		}
		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.WoodenArrowFriendly);
			projectile.aiStyle = -1;
			projectile.penetrate = 2;
			projectile.timeLeft = 720;
		}
		bool Enchanted => projectile.ai[0] == 1;
		public override void AI()
		{
			projectile.rotation = projectile.velocity.ToRotation() - MathHelper.Pi/2;

			if (Enchanted)
			{
				Lighting.AddLight(projectile.position, Color.White.ToVector3() / 2);
				projectile.extraUpdates = 1;
			}
			else if (++projectile.ai[1] > 20)
				projectile.velocity.Y += 0.25f;
		}
		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 10; i++) {
				Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.PlatinumCoin);
			}
			Main.PlaySound(SoundID.Dig, (int)projectile.position.X, (int)projectile.position.Y);
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if(Enchanted)
				target.AddBuff(ModContent.BuffType<TopazMarked>(), 240);
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor) //refactor to be drawadditive when ported to main??
		{
			Texture2D projtexture = Main.projectileTexture[projectile.type];

			void drawtexture(Vector2 position, Color color, float Opacity, Texture2D tex) => spriteBatch.Draw(tex, position - Main.screenPosition, null, color * Opacity, projectile.rotation, tex.Size() / 2, Opacity * projectile.scale, SpriteEffects.None, 0);

			for (int i = 0; i < traillength; i += 1)
			{
				Vector2 drawpos = projectile.oldPos[i] + projectile.Size / 2;
				float Opacity = (traillength - i) / (float)traillength;
				Opacity *= 0.8f;

				drawtexture(drawpos, lightColor, Opacity, projtexture);
			}
			drawtexture(projectile.Center, lightColor, 1, projtexture);
			return false;
		}
	}
}
