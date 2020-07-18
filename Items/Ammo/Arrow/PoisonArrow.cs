using SpiritMod.Projectiles.Arrow;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Ammo.Arrow
{
	class PoisonArrow : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Poison Arrow");
			Tooltip.SetDefault("Hit enemies are poisoned");
		}

		public override void SetDefaults()
		{
			item.width = 10;
			item.height = 28;
			item.rare = ItemRarityID.Blue;
			item.value = Terraria.Item.buyPrice(0, 0, 0, 40);

			item.maxStack = 999;

			item.damage = 7;
			item.knockBack = 0;
			item.ammo = AmmoID.Arrow;

			item.ranged = true;
			item.consumable = true;

			item.shoot = ModContent.ProjectileType<PoisonArrowProj>();
			item.shootSpeed = 4f;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.WoodenArrow, 25);
			recipe.AddIngredient(ItemID.Stinger, 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this, 50);
			recipe.AddRecipe();
		}
	}
}
