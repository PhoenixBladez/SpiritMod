
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
			item.width = item.height = 22;
			item.rare = ItemRarityID.Blue;
			item.maxStack = 99;
			item.noUseGraphic = true;
			item.useStyle = ItemUseStyleID.EatingUsing;
			item.useTime = item.useAnimation = 30;

			item.buffType = BuffID.WellFed;
			item.buffTime = 18000;
			item.noMelee = true;
			item.consumable = true;
			item.UseSound = SoundID.Item2;
			item.autoReuse = false;

		}
		public override void AddRecipes()
		{
			ModRecipe recipe1 = new ModRecipe(mod);
			recipe1.AddIngredient(ModContent.ItemType<Tiles.Ambient.Ocean.MusselItem>(), 3);
			recipe1.AddIngredient(ModContent.ItemType<Items.Sets.FloatingItems.Kelp>(), 1);
			recipe1.AddTile(TileID.CookingPots);
			recipe1.SetResult(this, 1);
			recipe1.AddRecipe();
		}
	}
}
