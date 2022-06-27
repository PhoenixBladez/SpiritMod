using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.MarbleSet
{
	public class MarbleChunk : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient Marble Chunk");
			Tooltip.SetDefault("'Contains fragments of past civilizations'");
		}


		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 36;
			Item.maxStack = 999;
			Item.rare = ItemRarityID.Green;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 10;
			Item.useAnimation = 15;

			Item.autoReuse = true;
			Item.consumable = true;

			Item.createTile = ModContent.TileType<MarbleOre>();
		}
	}
}