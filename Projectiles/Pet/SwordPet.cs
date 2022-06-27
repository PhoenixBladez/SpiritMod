using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Projectiles.Pet
{
	public class SwordPet : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Haunted Tome");
			Main.projFrames[Projectile.type] = 10;
			Main.projPet[Projectile.type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.ZephyrFish);
			AIType = ProjectileID.ZephyrFish;
			Projectile.width = 40;
			Projectile.height = 46;
		}

		public override bool PreAI()
		{
			Player player = Main.player[Projectile.owner];
			player.zephyrfish = false; // Relic from aiType
			Projectile.spriteDirection = 0;
			return true;
		}

		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			MyPlayer modPlayer = player.GetSpiritPlayer();
			if (player.dead)
				modPlayer.SwordPet = false;

			if (modPlayer.SwordPet)
				Projectile.timeLeft = 2;


			int range = 10000;   //How many tiles away the projectile targets NPCs

			//TARGET NEAREST NPC WITHIN RANGE
			float lowestDist = float.MaxValue;
			for (int i = 0; i < 200; ++i) {
				NPC npc = Main.npc[i];
				//if npc is a valid target (active, not friendly, and not a critter)
				if (npc.active && npc.CanBeChasedBy(Projectile)) {
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
																		 //firing
			Projectile.ai[0]++;
			if (target.active) {
				Vector2 ShootArea = new Vector2(Projectile.Center.X, Projectile.Center.Y);
				Vector2 direction = target.Center - ShootArea;
				direction.Normalize();
				Projectile.rotation = direction.ToRotation() + 0.78f;
				Projectile.spriteDirection = 0;
			}
		}

	}
}