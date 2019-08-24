using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;


namespace SpiritMod.NPCs
{
	public class ForestWraith : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Glade Wraith");
			Main.npcFrameCount[npc.type] = 3;
		}

		public override void SetDefaults()
		{
			npc.width = 44;
			npc.height = 43;
			npc.damage = 28;
			npc.defense = 10;
			npc.lifeMax = 300;
			npc.boss = true;
			npc.HitSound = SoundID.NPCHit2;
			npc.DeathSound = SoundID.NPCDeath6;
			npc.value = 641f;
			npc.knockBackResist = .10f;
			npc.noGravity = true;
			npc.noTileCollide = true;
			npc.aiStyle = 22;
			aiType = NPCID.Wraith;
		}

		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += 0.15f;
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
		}

		bool rotationspawns1 = false;
		public override bool PreAI()
		{
			bool expertMode = Main.expertMode;
			npc.spriteDirection = npc.direction;
			if (!rotationspawns1)
			{
				for (int I = 0; I < 3; I++)
				{
					//cos = y, sin = x
					int GeyserEye = NPC.NewNPC((int)(npc.Center.X + (Math.Sin(I * 120) * 100)), (int)(npc.Center.Y + (Math.Cos(I * 120) * 100)), mod.NPCType("GrassEnergy"), npc.whoAmI, 0, 0, 0, -1);
					NPC Eye = Main.npc[GeyserEye];
					Eye.ai[0] = I * 120;
					Eye.ai[3] = I * 120;
					rotationspawns1 = true;
				}
			}

			int npcType = mod.NPCType("GrassEnergy");
			bool plantAlive = false;
			for (int num569 = 0; num569 < 200; num569++)
			{
				if ((Main.npc[num569].active && Main.npc[num569].type == (npcType)))
					plantAlive = true;
			}

			if (plantAlive)
				npc.dontTakeDamage = true;
			else
			{
				npc.dontTakeDamage = false;
				if (Main.rand.Next(160) == 1)
				{
					Main.PlaySound(6, (int)npc.position.X, (int)npc.position.Y, 0);
					Vector2 dir = Main.player[npc.target].Center - npc.Center;
					dir.Normalize();
					dir.X *= 12f;
					dir.Y *= 12f;

					int amountOfProjectiles = 1;
					for (int i = 0; i < amountOfProjectiles; ++i)
					{
						float A = (float)Main.rand.Next(-200, 200) * 0.01f;
						float B = (float)Main.rand.Next(-200, 200) * 0.01f;
						int damage = expertMode ? 9 : 12;
						int p = Projectile.NewProjectile(npc.Center.X, npc.Center.Y, dir.X + A, dir.Y + B, mod.ProjectileType("OvergrowthLeaf"), damage, 1, Main.myPlayer, 0, 0);
						Main.projectile[p].hostile = true;
						Main.projectile[p].friendly = false;
						Main.projectile[p].tileCollide = false;

					}
				}
			}

			return true;
		}

		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			npc.lifeMax = (int)(npc.lifeMax * 0.55f * bossLifeScale);
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				npc.position.X = npc.position.X + (float)(npc.width / 2);
				npc.position.Y = npc.position.Y + (float)(npc.height / 2);
				npc.width = 30;
				npc.height = 30;
				npc.position.X = npc.position.X - (float)(npc.width / 2);
				npc.position.Y = npc.position.Y - (float)(npc.height / 2);
				for (int num621 = 0; num621 < 20; num621++)
				{
					int num622 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 3, 0f, 0f, 100, default(Color), 2f);
					Main.dust[num622].velocity *= 3f;
					if (Main.rand.Next(2) == 0)
					{
						Main.dust[num622].scale = 0.5f;
						Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
					}
				}
			}
		}

		public override void NPCLoot()
		{
			string[] lootTable = { "SacredVine", "OakHeart" };
			int loot = Main.rand.Next(lootTable.Length);
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType(lootTable[loot]));

			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("AncientBark"), 3);
		}

	}
}
