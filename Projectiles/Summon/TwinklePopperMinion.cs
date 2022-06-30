using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Summon
{
	public class TwinklePopperMinion : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Twinkle Popper");
			Main.projFrames[base.Projectile.type] = 6;
		}

		public override void SetDefaults()
		{
			Projectile.width = 48;
			Projectile.height = 48;
			Projectile.sentry = true;
			Projectile.timeLeft = Projectile.SentryLifeTime;
			Projectile.friendly = false;
			Projectile.hostile = false;
			Projectile.penetrate = -1;
			Projectile.ignoreWater = true;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			return false;
		}
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			fallThrough = false;
			return true;
		}
		public override void AI()
		{
			Projectile.velocity.Y = 5;
			//CONFIG INFO
			int range = 50;   //How many tiles away the projectile targets NPCs
			int animSpeed = 2;  //how many game frames per frame :P note: firing anims are twice as fast currently
			int targetingMax = 15; //how many frames allowed to target nearest instead of shooting
			float shootVelocity = 6f; //magnitude of the shoot vector (speed of arrows shot)

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

			NPC target = (Main.npc[(int)Projectile.ai[1]] ?? new NPC()); //our target
			if (Projectile.frame < 5) {
				//do nuffin... until target in range
				if (target.active && Projectile.Distance(target.Center) / 16 < range) {
					Projectile.frameCounter++;
					//proceed if rotated in the right direction
					if (Projectile.rotation == Projectile.DirectionTo(target.position).ToRotation() && Projectile.frameCounter % 2 == 1) {
						Projectile.frame++;
						Projectile.frameCounter = 0;
					}
					//proceed if still haven't locked on (targets change too quickly, etc)
					else if (Projectile.frameCounter >= targetingMax) {
						Projectile.frame++;
						Projectile.frameCounter = 0;
					}
				}
				else
					Projectile.frameCounter = 0;
			}
			//firing
			else if (Projectile.frame == 5) {
				Projectile.frameCounter++;
				//fire!!
				if (Projectile.frameCounter % animSpeed == 0) {
					//spawn the arrow centered on the bow (this code aligns the centers :3)
					Vector2 vel = Projectile.DirectionTo(target.Center);
					Vector2 vel7 = new Vector2(-1, 0);
					vel7 *= shootVelocity;
					vel7 = vel7.RotatedBy(System.Math.PI / 13);
					for (int K = 0; K < 18; K++) {
						vel7 = vel7.RotatedBy(System.Math.PI / 13);
						int proj2 = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center.X, Projectile.Center.Y, vel7.X, vel7.Y, ProjectileID.Twinkle, Projectile.damage, 0, Projectile.owner);
						Projectile newProj2 = Main.projectile[proj2];
						newProj2.friendly = true;
						newProj2.hostile = false;
						Projectile.frame = 1;
					}

					Projectile.frame++;
				}
			}

			//finish firing anim, revert back to 0
			if (Projectile.frame == 6) {
				Projectile.frame = 1;
				Projectile.frameCounter = 0;
			}
		}

	}
}