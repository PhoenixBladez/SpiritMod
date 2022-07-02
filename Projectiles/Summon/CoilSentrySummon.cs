using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Projectiles.Bullet;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Summon
{
	public class CoilSentrySummon : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Electric Turret");
		}

		public override void SetDefaults()
		{
			Projectile.width = 32;
			Projectile.height = 28;
			Projectile.timeLeft = Projectile.SentryLifeTime;
			Projectile.friendly = false;
			Projectile.hostile = false;
			Projectile.penetrate = -1;
			Projectile.sentry = true;
			Projectile.ignoreWater = true;
			Projectile.sentry = true;
		}
		float alphaCounter = 0;
		public override bool OnTileCollide(Vector2 oldVelocity) => false;
		public override void PostDraw(Color lightColor)
		{
			float sineAdd = (float)Math.Sin(alphaCounter) + 3;
			Main.spriteBatch.Draw(Terraria.GameContent.TextureAssets.Extra[49].Value, (Projectile.Center - Main.screenPosition) - new Vector2(-2, 8), null, new Color((int)(7.5f * sineAdd), (int)(16.5f * sineAdd), (int)(18f * sineAdd), 0), 0f, new Vector2(50, 50), 0.25f * (sineAdd + 1), SpriteEffects.None, 0f);
		}
		public override void AI()
		{
			alphaCounter += 0.04f;
			Projectile.velocity.Y = 5;
			//CONFIG INFO
			int range = 15;   //How many tiles away the projectile targets NPCs
			float shootVelocity = 20f; //magnitude of the shoot vector (speed of arrows shot)

			//TARGET NEAREST NPC WITHIN RANGE
			float lowestDist = float.MaxValue;
			for (int i = 0; i < 200; ++i) {
				NPC npc = Main.npc[i];
				//if npc is a valid target (active, not friendly, and not a critter)
				if (npc.active && npc.CanBeChasedBy(Projectile) && !npc.friendly) {
					//if npc is within 50 blocks
					float dist = Projectile.Distance(npc.Center);
					if (dist / 16 < range) {
						//if npc is closer than closest found npc
						if (dist < lowestDist) {
							lowestDist = dist;

							//target this npc
							Projectile.ai[1] = npc.whoAmI;
							Projectile.netUpdate = true;
						}
					}
				}
			}

			NPC target = (Main.npc[(int)Projectile.ai[1]] ?? new NPC()); //our target
																		 //firing
			Projectile.ai[0]++;
			if (Projectile.ai[0] >= 30 && target.active && !target.friendly && Projectile.Distance(target.Center) / 16 < range) {
				Projectile.ai[0] = 0;
				Vector2 ShootArea = new Vector2(Projectile.Center.X, Projectile.Center.Y - 13);
				Vector2 direction = Vector2.Normalize(target.Center - ShootArea) * shootVelocity;
				if(Main.netMode != NetmodeID.MultiplayerClient) {
					int proj2 = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center.X, Projectile.Center.Y - 13, direction.X, direction.Y, ModContent.ProjectileType<CoilBullet1>(), Projectile.damage, 0, Main.myPlayer);
					Main.projectile[proj2].DamageType = DamageClass.Summon;
				}
				
				SoundEngine.PlaySound(SoundID.Item, Projectile.Center, 12);  //make bow shooty sound
			}
			Vector2 globePos = new Vector2(Projectile.Center.X + 2, Projectile.position.Y + 6);
			if (Main.rand.Next(11) == 1) {
				Vector2 vector2 = Vector2.UnitY.RotatedByRandom(6.28318548202515) * new Vector2(8f, 8f) * Projectile.scale * 1.45f / 2f;
				int index = Dust.NewDust(globePos + vector2, 0, 0, DustID.Electric, 0.0f, 0.0f, 0, new Color(), .8f);
				Main.dust[index].position = globePos + vector2;
				Main.dust[index].velocity = new Vector2(0, -1);
				Main.dust[index].noGravity = true;
				Main.dust[index].noLight = true;
			}
			//old dust effect incase you don't like this
			/*for (int i = 0; i < 2; i++)
            {
                Vector2 vector2 = Vector2.UnitY.RotatedByRandom(6.28318548202515) * new Vector2((float)projectile.height, (float)projectile.height) * projectile.scale * 1.45f / 2f;
                int index = Dust.NewDust(projectile.Center + vector2, 0, 0, 226, 0.0f, 0.0f, 0, new Color(), .8f);
                Main.dust[index].position = projectile.Center + vector2;
                Main.dust[index].velocity = Vector2.Zero;
                Main.dust[index].noGravity = true;
            }*/
		}
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			fallThrough = false;
			return true;
		}
		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
			for (int i = 0; i < 40; i++) {
				int num = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0f, -2f, 0, default, 1.5f);
				Main.dust[num].noGravity = true;
				Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
				Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
				if (Main.dust[num].position != Projectile.Center) {
					Main.dust[num].velocity = Projectile.DirectionTo(Main.dust[num].position) * 6f;
				}
			}
		}
	}
}