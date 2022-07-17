using SpiritMod.Items.Material;
using SpiritMod.Items.Sets.SpiritSet;
using SpiritMod.Items.Sets.SeraphSet;
using SpiritMod.Items.Sets.RunicSet;
using SpiritMod.Items.Sets.SpiritBiomeDrops;
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
			Item.width = 20;
			Item.height = 20;
			Item.rare = ItemRarityID.Pink;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.createTile = ModContent.TileType<SpiritCrateTile>();
			Item.maxStack = 999;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.consumable = true;
		}

		public override bool CanRightClick() => true;

		public override void RightClick(Player player)
		{
			int[] lootTable = {
				ModContent.ItemType<SpiritOre>(),
				ModContent.ItemType<Rune>(),
				ModContent.ItemType<StarPiece>(),
				ModContent.ItemType<MoonStone>()
			};
			int loot = Main.rand.Next(lootTable.Length);

			player.QuickSpawnItem(player.GetSource_OpenItem(Item.type, "RightClick"), lootTable[loot], Main.rand.Next(3, 5));
			if (Main.rand.Next(4) == 1) {
				int[] lootTable3 = {
					ModContent.ItemType<SpiritOre>(),
					ModContent.ItemType<Rune>(),
					ModContent.ItemType<StarPiece>(),
					ModContent.ItemType<MoonStone>()
				};
				int loot3 = Main.rand.Next(lootTable3.Length);

				player.QuickSpawnItem(player.GetSource_OpenItem(Item.type, "RightClick"), lootTable3[loot3], Main.rand.Next(3, 5));
			}
            if (Main.rand.Next(10) == 0)
            {
                player.QuickSpawnItem(player.GetSource_OpenItem(Item.type, "RightClick"), ModContent.ItemType<Items.Books.Book_SpiritArt>());
            }
            if (Main.rand.Next(6) == 0) {
				int[] lootTable2 = {
					ModContent.ItemType<StarCutter>(),
					ModContent.ItemType<GhostJellyBomb>()
				};
				int loot2 = Main.rand.Next(lootTable2.Length);

				player.QuickSpawnItem(player.GetSource_OpenItem(Item.type, "RightClick"), lootTable2[loot2], Main.rand.Next(30, 80));
			}
		}
	}
}
