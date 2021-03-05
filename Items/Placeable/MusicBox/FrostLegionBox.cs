using Terraria.ID;
using Terraria.ModLoader;
using FrostLegionBoxTile = SpiritMod.Tiles.MusicBox.FrostLegionBox;
namespace SpiritMod.Items.Placeable.MusicBox
{
	public class FrostLegionBox : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Music Box (Frost Legion)");
		}

		public override void SetDefaults()
		{
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTurn = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.autoReuse = true;
			item.consumable = true;
			item.createTile = ModContent.TileType<FrostLegionBoxTile>();
			item.width = 24;
			item.height = 24;
			item.rare = ItemRarityID.LightRed;
			item.value = 100000;
			item.accessory = true;
		}
	}
}
