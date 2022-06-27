using SpiritMod.Tiles.Ambient;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Placeable.Furniture
{
	public class PrintProbe : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Destroyer Blueprint");
			Tooltip.SetDefault("WARNING: Release may cause seismic anomalies exceeding 10f.");
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

			Item.createTile = ModContent.TileType<ProbePrint>();
		}
	}
}