using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable
{
    public class ChaosPearl : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chaos Pearl");
			Tooltip.SetDefault("Teleports you to landing area.");
		}


        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 20;
            item.value = Item.sellPrice(0, 0, 3, 0);
            item.rare = 6;
            item.maxStack = 999;
            //item.crit = 4;
            item.damage = 0;
           // item.knockBack = 3;
            item.useStyle = 1;
            item.useTime = item.useAnimation = 18;
          //  item.thrown = true;
            item.noMelee = true;
            item.autoReuse = true;
            item.consumable = true;
            item.noUseGraphic = true;
            item.shoot = mod.ProjectileType("ChaosPearl");
            item.shootSpeed = 9;
            item.UseSound = SoundID.Item1;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(502, 1);
            recipe.AddIngredient(520, 1);
			recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 4);
            recipe.AddRecipe();
        }
    }
}