using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Weapon.Thrown
{
    public class PaleolithShuriken : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Paleolith Shuriken");
			Tooltip.SetDefault("Homes in on foes");
		}


        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 20;
            item.value = Item.sellPrice(0, 0, 0, 40);
            item.rare = 6;
            item.maxStack = 999;
            item.crit = 4;
            item.damage = 55;
            item.knockBack = 2;
            item.useStyle = 1;
            item.useTime = item.useAnimation = 12;            
            item.thrown = true;
            item.noMelee = true;
            item.autoReuse = true;
            item.consumable = true;
            item.noUseGraphic = true;
            item.shoot = mod.ProjectileType("PaleolithShuriken");
            item.shootSpeed = 11;
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