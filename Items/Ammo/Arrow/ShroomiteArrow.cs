using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Ammo.Arrow
{
	class ShroomiteArrow : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shroomite Arrow");
			Tooltip.SetDefault("Flies straight and deals two ticks of damage to hit enemies!");
		}

		public override void SetDefaults()
		{
			Item.width = 10;
			Item.height = 28;
			Item.rare = ItemRarityID.Yellow;
			Item.value = 1000;

			Item.maxStack = 999;

			Item.damage = 16;
			Item.knockBack = 0;
			Item.ammo = AmmoID.Arrow;

			Item.DamageType = DamageClass.Ranged;
			Item.consumable = true;

			Item.shoot = ModContent.ProjectileType<Projectiles.Arrow.ShroomiteArrow>();
			Item.shootSpeed = 6f;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(50);
			recipe.AddIngredient(ItemID.ShroomiteBar);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}
