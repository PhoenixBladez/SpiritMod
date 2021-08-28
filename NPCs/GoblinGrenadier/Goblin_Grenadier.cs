using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace SpiritMod.NPCs.GoblinGrenadier
{
	public class Goblin_Grenadier : ModNPC
	{
		public int bombTimer = 0;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Goblin Grenadier");
			Main.npcFrameCount[npc.type] = 14;
		}

		public override void SetDefaults()
		{
			npc.aiStyle = 3;
			npc.lifeMax = 80;
			npc.defense = 6;
			npc.value = 200f;
			aiType = 0;
			npc.knockBackResist = 0.3f;
			npc.width = 30;
			npc.height = 42;
			npc.damage = 20;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
		}

		public override void AI()
		{
			npc.TargetClosest(true);
			npc.spriteDirection = npc.direction;
			bombTimer++;
			if (bombTimer >= 150)
			{
				if (bombTimer == 150)
					npc.frameCounter = 0;
				npc.velocity.X = 0f;
				npc.velocity.Y = 5f;
				if (bombTimer > 150+49)
				{
					npc.netUpdate = true;
					bombTimer = 0;
				}
			}
		}
		
		public override void NPCLoot()
		{
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, 168, Main.rand.Next(5, 15));
			if (Main.invasionType == 1)
			{
				Main.invasionSize -= 1;
				if (Main.invasionSize < 0)
					Main.invasionSize = 0;
				if (Main.netMode != NetmodeID.MultiplayerClient)
					Main.ReportInvasionProgress(Main.invasionSizeStart - Main.invasionSize, Main.invasionSizeStart, 4, 0);
				if (Main.netMode == NetmodeID.Server)
					NetMessage.SendData(MessageID.InvasionProgressReport, -1, -1, null, Main.invasionProgress, Main.invasionProgressMax, Main.invasionProgressIcon, 0f, 0, 0, 0);
			}
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 7; k++)
			{
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.Blood, 2.5f * hitDirection, -2.5f, 0, default, 1.2f);
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.Blood, 2.5f * hitDirection, -2.5f, 0, default, 0.5f);
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.Blood, 2.5f * hitDirection, -2.5f, 0, default, 0.7f);
			}
			if (npc.life <= 0) //Kill gores
				for (int i = 1; i < 5; ++i)
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/GoblinGrenadier/GoblinGrenadierGore" + i), 1f);
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) => SpawnCondition.GoblinArmy.Chance * 0.15f;

		public override void FindFrame(int frameHeight)
		{
			const int Frame_14 = 13;

			Player player = Main.player[npc.target];
			npc.frameCounter++;
			if (npc.velocity.Y != 0f)
			{
				npc.frame.Y = Frame_14 * frameHeight;
			}
			else if (bombTimer < 150)
			{
				if (npc.frameCounter < 7) //First frame
					npc.frame.Y = 0;
				else if (npc.frameCounter < 14) //Second frame...
					npc.frame.Y = frameHeight;
				else if (npc.frameCounter < 21)
					npc.frame.Y = 2 * frameHeight;
				else if (npc.frameCounter < 28)
					npc.frame.Y = 3 * frameHeight;
				else if (npc.frameCounter < 35) //...Final frame
					npc.frame.Y = 4 * frameHeight;
				else //Reset
					npc.frameCounter = 0;
			}
			else if (bombTimer >= 150)
			{
				if (npc.frameCounter < 7) //Seventh frame
					npc.frame.Y = 6 * frameHeight;
				else if (npc.frameCounter < 14) //Eighth frame...
					npc.frame.Y = 7 * frameHeight;
				else if (npc.frameCounter < 21)
					npc.frame.Y = 8 * frameHeight;
				else if (npc.frameCounter < 28)
					npc.frame.Y = 9 * frameHeight;
				else if (npc.frameCounter < 35)
					npc.frame.Y = 10 * frameHeight;
				else if (npc.frameCounter < 42)
					npc.frame.Y = 11 * frameHeight;
				else if (npc.frameCounter < 49) //Final frame + throw bomb
				{
					npc.frame.Y = 12 * frameHeight;
					if (npc.frameCounter == 43 && Main.netMode != NetmodeID.MultiplayerClient)
					{
						Main.PlaySound(SoundID.Item18, npc.position);
						float num5 = 8f;
						Vector2 vector2 = new Vector2(npc.Center.X, npc.position.Y - 13 + npc.height * 0.5f);
						float num6 = Main.player[npc.target].position.X + Main.player[npc.target].width * 0.5f - vector2.X;
						float num7 = Math.Abs(num6) * 0.2f;
						float num8 = Main.player[npc.target].position.Y + Main.player[npc.target].height * 0.5f - vector2.Y - num7;
						float num14 = (float)Math.Sqrt(num6 * num6 + num8 * num8);
						npc.netUpdate = true;
						float num15 = num5 / num14;
						float num16 = num6 * num15;
						float SpeedY = num8 * num15;
						int p = Projectile.NewProjectile(vector2.X, vector2.Y, num16, SpeedY, ProjectileID.Grenade, 20, 0.0f, Main.myPlayer, 0.0f, 0.0f);
						Main.projectile[p].friendly = false;
						Main.projectile[p].hostile = true;
						Main.projectile[p].timeLeft = 45;
					}
				}
				else //Reset
					npc.frameCounter = 0;
			}
		}
	}
}