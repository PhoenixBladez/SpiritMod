using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;


namespace SpiritMod.NPCs.SporeWheezer
{
	public class WheezerSporeHostile : ModProjectile
	{
		int moveSpeed = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spore");
		}

		public override void SetDefaults()
		{
			Projectile.hostile = true;
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.timeLeft = 1000;
			;
			Projectile.tileCollide = false;
			Projectile.friendly = false;
			Projectile.penetrate = 1;
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			Projectile.Kill();
		}

		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.NPCKilled, (int)Projectile.position.X, (int)Projectile.position.Y, 1);
			for (int k = 0; k < 15; k++) {
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Harpy);
				Main.dust[dust].scale = .61f;
				Main.dust[dust].noGravity = true;
			}
		}
		public override void AI()
		{
			var list = Main.projectile.Where(x => x.Hitbox.Intersects(Projectile.Hitbox));
			foreach (var proj in list) {
				if (Projectile != proj && proj.friendly) {
					Projectile.Kill();
				}
			}
			int range = 650;   //How many tiles away the projectile targets NPCs
							   //int targetingMax = 20; //how many frames allowed to target nearest instead of shooting
							   //float shootVelocity = 16f; //magnitude of the shoot vector (speed of arrows shot)

			//TARGET NEAREST NPC WITHIN RANGE
			float lowestDist = float.MaxValue;
			foreach (Player player in Main.player) {
				//if npc is a valid target (active, not friendly, and not a critter)
				if (player.active) {
					//if npc is within 50 blocks
					float dist = Projectile.Distance(player.Center);
					if (dist / 16 < range) {
						//if npc is closer than closest found npc
						if (dist < lowestDist) {
							lowestDist = dist;

							//target this npc
							Projectile.ai[1] = player.whoAmI;
						}
					}
				}
			}

			Player target = (Main.player[(int)Projectile.ai[1]] ?? new Player());
			if (target.active && Projectile.Distance(target.Center) / 16 < range && Projectile.timeLeft < 945) {
				if (Projectile.Center.X >= target.Center.X && moveSpeed >= -30) // flies to players x position
				{
					moveSpeed--;
				}

				if (Projectile.Center.X <= target.Center.X && moveSpeed <= 30) {
					moveSpeed++;
				}

				Projectile.velocity.X = moveSpeed * 0.1f;
				Projectile.velocity.Y = 1.4f;
			}
		}
	}
}