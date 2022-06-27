using SpiritMod.Tiles.Ambient;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Placeable.Furniture
{
	public class BlueprintTwins : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Twins Blueprint");
			Tooltip.SetDefault("They can't kill what they can't see... They see everything.");
		}


		public override void SetDefaults()
		{
			Item.width = 94;
			Item.height = 62;
			Item.value = 15000;
			Item.rare = ItemRarityID.LightPurple;

			Item.maxStack = 99;

			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 10;
			Item.useAnimation = 15;


			Item.createTile = ModContent.TileType<TwinsPrint>();
		}
	}
}