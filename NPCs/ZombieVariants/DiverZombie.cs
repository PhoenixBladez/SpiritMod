using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.ZombieVariants
{
	public class DiverZombie : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Zombie");
			Main.npcFrameCount[npc.type] = 4;
		}

		public override void SetDefaults()
		{
			npc.width = 28;
			npc.height = 42;
			npc.damage = 16;
			npc.defense = 6;
			npc.lifeMax = 40;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath2;
			npc.value = 70f;
			npc.knockBackResist = .45f;
			npc.aiStyle = 3;
			aiType = NPCID.Zombie;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 20; k++)
			{
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.Blood, 2.5f * hitDirection, -2.5f, 0, Color.White, 0.78f);
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.Blood, 2.5f * hitDirection, -2.5f, 0, default, .54f);
			}
			if (npc.life <= 0)
			{
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/DiverZombie/DiverZombie1"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/DiverZombie/DiverZombie2"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/DiverZombie/DiverZombie3"), 1f);
			}
		}

		int frameTimer;
		int frame;
		public override void AI()
		{
			npc.spriteDirection = npc.direction;
			frameTimer++;
			if (npc.wet)
			{
				npc.noGravity = true;
				npc.velocity.Y *= .9f;
				npc.velocity.Y -= .09f;
				npc.velocity.X *= .95f;
				npc.rotation = npc.velocity.X * .1f;
				if (frameTimer >= 50)
				{
					frame++;
					frameTimer = 0;
				}

				if (frame > 3 || frame < 2)
					frame = 2;
			}
			else
			{
				npc.noGravity = false;
				if (npc.velocity.Y != 0)
				{
					frame = 2;
				}
				else
				{
					if (frameTimer >= 12)
					{
						frame++;
						frameTimer = 0;
					}

					if (frame > 2)
						frame = 0;
				}
			}
		}

		public override void FindFrame(int frameHeight) => npc.frame.Y = frameHeight * frame;

		public override void NPCLoot()
		{
			if (Main.rand.Next(50) == 0)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Shackle);
			if (Main.rand.Next(250) == 0)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.ZombieArm);
			if (Main.rand.Next(100) == 0)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Flipper);

			string[] lootTable = { "DiverLegs", "DiverHead", "DiverBody" };
			if (Main.rand.Next(65) == 0)
			{
				int loot = Main.rand.Next(lootTable.Length);
				npc.DropItem(mod.ItemType(lootTable[loot]));
			}
		}
	}
}