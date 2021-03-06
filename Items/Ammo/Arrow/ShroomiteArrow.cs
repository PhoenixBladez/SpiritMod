﻿using Terraria.ID;
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
			item.width = 10;
			item.height = 28;
			item.rare = ItemRarityID.Yellow;
			item.value = 1000;

			item.maxStack = 999;

			item.damage = 16;
			item.knockBack = 0;
			item.ammo = AmmoID.Arrow;

			item.ranged = true;
			item.consumable = true;

			item.shoot = ModContent.ProjectileType<Projectiles.Arrow.ShroomiteArrow>();
			item.shootSpeed = 6f;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.ShroomiteBar);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this, 50);
			recipe.AddRecipe();
		}
	}
}
