using Terraria.ID;
using Terraria.ModLoader;
using MJWBoxTile = SpiritMod.Tiles.MusicBox.MJWBox;
namespace SpiritMod.Items.Placeable.MusicBox
{
	public class MJWBox : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Music Box (Moon Jelly Wizard)");
		}

		public override void SetDefaults()
		{
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTurn = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.autoReuse = true;
			item.consumable = true;
			item.createTile = ModContent.TileType<MJWBoxTile>();
			item.width = 24;
			item.height = 24;
			item.rare = ItemRarityID.LightRed;
			item.value = Terraria.Item.buyPrice(gold : 2);
			item.accessory = true;
		}
	}
}
