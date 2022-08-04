using SpiritMod.Tiles.Ambient.Forest;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.ByBiome.Forest.Placeable.Decorative
{
	public class CloudstalkSeed : ModItem
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Cloudstalk Seeds");

		public override void SetDefaults()
		{
			Item.autoReuse = true;
			Item.useTurn = true;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useAnimation = 15;
			Item.rare = ItemRarityID.White;
			Item.useTime = 15;
			Item.maxStack = 99;
			Item.consumable = true;
			Item.placeStyle = 0;
			Item.width = 22;
			Item.height = 18;
			Item.value = 0;
			Item.createTile = ModContent.TileType<Cloudstalk>();
		}
	}
}