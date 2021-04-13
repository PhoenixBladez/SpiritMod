using Microsoft.Xna.Framework;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Undead_Warlock
{
	public class Undead_Warlock : ModNPC
	{
		public bool spawningCrystals = false;
		public int spawningCrystalsTimer = 0;
		public int crystalTimer = 0;
		public int projectileTimer = 1;
		public int resetTimer = 0;
		public int spawnedProjectiles = 0;
		public int crystal1 = 0;
		public int crystal2 = 0;
		public int crystal3 = 0;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Undead Warlock");
			Main.npcFrameCount[npc.type] = 5;
		}
		public override void SetDefaults()
		{
			npc.aiStyle = 3;
			npc.lifeMax = 400;
			npc.defense = 20;
			npc.value = 3000f;
			aiType = 3;
			npc.knockBackResist = 0.2f;
			npc.width = 24;
			npc.height = 36;
			npc.damage = 35;
			npc.lavaImmune = false;
			npc.HitSound = new Terraria.Audio.LegacySoundStyle(3, 1);
		}
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(spawningCrystals);
			writer.Write(spawningCrystalsTimer);
			writer.Write(crystalTimer);
			writer.Write(projectileTimer);
			writer.Write(resetTimer);
			writer.Write(spawnedProjectiles);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			spawningCrystals = reader.ReadBoolean();
			spawningCrystalsTimer = reader.ReadInt32();
			crystalTimer = reader.ReadInt32();
			projectileTimer = reader.ReadInt32();
			resetTimer = reader.ReadInt32();
			spawnedProjectiles = reader.ReadInt32();
		}
		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter++;
			if (npc.velocity.X != 0f && npc.velocity.Y == 0f)
			{
				if (npc.frameCounter >= 7)
				{
					npc.frameCounter = 0;
					npc.frame.Y = ((npc.frame.Y + 1) % 5) * frameHeight;
				}
			}
			else
				npc.frame.Y = 2*frameHeight;
		}
		public override void AI()
		{
			Player player = Main.player[npc.target];
			npc.TargetClosest(true);
			npc.spriteDirection = npc.direction;
			
			spawningCrystalsTimer++;
			if (spawningCrystalsTimer >= 240)
				spawningCrystals = true;
			else
				spawningCrystals = false;
			
			if (spawningCrystals)
			{
				
				npc.aiStyle = -1;
				npc.velocity.X = 0f;		
				npc.velocity.Y = 4f;
				if (npc.collideY)
				{
					crystalTimer++;
					if (crystalTimer > 45)
						projectileTimer++;
					if (crystalTimer == 45 && Main.netMode != NetmodeID.MultiplayerClient)
					{
						Main.PlaySound(2, (int)npc.position.X, (int)npc.position.Y, 8, 1f, 0f);
						crystal1 = Projectile.NewProjectile(npc.Center.X, npc.Center.Y - 90, 0f, 0f, mod.ProjectileType("Undead_Warlock_Crystal"), 10, 3f, 0);
						crystal2 = Projectile.NewProjectile(npc.Center.X, npc.Center.Y - 90, 0f, 0f, mod.ProjectileType("Undead_Warlock_Crystal"), 10, 3f, 0);
						crystal3 = Projectile.NewProjectile(npc.Center.X, npc.Center.Y - 90, 0f, 0f, mod.ProjectileType("Undead_Warlock_Crystal"), 10, 3f, 0);
						Main.projectile[crystal1].Center += new Vector2(0, 35).RotatedBy(MathHelper.ToRadians(120));
						Main.projectile[crystal2].Center += new Vector2(0, 35).RotatedBy(MathHelper.ToRadians(240));
						Main.projectile[crystal3].Center += new Vector2(0, 35).RotatedBy(MathHelper.ToRadians(360));
						Main.projectile[crystal1].ai[1] = npc.whoAmI;
						Main.projectile[crystal2].ai[1] = npc.whoAmI;
						Main.projectile[crystal3].ai[1] = npc.whoAmI;
					}
					
					if (projectileTimer % 60 == 0 && Main.netMode != NetmodeID.MultiplayerClient)
					{
						int chosenProjectile = 0;
						switch (Main.rand.Next(4))
						{
							case 0:
								chosenProjectile = mod.ProjectileType("Letter_1");
								break;
							case 1:
								chosenProjectile = mod.ProjectileType("Letter_2");
								break;
							case 2:
								chosenProjectile = mod.ProjectileType("Letter_3");
								break;
							case 3:
								chosenProjectile = mod.ProjectileType("Letter_4");
								break;
							default:
								break;
						}
						spawnedProjectiles++;
						Main.PlaySound(2, (int)npc.position.X, (int)npc.position.Y, 28, 1f, 0f);
						int chosenDust = Main.rand.Next(2)==0 ? 173 : 157;
						int p = Projectile.NewProjectile(npc.Center.X + 2, npc.Center.Y - 88, 0f, 0f, chosenProjectile, 20, 3f, 0);
						Main.projectile[p].ai[1] = npc.whoAmI;
						int num = 10;
						for (int index1 = 0; index1 < num; ++index1)
						{
						  int index2 = Dust.NewDust(new Vector2(npc.Center.X + 2, npc.Center.Y - 88), 0, 0, chosenDust, 0.0f, 0.0f, 0, new Color(), 0.75f);
						  Main.dust[index2].velocity *= 1.2f;
						  --Main.dust[index2].velocity.Y;
						  Main.dust[index2].position = Vector2.Lerp(Main.dust[index2].position, new Vector2(npc.Center.X + 2, npc.Center.Y - 88), 0.75f);
						}
					}
					if (spawnedProjectiles >= 2)
						resetTimer++;
					if (resetTimer > 200)
					{
						Main.projectile[crystal1].Kill();
						Main.projectile[crystal2].Kill();
						Main.projectile[crystal3].Kill();
						crystalTimer = 0;		
						spawningCrystalsTimer = 0;				
						spawnedProjectiles = 0;
						projectileTimer = 1;
						resetTimer = 0;	
						npc.netUpdate = true;	
					}
					if (Main.projectile[crystal1].active && Main.projectile[crystal2].active && Main.projectile[crystal3].active && crystalTimer > 59)
					{
						drawDustBeetweenThisAndThat(Main.projectile[crystal1].Center, Main.projectile[crystal2].Center);
						drawDustBeetweenThisAndThat(Main.projectile[crystal2].Center, Main.projectile[crystal3].Center);
						drawDustBeetweenThisAndThat(Main.projectile[crystal3].Center, Main.projectile[crystal1].Center);
						if (Main.rand.Next(2)==0)
						{
							for (int i = 0; i < 7; i++)
							{
								int chosenDust = Main.rand.Next(2)==0 ? 173 : 157;
								Vector2 offset = new Vector2();
								double angle = Main.rand.NextDouble() * 2d * Math.PI;
								offset.X += (float)(Math.Sin(angle) * 17f);
								offset.Y += (float)(Math.Cos(angle) * 17f);
								Dust dust = Main.dust[Dust.NewDust(new Vector2(npc.Center.X, npc.Center.Y - 88) + offset - new Vector2(2, 4), 0, 0, chosenDust, 0, 0, 0, new Color(), 0.95f)];
								dust.velocity = Vector2.Zero;
								dust.fadeIn = 0.2f;
								dust.noGravity = true;
								dust.noLight = false;
							}
						}
					}
				}
			}
			else
				npc.aiStyle = 3;
		}
		public static void drawDustBeetweenThisAndThat(Vector2 vector3, Vector2 vector1)
		{
			Vector2 range = vector3 - vector1;
			if (Main.rand.Next(12)==0)
			{
				for (int i = 0; i < 8; i++)
				{
					int chosenDust = Main.rand.Next(2)==0 ? 173 : 157;
					Dust dust = Main.dust[Dust.NewDust(vector1 + range * Main.rand.NextFloat() + Vector2.Zero, 0, 0, chosenDust)];
					dust.noGravity = true;
					dust.noLight = false;
					dust.velocity = range * 0.001f;
					dust.scale = 1.24f;
				}
			}
		}
		public override void NPCLoot()
		{
			if (Main.rand.Next(15)==0)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("Jewel_of_Night"));
			}
			if (Main.rand.Next(15)==0)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("Undead_Warlock_Staff"));
			}
			if (Main.rand.Next(15)==0)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("Undead_Warlock_Medallion"));
			}
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/UndeadScientistGore4"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/UndeadScientistGore3"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/UndeadScientistGore2"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/UndeadScientistGore1"), 1f);
				Main.PlaySound(29, (int)npc.position.X, (int)npc.position.Y, 22, 1f, -0.9f);
			}
			for (int k = 0; k < 7; k++)
			{
				Dust.NewDust(npc.position, npc.width, npc.height, 5, 2.5f * hitDirection, -2.5f, 0, default(Color), 1.2f);
				Dust.NewDust(npc.position, npc.width, npc.height, 5, 2.5f * hitDirection, -2.5f, 0, default(Color), 0.5f);
				Dust.NewDust(npc.position, npc.width, npc.height, 5, 2.5f * hitDirection, -2.5f, 0, default(Color), 0.7f);
			}			
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (!Main.hardMode)
			{
				return 0f;
			}
			return SpawnCondition.OverworldNightMonster.Chance * 0.01f;
		}
	}
}