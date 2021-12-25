using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Ammo
{
	class FaerieStar : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Faerie Star");
			Tooltip.SetDefault("Can be used as ammunition");
		}

		public override void SetDefaults()
		{

			item.width = 28;
			item.height = 28;
			item.value = 50;
			item.rare = ItemRarityID.LightRed;

			item.maxStack = 999;

			item.ammo = AmmoID.FallenStar;

			item.ranged = true;
			item.consumable = true;

			item.shoot = ModContent.ProjectileType<Projectiles.FaerieStar>();
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.FallenStar, 1);
			recipe.AddIngredient(ItemID.PixieDust, 2);
			recipe.AddTile(TileID.CrystalBall);
			recipe.SetResult(this, 10);
			recipe.AddRecipe();
		}
	}
}
