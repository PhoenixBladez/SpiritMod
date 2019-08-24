using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Thrown
{
	public class StardustKnife : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stardust Knife");
			Tooltip.SetDefault("Homes in on enemies");
		}


        public override void SetDefaults()
        {
            item.useStyle = 1;
            item.width = 22;
            item.height = 22;
			item.autoReuse = true;
            item.noUseGraphic = true;
            item.UseSound = SoundID.Item1;
            item.thrown = true;
            item.channel = true;
            item.noMelee = true;
            item.shoot = mod.ProjectileType("StardustKnifeProj");
            item.useAnimation = 12;
            item.consumable = true;
            item.maxStack = 999;
            item.useTime = 12;
            item.shootSpeed = 10.0f;
            item.damage = 105;
            item.knockBack = 3f;
			item.value = Terraria.Item.sellPrice(0, 10, 0, 0);
            item.rare = 9;
            item.autoReuse = true;
            item.maxStack = 999;
            item.consumable = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(3459, 1);
            recipe.AddTile(412);
            recipe.SetResult(this, 111);
            recipe.AddRecipe();
        }
    }
}
