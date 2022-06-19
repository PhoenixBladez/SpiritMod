using SpiritMod.Items.Consumable;
using SpiritMod.Items.Consumable.Fish;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Critters
{
	public class FishCrate : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Packing Crate");

			Main.npcFrameCount[npc.type] = 1;
			Main.npcCatchable[npc.type] = true;
		}

		public override void SetDefaults()
		{
			npc.width = 40;
			npc.height = 40;
			npc.damage = 0;
			npc.knockBackResist = 0f;
			npc.defense = 1000;
			npc.HitSound = SoundID.NPCHit4;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.lifeMax = 10;
			npc.aiStyle = 1;
			npc.npcSlots = 0;
			npc.catchItem = (short)ModContent.ItemType<Items.Placeable.FishCrate>();
			npc.noGravity = false;
			aiType = NPCID.Grasshopper;
			npc.alpha = 40;
			npc.dontCountMe = true;
		}

		public override void AI()
		{
			npc.spriteDirection = -npc.direction;

			if (npc.wet)
			{
				npc.aiStyle = 1;
				npc.npcSlots = 0;
				npc.noGravity = false;
				aiType = NPCID.Grasshopper;
				npc.velocity.X *= 0f;
				npc.velocity.Y *= .9f;
			}
			else
			{
				npc.aiStyle = 0;
				npc.npcSlots = 0;
				npc.noGravity = false;
				aiType = NPCID.BoundGoblin;
			}
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/FishCrate/FishCrate1"), 1f);
				for (int i = 0; i < 6; i++)
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/FishCrate/FishCrate2"), Main.rand.NextFloat(.5f, 1f));
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/FishCrate/FishCrate3"), 1f);
			}
		}

		public override void NPCLoot()
		{
			if (Main.rand.Next(2) == 0)
				npc.DropItem(ModContent.ItemType<RawFish>());

			if (Main.rand.Next(4) == 0)
				npc.DropItem(Main.rand.NextBool() ? ModContent.ItemType<FloaterItem>() : ModContent.ItemType<LuvdiscItem>());

			int[] lootTable = { ItemID.Shrimp, ItemID.Salmon, ItemID.Bass, ItemID.RedSnapper, ItemID.Trout };
			npc.DropItem(Main.rand.Next(lootTable), Main.rand.Next(3, 5));

			if (Main.rand.Next(4) == 1)
			{
				int[] lootTable3 = { ItemID.ArmoredCavefish, ItemID.Damselfish, ItemID.DoubleCod, ItemID.FrostMinnow };
				npc.DropItem(Main.rand.Next(lootTable3));
			}

			if (Main.rand.Next(27) == 0)
			{
				int[] lootTable4 = { ItemID.ReaverShark, ItemID.Swordfish, ItemID.SawtoothShark };
				npc.DropItem(Main.rand.Next(lootTable4));
			}

			if (Main.rand.Next(14) == 0)
			{
				string[] lootTable2123 = { "DiverLegs", "DiverHead", "DiverBody" };
				npc.DropItem(mod.ItemType(Main.rand.Next(lootTable2123)));
			}

			if (Main.rand.Next(3) == 0)
			{
				int[] lootTable2 = { ItemID.FrostDaggerfish, ItemID.BombFish };
				npc.DropItem(Main.rand.Next(lootTable2), Main.rand.Next(9, 12));
			}

			if (Main.hardMode && Main.rand.Next(10) == 0)
			{
				int[] lootTable51 = { ItemID.FlarefinKoi, ItemID.Obsidifish, ItemID.Prismite, ItemID.PrincessFish };
				npc.DropItem(Main.rand.Next(lootTable51), 1);
			}

			if (Main.rand.Next(3) == 0)
				npc.DropItem(ItemID.GoldCoin, Main.rand.Next(10, 90));

			if (Main.rand.Next(7) == 0)
				npc.DropItem(ItemID.GoldCoin, Main.rand.Next(1, 3));
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) => SpawnCondition.OceanMonster.Chance * 0.05f;
	}
}