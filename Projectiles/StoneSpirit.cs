using Microsoft.Xna.Framework;
using System;
using System.Linq;
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
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 9;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			Projectile.width = 6;
			Projectile.height = 6;
			Projectile.friendly = true;
			Projectile.penetrate = 1;
			Projectile.tileCollide = false;
			Projectile.alpha = 255;
			Projectile.timeLeft = 500;
			Projectile.light = 0;
			Projectile.extraUpdates = 1;
		}

		Vector2 offset = new Vector2(40, 40);
		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			if (player.GetSpiritPlayer().SoulStone == true && player.active && !player.dead) {
				Projectile.timeLeft = 2;
			}
			timer++;
			int range = 15;   //How many tiles away the projectile targets NPCs
							  //int animSpeed = 2;  //how many game frames per frame :P note: firing anims are twice as fast currently
							  //int targetingMax = 15; //how many frames allowed to target nearest instead of shooting
							  //float shootVelocity = 2f; //magnitude of the shoot vector (speed of arrows shot)

			//TARGET NEAREST NPC WITHIN RANGE
			float lowestDist = float.MaxValue;
			foreach (NPC npc in Main.npc) {
				//if npc is a valid target (active, not friendly, and not a critter)
				if (npc.active && !npc.friendly && npc.catchItem == 0) {
					//if npc is within 50 blocks
					float dist = Projectile.Distance(npc.Center);
					if (dist / 16 < range) {
						//if npc is closer than closest found npc
						if (dist < lowestDist) {
							lowestDist = dist;

							//target this npc
							Projectile.ai[1] = npc.whoAmI;
						}
					}
				}
			}

			int dust = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.Flare_Blue, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
			int dust2 = Dust.NewDust(Projectile.position + Projectile.velocity, Projectile.width, Projectile.height, DustID.Flare_Blue, Projectile.velocity.X * 0.5f, Projectile.velocity.Y * 0.5f);
			Main.dust[dust].noGravity = true;
			Main.dust[dust2].noGravity = true;
			Main.dust[dust].velocity *= 0f;
			Main.dust[dust2].velocity *= 0f;
			Main.dust[dust2].scale = 1.2f;
			Main.dust[dust].scale = 1.2f;
			Projectile.rotation = Projectile.velocity.ToRotation() + (float)(Math.PI / 2);

			NPC target = (Main.npc[(int)Projectile.ai[1]] ?? new NPC()); //our target
			if (target.active && !target.friendly && Projectile.Distance(target.Center) / 16 < range && timer > 100) {
				Vector2 direction = target.Center - Projectile.Center;
				direction.Normalize();
				direction *= 10f;
				Projectile.velocity = direction;
			}
			else {
				var list = Main.projectile.Where(x => x.Hitbox.Intersects(Projectile.Hitbox));
				foreach (var proj in list) {
					if (Projectile != proj && proj.hostile)
						proj.Kill();
					{

						Projectile.ai[0] += .02f;
						Projectile.Center = player.Center + offset.RotatedBy(Projectile.ai[0] + Projectile.ai[1] * (Math.PI * 10 / 1));
					}


				}
			}
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 5; i++) {
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Flare_Blue);
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