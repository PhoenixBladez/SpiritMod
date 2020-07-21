using SpiritMod.Tiles.Block;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Placeable.Tiles
{
	public class SpaceJunkItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Space Junk");
			Tooltip.SetDefault("Can be used by the Extractinator\n'What's hidden behind this jumbled mess?'");
			ItemID.Sets.ExtractinatorMode[item.type] = item.type;
		}


		public override void SetDefaults()
		{
			item.width = 16;
			item.height = 14;

			item.maxStack = 999;

			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = 10;
			item.useAnimation = 15;

			item.useTurn = true;
			item.autoReuse = true;
			item.consumable = true;

			item.createTile = ModContent.TileType<SpaceJunkTile>();
		}
		public override void ExtractinatorUse(ref int resultType, ref int resultStack)
		{
			if (Main.rand.Next(6) == 0) {
				string[] lootTable = { "ScarpItem2", "ScrapItem3", "ScrapItem5" };
				int loot = Main.rand.Next(lootTable.Length);
				resultType = mod.ItemType(lootTable[loot]);
				resultStack = Main.rand.Next(1, 4);
			}
			else if (Main.rand.Next(10) == 0) {
				string[] lootTable1 = { "ScrapItem1", "ScrapItem4", "ScrapItem6" };
				int loot2 = Main.rand.Next(lootTable1.Length);
				resultType = mod.ItemType(lootTable1[loot2]);
				resultStack = 1;
			}
			else if (Main.rand.Next(9) == 0) {
				resultType = Main.rand.Next(new int[] { 2337, 2339 });
				resultStack = 1;
			}
			else if (Main.rand.Next(40) == 0) {
				//resultType = ModContent.ItemType<ScrapGunHarold>();
				//resultStack = 1;
			}
			else {
				resultType = Main.rand.Next(new int[] { 12, 699, 11, 700, 13, 173, 702, 701 });
				resultStack = Main.rand.Next(2, 4);
			}
		}
	}
}