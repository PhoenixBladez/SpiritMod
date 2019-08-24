using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	public class StoneSpirit : ModProjectile
	{
		int timer = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spirit");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 9;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			projectile.width = 6;
			projectile.height = 6;
			projectile.friendly = true;
			projectile.penetrate = 1;
			projectile.tileCollide = false;
			projectile.alpha = 255;
			projectile.timeLeft = 500;
			projectile.light = 0;
			projectile.extraUpdates = 1;
		}

		Vector2 offset = new Vector2(40, 40);
		public override void AI()
		{
			Player player = Main.player[projectile.owner];
			if (player.GetModPlayer<MyPlayer>(mod).SoulStone == true && player.active && !player.dead)
			{
				projectile.timeLeft = 2;
			}
			timer++;
			int range = 15;   //How many tiles away the projectile targets NPCs
			int animSpeed = 2;  //how many game frames per frame :P note: firing anims are twice as fast currently
			int targetingMax = 15; //how many frames allowed to target nearest instead of shooting
			float shootVelocity = 2f; //magnitude of the shoot vector (speed of arrows shot)

			//TARGET NEAREST NPC WITHIN RANGE
			float lowestDist = float.MaxValue;
			foreach (NPC npc in Main.npc)
			{
				//if npc is a valid target (active, not friendly, and not a critter)
				if (npc.active && !npc.friendly && npc.catchItem == 0)
				{
					//if npc is within 50 blocks
					float dist = projectile.Distance(npc.Center);
					if (dist / 16 < range)
					{
						//if npc is closer than closest found npc
						if (dist < lowestDist)
						{
							lowestDist = dist;

							//target this npc
							projectile.ai[1] = npc.whoAmI;
						}
					}
				}
			}

			int dust = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 187, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
			int dust2 = Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 187, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
			Main.dust[dust].noGravity = true;
			Main.dust[dust2].noGravity = true;
			Main.dust[dust].velocity *= 0f;
			Main.dust[dust2].velocity *= 0f;
			Main.dust[dust2].scale = 1.2f;
			Main.dust[dust].scale = 1.2f;
			projectile.rotation = projectile.velocity.ToRotation() + (float)(Math.PI / 2);

			NPC target = (Main.npc[(int)projectile.ai[1]] ?? new NPC()); //our target
			if (target.active && projectile.Distance(target.Center) / 16 < range && timer > 100)
			{
				Vector2 direction = target.Center - projectile.Center;
				direction.Normalize();
				direction *= 10f;
				projectile.velocity = direction;
			}
			else
			{
				var list = Main.projectile.Where(x => x.Hitbox.Intersects(projectile.Hitbox));
				foreach (var proj in list)
				{
					if (projectile != proj && proj.hostile)
						proj.Kill();
					{

						projectile.ai[0] += .02f;
						projectile.Center = player.Center + offset.RotatedBy(projectile.ai[0] + projectile.ai[1] * (Math.PI * 10 / 1));
					}


				}
			}
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 5; i++)
			{
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 187);
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