using Terraria.ID;
using Terraria.ModLoader;
using ReachBoxTile = SpiritMod.Tiles.MusicBox.ReachBox;
namespace SpiritMod.Items.Placeable.MusicBox
{
	public class ReachBox : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Music Box (The Briar- Daytime)");
		}

		public override void SetDefaults()
		{
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTurn = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.autoReuse = true;
			item.consumable = true;
			item.createTile = ModContent.TileType<ReachBoxTile>();
			item.width = 24;
			item.height = 24;
			item.rare = ItemRarityID.LightRed;
			item.value = 100000;
			item.accessory = true;
		}
	}
}
