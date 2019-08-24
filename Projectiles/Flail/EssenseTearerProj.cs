using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Flail
{
	public class EssenseTearerProj : ModProjectile
	{
		int timer;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Essence Tearer");
		}

		public override void SetDefaults()
		{
			projectile.width = projectile.height = 22;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.melee = true;
		}

		public override bool PreAI()
		{
			ProjectileExtras.FlailAI(projectile.whoAmI);
			timer++;
			if (timer >= 15)
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
				direction.X *= 14f;
				direction.Y *= 14f;
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, direction.X, direction.Y, mod.ProjectileType("SpiritShardFriendly"), projectile.damage, 1, projectile.owner, 0, 0);
				timer = 0;
			}

			return false;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			return ProjectileExtras.FlailTileCollide(projectile.whoAmI, oldVelocity);
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			ProjectileExtras.DrawChain(projectile.whoAmI, Main.player[projectile.owner].MountedCenter,
				"SpiritMod/Projectiles/Flail/EssenseTearerChain");
			ProjectileExtras.DrawAroundOrigin(projectile.whoAmI, lightColor);
			return false;
		}

	}
}
