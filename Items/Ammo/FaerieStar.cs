using Terraria;
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

			Item.width = 28;
			Item.height = 28;
			Item.value = 50;
			Item.rare = ItemRarityID.LightRed;

			Item.maxStack = 999;

			Item.ammo = AmmoID.FallenStar;

			Item.DamageType = DamageClass.Ranged;
			Item.consumable = true;

			Item.shoot = ModContent.ProjectileType<Projectiles.FaerieStar>();
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(10);
			recipe.AddIngredient(ItemID.FallenStar, 1);
			recipe.AddIngredient(ItemID.PixieDust, 2);
			recipe.AddTile(TileID.CrystalBall);
			recipe.Register();
		}
	}
}
