using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Items.Consumable.Fish;
using Microsoft.Xna.Framework;

namespace SpiritMod.Items.Consumable.Food
{
	public class Sushi : FoodItem
	{
		internal override Point Size => new(26, 24);
		public override void StaticDefaults() => Tooltip.SetDefault("Minor improvements to all stats\nProvides underwater breathing\n'Cold and fresh!'");

		public override bool CanUseItem(Player player)
		{
			player.AddBuff(BuffID.Gills, 1800);
			return true;
		}

		public override void AddRecipes()
		{
			Recipe recipe1 = CreateRecipe(1);
			recipe1.AddIngredient(ModContent.ItemType<Items.Sets.FloatingItems.Kelp>(), 5);
			recipe1.AddIngredient(ModContent.ItemType<RawFish>(), 1);
			recipe1.AddTile(TileID.CookingPots);
			recipe1.Register();
		}
	}
}
