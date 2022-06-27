using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Ammo.Arrow
{
	class SpectreArrow : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ghast Arrow");
			Tooltip.SetDefault("Hitting enemies occasionally releases damaging spectre bolts");
		}

		public override void SetDefaults()
		{
			Item.width = 10;
			Item.height = 28;
			Item.rare = ItemRarityID.Yellow;
			Item.value = 1000;

			Item.maxStack = 999;

			Item.damage = 14;
			Item.knockBack = 2f;
			Item.ammo = AmmoID.Arrow;

			Item.DamageType = DamageClass.Ranged;
			Item.consumable = true;

			Item.shoot = ModContent.ProjectileType<Projectiles.Arrow.SpectreArrow>();
			Item.shootSpeed = 5f;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(50);
			recipe.AddIngredient(ItemID.SpectreBar);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}
