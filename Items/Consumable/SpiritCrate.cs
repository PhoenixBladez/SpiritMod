using SpiritMod.Items.Material;
using SpiritMod.Items.Weapon.Gun;
using SpiritMod.Items.Weapon.Thrown;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritCrateTile = SpiritMod.Tiles.Furniture.SpiritCrate;

namespace SpiritMod.Items.Consumable
{
	public class SpiritCrate : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spirit Crate");
			Tooltip.SetDefault("Right click to open");
		}


		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 20;
			item.rare = ItemRarityID.Pink;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.createTile = ModContent.TileType<SpiritCrateTile>();
			item.maxStack = 999;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.consumable = true;

		}
		public override bool CanRightClick()
		{
			return true;
		}

		public override void RightClick(Player player)
		{
			int[] lootTable = {
				ModContent.ItemType<SpiritOre>(),
				ModContent.ItemType<Rune>(),
				ModContent.ItemType<StarPiece>(),
				ModContent.ItemType<MoonStone>()
			};
			int loot = Main.rand.Next(lootTable.Length);

			player.QuickSpawnItem(lootTable[loot], Main.rand.Next(3, 5));
			if (Main.rand.Next(4) == 1) {
				int[] lootTable3 = {
					ModContent.ItemType<SpiritOre>(),
					ModContent.ItemType<Rune>(),
					ModContent.ItemType<StarPiece>(),
					ModContent.ItemType<MoonStone>()
				};
				int loot3 = Main.rand.Next(lootTable3.Length);

				player.QuickSpawnItem(lootTable3[loot3], Main.rand.Next(3, 5));
			}
			if (Main.rand.Next(7) == 0) {
				player.QuickSpawnItem(ModContent.ItemType<SoulStinger>());
			}
            if (Main.rand.Next(10) == 0)
            {
                player.QuickSpawnItem(ModContent.ItemType<Items.Books.Book_SpiritArt>());
            }
            if (Main.rand.Next(6) == 0) {
				int[] lootTable2 = {
					ModContent.ItemType<StarCutter>(),
					ModContent.ItemType<GhostJellyBomb>()
				};
				int loot2 = Main.rand.Next(lootTable2.Length);

				player.QuickSpawnItem(lootTable2[loot2], Main.rand.Next(30, 80));
			}
		}
	}
}
