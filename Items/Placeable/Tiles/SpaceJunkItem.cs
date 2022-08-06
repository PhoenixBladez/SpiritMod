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
			ItemID.Sets.ExtractinatorMode[Item.type] = Item.type;
		}

		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 14;

			Item.maxStack = 999;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 10;
			Item.useAnimation = 15;

			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;

			Item.createTile = ModContent.TileType<SpaceJunkTile>();
		}

		public override void ExtractinatorUse(ref int resultType, ref int resultStack)
		{
			if (Main.rand.NextBool(6))
			{
				string[] lootTable = { "ScrapItem2", "ScrapItem3", "ScrapItem5" };
				int loot = Main.rand.Next(lootTable.Length);
				resultType = Mod.Find<ModItem>(lootTable[loot]).Type;
				resultStack = Main.rand.Next(1, 4);
			}
			else if (Main.rand.NextBool(10))
			{
				string[] lootTable1 = { "ScrapItem1", "ScrapItem4", "ScrapItem6" };
				int loot2 = Main.rand.Next(lootTable1.Length);
				resultType = Mod.Find<ModItem>(lootTable1[loot2]).Type;
				resultStack = 1;
			}
			else if (Main.rand.NextBool(9))
			{
				resultType = Main.rand.Next(new int[] { 2337, 2339 });
				resultStack = 1;
			}
			else
			{
				resultType = Main.rand.Next(new int[] { ItemID.CopperOre, ItemID.TinOre, ItemID.IronOre, ItemID.LeadOre, ItemID.GoldOre, ItemID.Obsidian, ItemID.PlatinumOre, ItemID.SilverOre, ItemID.TungstenOre });
				resultStack = Main.rand.Next(2, 4);
			}
		}
	}
}