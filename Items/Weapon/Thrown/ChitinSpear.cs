using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Weapon.Thrown
{
    public class ChitinSpear : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chitin Spear");
		}


        public override void SetDefaults()
        {
            item.width = 5;
            item.height = 14;
            item.rare = 1;
            item.maxStack = 999;
            item.damage = 15;
            item.value = Terraria.Item.sellPrice(0, 0, 0, 20);
            item.knockBack = 7;
            item.useStyle = 1;
            item.useTime = item.useAnimation = 45;
            item.thrown = true;
            item.noMelee = true;
            item.autoReuse = true;
            item.consumable = true;
            item.noUseGraphic = true;
            item.shoot = mod.ProjectileType("ChitinSpearProj");
            item.shootSpeed = 7;
            item.UseSound = SoundID.Item1;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "Chitin", 2);
            recipe.SetResult(this, 50);
            recipe.AddRecipe();
        }
    }
}