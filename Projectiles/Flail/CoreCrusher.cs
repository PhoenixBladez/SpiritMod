using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Flail
{
	public class CoreCrusher : ModProjectile
	{
		int timer;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Core Crusher");
		}

		public override void SetDefaults()
		{
			projectile.width = 28;
			projectile.height = 12;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.melee = true;
			projectile.aiStyle = 15;
		}


		public override bool PreAI()
		{
			ProjectileExtras.FlailAI(projectile.whoAmI);
			timer++;
			if (timer >= 60)
			{
				float lowestDist = float.MaxValue;
				for (int i = 0; i < 200; ++i)
				{
					NPC npc = Main.npc[i];
					//if npc is a valid target (active, not friendly, and not a critter)
					if (npc.active && npc.CanBeChasedBy(projectile))
					{
						//if npc is within 50 blocks
						float dist = projectile.Distance(npc.Center);
						//if npc is closer than closest found npc
						if (dist < lowestDist)
						{
							lowestDist = dist;

							//target this npc
							projectile.ai[1] = npc.whoAmI;
						}
					}
				}

				NPC target = (Main.npc[(int)projectile.ai[1]] ?? new NPC());
				Vector2 direction = target.Center - projectile.Center;
				direction.Normalize();
				direction.X *= 8f;
				direction.Y *= 8f;
				int proj2 = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, direction.X, direction.Y, 85, projectile.damage, 1, projectile.owner, 0, 0);
				Projectile newProj2 = Main.projectile[proj2];
				newProj2.friendly = true;
				newProj2.hostile = false;
				newProj2.timeLeft = 30;
				timer = 0;
			}
			return true;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			return ProjectileExtras.FlailTileCollide(projectile.whoAmI, oldVelocity);
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(24, 240);
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			ProjectileExtras.DrawChain(projectile.whoAmI, Main.player[projectile.owner].MountedCenter,
				"SpiritMod/Projectiles/Flail/CoreCrusher_Chain");
			ProjectileExtras.DrawAroundOrigin(projectile.whoAmI, lightColor);
			return false;
		}

	}
}