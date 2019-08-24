using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.Infernon
{
	[AutoloadBossHead]
	public class Infernon : ModNPC
	{
		public static int _type;

		public int currentSpread;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Infernon");
		}

		public override void SetDefaults()
		{
			npc.width = 130;
			npc.height = 190;
			npc.damage = 36;
			npc.defense = 13;
			npc.lifeMax = 13000;
			npc.knockBackResist = 0;
			Main.npcFrameCount[npc.type] = 10;
			npc.boss = true;
			npc.noGravity = true;
			npc.noTileCollide = true;
			bossBag = mod.ItemType("InfernonBag");
			music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/Infernon");
			npc.npcSlots = 10;

			npc.HitSound = SoundID.NPCHit7;
			npc.DeathSound = SoundID.NPCDeath5;
		}

		public override bool PreAI()
		{
			npc.spriteDirection = npc.direction;
			if (!Main.player[npc.target].active || Main.player[npc.target].dead)
			{
				npc.TargetClosest(false);
				npc.velocity.Y = -100;
			}
			if (!NPC.AnyNPCs(mod.NPCType("InfernonSkull")))
			{
				if (Main.expertMode || npc.life <= 7000)
					NPC.NewNPC((int)npc.position.X, (int)npc.position.Y, mod.NPCType("InfernonSkull"), 0, 2, 1, 0, npc.whoAmI, npc.target);
			}

			if (npc.ai[0] == 0)
			{
				// Get the proper direction to move towards the current targeted player.
				if (npc.ai[2] == 0)
				{
					npc.TargetClosest(true);
					npc.ai[2] = npc.Center.X >= Main.player[npc.target].Center.X ? -1f : 1f;
				}
				npc.TargetClosest(true);

				Player player = Main.player[npc.target];
				if (!player.active || player.dead)
				{
					npc.TargetClosest(false);
					npc.velocity.Y = -100;
				}

				float currentXDist = Math.Abs(npc.Center.X - player.Center.X);
				if (npc.Center.X < player.Center.X && npc.ai[2] < 0)
					npc.ai[2] = 0;
				if (npc.Center.X > player.Center.X && npc.ai[2] > 0)
					npc.ai[2] = 0;

				float accelerationSpeed = 0.13F;
				float maxXSpeed = 9;
				npc.velocity.X = npc.velocity.X + npc.ai[2] * accelerationSpeed;
				npc.velocity.X = MathHelper.Clamp(npc.velocity.X, -maxXSpeed, maxXSpeed);

				float yDist = player.position.Y - (npc.position.Y + npc.height);
				if (yDist < 0)
					npc.velocity.Y = npc.velocity.Y - 0.2F;
				if (yDist > 150)
					npc.velocity.Y = npc.velocity.Y + 0.2F;
				npc.velocity.Y = MathHelper.Clamp(npc.velocity.Y, -6, 6);
				npc.rotation = npc.velocity.X * 0.03f;

				// If the NPC is close enough
				if ((currentXDist < 500 || npc.ai[3] < 0) && npc.position.Y < player.position.Y)
				{
					++npc.ai[3];
					int cooldown = 15;
					if (npc.life < npc.lifeMax * 0.75)
						cooldown = 154;
					if (npc.life < npc.lifeMax * 0.5)
						cooldown = 13;
					if (npc.life < npc.lifeMax * 0.25)
						cooldown = 12;
					cooldown++;
					if (npc.ai[3] > cooldown)
						npc.ai[3] = -cooldown;

					if (npc.ai[3] == 0 && Main.netMode != 1)
					{
						Vector2 position = npc.Center;
						position.X += npc.velocity.X * 7;

						float speedX = player.Center.X - npc.Center.X;
						float speedY = player.Center.Y - npc.Center.Y;
						float length = (float)Math.Sqrt(speedX * speedX + speedY * speedY);
						float speed = 6;
						if (npc.life < npc.lifeMax * 0.75)
							speed = 6f;
						if (npc.life < npc.lifeMax * 0.5)
							speed = 8f;
						if (npc.life < npc.lifeMax * 0.25)
							speed = 10f;
						float num12 = speed / length;
						speedX = speedX * num12;
						speedY = speedY * num12;
						Projectile.NewProjectile(position.X, position.Y, speedX, speedY, mod.ProjectileType("InfernalWave"), 28, 0, Main.myPlayer);
					}
				}
				else if (npc.ai[3] < 0)
					npc.ai[3]++;

				if (Main.netMode != 1)
				{
					npc.ai[1] += Main.rand.Next(1, 4);
					if (npc.ai[1] > 800 && currentXDist < 600)
						npc.ai[0] = -1;
				}
			}
			else if (npc.ai[0] == 1)
			{
				if (npc.ai[2] == 0)
				{
					npc.TargetClosest(true);
					npc.ai[2] = npc.Center.X >= Main.player[npc.target].Center.X ? -1f : 1f;
				}
				npc.TargetClosest(true);
				Player player = Main.player[npc.target];

				float currentXDist = Math.Abs(npc.Center.X - player.Center.X);
				if (npc.Center.X < player.Center.X && npc.ai[2] < 0)
					npc.ai[2] = 0;
				if (npc.Center.X > player.Center.X && npc.ai[2] > 0)
					npc.ai[2] = 0;

				float accelerationSpeed = 0.1F;
				float maxXSpeed = 7;
				npc.velocity.X = npc.velocity.X + npc.ai[2] * accelerationSpeed;
				npc.velocity.X = MathHelper.Clamp(npc.velocity.X, -maxXSpeed, maxXSpeed);

				float yDist = player.position.Y - (npc.position.Y + npc.height);
				if (yDist < 0)
					npc.velocity.Y = npc.velocity.Y - 0.2F;
				if (yDist > 150)
					npc.velocity.Y = npc.velocity.Y + 0.2F;
				npc.velocity.Y = MathHelper.Clamp(npc.velocity.Y, -6, 6);

				npc.rotation = npc.velocity.X * 0.03f;

				if (Main.netMode != 1)
				{
					npc.ai[3]++;
					if (npc.ai[3] % 5 == 0 && npc.ai[3] <= 25)
					{
						Vector2 pos = new Vector2(npc.Center.X, (npc.position.Y + npc.height - 14));
						if (!WorldGen.SolidTile((int)(pos.X / 16), (int)(pos.Y / 16)))
						{
							Vector2 dir = player.Center - pos;
							dir.Normalize();
							dir *= 12;
							Projectile.NewProjectile(pos.X, pos.Y, dir.X, dir.Y, mod.ProjectileType("FireSpike"), 22, 0, Main.myPlayer);
							currentSpread++;
						}
					}

					int cooldown = 80;
					if (npc.life < npc.lifeMax * 0.75)
						cooldown = 70;
					if (npc.life < npc.lifeMax * 0.5)
						cooldown = 60;
					if (npc.life < npc.lifeMax * 0.25)
						cooldown = 50;
					if (npc.life < npc.lifeMax * 0.1)
						cooldown = 35;
					if (npc.ai[3] >= cooldown)
						npc.ai[3] = 0;

					npc.ai[1] += Main.rand.Next(1, 4);
					if (npc.ai[1] > 600.0)
						npc.ai[0] = -1f;
				}
			}
			else if (npc.ai[0] == 2)
			{
				if (npc.velocity.X > 0)
					npc.velocity.X -= 0.1F;
				if (npc.velocity.X < 0)
					npc.velocity.X += 0.1F;
				if (npc.velocity.X > -0.2F && npc.velocity.X < 0.2F)
					npc.velocity.X = 0;
				if (npc.velocity.Y > 0)
					npc.velocity.Y -= 0.1F;
				if (npc.velocity.Y < 0)
					npc.velocity.Y += 0.1F;
				if (npc.velocity.Y > -0.2F && npc.velocity.Y < 0.2F)
					npc.velocity.Y = 0;

				npc.rotation = npc.velocity.X * 0.03F;

				npc.ai[3]++;
				if (npc.ai[3] >= 60)
				{
					if (npc.ai[3] % 20 == 0)
					{
						int dust = Dust.NewDust(npc.position, npc.width, npc.height, 6);
						Main.dust[dust].noGravity = true;
						Main.dust[dust].scale = 1.9f;
						int dust1 = Dust.NewDust(npc.position, npc.width, npc.height, 6);
						Main.dust[dust1].noGravity = true;
						Main.dust[dust1].scale = 1.9f;
						int dust2 = Dust.NewDust(npc.position, npc.width, npc.height, 6);
						Main.dust[dust2].noGravity = true;
						Main.dust[dust2].scale = 1.9f;
						int dust3 = Dust.NewDust(npc.position, npc.width, npc.height, 6);
						Main.dust[dust3].noGravity = true;
						Main.dust[dust3].scale = 1.9f;
						Vector2 direction = Vector2.One.RotatedByRandom(MathHelper.ToRadians(100));
						int newNPC = NPC.NewNPC((int)npc.position.X, (int)npc.position.Y, mod.NPCType("InfernonSkullMini"));
						Main.npc[newNPC].velocity = direction * 8;
					}
					// Shoot mini skulls.
				}

				if (Main.netMode != 1)
				{
					npc.ai[1] += Main.rand.Next(1, 4);
					if (npc.ai[1] > 500)
						npc.ai[0] = -1f;
				}
			}
			else if (npc.ai[0] == 3)
			{
				npc.velocity.Y -= 0.1F;
				npc.alpha += 2;
				if (npc.alpha >= 255)
					npc.active = false;
				if (npc.velocity.X > 0)
					npc.velocity.X -= 0.2F;
				if (npc.velocity.X < 0)
					npc.velocity.X += 0.2F;
				if (npc.velocity.X > -0.2F && npc.velocity.X < 0.2F)
					npc.velocity.X = 0;

				npc.rotation = npc.velocity.X * 0.03f;
			}

			int dust4 = Dust.NewDust(npc.position + npc.velocity, npc.width + 158, npc.height + 160, 6, npc.velocity.X * 0.5f, npc.velocity.Y * 0.5f);
			int dust5 = Dust.NewDust(npc.position + npc.velocity, npc.width + 162, npc.height + 160, 6, npc.velocity.X * 0.5f, npc.velocity.Y * 0.5f);
			Main.dust[dust4].velocity *= 0f;
			Main.dust[dust5].velocity *= 0f;

			if (!Main.player[npc.target].active || Main.player[npc.target].dead)
			{
				npc.TargetClosest(true);
				if (!Main.player[npc.target].active || Main.player[npc.target].dead)
				{
					npc.ai[0] = 3;
					npc.ai[3] = 0;
				}
			}

			if (npc.ai[0] != -1)
				return false;

			int num = Main.rand.Next(3);
			npc.TargetClosest(true);
			if (Math.Abs(npc.Center.X - Main.player[npc.target].Center.X) > 1000)
				num = 0;
			npc.ai[0] = num;
			npc.ai[1] = 0;
			npc.ai[2] = 0;
			npc.ai[3] = 0;

			return true;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 5; k++)
			{
				Dust.NewDust(npc.position, npc.width, npc.height, 6, hitDirection, -1f, 0, default(Color), 1f);
			}
			if (npc.life <= 0)
			{
				if (Main.netMode != 1 && npc.life <= 0)
				{
					if (Main.expertMode)
					{

						Main.NewText("You have yet to defeat the true master of Hell...", 220, 100, 100, true);
						Vector2 spawnAt = npc.Center + new Vector2(0f, (float)npc.height);
						NPC.NewNPC((int)spawnAt.X, (int)spawnAt.Y, mod.NPCType("InfernoSkull"));
					}
				}
				npc.position.X = npc.position.X + (float)(npc.width / 2);
				npc.position.Y = npc.position.Y + (float)(npc.height / 2);
				npc.width = 156;
				npc.height = 180;
				npc.position.X = npc.position.X - (float)(npc.width / 2);
				npc.position.Y = npc.position.Y - (float)(npc.height / 2);
				for (int num621 = 0; num621 < 200; num621++)
				{
					int num622 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 6, 0f, 0f, 100, default(Color), 2f);
					Main.dust[num622].velocity *= 3f;
					if (Main.rand.Next(2) == 0)
					{
						Main.dust[num622].scale = 0.5f;
						Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
					}
				}
				for (int num623 = 0; num623 < 400; num623++)
				{
					int num624 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 6, 0f, 0f, 100, default(Color), 3f);
					Main.dust[num624].noGravity = true;
					Main.dust[num624].velocity *= 5f;
					num624 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 6, 0f, 0f, 100, default(Color), 2f);
					Main.dust[num624].velocity *= 2f;
				}
			}
		}

		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += 0.15f;
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
		}


		public override bool PreNPCLoot()
		{
			if (Main.expertMode)
				return false;

			MyWorld.downedInfernon = true;
			return true;
		}

		public override void NPCLoot()
		{
			npc.DropItem(mod.ItemType("InfernalAppendage"), 25, 36);

			string[] lootTable = { "InfernalJavelin", "InfernalSword", "DiabolicHorn", "SevenSins", "InfernalStaff", "EyeOfTheInferno", "InfernalShield" };
			int loot = Main.rand.Next(lootTable.Length);
			npc.DropItem(mod.ItemType(lootTable[loot]));

			npc.DropItem(mod.ItemType("SearingBand"), 10f / 85);
			npc.DropItem(Items.Armor.Masks.InfernonMask._type, 1f / 7);
			npc.DropItem(Items.Boss.Trophy4._type, 1f / 10);
		}

		public override void BossLoot(ref string name, ref int potionType)
		{
			potionType = ItemID.GreaterHealingPotion;
		}
	}
}
