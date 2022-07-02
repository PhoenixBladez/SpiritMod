using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.DonatorItems
{
	public class NeutronStar : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Neutron Star");
			Main.projFrames[base.Projectile.type] = 10;
		}

		public override void SetDefaults()
		{
			Projectile.width = 80;
			Projectile.height = 112;
			Projectile.timeLeft = 3000;
			Projectile.friendly = false;
			Projectile.hostile = false;
			Projectile.penetrate = -1;
			Projectile.ignoreWater = true;
			Projectile.minion = true;
			Projectile.minionSlots = 2;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			return false;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

		public override void AI()
		{
			//projectile.velocity.Y = 5;
			//CONFIG INFO
			int range = 25;   //How many tiles away the projectile targets NPCs
			int animSpeed = -4;  //how many game frames per frame :P note: firing anims are twice as fast currently
			float shootVelocity = 7f; //magnitude of the shoot vector (speed of arrows shot)

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
			if (Projectile.frame < 9) {
				//do nuffin... until target in range
				if (target.active && Projectile.Distance(target.Center) / 16 < range) {
					Projectile.frameCounter++;
					//proceed if rotated in the right direction
					if ((float)Projectile.frameCounter >= 6f) {
						Projectile.frame++;
						Projectile.frameCounter = 0;
					}
					//proceed if rotated in the right direction

				}
				else {
					Projectile.frameCounter++;
					if ((float)Projectile.frameCounter >= 6f) {
						Projectile.frame = ((Projectile.frame + 1) % 5) + 2;
						Projectile.frameCounter = 0;
					}
				}
			}
			//firing
			else if (Projectile.frame == 9) {
				Projectile.frameCounter++;
				//fire!!
				if (Projectile.frameCounter % animSpeed == 0) {
					//spawn the arrow centered on the bow (this code aligns the centers :3)
					Vector2 ShootArea = new Vector2(Projectile.Center.X, Projectile.Center.Y);
					Vector2 direction = target.Center - ShootArea;
					direction.Normalize();
					direction.X *= shootVelocity;
					direction.Y *= shootVelocity;
					int proj2 = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center.X, Projectile.Center.Y, direction.X, direction.Y, ProjectileID.VortexLightning, Projectile.damage, .5f, Projectile.owner, direction.ToRotation(), Main.rand.Next(100));
					Projectile newProj2 = Main.projectile[proj2];
					newProj2.friendly = true;
					newProj2.hostile = false;
					Projectile.frame = 1;

					SoundEngine.PlaySound(new SoundStyle("SpiritMod/Sounds/Thunder"), Projectile.Center);

					Projectile.frame++;
				}
			}

			//finish firing anim, revert back to 0
			if (Projectile.frame == 10) {
				Projectile.frame = 1;
				Projectile.frameCounter = 0;
			}
		}

	}
}