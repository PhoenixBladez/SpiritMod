using Microsoft.Xna.Framework;
using System.Linq;
using Terraria;
using Terraria.Audio;
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
			Main.projFrames[Projectile.type] = 2;
		}

		public override void SetDefaults()
		{
			Projectile.hostile = true;
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.timeLeft = 1000;
			;
			Projectile.friendly = false;
			Projectile.penetrate = 1;
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			Projectile.Kill();
		}

		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
			for (int k = 0; k < 15; k++) {
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Torch);
				Main.dust[dust].scale = 2f;
				Main.dust[dust].noGravity = true;
			}
            Gore.NewGore(Projectile.position, Projectile.velocity, Mod.Find<ModGore>("Gores/Mech7").Type, 1f);
            Gore.NewGore(Projectile.position, Projectile.velocity, Mod.Find<ModGore>("Gores/Mech8").Type, 1f);
        }
		public override bool PreAI()
		{
			var list = Main.projectile.Where(x => x.Hitbox.Intersects(Projectile.Hitbox));
			foreach (var proj in list) {
				if (Projectile != proj && proj.friendly) {
					Projectile.Kill();
				}
			}
			Projectile.frameCounter++;
			if (Projectile.frameCounter >= 4) {
				Projectile.frame++;
				Projectile.frameCounter = 0;
				if (Projectile.frame >= 2)
					Projectile.frame = 0;
			}
			return true;
		}
		public override void AI()
		{
			Vector2 vector207 = new Vector2((float)Projectile.width * 2, (float)Projectile.height * 2) * Projectile.scale * 0.85f;
			vector207 /= 2f;
			Vector2 value112 = Vector2.UnitY.RotatedByRandom(6.2831854820251465) * vector207;
			Vector2 position178 = Projectile.Center + value112;
			int num1442 = Dust.NewDust(position178, 0, 0, DustID.Electric, 0f, 0f, 0, default, .5f);
			Main.dust[num1442].position = Projectile.Center + value112;
			Main.dust[num1442].velocity = Vector2.Zero;
			Main.dust[num1442].noGravity = true;

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

				Projectile.velocity.X = moveSpeed * 0.08f;
				Projectile.velocity.Y = 1f;
			}
		}
	}
}