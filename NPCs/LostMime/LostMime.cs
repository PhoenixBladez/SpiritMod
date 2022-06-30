using SpiritMod.Items.Armor;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Items.Weapon.Thrown;
using Terraria.ModLoader.Utilities;

namespace SpiritMod.NPCs.LostMime
{
	public class LostMime : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lost Mime");
			Main.npcFrameCount[NPC.type] = 14;
		}

		public override void SetDefaults()
		{
			NPC.width = 24;
			NPC.height = 42;
			NPC.damage = 30;
			NPC.defense = 10;
			NPC.lifeMax = 200;
			NPC.value = 80f;
			NPC.knockBackResist = .25f;
			NPC.aiStyle = 3;
			NPC.buffImmune[BuffID.Confused] = true;
			AIType = NPCID.SnowFlinx;
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<Items.Banners.LostMimeBanner>();
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (spawnInfo.PlayerSafe)
				return 0f;
			return SpawnCondition.Cavern.Chance * 0.015f;
		}

		public override void FindFrame(int frameHeight)
		{
			NPC.frameCounter += 0.25f;
			NPC.frameCounter %= Main.npcFrameCount[NPC.type];
			int frame = (int)NPC.frameCounter;
			NPC.frame.Y = frame * frameHeight;
		}

		public override void AI() => NPC.spriteDirection = NPC.direction;

		public override void OnHitPlayer(Player target, int damage, bool crit) => target.AddBuff(BuffID.Confused, 60);

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 10; k++)
			{
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, 2.5f * hitDirection, -2.5f, 0, default, 0.27f);
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, 2.5f * hitDirection, -2.5f, 0, default, 0.87f);
			}

			if (NPC.life <= 0)
			{
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/LostMimeGore").Type, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, 99);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, 99);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, 99);
			}
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.AddCommon(ModContent.ItemType<MimeMask>(), 16);
			npcLoot.AddFood(ModContent.ItemType<Items.Consumable.Food.Baguette>(), 30);
			npcLoot.AddCommon(ModContent.ItemType<MimeBomb>(), 16, 12, 22);
		}
	}
}