using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using SpiritMod.Items.Armor.DiverSet;

namespace SpiritMod.NPCs.ZombieVariants
{
	public class DiverZombie : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Zombie");
			Main.npcFrameCount[NPC.type] = 4;
		}

		public override void SetDefaults()
		{
			NPC.width = 28;
			NPC.height = 42;
			NPC.damage = 16;
			NPC.defense = 6;
			NPC.lifeMax = 40;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath2;
			NPC.value = 70f;
			NPC.knockBackResist = .45f;
			NPC.aiStyle = 3;
			AIType = NPCID.Zombie;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 20; k++)
			{
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, 2.5f * hitDirection, -2.5f, 0, Color.White, 0.78f);
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, 2.5f * hitDirection, -2.5f, 0, default, .54f);
			}
			
			if (NPC.life <= 0)
			{
				for (int i = 1; i < 4; ++i)
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("SpiritMod/Gores/DiverZombie/DiverZombie" + i).Type, 1f);
			}
		}

		int frameTimer;
		int frame;

		public override void AI()
		{
			NPC.spriteDirection = NPC.direction;
			frameTimer++;
			if (NPC.wet)
			{
				NPC.noGravity = true;
				NPC.velocity.Y *= .9f;
				NPC.velocity.Y -= .09f;
				NPC.velocity.X *= .95f;
				NPC.rotation = NPC.velocity.X * .1f;
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
				NPC.noGravity = false;
				if (NPC.velocity.Y != 0)
					frame = 2;
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

		public override void FindFrame(int frameHeight) => NPC.frame.Y = frameHeight * frame;

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.AddCommon(ItemID.Shackle, 50);
			npcLoot.AddCommon(ItemID.ZombieArm, 250);
			npcLoot.AddCommon(ItemID.Flipper, 100);
			npcLoot.AddOneFromOptions(65, ModContent.ItemType<DiverLegs>(), ModContent.ItemType<DiverHead>(), ModContent.ItemType<DiverBody>());
		}
	}
}