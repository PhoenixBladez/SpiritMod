using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace SpiritMod.NPCs
{
	public class Wheezer : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wheezer");
			Main.npcFrameCount[npc.type] = 16;
		}

		public override void SetDefaults()
		{
			npc.width = 40;
			npc.height = 36;
			npc.damage = 18;
			npc.defense = 9;
			npc.lifeMax = 60;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath53;
			npc.value = 960f;
			npc.knockBackResist = .35f;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (SpawnHelper.SupressSpawns(spawnInfo, SpawnFlags.None, SpawnZones.Underground))
				return 0;

			if (!NPC.downedBoss1)
				return 0;
			
			return SpawnCondition.Cavern.Chance * 0.07f;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 11; k++)
			{
				Dust.NewDust(npc.position, npc.width, npc.height, 5, hitDirection, -1f, 0, default(Color), .61f);
			}
			if (npc.life <= 0)
			{
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Wheezer_Head"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Wheezer_legs"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Wheezer_legs"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Wheezer_legs"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Wheezer_legs"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Wheezer_tail"), 1f);
			}
		}

		public override void NPCLoot()
		{
			int Techs = Main.rand.Next(1, 3);
			for (int J = 0; J <= Techs; J++)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("Carapace"));
			}
			if (Main.rand.Next(15) == 1)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("WheezerScale"));
		}

		int frame = 0;
		int timer = 0;
		public override void AI()
		{
			npc.spriteDirection = npc.direction;
			Player target = Main.player[npc.target];
			int distance = (int)Math.Sqrt((npc.Center.X - target.Center.X) * (npc.Center.X - target.Center.X) + (npc.Center.Y - target.Center.Y) * (npc.Center.Y - target.Center.Y));
			if (distance < 320)
			{
				npc.velocity = Vector2.Zero;
				if (npc.velocity == Vector2.Zero)
				{
					npc.velocity.X = .01f * npc.spriteDirection;
					npc.velocity.Y = 12f;
				}
				npc.ai[0]++;
				if (npc.ai[0] >= 120)
				{
					Main.PlaySound(2, (int)npc.position.X, (int)npc.position.Y, 95);
					Vector2 direction = Main.player[npc.target].Center - npc.Center;
					direction.Normalize();
						direction.X *= 5f;
						direction.Y *= 5f;

						int amountOfProjectiles = 1;
						for (int i = 0; i < amountOfProjectiles; ++i)
						{
							for (int k = 0; k < 11; k++)
							{
								Dust.NewDust(npc.position, npc.width, npc.height, 5, -npc.direction, -1f, 0, default(Color), .61f);
							}
							float A = (float)Main.rand.Next(-50, 50) * 0.02f;
							float B = (float)Main.rand.Next(-50, 50) * 0.02f;
							int p = Projectile.NewProjectile(npc.Center.X + (npc.direction * 12), npc.Center.Y - 10, direction.X + A, direction.Y + B, mod.ProjectileType("WheezerCloud"), npc.damage / 3 * 2, 1, Main.myPlayer, 0, 0);
							Main.projectile[p].hostile = true;
						}
					npc.ai[0] = 0;
				}
				timer++;
				if(timer == 4)
				{
					frame++;
					timer = 0;
				}
				if(frame >= 15)
				{
					frame = 11;
				}
			}
			else
			{
				npc.ai[0] = 0;
				npc.aiStyle = 3;
				aiType = NPCID.Skeleton;
				timer++;
				if(timer == 4)
				{
					frame++;
					timer = 0;
				}
				if(frame >= 9)
				{
					frame = 1;
				}
			}	
		}
		public override void FindFrame(int frameHeight)
		{
			npc.frame.Y = frameHeight * frame;
		}
	}
}
