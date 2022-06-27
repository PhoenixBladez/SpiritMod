using SpiritMod.Projectiles.Arrow;
using Terraria;
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
			Item.width = 10;
			Item.height = 28;
			Item.rare = ItemRarityID.Blue;
			Item.value = Terraria.Item.buyPrice(0, 0, 0, 40);

			Item.maxStack = 999;

			Item.damage = 7;
			Item.knockBack = 0;
			Item.ammo = AmmoID.Arrow;

			Item.DamageType = DamageClass.Ranged;
			Item.consumable = true;

			Item.shoot = ModContent.ProjectileType<PoisonArrowProj>();
			Item.shootSpeed = 4f;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(50);
			recipe.AddIngredient(ItemID.WoodenArrow, 25);
			recipe.AddIngredient(ItemID.Stinger, 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}
}
