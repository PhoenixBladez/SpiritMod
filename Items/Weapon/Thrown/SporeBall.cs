using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Thrown
{
	public class SporeBall : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spore");
		}


        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.SpikyBall);
            item.width = 16;
            item.height = 16;           
            item.shoot = mod.ProjectileType("SporeBallProjectile");
            item.useAnimation = 16;
            item.useTime = 16;
            item.shootSpeed = 9f;
            item.damage = 20;
            item.knockBack = 1f;
			item.value = Terraria.Item.buyPrice(0, 0, 0, 45);
            item.crit = 5;
            item.rare = 2;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(161, 50);
			recipe.AddIngredient(331, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 50);
            recipe.AddRecipe();
        }
    }
}