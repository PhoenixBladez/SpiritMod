using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Utilities;
using System;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.StarjinxSet.Sagittarius
{
	public class SagittariusArrow : ModProjectile, ITrailProjectile
	{
		public override string Texture => "Terraria/Extra_89";
		public override void SetStaticDefaults() => DisplayName.SetDefault("Astral Arrow");

		public override void SetDefaults()
		{
			projectile.width = 8;
			projectile.height = 8;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.timeLeft = 70;
			projectile.ranged = true;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 10;
			projectile.extraUpdates = 1;
			projectile.arrow = true;
		}

		public void DoTrailCreation(TrailManager tM)
		{
			tM.CreateTrail(projectile, new GradientTrail(Color.White * 0.2f, Color.Transparent), new RoundCap(), new DefaultTrailPosition(), 60, 800);
			tM.CreateTrail(projectile, new StandardColorTrail(Color.White), new NoCap(), new DefaultTrailPosition(), 10, 600);
			tM.CreateTrail(projectile, new StandardColorTrail(new Color(101, 255, 245)), new NoCap(), new DefaultTrailPosition(), 120, 500, new ImageShader(mod.GetTexture("Textures/Trails/Trail_1"), 0.01f, 1f, 1));
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D tex = Main.projectileTexture[projectile.type];
			float Timer = (float)(Math.Abs(Math.Sin(Main.GlobalTime * 6f)) / 12f) + 0.7f;
			Vector2 scaleVerticalGlow = new Vector2(0.7f, 2f) * Timer;
			Vector2 scaleHorizontalGlow = new Vector2(0.7f, 4f) * Timer;
			Color blurcolor = new Color(255, 255, 255, 100) * 0.7f;
			spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, null, blurcolor * Timer, 0, tex.Size() / 2, scaleVerticalGlow, SpriteEffects.None, 0);
			spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, null, blurcolor * Timer, MathHelper.PiOver2, tex.Size() / 2, scaleHorizontalGlow, SpriteEffects.None, 0);

			spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, null, blurcolor, projectile.velocity.ToRotation() + MathHelper.PiOver2, tex.Size() / 2, new Vector2(0.4f, 4f), SpriteEffects.None, 0);
			return false;
		}
	}
}