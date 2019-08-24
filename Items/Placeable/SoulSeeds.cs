using Terraria.ModLoader;
using Terraria;
using Terraria.ID;

namespace SpiritMod.Items.Placeable
{
	public class SoulSeeds : ModItem
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Soulbloom Seeds");
        }

        public override void SetDefaults()
		{
            item.autoReuse = true;
			item.useTurn = true;
			item.useStyle = 1;
			item.useAnimation = 15;
            item.rare = 5;
			item.useTime = 10;
			item.maxStack = 99;
			item.consumable = true;
			item.placeStyle = 0;
			item.width = 12;
			item.height = 14;
			item.value = Item.buyPrice(0, 0, 5, 0);
            item.createTile = mod.TileType("SoulBloomTile");
        }
	}
}