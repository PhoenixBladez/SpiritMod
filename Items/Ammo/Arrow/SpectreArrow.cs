﻿using Terraria.ID;
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
			item.width = 10;
			item.height = 28;
			item.rare = ItemRarityID.Yellow;
			item.value = 1000;

			item.maxStack = 999;

			item.damage = 14;
			item.knockBack = 2f;
			item.ammo = AmmoID.Arrow;

			item.ranged = true;
			item.consumable = true;

			item.shoot = ModContent.ProjectileType<Projectiles.Arrow.SpectreArrow>();
			item.shootSpeed = 5f;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.SpectreBar);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this, 50);
			recipe.AddRecipe();
		}
	}
}
