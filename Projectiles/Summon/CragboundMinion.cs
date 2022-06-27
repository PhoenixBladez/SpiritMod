using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Summon
{
	public class CragboundMinion : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cragbound");
			Main.projFrames[base.Projectile.type] = 8;
		}

		public override void SetDefaults()
		{
			Projectile.width = 100;
			Projectile.height = 86;
			Projectile.timeLeft = 3000;
			Projectile.friendly = false;
			Projectile.hostile = false;
			Projectile.penetrate = -1;
			Projectile.ignoreWater = true;
			Projectile.minion = true;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			return false;
		}

		public override void AI()
		{
			bool flag64 = Projectile.type == ModContent.ProjectileType<CragboundMinion>();
			Player player = Main.player[Projectile.owner];
			MyPlayer modPlayer = player.GetSpiritPlayer();
			if (flag64) {
				if (player.dead)
					modPlayer.cragboundMinion = false;

				if (modPlayer.cragboundMinion)
					Projectile.timeLeft = 2;

			}

			Projectile.frameCounter++;
			if (Projectile.frameCounter >= 8) {
				Projectile.frame++;
				Projectile.frameCounter = 0;
				if (Projectile.frame >= 8)
					Projectile.frame = 0;

			}

			Projectile.velocity.Y = 5;
			//CONFIG INFO
			int range = 80;   //How many tiles away the projectile targets NPCs

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

			Projectile.ai[0]++;
			if (Projectile.ai[0] >= 60) {
				if (Projectile.ai[0] % 5 == 0) {
					for (int i = 0; i < 200; ++i) {
						if (Main.npc[i].active && (Projectile.position - Main.npc[i].position).Length() < 180 && Main.npc[i].CanBeChasedBy(Projectile)) {
							Projectile.direction = Main.npc[i].position.X < Projectile.position.X ? -1 : 1;

							Vector2 position = new Vector2(Projectile.position.X + Projectile.width * 0.5f + Main.rand.Next(201) * -Projectile.direction + (Main.npc[i].position.X - Projectile.position.X), Projectile.Center.Y - 600f);
							position.X = (position.X * 10f + Projectile.position.X) / 11f + (float)Main.rand.Next(-100, 101);
							position.Y -= 150;
							float speedX = (float)Main.npc[i].position.X - position.X;
							float speedY = (float)Main.npc[i].position.Y - position.Y;
							if (speedY < 0f)
								speedY *= -1f;
							if (speedY < 20f)
								speedY = 20f;

							float length = (float)Math.Sqrt((double)(speedX * speedX + speedY * speedY));
							length = 12 / length;
							speedX *= length;
							speedY *= length;
							speedX = speedX + (float)Main.rand.Next(-40, 41) * 0.03f;
							speedY = speedY + (float)Main.rand.Next(-40, 41) * 0.03f;
							speedX *= (float)Main.rand.Next(75, 150) * 0.01f;
							position.X += (float)Main.rand.Next(-50, 51);
							Projectile.NewProjectile(position.X, position.Y, speedX, speedY, ModContent.ProjectileType<PrismaticBolt>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
							break;
						}
					}
				}

				if (Projectile.ai[0] >= 90)
					Projectile.ai[0] = 0;

			}
		}
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            fallThrough = false;
            return true;
        }
    }
}
