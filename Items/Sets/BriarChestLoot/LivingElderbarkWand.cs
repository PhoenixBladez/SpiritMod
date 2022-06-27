using SpiritMod.Tiles.Block;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.BriarChestLoot
{
	public class LivingElderbarkWand : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Living Elderbark Wand");
			Tooltip.SetDefault("Places living elderbark");
		}

		public override void SetDefaults()
		{
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.autoReuse = true;
			Item.maxStack = 99;
			Item.tileWand = ModContent.ItemType<Items.Sets.HuskstalkSet.AncientBark>();
			Item.consumable = false;
			Item.createTile = ModContent.TileType<LivingBriarWood>();
			Item.width = 36;
			Item.height = 36;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.buyPrice(silver: 20);
		}
	}
}
