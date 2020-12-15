using Terraria.ID;
using Terraria.ModLoader;
using HallowNightBoxTile = SpiritMod.Tiles.MusicBox.HallowNightBox;
namespace SpiritMod.Items.Placeable.MusicBox
{
	public class HallowNightBox : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Music Box (Hallow- Night)");
		}

		public override void SetDefaults()
		{
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTurn = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.autoReuse = true;
			item.consumable = true;
			item.createTile = ModContent.TileType<HallowNightBoxTile>();
			item.width = 24;
			item.height = 24;
			item.rare = ItemRarityID.LightRed;
			item.value = 100000;
			item.accessory = true;
		}
	}
}
