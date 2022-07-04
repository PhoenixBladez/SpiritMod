using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.ZombieVariants
{
	public class SailorZombie : ModNPC
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
			NPC.damage = 14;
			NPC.defense = 6;
			NPC.lifeMax = 45;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath2;
			NPC.value = 90f;
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
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, 2.5f * hitDirection, -2.5f, 0, default, .54f);
			}
			if (NPC.life <= 0)
			{
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("SailorZombie1").Type, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("SailorZombie2").Type, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("SailorZombie3").Type, 1f);
			}
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.AddCommon(ItemID.Shackle, 50);
			npcLoot.AddCommon(ItemID.ZombieArm, 250);
			npcLoot.AddCommon<Items.Sets.FloatingItems.Kelp>(10, 1, 2);
			npcLoot.AddCommon<Items.Armor.Masks.SailorCap>(50);
		}
	}
}