using Terraria.ID;
using Terraria.ModLoader;
using TranquilWindsBoxTile = SpiritMod.Tiles.MusicBox.TranquilWindsBox;
namespace SpiritMod.Items.Placeable.MusicBox
{
	public class TranquilWindsBox : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Music Box (Tranquil Winds)");
		}

		public override void SetDefaults()
		{
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTurn = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.autoReuse = true;
			item.consumable = true;
			item.createTile = ModContent.TileType<TranquilWindsBoxTile>();
			item.width = 24;
			item.height = 24;
			item.rare = ItemRarityID.LightRed;
			item.value = Terraria.Item.buyPrice(gold : 2);
			item.accessory = true;
		}
	}
}
