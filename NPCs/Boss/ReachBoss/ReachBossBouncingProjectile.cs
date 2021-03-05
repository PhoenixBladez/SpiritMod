using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.ReachBoss
{   
	public class ReachBossBouncingProjectile : ModProjectile
	{
		int target;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bouncing Spore");
		}

		public override void SetDefaults()
		{
			projectile.hostile = true;
			projectile.friendly = false;
			projectile.penetrate = 5;
            projectile.aiStyle = -1;
			projectile.timeLeft = 600;
			projectile.alpha = 100;
			projectile.width = 64;
			projectile.height = 64;
		}
        bool pulseTrail;
		public override void AI()
		{		
        //	Lighting.AddLight((int)((projectile.position.X + (float)(projectile.width / 2)) / 16f), (int)((projectile.position.Y + (float)(projectile.height / 2)) / 16f), 0.201f, 0.110f, 0.226f);
			projectile.ai[1]++;
			projectile.width = 28;
			projectile.height = 28;

			if (projectile.ai[1] < 50)
            {
                projectile.tileCollide = false;
            }
            else
            {
                projectile.tileCollide = true;
            }
			if (projectile.ai[1] < 45)
			{
				projectile.velocity *= .98f;
				projectile.alpha-= 2;
			}
			else
			{
                pulseTrail = true;
				projectile.rotation += .3f;
				if (projectile.ai[1] > 60 && projectile.ai[1] < 62)
				{
					if (projectile.ai[0] == 0 && Main.netMode != NetmodeID.MultiplayerClient) {
						target = -1;
						float distance = 2000f;
						for (int k = 0; k < 255; k++) {
							if (Main.player[k].active && !Main.player[k].dead) {
								Vector2 center = Main.player[k].Center;
								float currentDistance = Vector2.Distance(center, projectile.Center);
								if (currentDistance < distance || target == -1) {
									distance = currentDistance;
									target = k;
								}
							}
						}
						if (target != -1) {
							projectile.ai[0] = 1;
							projectile.netUpdate = true;
						}
					}
					else if (target >= 0 && target < Main.maxPlayers) {
						Player targetPlayer = Main.player[target];
						Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 17);
						Vector2 direction = targetPlayer.Center - projectile.Center;
						direction.Normalize();
						direction *= 5f;
						projectile.velocity = direction;
					}
					projectile.velocity *= 1.11f;					
				}
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			projectile.penetrate--;
			if (projectile.penetrate <= 0)
				projectile.Kill();
			else {
				projectile.ai[0] += 0.1f;
				if (projectile.velocity.X != oldVelocity.X)
					projectile.velocity.X = -oldVelocity.X;

				if (projectile.velocity.Y != oldVelocity.Y)
					projectile.velocity.Y = -oldVelocity.Y;

				projectile.velocity *= 0.85f;
			}
			return false;
		}       
	}
}
