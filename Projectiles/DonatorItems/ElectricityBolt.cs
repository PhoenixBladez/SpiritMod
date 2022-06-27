using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.DonatorItems
{
	public class ElectricityBolt : ModProjectile
	{
		NPC[] hit = new NPC[8];
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Electricity Bolt");
		}

		public override void SetDefaults()
		{
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.penetrate = 8;
			Projectile.alpha = 255;
			Projectile.timeLeft = 60;
			Projectile.tileCollide = false;
			Projectile.extraUpdates = 7;
		}



		private int Mode {
			get { return (int)Projectile.ai[0]; }
			set { Projectile.ai[0] = value; }
		}

		private NPC Target {
			get { return Main.npc[(int)Projectile.ai[1]]; }
			set { Projectile.ai[1] = value.whoAmI; }
		}

		private Vector2 Origin {
			get { return new Vector2(Projectile.localAI[0], Projectile.localAI[1]); }
			set {
				Projectile.localAI[0] = value.X;
				Projectile.localAI[1] = value.Y;
			}
		}

		public override void AI()
		{
			if (Mode == 0) {
				Origin = Projectile.position;
				Mode = 1;
			}
			else {
				if (Mode == 2) {
					Projectile.extraUpdates = 0;
					Projectile.numUpdates = 0;
				}
				Trail(Origin, Projectile.position);
				Origin = Projectile.position;
			}
		}

		private void Trail(Vector2 from, Vector2 to)
		{
			float distance = Vector2.Distance(from, to);
			float step = 1 / distance;
			for (float w = 0; w < distance; w += 4) {
				Dust.NewDustPerfect(Vector2.Lerp(from, to, w * step), 226, Vector2.Zero).noGravity = true;
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
			float range = 16 * 14;
			range *= range;
			NPC target = null;
			var center = Projectile.Center;
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
			Projectile.velocity = Vector2.Zero;
			hit[Projectile.penetrate - 1] = target;

			target = TargetNext(target);
			if (target != null)
				Projectile.Center = target.Center;
			else
				Projectile.Kill();
		}
	}
}