using Microsoft.Xna.Framework;
using SpiritMod.Mechanics.Trails;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class AdamantiteStaffProj : ModProjectile, ITrailProjectile
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Adamantite Blast");

		public override void SetDefaults()
		{
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.alpha = 255;
			Projectile.timeLeft = 50;
		}

		public void DoTrailCreation(TrailManager tManager)
		{
			float trailwidth = 25;
			float traillength = 300;
			tManager.CreateTrail(Projectile, new StandardColorTrail(new Color(252, 3, 57) * 0.9f), new RoundCap(), new DefaultTrailPosition(), trailwidth / 2, traillength * 0.8f);
			tManager.CreateTrail(Projectile, new StandardColorTrail(new Color(255, 255, 255)), new RoundCap(), new DefaultTrailPosition(), trailwidth / 3, traillength * 0.75f);
			tManager.CreateTrail(Projectile, new GradientTrail(new Color(252, 3, 57) * 0.75f, new Color(255, 201, 213) * 0.75f), new RoundCap(), new DefaultTrailPosition(), trailwidth, traillength);
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) => Projectile.Kill();

		public override bool PreAI()
		{
			Projectile.rotation = Projectile.velocity.ToRotation() + 1.57f;
			return true;
		}

		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item110, Projectile.Center);
			float maxprojs = 8;
			for (int i = 0; i < 8; i++) {
				if (i != 3 && i != 7) {
					Vector2 BaseSpeed = new Vector2(0, 7.5f);
					BaseSpeed = BaseSpeed.RotatedBy(i * MathHelper.TwoPi / maxprojs);
					Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center, BaseSpeed, ModContent.ProjectileType<AdamantiteStaffProj2>(), Projectile.damage, 0f, Projectile.owner, 0f, 0f);
				}
			}
		}
	}
}
