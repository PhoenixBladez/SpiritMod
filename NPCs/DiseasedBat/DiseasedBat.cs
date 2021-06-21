using SpiritMod.Buffs;
using SpiritMod.Items.Sets.Bismite;
using SpiritMod.Items.Consumable.Food;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.DiseasedBat
{
	public class DiseasedBat : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Diseased Bat");
			Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.CaveBat];
		}

		public override void SetDefaults()
		{
			npc.width = 26;
			npc.height = 18;
			npc.damage = 16;
			npc.defense = 5;
			npc.lifeMax = 21;
			npc.buffImmune[BuffID.Poisoned] = true;
			npc.buffImmune[ModContent.BuffType<FesteringWounds>()] = true;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath4;
			npc.value = 60f;
			npc.knockBackResist = .45f;
			npc.aiStyle = 14;
			aiType = NPCID.CaveBat;
			animationType = NPCID.CaveBat;
			banner = npc.type;
			bannerItem = ModContent.ItemType<Items.Banners.DiseasedBatBanner>();
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (spawnInfo.playerSafe) {
				return 0f;
			}
			return SpawnCondition.Underground.Chance * 0.22f;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0) {
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/DBat1"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Dbat2"), 1f);
			}
		}
		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if (Main.rand.Next(3) == 0) {
				target.AddBuff(ModContent.BuffType<FesteringWounds>(), 180);
			}
			if (Main.rand.Next(10) == 0 && Main.expertMode) {
				target.AddBuff(148, 2000);
			}
		}
		public override void NPCLoot()
		{
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<BismiteCrystal>(), Main.rand.Next(2, 4) + 1);
			if (Main.rand.Next(250) == 0) {
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.DepthMeter);
			}
			if (Main.rand.Next(100) == 0) {
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.ChainKnife);
			}
            if (Main.rand.NextBool(16))
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Cake>());
            }
        }

	}
}
