using Terraria.ID;
using Terraria.ModLoader;
using StarplateBoxTile = SpiritMod.Tiles.MusicBox.StarplateBox;
namespace SpiritMod.Items.Placeable.MusicBox
{
	public class StarplateBox : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Music Box (Starplate Voyager)");
		}

		public override void SetDefaults()
		{
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTurn = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.autoReuse = true;
			item.consumable = true;
			item.createTile = ModContent.TileType<StarplateBoxTile>();
			item.width = 24;
			item.height = 24;
			item.rare = ItemRarityID.LightRed;
			item.value = 100000;
			item.accessory = true;
		}
	}
}
