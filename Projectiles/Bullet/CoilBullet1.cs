using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Bullet
{
	class CoilBullet1 : ModProjectile
	{
		NPC[] hit = new NPC[8];
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Coiled Bullet");
		}

		public override void SetDefaults()
		{
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.ranged = true;
			projectile.penetrate = 3;
			projectile.timeLeft = 300;
			projectile.aiStyle = -1;
			projectile.height = 10;
			projectile.width = 10;
			projectile.alpha = 255;
		}
		private int Mode {
			get => (int)projectile.ai[0];
			set => projectile.ai[0] = value;
		}

		private NPC Target {
			get => Main.npc[(int)projectile.ai[1]];
			set { projectile.ai[1] = value.whoAmI; }
		}

		private Vector2 Origin {
			get => new Vector2(projectile.localAI[0], projectile.localAI[1]);
			set {
				projectile.localAI[0] = value.X;
				projectile.localAI[1] = value.Y;
			}
		}

		public override void AI()
		{
			if (Mode == 0) {
				Origin = projectile.position;
				Mode = 1;
			}
			else {
				if (Mode == 2) {
					projectile.extraUpdates = 0;
					projectile.numUpdates = 0;
				}
				if (projectile.timeLeft < 300) {
					Trail(Origin, projectile.position);
				}
				Origin = projectile.position;
			}
		}

		private void Trail(Vector2 from, Vector2 to)
		{
			float distance = Vector2.Distance(from, to);
			float step = 1 / distance;
			for (float w = 0; w < distance; w += 2) {
				int d = Dust.NewDust(Vector2.Lerp(from, to, w * step), projectile.width, projectile.height, 226, 0f, 0f, 0, default, .3f * projectile.penetrate);
				Main.dust[d].noGravity = true;
				Main.dust[d].velocity = Vector2.Zero;
				Main.dust[d].scale *= .7f;
			}
		}

		private bool CanTarget(NPC target)
		{
			foreach (var npc in hit)
				if (target == npc)
					return false;
			return true;
		}

		private NPC TargetNext(NPC current)
		{
			float range = 25 * 14;
			range *= range;
			NPC target = null;
			var center = projectile.Center;
			for (int i = 0; i < 200; ++i) {
				NPC npc = Main.npc[i];
				//if npc is a valid target (active, not friendly, and not a critter)
				if (npc != current && npc.active && npc.CanBeChasedBy(null) && CanTarget(npc)) {
					float dist = Vector2.DistanceSquared(center, npc.Center);
					if (dist < range) {
						range = dist;
						target = npc;
					}
				}
			}
			return target;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			projectile.velocity = Vector2.Zero;
			hit[projectile.penetrate - 1] = target;
			projectile.damage -= 3;
			target = TargetNext(target);
			if (target != null)
				projectile.Center = target.Center;
			else
				projectile.Kill();

			projectile.netUpdate = true;
		}

	}
}
