using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Thrown
{
	public class WayfinderTorch : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wayfinder's Torch");
			Tooltip.SetDefault("Releases multiple embers on striking enemies");
		}


        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.Shuriken);
            item.width = 32;
            item.height = 32;           
            item.shoot = mod.ProjectileType("WayfinderTorch");
            item.useAnimation = 21;
            item.useTime = 21;
            item.shootSpeed = 12f;
            item.damage = 43;
            item.knockBack = 1f;
			item.value = Terraria.Item.buyPrice(0, 0, 0, 50);
            item.rare = 5;
            item.autoReuse = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "MoonStone", 2);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 55);
            recipe.AddRecipe();
        }
    }
}