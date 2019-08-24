using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Thrown
{
	public class TinShuriken : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tin Shuriken");
		}


        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.Shuriken);
            item.width = 22;
            item.height = 22;           
            item.shoot = mod.ProjectileType("TinShurikenProjectile");
            item.useAnimation = 16;
            item.useTime = 16;
            item.shootSpeed = 9f;
            item.damage = 9;
            item.knockBack = 0f;
			item.value = Terraria.Item.buyPrice(0, 0, 0, 25);
            item.crit = 5;
            item.rare = 1;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.TinBar, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 50);
            recipe.AddRecipe();
        }
    }
}
