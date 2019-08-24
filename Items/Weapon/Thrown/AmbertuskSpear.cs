using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Weapon.Thrown
{
    public class AmbertuskSpear : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ambertusk Spear");
			Tooltip.SetDefault("Enemies hit are afflicted by a damaging debuff");
		}


        public override void SetDefaults()
        {
            item.width = item.height = 42;
            item.rare = 6;
            item.maxStack = 999;
            item.crit = 10;
            item.damage = 60;
            item.value = Terraria.Item.sellPrice(0, 0, 5, 0);
            item.knockBack = 9;
            item.useStyle = 1;
            item.useTime = item.useAnimation = 24;
            item.thrown = true;
            item.noMelee = true;
            item.autoReuse = true;
            item.consumable = true;
            item.noUseGraphic = true;
            item.shoot = mod.ProjectileType("AmbertuskSpear");
            item.shootSpeed = 15;
            item.UseSound = SoundID.Item1;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "DuneEssence", 5);
            recipe.AddTile(null, "EssenceDistorter");
            recipe.SetResult(this, 100);
            recipe.AddRecipe();
        }
    }
}