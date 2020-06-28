using Terraria.ID;
using Terraria.ModLoader;
using SkullStickTile = SpiritMod.Tiles.Ambient.SkullStick;
namespace SpiritMod.Items.Placeable.Furniture
{
	public class SkullStick : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Skull on a Stick");
		}


		public override void SetDefaults()
		{
			item.width = 32;
			item.height = 28;
			item.value = 500;

			item.maxStack = 99;
			item.rare = ItemRarityID.Green;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = 10;
			item.useAnimation = 15;

			item.useTurn = true;
			item.autoReuse = true;
			item.consumable = true;

			item.createTile = ModContent.TileType<SkullStickTile>();
		}
	}
}