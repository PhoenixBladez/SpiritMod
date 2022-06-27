using SpiritMod.Items.Consumable;
using SpiritMod.Items.Consumable.Fish;
using SpiritMod.Tiles.Furniture;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Placeable
{
	public class FishCrate : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Packing Crate");
			Tooltip.SetDefault("'A logo from a popular fishing company can be seen'\nRight click to open\nContains different types of fish");
		}

		public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 20;
			Item.rare = ItemRarityID.Orange;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.createTile = ModContent.TileType<FishCrate_Tile>();
			Item.maxStack = 999;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.consumable = true;
		}

		public override bool CanRightClick() => true;

		public override void Update(ref float gravity, ref float maxFallSpeed)
		{
			if (Item.wet)
			{
				gravity *= 0f;
				maxFallSpeed *= -.09f;
			}
			else
			{
				maxFallSpeed *= 1f;
				gravity *= 1f;
			}
		}

		public override void RightClick(Player player)
		{
			if (Main.rand.Next(2) == 0)
				player.QuickSpawnItem(ModContent.ItemType<RawFish>());

			if (Main.rand.Next(4) == 0)
			{
				if (Main.rand.NextBool())
					player.QuickSpawnItem(ModContent.ItemType<FloaterItem>());
				else
					player.QuickSpawnItem(ModContent.ItemType<LuvdiscItem>());
			}

			int[] lootTable = { ItemID.Shrimp, ItemID.Salmon, ItemID.Bass, ItemID.RedSnapper, ItemID.Trout };
			player.QuickSpawnItem(lootTable[Main.rand.Next(lootTable.Length)], Main.rand.Next(3, 5));

			if (Main.rand.Next(4) == 1)
			{
				int[] lootTable3 = { ItemID.ArmoredCavefish, ItemID.Damselfish, ItemID.DoubleCod, ItemID.FrostMinnow };
				player.QuickSpawnItem(lootTable3[Main.rand.Next(lootTable3.Length)], Main.rand.Next(1, 2));
			}

			if (Main.rand.Next(27) == 0)
			{
				int[] lootTable4 = { ItemID.ReaverShark, ItemID.Swordfish, ItemID.SawtoothShark };
				player.QuickSpawnItem(lootTable4[Main.rand.Next(lootTable4.Length)]);
			}

			if (Main.rand.Next(14) == 0)
			{
				string[] lootTable2123 = { "DiverLegs", "DiverHead", "DiverBody" };
				int loot443 = Main.rand.Next(lootTable2123.Length);
				player.QuickSpawnItem(Mod.Find<ModItem>(lootTable2123[loot443]).Type);
			}

			if (Main.rand.Next(3) == 0)
			{
				int[] lootTable2 = { ItemID.FrostDaggerfish, ItemID.BombFish };
				player.QuickSpawnItem(lootTable2[Main.rand.Next(lootTable2.Length)], Main.rand.Next(9, 12));
			}

			if (Main.hardMode && Main.rand.Next(10) == 0)
			{
				int[] lootTable51 = { ItemID.FlarefinKoi, ItemID.Obsidifish, ItemID.Prismite, ItemID.PrincessFish };
				player.QuickSpawnItem(lootTable51[Main.rand.Next(lootTable51.Length)], Main.rand.Next(1, 3));
			}

			if (Main.rand.Next(3) == 0)
				player.QuickSpawnItem(ItemID.SilverCoin, Main.rand.Next(10, 90));

			if (Main.rand.Next(7) == 0)
				player.QuickSpawnItem(ItemID.GoldCoin, Main.rand.Next(1, 3));
		}
	}
}