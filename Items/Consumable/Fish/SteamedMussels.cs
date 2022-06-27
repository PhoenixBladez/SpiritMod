
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable.Fish
{
	public class SteamedMussels : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Steamed Mussels");
			Tooltip.SetDefault("Minor improvements to all stats");
		}


		public override void SetDefaults()
		{
			Item.width = Item.height = 22;
			Item.rare = ItemRarityID.Blue;
			Item.maxStack = 99;
			Item.noUseGraphic = true;
			Item.useStyle = ItemUseStyleID.EatFood;
			Item.useTime = Item.useAnimation = 30;

			Item.buffType = BuffID.WellFed;
			Item.buffTime = 18000;
			Item.noMelee = true;
			Item.consumable = true;
			Item.UseSound = SoundID.Item2;
			Item.autoReuse = false;

		}
		public override void AddRecipes()
		{
			Recipe recipe1 = CreateRecipe(1);
			recipe1.AddIngredient(ModContent.ItemType<Tiles.Ambient.Ocean.MusselItem>(), 3);
			recipe1.AddIngredient(ModContent.ItemType<Items.Sets.FloatingItems.Kelp>(), 1);
			recipe1.AddTile(TileID.CookingPots);
			recipe1.Register();
		}
	}
}
