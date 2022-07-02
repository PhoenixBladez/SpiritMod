using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace SpiritMod.NPCs.Dead_Scientist
{
	public class Dead_Scientist : ModNPC
	{
		public bool isPuking = false;
		public int pukeTimer = 0;
		public int delayTimer = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Undead Scientist");
			Main.npcFrameCount[NPC.type] = 11;
		}
		public override void SetDefaults()
		{
			NPC.aiStyle = 3;
			NPC.lifeMax = 50;
			NPC.defense = 6;
			NPC.value = 100f;
			AIType = 3;
			NPC.knockBackResist = 0.5f;
			NPC.width = 24;
			NPC.height = 36;
			NPC.damage = 28;
			NPC.lavaImmune = false;
			NPC.HitSound = SoundID.NPCHit1;
		}

		public override void AI()
		{
			Player player = Main.player[NPC.target];
			NPC.TargetClosest(true);
			NPC.spriteDirection = NPC.direction;
			pukeTimer++;
			
			if (Main.rand.Next(300)==0)
				SoundEngine.PlaySound(SoundID.Item9, NPC.Center);
			
			if (Vector2.Distance(player.Center, NPC.Center) < 300f && !isPuking && player.position.Y > NPC.position.Y - 100 && Collision.CanHitLine(NPC.Center, 0, 0, Main.player[NPC.target].Center, 0, 0) && delayTimer < 180) 
			{
				SoundEngine.PlaySound(SoundID.Item9, NPC.Center);
				NPC.frameCounter = 0;
				isPuking = true;
			}

			if (Vector2.Distance(player.Center, NPC.Center) >= 300f && isPuking)
			{
				NPC.frameCounter = 0;
				isPuking = false;
			}

			if (player.position.Y < NPC.position.Y - 100)
				isPuking = false;
			
			if (!Collision.CanHitLine(NPC.Center, 0, 0, Main.player[NPC.target].Center, 0, 0))
				isPuking = false;
			
			if (isPuking && NPC.frameCounter >= 14)
			{
				if (pukeTimer % 2 == 0 && Main.netMode != NetmodeID.MultiplayerClient)
				{
					float num5 = 8f;
					Vector2 vector2 = new Vector2(NPC.Center.X, NPC.position.Y - 13 + NPC.height * 0.5f);
					float num6 = Main.player[NPC.target].position.X + Main.player[NPC.target].width * 0.5f - vector2.X;
					float num7 = Math.Abs(num6) * 0.1f;
					float num8 = Main.player[NPC.target].position.Y + Main.player[NPC.target].height * 0.5f - vector2.Y - num7;
					float num14 = (float)Math.Sqrt(num6 * num6 + num8 * num8);
					NPC.netUpdate = true;
					float num15 = num5 / num14;
					float num16 = num6 * num15;
					float SpeedY = num8 * num15;
					Projectile.NewProjectile(NPC.GetSource_FromAI(), vector2.X, vector2.Y, num16, SpeedY, ModContent.ProjectileType<Zombie_Puke>(), 10, 0.0f, Main.myPlayer, 0.0f, 0.0f);
				}
			}

			if (Vector2.Distance(player.Center, NPC.Center) < 300f)
				delayTimer++;

			if (delayTimer >= 180)
			{
				isPuking = false;
				if (delayTimer >= 360)
					delayTimer = 0;
			}
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.AddCommon(216, 3);
			npcLoot.AddCommon(1304, 10);
			npcLoot.AddCommon<Items.Sets.ThrownMisc.FlaskofGore.FlaskOfGore>(1, 108, 162);
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (NPC.life <= 0)
			{
				for (int i = 1; i < 5; ++i)
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/UndeadScientist/UndeadScientistGore" + i).Type, 1f);
				SoundEngine.PlaySound(SoundID.Item9, NPC.Center);
			}

			for (int k = 0; k < 20; k++)
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, 2.5f * hitDirection, -2.5f, 0, default, Main.rand.NextFloat(0.5f, 1.2f));
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (NPC.AnyNPCs(ModContent.NPCType<Dead_Scientist>()))
				return 0f;
			return SpawnCondition.OverworldNightMonster.Chance * 0.002f;
		}

		public override void FindFrame(int frameHeight)
		{
			NPC.frameCounter++;
			
			if (NPC.velocity.Y == 0f)
			{
				if (!isPuking)
				{
					if (NPC.frameCounter < 7)
						NPC.frame.Y = 1 * frameHeight;
					else if (NPC.frameCounter < 14)
						NPC.frame.Y = 2 * frameHeight;
					else if (NPC.frameCounter < 21)
						NPC.frame.Y = 3 * frameHeight;
					else if (NPC.frameCounter < 28)
						NPC.frame.Y = 4 * frameHeight;
					else if (NPC.frameCounter < 35)
						NPC.frame.Y = 5 * frameHeight;
					else
						NPC.frameCounter = 0;
				}
				else
				{
					NPC.velocity.X = 0f;
					if (NPC.frameCounter < 7)
						NPC.frame.Y = 6 * frameHeight;
					else if (NPC.frameCounter < 14)
						NPC.frame.Y = 7 * frameHeight;
					else if (NPC.frameCounter < 21)
						NPC.frame.Y = 8 * frameHeight;
					else if (NPC.frameCounter < 28)
						NPC.frame.Y = 9 * frameHeight;
					else if (NPC.frameCounter < 35)
						NPC.frame.Y = 10 * frameHeight;
					else
						NPC.frameCounter = 14;
				}
			}
			else
				NPC.frame.Y = 0 * frameHeight;
		}
	}
}