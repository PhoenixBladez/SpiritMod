using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable
{
	public class PirateCrate : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pirate Loot Crate");
			Tooltip.SetDefault("'Contains booty!'");
		}


		public override void SetDefaults()
		{
			Item.width = Item.height = 16;
			Item.rare = ItemRarityID.LightRed;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.maxStack = 99;
			Item.createTile = ModContent.TileType<Tiles.Furniture.PirateCrate>();
			Item.useTime = Item.useAnimation = 20;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.noMelee = true;
			Item.autoReuse = false;
		}

		public override bool CanRightClick() => true;

		public override void RightClick(Player player)
		{
			int[] lootTable = {
				ItemID.GoldRing,
				ItemID.GoldRing,
				ItemID.GoldRing,
				ItemID.GoldRing,
				ItemID.GoldRing,
				ItemID.GoldRing,
				ItemID.LuckyCoin,
				ItemID.LuckyCoin,
				ItemID.LuckyCoin,
				ItemID.CoinGun,
				ItemID.DiscountCard,
				ItemID.DiscountCard,
				ItemID.DiscountCard,
				ItemID.DiscountCard,
				ItemID.DiscountCard,
				ItemID.DiscountCard
			};
			int loot = Main.rand.Next(lootTable.Length);
			player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), lootTable[loot]);
			if (Main.rand.Next(4) > 0) {
				int[] lootTable2 = {
					ItemID.GoldBar,
					ItemID.SilverBar,
					ItemID.TungstenBar,
					ItemID.PlatinumBar
				};
				int loot2 = Main.rand.Next(lootTable2.Length);
				int Booty = Main.rand.Next(15, 30);
				for (int j = 0; j < Booty; j++) {
					player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), lootTable2[loot2]);
				}

			}
			if (Main.rand.Next(2) == 1) {
				int Gems = Main.rand.Next(15, 30);
				for (int I = 0; I < Gems; I++) {
					int[] lootTable3 = {
						ItemID.Ruby,
						ItemID.Emerald,
						ItemID.Topaz,
						ItemID.Amethyst,
						ItemID.Diamond,
						ItemID.Sapphire,
						ItemID.Amber
					};
					int loot3 = Main.rand.Next(lootTable3.Length);
					player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), lootTable3[loot3]);
				}
			}
			int Coins = Main.rand.Next(10, 25);
			for (int K = 0; K < Coins; K++)
				player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ItemID.GoldCoin);
		}
	}
}
