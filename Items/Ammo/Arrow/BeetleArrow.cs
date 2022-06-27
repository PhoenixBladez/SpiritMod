using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Ammo.Arrow
{
	class BeetleArrow : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Beetle Arrow");
			Tooltip.SetDefault("Successful hits on enemies reduces damage taken by 1%, maxing out at 15%");
		}

		public override void SetDefaults()
		{

			Item.width = 10;
			Item.height = 28;
			Item.value = 450;
			Item.rare = ItemRarityID.Yellow;

			Item.maxStack = 999;

			Item.damage = 16;
			Item.knockBack = 2f;
			Item.ammo = AmmoID.Arrow;

			Item.DamageType = DamageClass.Ranged;
			Item.consumable = true;

			Item.shoot = ModContent.ProjectileType<Projectiles.Arrow.BeetleArrow>();
			Item.shootSpeed = 2.5f;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(111);
			recipe.AddIngredient(ItemID.BeetleHusk, 2);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}
