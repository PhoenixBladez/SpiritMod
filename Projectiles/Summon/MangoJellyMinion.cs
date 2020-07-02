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
		public override void AI()
		{
			bool flag64 = projectile.type == ModContent.ProjectileType<MangoJellyMinion>();
			Player player = Main.player[projectile.owner];
			if (projectile.Distance(player.Center) > 1500)
			{
				projectile.position = player.position;
				for (int i = 0; i < 25; i++)
				{
					Dust.NewDust(projectile.position, projectile.width, projectile.height, 272);
				}
			}
			MyPlayer modPlayer = player.GetSpiritPlayer();
			if(flag64) {
				if(player.dead)
					modPlayer.mangoMinion = false;

				if(modPlayer.mangoMinion)
					projectile.timeLeft = 2;
			}

			int range = 50;   //How many tiles away the projectile targets NPCs

			float lowestDist = float.MaxValue;
			for(int i = 0; i < 200; ++i) {
				NPC npc = Main.npc[i];
				//if npc is a valid target (active, not friendly, and not a critter)
				float dist = projectile.Distance(npc.Center);
				if(dist / 16 < range) {
					if(npc.active && npc.CanBeChasedBy(projectile) && !npc.friendly) {
						//if npc is closer than closest found npc
						if(dist < lowestDist) {
							lowestDist = dist;

							//target this npc
							projectile.ai[1] = npc.whoAmI;
						}
					}
				}
			}
			NPC target = (Main.npc[(int)projectile.ai[1]] ?? new NPC()); //our target
																		 //firing
			if(target.active && !target.friendly)
			{
				if(target.position.X > projectile.position.X) {
					xoffset = Main.rand.Next(8,14);
				} else {
					xoffset = Main.rand.Next(-14,-8);
				}
				projectile.ai[0]++;
				projectile.velocity.X *= 0.99f;
				if(!jump) {
					if(projectile.velocity.Y < 7.5f) {
						projectile.velocity.Y += 0.05f;
					}
					if(target.position.Y < projectile.position.Y && projectile.ai[0] % 20 == 0) {
						jump = true;
						projectile.velocity.X = xoffset / 1.25f;
						projectile.velocity.Y = -6.5f;
					}
				}
				if(jump) {
					projectile.velocity *= 0.97f;
					if(Math.Abs(projectile.velocity.X) < 0.3f) {
						jump = false;
					}
					projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
				}
			}
			else
			{
				if(player.position.X > projectile.position.X) {
					xoffset = Main.rand.Next(16,24);
				} else {
					xoffset = Main.rand.Next(-24,-16);
				}
				projectile.ai[0]++;
				projectile.velocity.X *= 0.99f;
				if(!jump) {
					if(projectile.velocity.Y < 7.5f) {
						projectile.velocity.Y += 0.05f;
					}
					if(player.position.Y < projectile.position.Y && projectile.ai[0] % 20 == 0) {
						jump = true;
						projectile.velocity.X = xoffset / 1.25f;
						projectile.velocity.Y = -9;
					}
					projectile.rotation = 0f;
				}
				if(jump) {
					projectile.velocity *= 0.97f;
					if(Math.Abs(projectile.velocity.X) < 0.3f) {
						jump = false;
					}
					projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
				}
			}
			projectile.frameCounter++;
			if(projectile.frameCounter >= 7) {
				projectile.frame++;
				projectile.frameCounter = 0;
				if(projectile.frame >= 4)
					projectile.frame = 0;
			}
		}

		public override bool MinionContactDamage()
		{
			return true;
		}

	}
}