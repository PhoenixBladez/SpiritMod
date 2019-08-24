using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Thrown
{
	public class VortexSlasher : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vortex Slasher");
			Tooltip.SetDefault("Multiple Vortex Slashers rip through enemies");
		}


        public override void SetDefaults()
        {
            item.useStyle = 1;
            item.width = 9;
            item.height = 15;
            item.noUseGraphic = true;
            item.UseSound = SoundID.Item1;
            item.thrown = true;
            item.channel = true;
            item.noMelee = true;
            item.consumable = true;
            item.maxStack = 999;
            item.shoot = mod.ProjectileType("VortexSlasherProjectile");
            item.useAnimation = 13;
            item.useTime = 13;
            item.shootSpeed = 12f;
            item.damage = 115;
            item.knockBack = 3.5f;
			item.value = Item.sellPrice(0, 0, 1, 0);
            item.crit = 24;
            item.rare = 9;
            item.autoReuse = true;
            item.consumable = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(3456, 1);
            recipe.AddTile(412);
            recipe.SetResult(this, 111);
            recipe.AddRecipe();
        }
    }
}