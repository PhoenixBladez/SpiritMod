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
			Main.npcFrameCount[npc.type] = 14;
			Main.npcCatchable[npc.type] = true;
		}

		public override void SetDefaults()
		{
			npc.width = 18;
			npc.height = 20;
			npc.damage = 0;
			npc.dontCountMe = true;
			npc.defense = 0;
			npc.lifeMax = 5;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.catchItem = (short)ModContent.ItemType<VibeshroomItem>();
			npc.knockBackResist = .45f;
			npc.aiStyle = 0;
			npc.npcSlots = 0;
			npc.noGravity = false;
			npc.dontTakeDamageFromHostiles = false;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 20; k++)
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.Corruption_Gravity, 2.5f * hitDirection, -2.5f, 0, Color.White, Main.rand.NextFloat(.2f, .8f));

			if (npc.life <= 0)
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Vibeshroom1"), 1f);
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			bool valid = spawnInfo.spawnTileType == TileID.MushroomGrass && NPC.CountNPCS(ModContent.NPCType<Vibeshroom>()) < 5;
			if (!valid)
				return 0;
			if (QuestManager.GetQuest<SporeSalvage>().IsActive && !NPC.AnyNPCs(ModContent.NPCType<Vibeshroom>()))
				return 1.15f;
			return 0.03f;
		}

		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += 0.08f;
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
		}
		public override bool PreAI()
		{
			Lighting.AddLight((int)((npc.position.X + (float)(npc.width / 2)) / 16f), (int)((npc.position.Y + (float)(npc.height / 2)) / 16f), .042f * 2, .115f * 2, .233f * 2);
			return true;
		}
	}
}
