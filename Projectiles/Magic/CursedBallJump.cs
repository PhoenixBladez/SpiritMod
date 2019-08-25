using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Magic
{
	public class CursedBallJump : ModProjectile
	{
		NPC[] hit = new NPC[3];
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Jumping Flame");
		}

		public override void SetDefaults()
		{
			projectile.hostile = false;
			projectile.melee = true;
			projectile.width = 12;
			projectile.height = 12;
			projectile.aiStyle = -1;
			projectile.friendly = true;
			projectile.penetrate = 3;
			projectile.alpha = 255;
			projectile.timeLeft = 180;
			projectile.tileCollide = true;
		}
		
		

		private int Mode
		{
			get { return (int)projectile.ai[0]; }
			set { projectile.ai[0] = value; }
		}

		private NPC Target
		{
			get { return Main.npc[(int)projectile.ai[1]]; }
			set { projectile.ai[1] = value.whoAmI; }
		}

		private Vector2 Origin
		{
			get { return new Vector2(projectile.localAI[0], projectile.localAI[1]); }
			set
			{
				projectile.localAI[0] = value.X;
				projectile.localAI[1] = value.Y;
			}
		}

		public override void AI()
		{
			if (Mode == 0)
			{
				Origin = projectile.position;
				Mode = 1;
			}
			else
			{
				if (Mode == 2)
				{
					projectile.extraUpdates = 0;
					projectile.numUpdates = 0;
				}
				Trail(Origin, projectile.position);
				Origin = projectile.position;
			}
		}

		private void Trail(Vector2 from, Vector2 to)
		{
			float distance = Vector2.Distance(from, to);
			float step = 1 / distance;
			for (float w = 0; w < distance; w+= 6)
			{
				Dust.NewDustPerfect(Vector2.Lerp(from, to, w * step), 75, Vector2.Zero).noGravity = true;
                Dust.NewDustPerfect(Vector2.Lerp(from, to, w * step), 75, Vector2.Zero).scale *= 1.2f;
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
			var center = projectile.Center;
			for (int i = 0; i < 200; ++i)
			{
				NPC npc = Main.npc[i];
				//if npc is a valid target (active, not friendly, and not a critter)
				if (npc != current && npc.active && npc.CanBeChasedBy(null) && CanTarget(npc))
				{
					float dist = Vector2.DistanceSquared(center , npc.Center);
					if (dist < range)
					{
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

			target = TargetNext(target);
			if (target != null)
				projectile.Center = target.Center;
			else
				projectile.Kill();
		}
	}
}