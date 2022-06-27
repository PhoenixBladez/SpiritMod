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
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.autoReuse = true;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<FrostLegionBoxTile>();
			Item.width = 24;
			Item.height = 24;
			Item.rare = ItemRarityID.LightRed;
			Item.value = 100000;
			Item.accessory = true;
		}
	}
}
