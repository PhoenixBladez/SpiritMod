
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	public class QuicksilverBolt : ModProjectile
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Quicksilver Droplet");
		}

		public override void SetDefaults()
		{
			Projectile.width = 2;
			Projectile.height = 2;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.tileCollide = false;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 210;
			Projectile.extraUpdates = 0;
			Projectile.ignoreWater = true;
			Projectile.alpha = 255;
			Projectile.aiStyle = -1;
		}

		public override void AI()
		{
			if (Projectile.ai[1] == 0)
				Projectile.ai[0] = -1;

			Projectile.ai[1] += 1f;
			bool chasing = false;
			if (Projectile.ai[1] >= 30f) {
				chasing = true;

				Projectile.friendly = true;
				NPC target = null;
				if (Projectile.ai[0] < 0 || Projectile.ai[0] >= Main.maxNPCs) {
					target = ProjectileExtras.FindRandomNPC(Projectile.Center, 960f, false);
				}
				else {
					target = Main.npc[(int)Projectile.ai[0]];
					if (!target.active || !target.CanBeChasedBy())
						target = ProjectileExtras.FindRandomNPC(Projectile.Center, 960f, false);
				}

				if (target == null) {
					chasing = false;
					Projectile.ai[0] = -1f;
				}
				else {
					Projectile.ai[0] = (float)target.whoAmI;
					ProjectileExtras.HomingAI(this, target, 10f, .5f);
				}
			}

			ProjectileExtras.LookAlongVelocity(this);
			if (!chasing) {
				Vector2 dir = Projectile.velocity;
				float vel = Projectile.velocity.Length();
				if (vel != 0f) {
					if (vel < 8f) {
						dir *= 1 / vel;
						Projectile.velocity += dir * 0.0625f;
					}
				}
				else {
					//Stops the projectile from spazzing out
					Projectile.velocity.X += Main.rand.Next(2) == 0 ? 0.1f : -0.1f;
				}
			}

			for (int i = 0; i < 10; i++) {
				Vector2 pos = Projectile.Center - Projectile.velocity * ((float)i / 10f);
				int num = Dust.NewDust(pos, 2, 2, DustID.SilverCoin);
				Main.dust[num].alpha = Projectile.alpha;
				//Main.dust[num].position = pos;
				Main.dust[num].velocity = Vector2.Zero;
				Main.dust[num].noGravity = true;
			}
		}

		public override bool? CanHitNPC(NPC target)
		{
			return Projectile.ai[1] < 30 ? false : (bool?)null;
		}

	}
}
