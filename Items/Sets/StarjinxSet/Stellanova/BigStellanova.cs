using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using System;
using SpiritMod.Utilities;
using SpiritMod.Mechanics.Trails;
using SpiritMod.Prim;

namespace SpiritMod.Items.Sets.StarjinxSet.Stellanova
{
	public class BigStellanova : ModProjectile, ITrailProjectile
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Starfire");

		public override void SetDefaults()
		{
			projectile.width = 32;
			projectile.height = 32;
			projectile.aiStyle = 0;
			projectile.tileCollide = true;
			projectile.timeLeft = 720;
			projectile.ranged = true;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.ignoreWater = true;
			projectile.alpha = 255;
			projectile.scale = 0.1f;
		}

		public override void AI()
		{
			projectile.rotation = projectile.velocity.ToRotation() - MathHelper.PiOver2;
			projectile.ai[0]++; //make ref variable later for readability
			int fadeTime = 60;
			float progress;

			if (projectile.timeLeft > fadeTime) //fade in
				progress = Math.Min(projectile.ai[0] / fadeTime, 1);

			else //fade out
				progress = Math.Min(projectile.timeLeft / (float)fadeTime, 1);

			progress = EaseFunction.EaseCubicOut.Ease(progress);
			projectile.scale = 1 * progress;
			projectile.alpha = (int)(255 * (1 - progress));

			projectile.velocity = Vector2.Lerp(projectile.velocity, Vector2.Zero, 0.06f);

			for (int i = 0; i < Main.maxProjectiles; ++i) //Gravitate StellanovaStarfire projectiles to this proj
			{
				Projectile p = Main.projectile[i];
				if (p.active && p.DistanceSQ(projectile.Center) < 400 * 400 && p.type == ModContent.ProjectileType<StellanovaStarfire>())
				{
					p.velocity = p.velocity.Length() * Vector2.Normalize(Vector2.Lerp(p.velocity, p.DirectionTo(projectile.Center) * p.velocity.Length(), 0.1f)) * 0.85f;
					p.velocity += Vector2.Lerp(p.velocity, p.DirectionTo(projectile.Center) * 25, 0.1f) * 0.15f;
				}
			}
		}

		public void DoTrailCreation(TrailManager tM) => tM.CreateTrail(projectile, new StarjinxTrail(projectile, Main.GlobalTime, 1, 0.66f), new RoundCap(), new DefaultTrailPosition(), 50f * projectile.scale, 240f * projectile.scale, new ImageShader(mod.GetTexture("Textures/Trails/Trail_4"), 0.02f, 1f, 1f));


		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Color pink = new Color(255, 158, 241) * projectile.Opacity;
			Color purple = new Color(255, 69, 187) * projectile.Opacity;
			Color lightPink = Color.Lerp(pink, Color.White * projectile.Opacity, 0.66f) * 0.8f * Math.Max(projectile.Opacity - 0.9f, 0) * 10;

			float squaredScale = (float)Math.Pow(projectile.scale, 2);
			DrawGodray.DrawGodrays(spriteBatch, projectile.Center - Main.screenPosition, lightPink, 30 * squaredScale, 12 * squaredScale, 20);

			//not ideal, basically only stable because no sprites are drawn after this in the duration of the current spritebatch, as the current spritebatch is ended
			//convert to spritebatch drawing later if a suitable additive mask for a blur line is found?
			//alternatively just literally use the same ray texture godrays use they look basically the same when obscured 
			float blurLength = 170 * squaredScale;
			float blurWidth = 12 * squaredScale;
			float flickerStrength = (((float)Math.Sin(Main.GlobalTime * 12) % 1) * 0.1f) + 1f;
			Effect blurEffect = mod.GetEffect("Effects/BlurLine");

			var blurLine = new SquarePrimitive()
			{
				Position = projectile.Center - Main.screenPosition,
				Height = blurWidth * flickerStrength,
				Length = blurLength * flickerStrength,
				Rotation = 0,
				Color = lightPink * flickerStrength
			};

			PrimitiveRenderer.DrawPrimitiveShape(blurLine, blurEffect);

			Effect effect = SpiritMod.Instance.GetEffect("Effects/StellanovaOrb");
			effect.Parameters["uTexture"].SetValue(mod.GetTexture("Textures/Milky2"));
			effect.Parameters["distortTexture"].SetValue(mod.GetTexture("Textures/noiseNormal"));
			effect.Parameters["timer"].SetValue(Main.GlobalTime * 0.66f);
			effect.Parameters["intensity"].SetValue(1.6f);
			effect.Parameters["lightColor"].SetValue(pink.ToVector4());
			effect.Parameters["darkColor"].SetValue(purple.ToVector4());
			effect.Parameters["coordMod"].SetValue(0.5f);

			spriteBatch.End(); spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, default, default, RasterizerState.CullNone, effect, Main.GameViewMatrix.ZoomMatrix);

			Texture2D projTex = Main.projectileTexture[projectile.type];
			spriteBatch.Draw(projTex, projectile.Center - Main.screenPosition, null, pink, Main.GlobalTime * 0.1f,
				projTex.Size() / 2, 0.1f * projectile.scale, SpriteEffects.None, 0);

			spriteBatch.End(); spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, default, default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
			return false;
		}
	}
}