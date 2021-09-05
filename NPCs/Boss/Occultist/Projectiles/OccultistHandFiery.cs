using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

namespace SpiritMod.NPCs.Boss.Occultist.Projectiles
{
	public class OccultistHandFiery : OccultistHand, IDrawAdditive
	{
		public override string Texture => "SpiritMod/NPCs/Boss/Occultist/Projectiles/OccultistHand";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dark Grasp");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 10;
			ProjectileID.Sets.TrailingMode[projectile.type] = 2;
		}

		public override void SetDefaults()
		{
			maxTimeLeft = 360;
			base.SetDefaults();
			period = 120;
			DoAcceleration = false;
			TileCollideCheck = false;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			base.PostDraw(spriteBatch, lightColor);
			projectile.QuickDraw(spriteBatch, drawColor: Color.Black * projectile.Opacity);
		}

		public void AdditiveCall(SpriteBatch sB)
		{
			Texture2D extraTex = Main.extraTexture[55];
			Texture2D bloom = mod.GetTexture("Effects/Masks/CircleGradient");
			int frame = (int)((Main.GlobalTime * 5) % 4);
			Rectangle drawFrame = new Rectangle(0, frame * extraTex.Height / 4, extraTex.Width, extraTex.Height / 4);
			float rotation = projectile.rotation + MathHelper.Pi;
			float scale = projectile.scale * 0.7f;
			Color color = new Color(252, 68, 166, 100) * projectile.Opacity * 0.33f;
			for (int j = 0; j < ProjectileID.Sets.TrailCacheLength[projectile.type]; j++)
			{
				Vector2 drawPos = projectile.oldPos[j] + projectile.Size / 2;
				float Opacity = (ProjectileID.Sets.TrailCacheLength[projectile.type] - j) / (float)ProjectileID.Sets.TrailCacheLength[projectile.type];
				Opacity = (Opacity / 2) + 0.5f;
				float trailScale = scale * Opacity;
				float trailRot = projectile.oldRot[j] + MathHelper.Pi;
				sB.Draw(extraTex, drawPos - Main.screenPosition, drawFrame, color * Opacity, trailRot, drawFrame.Size() / 2, trailScale, SpriteEffects.None, 0f);
				sB.Draw(bloom, drawPos - Main.screenPosition, null, color * Opacity * 1.2f, trailRot, bloom.Size() / 2, trailScale * 0.8f, SpriteEffects.None, 0f);
			}
			for (int i = -1; i <= 1; i++)
			{
				Vector2 drawPos = projectile.velocity.RotatedBy(MathHelper.Pi * i / 4f);
				sB.Draw(extraTex, projectile.Center + drawPos - Main.screenPosition, drawFrame, color, rotation, drawFrame.Size() / 2, scale, SpriteEffects.None, 0f);
				sB.Draw(bloom, projectile.Center + drawPos - Main.screenPosition, null, color * 1.2f, 0, bloom.Size() / 2, scale * 0.8f, SpriteEffects.None, 0f);
			}
		}
	}
}