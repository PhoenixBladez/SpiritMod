using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Graphics.Shaders;
using SpiritMod.Mechanics.Trails;

namespace SpiritMod.Items.Sets.GunsMisc.HeavenFleet
{
	public class HeavenfleetStar : ModProjectile, ITrailProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
			DisplayName.SetDefault("Heaven Fleet");
		}

		public override void SetDefaults()
		{
			Projectile.width = 22;
			Projectile.height = 22;
			Projectile.friendly = true;
			Projectile.penetrate = 1;
			Projectile.rotation = Main.rand.NextFloat(MathHelper.Pi);
			Projectile.tileCollide = true;
			Projectile.timeLeft = 150;
		}

		public void DoTrailCreation(TrailManager tManager)
		{
			tManager.CreateTrail(Projectile, new GradientTrail(new Color(108, 215, 245), new Color(105, 213, 255)), new RoundCap(), new DefaultTrailPosition(), 13f, 150f, new DefaultShader());
			tManager.CreateTrail(Projectile, new GradientTrail(new Color(255, 255, 255) * .35f, new Color(255, 255, 255) * .25f), new RoundCap(), new DefaultTrailPosition(), 22f, 150f, new DefaultShader());
			tManager.CreateTrail(Projectile, new GradientTrail(new Color(255, 255, 255) * .125f, new Color(255, 255, 255) * .25f), new RoundCap(), new DefaultTrailPosition(), 34f, 250f, new DefaultShader());
			tManager.CreateTrail(Projectile, new GradientTrail(new Color(255, 255, 255) * .1f, new Color(255, 255, 255) * .25f), new RoundCap(), new DefaultTrailPosition(), 46f, 250f, new DefaultShader());
			tManager.CreateTrail(Projectile, new GradientTrail(new Color(108, 215, 245) * .24f, new Color(105, 213, 255)), new RoundCap(), new DefaultTrailPosition(), 18f, 220f, new DefaultShader());
		}

		public override void AI()
		{
			Projectile.rotation += .05f*Projectile.velocity.X;
			Projectile.ai[0] += .0205f;
			for (int i = 0; i < 2; i++)
			{
				int num = Dust.NewDust(Projectile.Center, 6, 6, DustID.FireworkFountain_Blue, 0f, 0f, 0, default, .65f);
				Main.dust[num].position = Projectile.Center - Projectile.velocity / num * (float)i;

				Main.dust[num].shader = GameShaders.Armor.GetSecondaryShader(27, Main.LocalPlayer);
				Main.dust[num].velocity = Projectile.velocity;
				Main.dust[num].scale = MathHelper.Clamp(Projectile.ai[0], .015f, 1.25f);
				Main.dust[num].noGravity = true;
				Main.dust[num].fadeIn = (float)(110 + Projectile.owner);
			}
			Projectile.velocity *= .96f;
			Projectile.velocity.Y += .016f;
		}

		public override void Kill(int timeLeft)
		{
			if(timeLeft > 0) //Dying through pierce/tile collision rather than naturally
			{
				if (!Main.dedServ)
					for (int i = 0; i < 6; i++)
						Particles.ParticleHandler.SpawnParticle(new Particles.StarParticle(Projectile.Center, Projectile.velocity.RotatedByRandom(MathHelper.Pi / 8) * Main.rand.NextFloat(0.05f, 0.2f), 
							Color.White, Color.Cyan, Main.rand.NextFloat(0.1f, 0.2f), 20));
			}

			for (int k = 0; k < 10; k++)
			{
				Dust d = Dust.NewDustPerfect(Projectile.Center + (Projectile.velocity / 10), DustID.DungeonSpirit, Main.rand.NextVector2Circular(2, 2) + (Projectile.oldVelocity / 20), Scale : Main.rand.NextFloat(0.8f, 1.8f));
				d.noGravity = true;
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(Color.White * .65f) * ((float)(Projectile.oldPos.Length - k) / Projectile.oldPos.Length);
				Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			}

			Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, Projectile.Center - Main.screenPosition, TextureAssets.Projectile[Projectile.type].Value.Bounds, Projectile.GetAlpha(Color.White * .9f), Projectile.rotation,
				TextureAssets.Projectile[Projectile.type].Value.Size() / 2, Projectile.scale, SpriteEffects.None, 0);
			return false;
		}
	}
}