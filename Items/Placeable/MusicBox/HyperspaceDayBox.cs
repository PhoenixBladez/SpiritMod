using Terraria.ID;
using Terraria.ModLoader;
using HyperspaceDayBoxTile = SpiritMod.Tiles.MusicBox.HyperspaceDayBox;
namespace SpiritMod.Items.Placeable.MusicBox
{
	public class HyperspaceDayBox : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Music Box (Hyperspace- Day)");
		}

		public override void SetDefaults()
		{
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.autoReuse = true;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<HyperspaceDayBoxTile>();
			Item.width = 24;
			Item.height = 24;
			Item.rare = ItemRarityID.LightRed;
			Item.value = 100000;
			Item.accessory = true;
		}
    }
}
