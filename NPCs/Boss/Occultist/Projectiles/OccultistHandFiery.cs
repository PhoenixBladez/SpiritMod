using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;

namespace SpiritMod.NPCs.Boss.Occultist.Projectiles
{
	public class OccultistHandFiery : OccultistHand, IDrawAdditive
	{
		public override string Texture => "SpiritMod/NPCs/Boss/Occultist/Projectiles/OccultistHand";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dark Grasp");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
		}

		public override void SetDefaults()
		{
			maxTimeLeft = 360;
			base.SetDefaults();
			period = 120;
			DoAcceleration = false;
			TileCollideCheck = false;
		}

		public override void PostDraw(Color lightColor)
		{
			base.PostDraw(lightColor);
			Projectile.QuickDraw(Main.spriteBatch, drawColor: Color.Black * Projectile.Opacity);
		}

		public void AdditiveCall(SpriteBatch sB, Vector2 screenPos)
		{
			Texture2D extraTex = TextureAssets.Extra[55].Value;
			Texture2D bloom = Mod.Assets.Request<Texture2D>("Effects/Masks/CircleGradient", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			int frame = (int)((Main.GlobalTimeWrappedHourly * 5) % 4);
			Rectangle drawFrame = new Rectangle(0, frame * extraTex.Height / 4, extraTex.Width, extraTex.Height / 4);
			float rotation = Projectile.rotation + MathHelper.Pi;
			float scale = Projectile.scale * 0.7f;
			Color color = new Color(252, 68, 166, 100) * Projectile.Opacity * 0.33f;
			for (int j = 0; j < ProjectileID.Sets.TrailCacheLength[Projectile.type]; j++)
			{
				Vector2 drawPos = Projectile.oldPos[j] + Projectile.Size / 2;
				float Opacity = (ProjectileID.Sets.TrailCacheLength[Projectile.type] - j) / (float)ProjectileID.Sets.TrailCacheLength[Projectile.type];
				Opacity = (Opacity / 2) + 0.5f;
				float trailScale = scale * Opacity;
				float trailRot = Projectile.oldRot[j] + MathHelper.Pi;
				sB.Draw(extraTex, drawPos - Main.screenPosition, drawFrame, color * Opacity, trailRot, drawFrame.Size() / 2, trailScale, SpriteEffects.None, 0f);
				sB.Draw(bloom, drawPos - Main.screenPosition, null, color * Opacity * 1.2f, trailRot, bloom.Size() / 2, trailScale * 0.8f, SpriteEffects.None, 0f);
			}
			for (int i = -1; i <= 1; i++)
			{
				Vector2 drawPos = Projectile.velocity.RotatedBy(MathHelper.Pi * i / 4f);
				sB.Draw(extraTex, Projectile.Center + drawPos - Main.screenPosition, drawFrame, color, rotation, drawFrame.Size() / 2, scale, SpriteEffects.None, 0f);
				sB.Draw(bloom, Projectile.Center + drawPos - Main.screenPosition, null, color * 1.2f, 0, bloom.Size() / 2, scale * 0.8f, SpriteEffects.None, 0f);
			}
		}
	}
}