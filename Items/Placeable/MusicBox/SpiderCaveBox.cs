using Terraria.ID;
using Terraria.ModLoader;
using SpiderCaveBoxTile = SpiritMod.Tiles.MusicBox.SpiderCaveBox;
namespace SpiritMod.Items.Placeable.MusicBox
{
	public class SpiderCaveBox : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Music Box (Spider Caves)");
		}

		public override void SetDefaults()
		{
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTurn = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.autoReuse = true;
			item.consumable = true;
			item.createTile = ModContent.TileType<SpiderCaveBoxTile>();
			item.width = 24;
			item.height = 24;
			item.rare = ItemRarityID.LightRed;
			item.value = 100000;
			item.accessory = true;
		}
	}
}
