using SpiritMod.Items.Consumable.Fish;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Bestiary;

namespace SpiritMod.NPCs.Critters
{
	public class Bladetongue : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bladetongue");
			Main.npcFrameCount[NPC.type] = 6;
		}

		public override void SetDefaults()
		{
			NPC.width = 44;
			NPC.height = 24;
			NPC.damage = 50;
			NPC.defense = 8;
			NPC.lifeMax = 380;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.knockBackResist = 0f;
			NPC.aiStyle = 16;
			NPC.dontCountMe = true;
			NPC.noGravity = true;
			NPC.npcSlots = 0;
			AIType = NPCID.Shark;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCrimson,
				new FlavorTextBestiaryInfoElement("If you can manage to capture one, it may make for an excellent weapon. But do so carefully, for its fatal ichor spit can leave you defenseless."),
			});
		}

		public override void FindFrame(int frameHeight)
		{
			NPC.frameCounter += 0.15f;
			NPC.frameCounter %= Main.npcFrameCount[NPC.type];
			int frame = (int)NPC.frameCounter;
			NPC.frame.Y = frame * frameHeight;
		}

		public override void AI() => NPC.spriteDirection = NPC.direction;

		public override void HitEffect(int hitDirection, double damage)
		{
			if (NPC.life <= 0)
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("BladetongueGore").Type, 1f);

			for (int k = 0; k < 11; k++)
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, NPC.direction, -1f, 1, default, .91f);
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if (Main.rand.NextBool(4))
				target.AddBuff(BuffID.Bleeding, 1200);
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.AddCommon(ItemID.Bladetongue, 2);
			npcLoot.AddCommon<RawFish>(2);
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) => spawnInfo.Player.ZoneCrimson && spawnInfo.Water && Main.hardMode ? 0.0075f : 0f;
	}
}
