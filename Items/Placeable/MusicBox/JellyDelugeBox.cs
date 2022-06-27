using Terraria.ID;
using Terraria.ModLoader;
using JellyDelugeBoxTile = SpiritMod.Tiles.MusicBox.JellyDelugeBox;
namespace SpiritMod.Items.Placeable.MusicBox
{
	public class JellyDelugeBox : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Music Box (Jelly Deluge)");
		}

		public override void SetDefaults()
		{
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.autoReuse = true;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<JellyDelugeBoxTile>();
			Item.width = 24;
			Item.height = 24;
			Item.rare = ItemRarityID.LightRed;
			Item.value = Terraria.Item.buyPrice(gold : 2);
			Item.accessory = true;
		}
	}
}
