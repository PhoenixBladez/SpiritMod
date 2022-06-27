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
			Item.width = 30;
			Item.height = 40;
			Item.value = Item.buyPrice(0, 10, 0, 0);

			Item.maxStack = 99;

			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 10;
			Item.useAnimation = 15;

			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;

			Item.createTile = ModContent.TileType<IceDeityDecor>();
		}

	}
}