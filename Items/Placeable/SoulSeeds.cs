using SpiritMod.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Placeable
{
	public class SoulSeeds : ModItem
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Soulbloom Seeds");

		public override void SetDefaults()
		{
			Item.autoReuse = true;
			Item.useTurn = true;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useAnimation = 15;
			Item.rare = ItemRarityID.Pink;
			Item.useTime = 15;
			Item.maxStack = 99;
			Item.consumable = true;
			Item.placeStyle = 0;
			Item.width = 12;
			Item.height = 14;
			Item.value = Item.buyPrice(0, 0, 5, 0);
			Item.createTile = ModContent.TileType<SoulBloomTile>();
		}
	}
}