using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.ByBiome.Forest.Placeable.Decorative
{
	public class CloudstalkItem : ModItem
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Cloudstalk");

		public override void SetDefaults()
		{
			Item.autoReuse = false;
			Item.useTurn = true;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.rare = ItemRarityID.White;
			Item.useAnimation = 15;
			Item.useTime = 15;
			Item.maxStack = 999;
			Item.width = 22;
			Item.height = 18;
			Item.value = 0;
		}

		public override void AddRecipes()
		{
			Recipe.Create(ItemID.FeatherfallPotion, 1).
				AddIngredient(ItemID.Blinkroot).
				AddIngredient(ItemID.Daybloom).
				AddIngredient(ItemID.BottledWater).
				AddIngredient<CloudstalkItem>().
				AddTile(TileID.Bottles).
				Register();

			Recipe.Create(ItemID.GravitationPotion, 1).
				AddIngredient(ItemID.Blinkroot).
				AddIngredient(ItemID.Fireblossom).
				AddIngredient(ItemID.Deathweed).
				AddIngredient(ItemID.BottledWater).
				AddIngredient<CloudstalkItem>().
				AddTile(TileID.Bottles).
				Register();
		}
	}
}