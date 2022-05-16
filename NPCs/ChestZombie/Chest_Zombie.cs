using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.ChestZombie
{
	public class Chest_Zombie : ModNPC
	{
		public int dashTimer = 0;
		public bool isDashing = false;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chest Zombie");
			Main.npcFrameCount[npc.type] = 17;
		}

		public override void SetDefaults()
		{
			npc.lifeMax = 120;
			npc.defense = 10;
			npc.value = 100f;
			aiType = -1;
			npc.knockBackResist = 0.2f;
			npc.width = 30;
			npc.height = 50;
			npc.damage = 17;
			npc.lavaImmune = true;
			npc.noTileCollide = false;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath2;
			banner = npc.type;
			bannerItem = ModContent.ItemType<Items.Banners.ChestZombieBanner>();
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) => SpawnCondition.OverworldNightMonster.Chance * 0.011f;

		public override void AI()
		{
			npc.TargetClosest(true);

			if (npc.velocity.X > 0f)
				npc.spriteDirection = 1;
			else
				npc.spriteDirection = -1;

			dashTimer++;
			npc.aiStyle = 3;
			if (dashTimer >= 120)
			{
				npc.aiStyle = -1;
				npc.velocity.X *= 0.98f;
				npc.damage = 60;
				if (dashTimer == 120)
				{
					npc.frameCounter = 0;
					npc.netUpdate = true;
				}
				isDashing = true;

				Collision.StepUp(ref npc.position, ref npc.velocity, npc.width, npc.height, ref npc.stepSpeed, ref npc.gfxOffY);

				if (npc.velocity.X == 0)
					dashTimer = 0;
			}
			else
			{
				npc.damage = 30;
				if (Math.Abs(npc.velocity.X) > 4)
					npc.velocity.X *= 0.94f;
			}
		}

		public override void NPCLoot()
		{
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("Zombie_Chest"));
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("OldLeather"), 5);
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 4; k++)
			{
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.Blood, 2.5f * hitDirection, -2.5f, 0, default, 1.2f);
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.Blood, 2.5f * hitDirection, -2.5f, 0, default, 0.5f);
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.Blood, 2.5f * hitDirection, -2.5f, 0, default, 0.7f);
			}

			if (npc.life <= 0)
				for (int i = 1; i < 5; ++i)
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/ChestZombie/ChestZombieGore" + i), 1f);
		}

		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter++;

			if (npc.velocity.X != 0f)
			{
				if (npc.velocity.Y != 0f)
					npc.frame.Y = 0 * frameHeight;
				else if (isDashing)
				{
					if (npc.frameCounter < 6)
						npc.frame.Y = 8 * frameHeight;
					else if (npc.frameCounter < 12)
						npc.frame.Y = 9 * frameHeight;
					else if (npc.frameCounter < 18)
						npc.frame.Y = 10 * frameHeight;
					else if (npc.frameCounter < 24)
						npc.frame.Y = 11 * frameHeight;
					else if (npc.frameCounter < 30)
					{
						if (npc.frameCounter == 25)
						{
							Main.PlaySound(SoundID.Trackable, (int)npc.position.X, (int)npc.position.Y, 220, 1f, 0f);
							npc.velocity.X = 10f * npc.direction;
							npc.velocity.Y = 0f;
							npc.netUpdate = true;
						}
						npc.frame.Y = 12 * frameHeight;
					}
					else if (npc.frameCounter < 36)
						npc.frame.Y = 13 * frameHeight;
					else if (npc.frameCounter < 42)
						npc.frame.Y = 14 * frameHeight;
					else if (npc.frameCounter < 48)
						npc.frame.Y = 15 * frameHeight;
					else if (npc.frameCounter < 54)
					{
						npc.frame.Y = 16 * frameHeight;
						if (npc.frameCounter == 49)
						{
							dashTimer = 0;
							isDashing = false;
						}
					}
					else
						npc.frameCounter = 0;
				}
				else
				{
					if (npc.frameCounter < 7)
						npc.frame.Y = 0 * frameHeight;
					else if (npc.frameCounter < 14)
						npc.frame.Y = 1 * frameHeight;
					else if (npc.frameCounter < 21)
						npc.frame.Y = 2 * frameHeight;
					else if (npc.frameCounter < 28)
						npc.frame.Y = 3 * frameHeight;
					else if (npc.frameCounter < 35)
						npc.frame.Y = 4 * frameHeight;
					else if (npc.frameCounter < 42)
						npc.frame.Y = 5 * frameHeight;
					else if (npc.frameCounter < 49)
						npc.frame.Y = 6 * frameHeight;
					else if (npc.frameCounter < 56)
						npc.frame.Y = 7 * frameHeight;
					else
						npc.frameCounter = 0;
				}
			}
			else
				npc.frame.Y = 0 * frameHeight;
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