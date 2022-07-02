using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Particles;
using SpiritMod.Mechanics.Trails;
using SpiritMod.Prim;

namespace SpiritMod.NPCs.StarjinxEvent.Enemies.Archon.Projectiles
{
	public class ArchonStarFragment : ModProjectile, ITrailProjectile, IDrawAdditive
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Star Fragment");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
		}

		public override void SetDefaults()
		{
			Projectile.Size = new Vector2(10, 10);
			Projectile.scale = Main.rand.NextFloat(0.8f, 1.2f);
			Projectile.hostile = true;
			Projectile.timeLeft = 60;
			Projectile.ignoreWater = true;
		}

		public override void AI()
		{
			Projectile.velocity *= 0.98f;
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
			Lighting.AddLight(Projectile.Center, Color.LightCyan.ToVector3() / 3);

			if (Main.rand.NextBool(5) && !Main.dedServ)
				ParticleHandler.SpawnParticle(new StarParticle(Projectile.Center, Projectile.velocity.RotatedByRandom(MathHelper.PiOver4) * Main.rand.NextFloat(0.3f), Color.White, Color.Cyan, Main.rand.NextFloat(0.1f, 0.2f) * Projectile.scale, 25));
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Projectile.QuickDrawTrail(Main.spriteBatch, 0.25f);
			Projectile.QuickDraw(Main.spriteBatch);
			return false;
		}

		public override Color? GetAlpha(Color lightColor) => Color.White * Projectile.Opacity;

		public void DoTrailCreation(TrailManager tManager)
		{
			float scalemod = Projectile.scale;
			tManager.CreateTrail(Projectile, new GradientTrail(new Color(145, 255, 253), new Color(61, 178, 224)), new RoundCap(), new ArrowGlowPosition(), 10f * scalemod, 70f * scalemod, new ImageShader(Mod.Assets.Request<Texture2D>("Textures/Trails/Trail_2").Value, 0.01f, 1f, 1f));
			tManager.CreateTrail(Projectile, new GradientTrail(new Color(145, 255, 253) * .5f, new Color(61, 178, 224) * .5f), new RoundCap(), new ArrowGlowPosition(), 30f * scalemod, 140f * scalemod, new DefaultShader());
			tManager.CreateTrail(Projectile, new StandardColorTrail(Color.White * 0.3f), new RoundCap(), new ArrowGlowPosition(), 10f * scalemod, 40f * scalemod, new DefaultShader());
			tManager.CreateTrail(Projectile, new StandardColorTrail(Color.White * 0.3f), new RoundCap(), new ArrowGlowPosition(), 10f * scalemod, 40f * scalemod, new DefaultShader());
			tManager.CreateTrail(Projectile, new StandardColorTrail(Color.White * 0.2f), new RoundCap(), new ArrowGlowPosition(), 30f * scalemod, 10f * scalemod, new DefaultShader());
		}
		public void AdditiveCall(SpriteBatch sb)
		{
			float blurLength = 100 * Projectile.scale * Projectile.Opacity;
			float blurWidth = 25 * Projectile.scale * Projectile.Opacity;

			Effect blurEffect = ModContent.Request<Effect>("Effects/BlurLine", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			SquarePrimitive blurLine = new SquarePrimitive()
			{
				Position = Projectile.Center - Main.screenPosition,
				Height = blurWidth,
				Length = blurLength,
				Rotation = Projectile.velocity.X * .1f,
				Color = new Color(181, 245, 255) * .5f
			};
			PrimitiveRenderer.DrawPrimitiveShape(blurLine, blurEffect);
		}
	}
}