using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.DonatorItems
{
	public class TheCouch : ModItem
	{
		public static int _type;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("The Couch");
			Tooltip.SetDefault("Defense is increased, but movement speed reduced, when nearby\n'Hello Terraria Enthusiasts'");
		}


		public override void SetDefaults()
		{
			item.width = 52;
			item.height = 30;
			item.value = 50000;

			item.maxStack = 99;
			item.rare = ItemRarityID.LightRed;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = 10;
			item.useAnimation = 15;

			item.useTurn = true;
			item.autoReuse = true;
			item.consumable = true;

			item.createTile = ModContent.TileType<Tiles.Furniture.Donator.TheCouch>();
		}

	}
}