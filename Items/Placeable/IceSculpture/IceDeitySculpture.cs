using SpiritMod.Items.Sets.CryoliteSet;
using SpiritMod.Items.Placeable.Tiles;
using SpiritMod.Tiles.Ambient.IceSculpture;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Placeable.IceSculpture
{
	public class IceDeitySculpture : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ice Deity Sculpture");
		}


		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 40;
			item.value = Item.buyPrice(0, 10, 0, 0);

			item.maxStack = 99;

			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = 10;
			item.useAnimation = 15;

			item.useTurn = true;
			item.autoReuse = true;
			item.consumable = true;

			item.createTile = ModContent.TileType<IceDeityDecor>();
		}

	}
}