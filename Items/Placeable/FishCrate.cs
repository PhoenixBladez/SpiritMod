using SpiritMod.Items.Consumable;
using SpiritMod.Items.Consumable.Fish;
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
			item.width = 20;
			item.height = 20;
			item.rare = ItemRarityID.Orange;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.createTile = mod.TileType("FishCrate_Tile");
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
		public override void Update(ref float gravity, ref float maxFallSpeed)
		{
			if(item.wet) {
				gravity *= 0f;
				maxFallSpeed *= -.09f;
			} else {
				maxFallSpeed *= 1f;
				gravity *= 1f;
			}
		}

		public override void RightClick(Player player)
		{
			if(Main.rand.Next(2) == 0) {

				player.QuickSpawnItem(ModContent.ItemType<RawFish>());
			}
			if(Main.rand.Next(4) == 0) {
				if(Main.rand.NextBool()) {
					player.QuickSpawnItem(ModContent.ItemType<FloaterItem>());
				} else {
					player.QuickSpawnItem(ModContent.ItemType<LuvdiscItem>());
				}
			}
			int[] lootTable = {
				ItemID.Shrimp,
				ItemID.Salmon,
				ItemID.Bass,
				ItemID.RedSnapper,
				ItemID.Trout
			};
			int loot = Main.rand.Next(lootTable.Length);
			int Fish = Main.rand.Next(3, 5);
			for(int j = 0; j < Fish; j++) {
				player.QuickSpawnItem(lootTable[loot]);
			}
			if(Main.rand.Next(4) == 1) {
				int[] lootTable3 = {
					ItemID.ArmoredCavefish,
					ItemID.Damselfish,
					ItemID.DoubleCod,
					ItemID.FrostMinnow
				};
				int loot3 = Main.rand.Next(lootTable3.Length);
				int Booty = Main.rand.Next(1, 2);
				for(int j = 0; j < Booty; j++) {
					player.QuickSpawnItem(lootTable3[loot3]);
				}

			}
			if(Main.rand.Next(27) == 0) {
				int[] lootTable4 = {
					ItemID.ReaverShark,
					ItemID.Swordfish,
					ItemID.SawtoothShark
				};
				int loot4 = Main.rand.Next(lootTable4.Length);
				player.QuickSpawnItem(lootTable4[loot4]);
			}
            string[] lootTable2123 = { "DiverLegs", "DiverHead", "DiverBody" };
            if (Main.rand.Next(14) == 0)
            {
                int loot443 = Main.rand.Next(lootTable2123.Length);
                {
                    player.QuickSpawnItem(mod.ItemType(lootTable2123[loot443]));
                }
            }
            if (Main.rand.Next(3) == 0) {
				int[] lootTable2 = {
					ItemID.FrostDaggerfish,
					ItemID.BombFish
				};
				int loot2 = Main.rand.Next(lootTable2.Length);
				int Fish1 = Main.rand.Next(9, 12);
				for(int j = 0; j < Fish1; j++) {

					player.QuickSpawnItem((lootTable2[loot2]));
				}
			}
			if(Main.hardMode && Main.rand.Next(10) == 0) {
				int[] lootTable51 = {
					ItemID.FlarefinKoi,
					ItemID.Obsidifish,
					ItemID.Prismite,
					ItemID.PrincessFish
				};
				int loot51 = Main.rand.Next(lootTable51.Length);
				int Booty = Main.rand.Next(1, 2);
				for(int j = 0; j < Booty; j++) {
					player.QuickSpawnItem(lootTable51[loot51]);
				}

			}

			if(Main.rand.Next(3) == 0) {
				int[] lootTable21 = { ItemID.SilverCoin };
				int loot21 = Main.rand.Next(lootTable21.Length);
				int Fish1 = Main.rand.Next(10, 90);
				for(int j = 0; j < Fish1; j++) {

					player.QuickSpawnItem((lootTable21[loot21]));
				}
			}
			if(Main.rand.Next(7) == 0) {
				int[] lootTable212 = { ItemID.GoldCoin };
				int loot212 = Main.rand.Next(lootTable212.Length);
				int Fish1 = Main.rand.Next(1, 2);
				for(int j = 0; j < Fish1; j++) {

					player.QuickSpawnItem((lootTable212[loot212]));
				}
			}
		}
	}
}
