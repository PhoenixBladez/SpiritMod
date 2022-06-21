using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

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
			Main.npcFrameCount[npc.type] = 11;
		}
		public override void SetDefaults()
		{
			npc.aiStyle = 3;
			npc.lifeMax = 50;
			npc.defense = 6;
			npc.value = 100f;
			aiType = 3;
			npc.knockBackResist = 0.5f;
			npc.width = 24;
			npc.height = 36;
			npc.damage = 28;
			npc.lavaImmune = false;
			npc.HitSound = new Terraria.Audio.LegacySoundStyle(3, 1);
		}

		public override void AI()
		{
			Player player = Main.player[npc.target];
			npc.TargetClosest(true);
			npc.spriteDirection = npc.direction;
			pukeTimer++;
			
			if (Main.rand.Next(300)==0)
				Main.PlaySound(SoundID.Item9.SoundId, (int)npc.position.X, (int)npc.position.Y, 1, 1f, -0.9f);
			
			if (Vector2.Distance(player.Center, npc.Center) < 300f && !isPuking && player.position.Y > npc.position.Y - 100 && Collision.CanHitLine(npc.Center, 0, 0, Main.player[npc.target].Center, 0, 0) && delayTimer < 180) 
			{
				Main.PlaySound(SoundID.Item9.SoundId, (int)npc.position.X, (int)npc.position.Y, 9, 1f, -0.9f);
				npc.frameCounter = 0;
				isPuking = true;
			}

			if (Vector2.Distance(player.Center, npc.Center) >= 300f && isPuking)
			{
				npc.frameCounter = 0;
				isPuking = false;
			}

			if (player.position.Y < npc.position.Y - 100)
				isPuking = false;
			
			if (!Collision.CanHitLine(npc.Center, 0, 0, Main.player[npc.target].Center, 0, 0))
				isPuking = false;
			
			if (isPuking && npc.frameCounter >= 14)
			{
				if (pukeTimer % 2 == 0 && Main.netMode != NetmodeID.MultiplayerClient)
				{
					float num5 = 8f;
					Vector2 vector2 = new Vector2(npc.Center.X, npc.position.Y - 13 + npc.height * 0.5f);
					float num6 = Main.player[npc.target].position.X + Main.player[npc.target].width * 0.5f - vector2.X;
					float num7 = Math.Abs(num6) * 0.1f;
					float num8 = Main.player[npc.target].position.Y + Main.player[npc.target].height * 0.5f - vector2.Y - num7;
					float num14 = (float)Math.Sqrt(num6 * num6 + num8 * num8);
					npc.netUpdate = true;
					float num15 = num5 / num14;
					float num16 = num6 * num15;
					float SpeedY = num8 * num15;
					Projectile.NewProjectile(vector2.X, vector2.Y, num16, SpeedY, ModContent.ProjectileType<Zombie_Puke>(), 10, 0.0f, Main.myPlayer, 0.0f, 0.0f);
				}
			}

			if (Vector2.Distance(player.Center, npc.Center) < 300f)
				delayTimer++;

			if (delayTimer >= 180)
			{
				isPuking = false;
				if (delayTimer >= 360)
					delayTimer = 0;
			}
		}
		public override void NPCLoot()
		{
			if (Main.rand.Next(3) == 0)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, 216, 1);
			if (Main.rand.Next(10) == 0)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, 1304, 1);
            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.Sets.ThrownMisc.FlaskofGore.FlaskOfGore>(), Main.rand.Next(108, 163));
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				for (int i = 1; i < 5; ++i)
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/UndeadScientist/UndeadScientistGore" + i), 1f);
				Main.PlaySound(SoundID.Item9.SoundId, (int)npc.position.X, (int)npc.position.Y, 22, 1f, -0.9f);
			}

			for (int k = 0; k < 20; k++)
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.Blood, 2.5f * hitDirection, -2.5f, 0, default, Main.rand.NextFloat(0.5f, 1.2f));
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (NPC.AnyNPCs(ModContent.NPCType<Dead_Scientist>()))
				return 0f;
			return SpawnCondition.OverworldNightMonster.Chance * 0.002f;
		}

		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter++;
			
			if (npc.velocity.Y == 0f)
			{
				if (!isPuking)
				{
					if (npc.frameCounter < 7)
						npc.frame.Y = 1 * frameHeight;
					else if (npc.frameCounter < 14)
						npc.frame.Y = 2 * frameHeight;
					else if (npc.frameCounter < 21)
						npc.frame.Y = 3 * frameHeight;
					else if (npc.frameCounter < 28)
						npc.frame.Y = 4 * frameHeight;
					else if (npc.frameCounter < 35)
						npc.frame.Y = 5 * frameHeight;
					else
						npc.frameCounter = 0;
				}
				else
				{
					npc.velocity.X = 0f;
					if (npc.frameCounter < 7)
						npc.frame.Y = 6 * frameHeight;
					else if (npc.frameCounter < 14)
						npc.frame.Y = 7 * frameHeight;
					else if (npc.frameCounter < 21)
						npc.frame.Y = 8 * frameHeight;
					else if (npc.frameCounter < 28)
						npc.frame.Y = 9 * frameHeight;
					else if (npc.frameCounter < 35)
						npc.frame.Y = 10 * frameHeight;
					else
						npc.frameCounter = 14;
				}
			}
			else
				npc.frame.Y = 0 * frameHeight;
		}
	}
}