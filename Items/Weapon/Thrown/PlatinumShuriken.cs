using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Thrown
{
	public class PlatinumShuriken : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Platinum Shuriken");
			Tooltip.SetDefault("Occasionally inflicts Broken Armor");
		}


        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.Shuriken);
            item.width = 26;
            item.height = 26;
            item.shoot = mod.ProjectileType("PlatinumShurikenProjectile");
            item.useAnimation = 18;
            item.useTime = 18;
            item.shootSpeed = 9f;
            item.damage = 15;
            item.knockBack = 0f;
			item.value = Terraria.Item.buyPrice(0, 0, 0, 55);
            item.crit = 5;
            item.rare = 2;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.PlatinumBar, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 50);
            recipe.AddRecipe();
        }
    }
}
