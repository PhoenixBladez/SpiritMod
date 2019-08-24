using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Thrown
{
	public class CopperShuriken : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Copper Shuriken");
		}


        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.Shuriken);
            item.width = 30;
            item.height = 30;           
            item.shoot = mod.ProjectileType("CopperShurikenProjectile");
            item.useAnimation = 16;
            item.useTime = 16;
            item.shootSpeed = 9f;
            item.damage = 8;
            item.knockBack = 0f;
			item.value = Item.buyPrice(0, 0, 0, 20);
            item.crit = 5;
            item.rare = 1;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.CopperBar, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 50);
            recipe.AddRecipe();
        }
    }
}
