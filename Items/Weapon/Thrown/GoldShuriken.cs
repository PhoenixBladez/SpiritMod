using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Thrown
{
	public class GoldShuriken : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gold Shuriken");
			Tooltip.SetDefault("Occasionally inflicts Broken Armor");
		}


        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.Shuriken);
            item.width = 26;
            item.height = 26;
            item.shoot = mod.ProjectileType("GoldShurikenProjectile");
            item.useAnimation = 18;
            item.useTime = 18;
            item.shootSpeed = 9f;
            item.damage = 14;
            item.knockBack = 0f;
			item.value = Terraria.Item.buyPrice(0, 0, 0, 50);
            item.crit = 5;
            item.rare = 1;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.GoldBar, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 50);
            recipe.AddRecipe();
        }
    }
}
