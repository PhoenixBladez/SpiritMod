using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Utilities;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.StarjinxSet.StarfireLamp
{
    public class StarfireProj : ModProjectile, ITrailProjectile, IDrawAdditive
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Starfire");
            ProjectileID.Sets.Homing[projectile.type] = true;
			Main.projFrames[projectile.type] = 4;
		}

        public override void SetDefaults()
        {
			projectile.Size = new Vector2(20, 20);
            projectile.scale = Main.rand.NextFloat(0.8f, 1.1f);
            projectile.tileCollide = true;
            projectile.friendly = true;
            projectile.magic = true;
		}

		public void DoTrailCreation(TrailManager tM)
		{
			tM.CreateTrail(projectile, new StarjinxTrail(Main.GlobalTime, 1, 0.2f), new RoundCap(), new ArrowGlowPosition(), 40 * projectile.scale, 300 * projectile.scale);
			tM.CreateTrail(projectile, new StarjinxTrail(Main.GlobalTime), new NoCap(), new DefaultTrailPosition(), 20 * projectile.scale, 300 * projectile.scale, new ImageShader(mod.GetTexture("Textures/Trails/Trail_3"), 0.2f, 1f, 1f));
			tM.CreateTrail(projectile, new StarjinxTrail(Main.GlobalTime), new NoCap(), new DefaultTrailPosition(), 20 * projectile.scale, 300 * projectile.scale, new ImageShader(mod.GetTexture("Textures/Trails/Trail_3"), 0.2f, 1f, 1f));
		}

		public override void Kill(int timeLeft)
        {
            //Main.PlaySound(4, (int)projectile.position.X, (int)projectile.position.Y, 3, 1f, 0f);
            Gore.NewGore(projectile.Center, projectile.velocity, mod.GetGoreSlot("Gores/StarjinxGore"), 1);
        }

		public override void AI()
		{
			projectile.rotation = projectile.velocity.ToRotation() - MathHelper.PiOver2;
			if (projectile.frameCounter++ % 5 == 0)
			{
				projectile.frame = (projectile.frame + 1) % Main.projFrames[projectile.type];
			}
		}

		public void AdditiveCall(SpriteBatch spriteBatch)
		{
			projectile.QuickDraw(spriteBatch, drawColor: Color.White);
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D tex = Main.extraTexture[89];
			float Timer = (float)(Math.Abs(Math.Sin(Main.GlobalTime * 6f)) / 12f) + 0.7f;
			Vector2 scaleVerticalGlow = new Vector2(0.3f, 1f) * Timer;
			Vector2 scaleHorizontalGlow = new Vector2(0.3f, 2f) * Timer;
			Color blurcolor = new Color(255, 255, 255, 100) * 0.6f;
			spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, null, blurcolor * Timer, 0, tex.Size() / 2, scaleVerticalGlow, SpriteEffects.None, 0);
			spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, null, blurcolor * Timer, MathHelper.PiOver2, tex.Size() / 2, scaleHorizontalGlow, SpriteEffects.None, 0);
			return false;
		}
	}
}