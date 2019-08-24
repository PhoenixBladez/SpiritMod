using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Bow
{
    public class SpiritWoodBow : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Duskwood Bow");
		}



        public override void SetDefaults()
        {
            item.damage = 11;
            item.noMelee = true;
            item.ranged = true;
            item.width = 26;
            item.height = 62;
            item.useTime = 25;
			item.useAnimation = 25;
            item.useStyle = 5;
            item.shoot = 3;
            item.useAmmo = AmmoID.Arrow;
            item.knockBack = 1;
            item.useTurn = false;
            item.value = Terraria.Item.sellPrice(0, 0, 0, 0);
            item.rare = 0;
            item.UseSound = SoundID.Item5;          
            item.autoReuse = false;
            item.shootSpeed = 7f;
            item.crit = 8;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "SpiritWoodItem", 10);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}