using Terraria.ID;
using Terraria.ModLoader;
using BlueMoonBoxTile = SpiritMod.Tiles.MusicBox.BlueMoonBox;
namespace SpiritMod.Items.Placeable.MusicBox
{
	public class BlueMoonBox : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Music Box (Mystic Moon)");
		}

		public override void SetDefaults()
		{
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.autoReuse = true;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<BlueMoonBoxTile>();
			Item.width = 24;
			Item.height = 24;
			Item.rare = ItemRarityID.LightRed;
			Item.value = 100000;
			Item.accessory = true;
		}
	}
}
