using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	public class MechBat : ModProjectile
	{
		int moveSpeed = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mech Bat");
		}

		public override void SetDefaults()
		{
			projectile.hostile = true;
			projectile.width = 20;
			projectile.height = 20;
			projectile.timeLeft = 1000;
			projectile.light = 0.5f;
			;
			projectile.friendly = false;
			projectile.penetrate = 1;
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			projectile.Kill();
		}

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 14);
			for (int k = 0; k < 15; k++)
			{
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 6);
				Main.dust[dust].scale = 2f;
				Main.dust[dust].noGravity = true;
			}
		}

		public override void AI()
		{
			int range = 650;   //How many tiles away the projectile targets NPCs
							   //int targetingMax = 20; //how many frames allowed to target nearest instead of shooting
							   //float shootVelocity = 16f; //magnitude of the shoot vector (speed of arrows shot)

			//TARGET NEAREST NPC WITHIN RANGE
			float lowestDist = float.MaxValue;
			foreach (Player player in Main.player)
			{
				//if npc is a valid target (active, not friendly, and not a critter)
				if (player.active)
				{
					//if npc is within 50 blocks
					float dist = projectile.Distance(player.Center);
					if (dist / 16 < range)
					{
						//if npc is closer than closest found npc
						if (dist < lowestDist)
						{
							lowestDist = dist;

							//target this npc
							projectile.ai[1] = player.whoAmI;
						}
					}
				}
			}

			Player target = (Main.player[(int)projectile.ai[1]] ?? new Player());
			if (target.active && projectile.Distance(target.Center) / 16 < range && projectile.timeLeft < 945)
			{
				if (projectile.Center.X >= target.Center.X && moveSpeed >= -30) // flies to players x position
				{
					moveSpeed--;
				}

				if (projectile.Center.X <= target.Center.X && moveSpeed <= 30)
				{
					moveSpeed++;
				}

				projectile.velocity.X = moveSpeed * 0.1f;
				projectile.velocity.Y = 1;
			}
		}

		//public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		//{
		//    Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
		//    for (int k = 0; k < projectile.oldPos.Length; k++)
		//    {
		//        Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
		//        Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
		//        spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
		//    }
		//    return true;
		//}
	}
}