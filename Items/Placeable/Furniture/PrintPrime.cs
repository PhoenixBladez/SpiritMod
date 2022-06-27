using SpiritMod.Tiles.Ambient;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Placeable.Furniture
{
	public class PrintPrime : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Skeletron Prime Blueprint");
			Tooltip.SetDefault("Fore-Warned is Four-Armed.");
		}


		public override void SetDefaults()
		{
			Item.width = 94;
			Item.height = 62;
			Item.value = 15000;
			Item.rare = ItemRarityID.LightPurple;

			Item.maxStack = 99;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 10;
			Item.useAnimation = 15;

			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;

			Item.createTile = ModContent.TileType<PrimePrint>();
		}
	}
}