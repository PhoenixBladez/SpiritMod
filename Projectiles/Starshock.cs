using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.ID;
using SpiritMod.Mechanics.Trails;

namespace SpiritMod.Projectiles
{
	public class Starshock : ModProjectile, ITrailProjectile
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Starshock");

		public override void SetDefaults()
		{
			Projectile.width = 18;
			Projectile.height = 18;
			Projectile.hostile = true;
			Projectile.penetrate = 1;
			Projectile.rotation = Main.rand.NextFloat(MathHelper.Pi);
			Projectile.tileCollide = false;
			Projectile.timeLeft = 100;
		}

		public void DoTrailCreation(TrailManager tManager)
		{
			tManager.CreateTrail(Projectile, new GradientTrail(new Color(108, 215, 245), new Color(105, 213, 255)), new RoundCap(), new DefaultTrailPosition(), 8f, 150f, new ImageShader(Mod.GetTexture("Textures/Trails/Trail_2"), 0.01f, 1f, 1f));
			tManager.CreateTrail(Projectile, new GradientTrail(new Color(255, 255, 255) * .25f, new Color(255, 255, 255) * .25f), new RoundCap(), new DefaultTrailPosition(), 26f, 250f, new DefaultShader());
		}

		public override void AI()
		{
			if (++Projectile.ai[0] > 20f) {
				bool canaccelerate = Projectile.velocity.Length() < 18;
				Projectile.tileCollide = !canaccelerate;
				if (canaccelerate)
					Projectile.velocity = Projectile.velocity * 1.02f;
			}

			Projectile.rotation += 0.1f;

			if (Projectile.localAI[1] == 0f) {
				Projectile.localAI[1] = 1f;
			}
		}

		public override void Kill(int timeLeft)
		{
			for (int k = 0; k < 3; k++) {
				int d = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.DungeonSpirit, Projectile.oldVelocity.X * 0.1f, Projectile.oldVelocity.Y * 0.1f);
				Main.dust[d].noGravity = true;
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, Projectile.Center - Main.screenPosition, TextureAssets.Projectile[Projectile.type].Value.Bounds, Projectile.GetAlpha(Color.White), Projectile.rotation, 
				TextureAssets.Projectile[Projectile.type].Value.Size()/2, Projectile.scale, SpriteEffects.None, 0);
			return false;
		}
	}
}