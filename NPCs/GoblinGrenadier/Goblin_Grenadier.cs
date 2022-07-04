using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.ModLoader.Utilities;

namespace SpiritMod.NPCs.GoblinGrenadier
{
	public class Goblin_Grenadier : ModNPC
	{
		public int bombTimer = 0;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Goblin Grenadier");
			Main.npcFrameCount[NPC.type] = 14;
		}

		public override void SetDefaults()
		{
			NPC.aiStyle = 3;
			NPC.lifeMax = 80;
			NPC.defense = 6;
			NPC.value = 200f;
			AIType = 0;
			NPC.knockBackResist = 0.3f;
			NPC.width = 30;
			NPC.height = 42;
			NPC.damage = 20;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<Items.Banners.GoblinGrenadierBanner>();
		}

		public override void AI()
		{
			NPC.TargetClosest(true);
			NPC.spriteDirection = NPC.direction;
			bombTimer++;
			if (bombTimer >= 150)
			{
				if (bombTimer == 150)
					NPC.frameCounter = 0;
				NPC.velocity.X = 0f;
				NPC.velocity.Y = 5f;
				if (bombTimer > 150+49)
				{
					NPC.netUpdate = true;
					bombTimer = 0;
				}
			}
		}
		
		public override void OnKill()
		{
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

		public override void ModifyNPCLoot(NPCLoot npcLoot) => npcLoot.AddCommon(168, 1, 5, 14);

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 7; k++)
			{
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, 2.5f * hitDirection, -2.5f, 0, default, 1.2f);
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, 2.5f * hitDirection, -2.5f, 0, default, 0.5f);
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, 2.5f * hitDirection, -2.5f, 0, default, 0.7f);
			}
			if (NPC.life <= 0) //Kill gores
				for (int i = 1; i < 5; ++i)
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("SpiritMod/Gores/GoblinGrenadier/GoblinGrenadierGore" + i).Type, 1f);
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) => SpawnCondition.GoblinArmy.Chance * 0.15f;

		public override void FindFrame(int frameHeight)
		{
			const int Frame_14 = 13;

			Player player = Main.player[NPC.target];
			NPC.frameCounter++;
			if (NPC.velocity.Y != 0f)
			{
				NPC.frame.Y = Frame_14 * frameHeight;
			}
			else if (bombTimer < 150)
			{
				if (NPC.frameCounter < 7) //First frame
					NPC.frame.Y = 0;
				else if (NPC.frameCounter < 14) //Second frame...
					NPC.frame.Y = frameHeight;
				else if (NPC.frameCounter < 21)
					NPC.frame.Y = 2 * frameHeight;
				else if (NPC.frameCounter < 28)
					NPC.frame.Y = 3 * frameHeight;
				else if (NPC.frameCounter < 35) //...Final frame
					NPC.frame.Y = 4 * frameHeight;
				else //Reset
					NPC.frameCounter = 0;
			}
			else if (bombTimer >= 150)
			{
				if (NPC.frameCounter < 7) //Seventh frame
					NPC.frame.Y = 6 * frameHeight;
				else if (NPC.frameCounter < 14) //Eighth frame...
					NPC.frame.Y = 7 * frameHeight;
				else if (NPC.frameCounter < 21)
					NPC.frame.Y = 8 * frameHeight;
				else if (NPC.frameCounter < 28)
					NPC.frame.Y = 9 * frameHeight;
				else if (NPC.frameCounter < 35)
					NPC.frame.Y = 10 * frameHeight;
				else if (NPC.frameCounter < 42)
					NPC.frame.Y = 11 * frameHeight;
				else if (NPC.frameCounter < 49) //Final frame + throw bomb
				{
					NPC.frame.Y = 12 * frameHeight;
					if (NPC.frameCounter == 43 && Main.netMode != NetmodeID.MultiplayerClient)
					{
						SoundEngine.PlaySound(SoundID.Item18, NPC.position);
						float num5 = 8f;
						Vector2 vector2 = new Vector2(NPC.Center.X, NPC.position.Y - 13 + NPC.height * 0.5f);
						float num6 = Main.player[NPC.target].position.X + Main.player[NPC.target].width * 0.5f - vector2.X;
						float num7 = Math.Abs(num6) * 0.2f;
						float num8 = Main.player[NPC.target].position.Y + Main.player[NPC.target].height * 0.5f - vector2.Y - num7;
						float num14 = (float)Math.Sqrt(num6 * num6 + num8 * num8);
						NPC.netUpdate = true;
						float num15 = num5 / num14;
						float num16 = num6 * num15;
						float SpeedY = num8 * num15;
						int p = Projectile.NewProjectile(NPC.GetSource_FromAI(), vector2.X, vector2.Y, num16, SpeedY, ModContent.ProjectileType<GoblinGrenadierGrenade>(), 20, 0.0f, Main.myPlayer, 0.0f, 0.0f);
						Main.projectile[p].friendly = false;
						Main.projectile[p].hostile = true;
						Main.projectile[p].timeLeft = 45;
					}
				}
				else //Reset
					NPC.frameCounter = 0;
			}
		}
	}
}