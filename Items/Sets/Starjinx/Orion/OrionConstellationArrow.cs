using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Utilities;
using Terraria.Graphics.Shaders;
using SpiritMod.Prim;
using System;
using System.IO;

namespace SpiritMod.Items.Sets.Starjinx.Orion
{
	public class OrionConstellationArrow : ModProjectile, ITrailProjectile
	{
		public override string Texture => "Terraria/Extra_89";
		public override void SetStaticDefaults() => DisplayName.SetDefault("Astral Arrow");

		public override void SetDefaults()
		{
			projectile.width = 8;
			projectile.height = 8;
			projectile.friendly = true;
			projectile.penetrate = 1;
			projectile.ranged = true;
			projectile.timeLeft = 600;
			projectile.arrow = true;
		}

		public Vector2 TargetPos { get; set; }

		public void DoTrailCreation(TrailManager tM)
		{
			tM.CreateTrail(projectile, new StandardColorTrail(new Color(255, 255, 255)), new TriangleCap(), new DefaultTrailPosition(), 15, 150);
			tM.CreateTrail(projectile, new GradientTrail(new Color(255, 255, 255), Color.Transparent), new TriangleCap(), new DefaultTrailPosition(), 5, 150);
		}

		public override void SendExtraAI(BinaryWriter writer) => writer.WriteVector2(TargetPos);

		public override void ReceiveExtraAI(BinaryReader reader) => TargetPos = reader.ReadVector2();

		public override void AI()
		{
			if(projectile.ai[1] == 0)
			{
				float homestrength = MathHelper.Clamp(1 - projectile.Distance(TargetPos) / 500, 0.05f, 0.2f);
				projectile.velocity = Vector2.Lerp(projectile.velocity, projectile.DirectionTo(TargetPos) * 18, homestrength);
				if (projectile.Distance(TargetPos) < 40)
					projectile.ai[1]++;
			}
			else if (projectile.velocity.Length() < 25)
				projectile.velocity *= 1.02f;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D tex = Main.projectileTexture[projectile.type];
			float Timer = (float)(Math.Abs(Math.Sin(Main.GlobalTime * 1.5f)) / 6f) + 0.7f; 
			Vector2 scaleVerticalGlow = new Vector2(0.2f, 1f) * Timer;
			Vector2 scaleHorizontalGlow = new Vector2(0.2f, 2f) * Timer;
			Color blurcolor = new Color(255, 255, 255, 100) * 0.8f;
			spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, null, blurcolor * Timer, 0, tex.Size() / 2, scaleVerticalGlow, SpriteEffects.None, 0);
			spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, null, blurcolor * Timer, MathHelper.PiOver2, tex.Size() / 2, scaleHorizontalGlow, SpriteEffects.None, 0);
			return false;
		}
	}
}