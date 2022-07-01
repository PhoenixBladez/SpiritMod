using SpiritMod.Items.Material;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace SpiritMod.NPCs.ChestZombie
{
	public class Chest_Zombie : ModNPC
	{
		public int dashTimer = 0;
		public bool isDashing = false;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chest Zombie");
			Main.npcFrameCount[NPC.type] = 17;
		}

		public override void SetDefaults()
		{
			NPC.lifeMax = 120;
			NPC.defense = 10;
			NPC.value = 100f;
			AIType = -1;
			NPC.knockBackResist = 0.2f;
			NPC.width = 30;
			NPC.height = 50;
			NPC.damage = 17;
			NPC.lavaImmune = true;
			NPC.noTileCollide = false;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath2;
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<Items.Banners.ChestZombieBanner>();
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) => SpawnCondition.OverworldNightMonster.Chance * 0.011f;

		public override void AI()
		{
			NPC.TargetClosest(true);

			if (NPC.velocity.X > 0f)
				NPC.spriteDirection = 1;
			else
				NPC.spriteDirection = -1;

			dashTimer++;
			NPC.aiStyle = 3;
			if (dashTimer >= 120)
			{
				NPC.aiStyle = -1;
				NPC.velocity.X *= 0.98f;
				NPC.damage = 60;
				if (dashTimer == 120)
				{
					NPC.frameCounter = 0;
					NPC.netUpdate = true;
				}
				isDashing = true;

				Collision.StepUp(ref NPC.position, ref NPC.velocity, NPC.width, NPC.height, ref NPC.stepSpeed, ref NPC.gfxOffY);

				if (NPC.velocity.X == 0)
					dashTimer = 0;
			}
			else
			{
				NPC.damage = 30;
				if (Math.Abs(NPC.velocity.X) > 4)
					NPC.velocity.X *= 0.94f;
			}
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.AddCommon<Zombie_Chest>();
			npcLoot.AddCommon<OldLeather>(1, 5);
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 4; k++)
			{
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, 2.5f * hitDirection, -2.5f, 0, default, 1.2f);
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, 2.5f * hitDirection, -2.5f, 0, default, 0.5f);
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, 2.5f * hitDirection, -2.5f, 0, default, 0.7f);
			}

			if (NPC.life <= 0)
				for (int i = 1; i < 5; ++i)
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/ChestZombie/ChestZombieGore" + i).Type, 1f);
		}

		public override void FindFrame(int frameHeight)
		{
			NPC.frameCounter++;

			if (NPC.velocity.X != 0f)
			{
				if (NPC.velocity.Y != 0f)
					NPC.frame.Y = 0 * frameHeight;
				else if (isDashing)
				{
					if (NPC.frameCounter < 6)
						NPC.frame.Y = 8 * frameHeight;
					else if (NPC.frameCounter < 12)
						NPC.frame.Y = 9 * frameHeight;
					else if (NPC.frameCounter < 18)
						NPC.frame.Y = 10 * frameHeight;
					else if (NPC.frameCounter < 24)
						NPC.frame.Y = 11 * frameHeight;
					else if (NPC.frameCounter < 30)
					{
						if (NPC.frameCounter == 25)
						{
							SoundEngine.PlaySound(SoundID.Trackable, (int)NPC.position.X, (int)NPC.position.Y, 220, 1f, 0f);
							NPC.velocity.X = 10f * NPC.direction;
							NPC.velocity.Y = 0f;
							NPC.netUpdate = true;
						}
						NPC.frame.Y = 12 * frameHeight;
					}
					else if (NPC.frameCounter < 36)
						NPC.frame.Y = 13 * frameHeight;
					else if (NPC.frameCounter < 42)
						NPC.frame.Y = 14 * frameHeight;
					else if (NPC.frameCounter < 48)
						NPC.frame.Y = 15 * frameHeight;
					else if (NPC.frameCounter < 54)
					{
						NPC.frame.Y = 16 * frameHeight;
						if (NPC.frameCounter == 49)
						{
							dashTimer = 0;
							isDashing = false;
						}
					}
					else
						NPC.frameCounter = 0;
				}
				else
				{
					if (NPC.frameCounter < 7)
						NPC.frame.Y = 0 * frameHeight;
					else if (NPC.frameCounter < 14)
						NPC.frame.Y = 1 * frameHeight;
					else if (NPC.frameCounter < 21)
						NPC.frame.Y = 2 * frameHeight;
					else if (NPC.frameCounter < 28)
						NPC.frame.Y = 3 * frameHeight;
					else if (NPC.frameCounter < 35)
						NPC.frame.Y = 4 * frameHeight;
					else if (NPC.frameCounter < 42)
						NPC.frame.Y = 5 * frameHeight;
					else if (NPC.frameCounter < 49)
						NPC.frame.Y = 6 * frameHeight;
					else if (NPC.frameCounter < 56)
						NPC.frame.Y = 7 * frameHeight;
					else
						NPC.frameCounter = 0;
				}
			}
			else
				NPC.frame.Y = 0 * frameHeight;
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(dashTimer);
			writer.Write(isDashing);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			dashTimer = reader.ReadInt32();
			isDashing = reader.ReadBoolean();
		}
	}
}