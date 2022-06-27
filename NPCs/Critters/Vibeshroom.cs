using Microsoft.Xna.Framework;
using SpiritMod.Items.Consumable;
using SpiritMod.Mechanics.QuestSystem;
using SpiritMod.Mechanics.QuestSystem.Quests;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Critters
{
	public class Vibeshroom : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Quivershroom");
			Main.npcFrameCount[NPC.type] = 14;
			Main.npcCatchable[NPC.type] = true;
		}

		public override void SetDefaults()
		{
			NPC.width = 18;
			NPC.height = 20;
			NPC.damage = 0;
			NPC.dontCountMe = true;
			NPC.defense = 0;
			NPC.lifeMax = 5;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.catchItem = (short)ModContent.ItemType<VibeshroomItem>();
			NPC.knockBackResist = .45f;
			NPC.aiStyle = 0;
			NPC.npcSlots = 0;
			NPC.noGravity = false;
			NPC.dontTakeDamageFromHostiles = false;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 20; k++)
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Corruption_Gravity, 2.5f * hitDirection, -2.5f, 0, Color.White, Main.rand.NextFloat(.2f, .8f));

			if (NPC.life <= 0)
				Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/Vibeshroom1").Type, 1f);
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			bool valid = spawnInfo.SpawnTileType == TileID.MushroomGrass && NPC.CountNPCS(ModContent.NPCType<Vibeshroom>()) < 5;
			if (!valid)
				return 0;
			if (QuestManager.GetQuest<SporeSalvage>().IsActive && !NPC.AnyNPCs(ModContent.NPCType<Vibeshroom>()))
				return 1.15f;
			return 0.03f;
		}

		public override void FindFrame(int frameHeight)
		{
			NPC.frameCounter += 0.08f;
			NPC.frameCounter %= Main.npcFrameCount[NPC.type];
			int frame = (int)NPC.frameCounter;
			NPC.frame.Y = frame * frameHeight;
		}
		public override bool PreAI()
		{
			Lighting.AddLight((int)((NPC.position.X + (float)(NPC.width / 2)) / 16f), (int)((NPC.position.Y + (float)(NPC.height / 2)) / 16f), .042f * 2, .115f * 2, .233f * 2);
			return true;
		}
	}
}
