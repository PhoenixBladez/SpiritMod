using SpiritMod.Tiles.Relics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Placeable.Relics
{
	public abstract class BaseRelicItem : ModItem
	{
		internal abstract int TileType { get; }

		public override void SetDefaults()
		{
			Item.autoReuse = false;
			Item.useTurn = true;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useAnimation = 15;
			Item.master = true;
			Item.rare = ItemRarityID.Master;
			Item.useTime = 15;
			Item.maxStack = 99;
			Item.consumable = true;
			Item.width = 30;
			Item.height = 44;
			Item.value = Item.buyPrice(0, 1, 0, 0);
			Item.createTile = TileType;
		}
	}
}