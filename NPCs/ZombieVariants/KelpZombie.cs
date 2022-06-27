using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.ZombieVariants
{
	public class KelpZombie : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Zombie");
			Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.Zombie];
		}

		public override void SetDefaults()
		{
			NPC.width = 28;
			NPC.height = 42;
			NPC.damage = 12;
			NPC.defense = 8;
			NPC.lifeMax = 50;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath2;
			NPC.value = 50f;
			NPC.knockBackResist = .5f;
			NPC.aiStyle = 3;
			AIType = NPCID.Zombie;
			AnimationType = NPCID.Zombie;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 20; k++)
			{
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, 2.5f * hitDirection, -2.5f, 0, Color.White, 0.78f);
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, 2.5f * hitDirection, -2.5f, 0, Color.Green, .54f);
			}
			if (NPC.life <= 0)
			{
				Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/KelpZombie/KelpZombie1").Type, 1f);
				Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/KelpZombie/KelpZombie2").Type, 1f);
				Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/KelpZombie/KelpZombie3").Type, 1f);
			}
		}

		public override void OnKill()
		{
			if (Main.rand.Next(5) == 0)
				Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ModContent.ItemType<Items.Sets.FloatingItems.Kelp>(), Main.rand.Next(2) + 3);
			if (Main.rand.Next(50) == 0)
				Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ItemID.Shackle);
			if (Main.rand.Next(250) == 0)
				Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ItemID.ZombieArm);
		}
	}
}