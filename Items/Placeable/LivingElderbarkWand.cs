using SpiritMod.Tiles.Block;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Placeable
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
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTurn = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.autoReuse = true;
			item.maxStack = 99;
			item.tileWand = ModContent.ItemType<Items.Material.AncientBark>();
			item.consumable = false;
			item.createTile = ModContent.TileType<LivingBriarWood>();
			item.width = 36;
			item.height = 36;
			item.rare = 1;
			item.value = Item.buyPrice(silver: 20);
		}
	}
}
