using Terraria.ID;
using Terraria.ModLoader;
using SpiritBoxTile = SpiritMod.Tiles.MusicBox.SpiritBox1;
namespace SpiritMod.Items.Placeable.MusicBox
{
	public class SpiritBox1 : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Music Box (Spirit Biome- Overworld)");
		}

		public override void SetDefaults()
		{
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.autoReuse = true;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<SpiritBoxTile>();
			Item.width = 24;
			Item.height = 24;
			Item.rare = ItemRarityID.LightRed;
			Item.value = 100000;
			Item.accessory = true;
		}
	}
}
