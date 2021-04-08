using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Summon
{
	public class MangoJellyMinion : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mango Jelly Minion");
			Main.projFrames[base.projectile.type] = 4;
			ProjectileID.Sets.MinionSacrificable[base.projectile.type] = true;
			ProjectileID.Sets.Homing[base.projectile.type] = true;
			ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
		}

		public override void SetDefaults()
		{
			projectile.netImportant = true;
			projectile.width = 22;
			projectile.height = 23;
			projectile.friendly = true;
			Main.projPet[projectile.type] = true;
			projectile.minion = true;
			projectile.minionSlots = 1;
			projectile.penetrate = -1;
			projectile.timeLeft = 18000;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			aiType = ProjectileID.Raven;
		}
		bool jump = false;
		int xoffset = 0;
		public override Color? GetAlpha(Color lightColor)
		{
			return new Color(250, 210, 230);
		}
		public override void AI()
		{
			float num = 1f - (float)projectile.alpha / 255f;
			num *= projectile.scale;
			float num395 = Main.mouseTextColor / 155f - 0.35f;
			num395 *= 0.34f;
			projectile.scale = num395 + 0.55f;
			bool flag64 = projectile.type == ModContent.ProjectileType<MangoJellyMinion>();
			Player player = Main.player[projectile.owner];
			if (projectile.Distance(player.Center) > 1500) {
				projectile.position = player.position + new Vector2(Main.rand.Next(-125, 126), Main.rand.Next(-125, 126));
				for (int i = 0; i < 25; i++) {
					Dust.NewDust(projectile.position, projectile.width, projectile.height, 272);
				}
			}
			MyPlayer modPlayer = player.GetSpiritPlayer();
			if (flag64) {
				if (player.dead)
					modPlayer.mangoMinion = false;

				if (modPlayer.mangoMinion)
					projectile.timeLeft = 2;
			}

			int range = 40;   //How many tiles away the projectile targets NPCs
			float lowestDist = float.MaxValue;
			if (player.HasMinionAttackTargetNPC) {
				NPC npc = Main.npc[player.MinionAttackTargetNPC];
				float dist = projectile.Distance(npc.Center);
				if (dist / 16 < range) {
					projectile.ai[1] = npc.whoAmI;
				}
			}
			else
			{
				foreach (NPC npc in Main.npc) {
					//if npc is a valid target (active, not friendly, and not a critter)
					if (npc.active && !npc.friendly && npc.catchItem == 0 && npc.CanBeChasedBy(projectile, false)) {
						//if npc is within 50 blocks
						float dist = projectile.Distance(npc.Center);
						if (dist / 16 < range) {
							//if npc is closer than closest found npc
							if (dist < lowestDist) {
								lowestDist = dist;

								//target this npc
								projectile.ai[1] = npc.whoAmI;
							}
						}
					}
				}
			}
			NPC target = (Main.npc[(int)projectile.ai[1]] ?? new NPC()); //our target
			if (target.active && !target.friendly && target.type != NPCID.TargetDummy && target.type != NPCID.DD2LanePortal) {
				if (target.position.X > projectile.position.X) {
					xoffset = Main.rand.Next(24, 28);
				}
				else {
					xoffset = Main.rand.Next(-28, -24);
				}
				projectile.ai[0]++;
				projectile.velocity.X *= 0.99f;
				if (!jump) {
					if (projectile.velocity.Y < 7.5f) {
						projectile.velocity.Y += 0.095f;
					}
					if (target.position.Y < projectile.position.Y && projectile.ai[0] % 8 == 0) {
						jump = true;
						projectile.velocity.X = xoffset / 1.75f;
						if (target.position.Y < projectile.position.Y - 150) {
							projectile.velocity.Y = -9;
						}
						else {
							projectile.velocity.Y = -1.5f;
						}
					}
					projectile.rotation = 0f;
				}
				if (jump) {
					projectile.velocity *= 0.96f;
					if (Math.Abs(projectile.velocity.X) < 0.9f) {
						jump = false;
					}
					projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
				}
			}
			else {
				if (player.position.X > projectile.position.X) {
					xoffset = Main.rand.Next(16, 24);
				}
				else {
					xoffset = Main.rand.Next(-24, -16);
				}
				projectile.ai[0]++;
				projectile.velocity.X *= 0.99f;
				if (!jump) {
					if (projectile.velocity.Y < 7.5f) {
						projectile.velocity.Y += 0.05f;
					}
					if (player.position.Y < projectile.position.Y && projectile.ai[0] % 20 == 0) {
						jump = true;
						projectile.velocity.X = xoffset / 1.25f;
						if (player.position.Y < projectile.position.Y - 150) {
							projectile.velocity.Y = -9;
						}
						else {
							projectile.velocity.Y = -1.5f;
						}
					}
					projectile.rotation = 0f;
				}
				if (jump) {
					projectile.velocity *= 0.97f;
					if (Math.Abs(projectile.velocity.X) < 0.3f) {
						jump = false;
					}
					projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
				}
			}
			projectile.frameCounter++;
			if (projectile.frameCounter >= 7) {
				projectile.frame++;
				projectile.frameCounter = 0;
				if (projectile.frame >= 4)
					projectile.frame = 0;
			}
		}

		public override bool MinionContactDamage()
		{
			return true;
		}

	}
}