using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Ammo
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
			
			item.width = 10;
			item.height = 28;
            item.value = 450;
            item.rare = 8;

            item.maxStack = 999;

            item.damage = 16;
			item.knockBack = 2f;
            item.ammo = AmmoID.Arrow;

            item.ranged = true;
            item.consumable = true;

            item.shoot = mod.ProjectileType("BeetleArrow");
            item.shootSpeed = 2.5f;
        }

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.BeetleHusk, 2);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 111);
            recipe.AddRecipe();
		}
	}
}
