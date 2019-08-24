using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Weapon.Swung
{
    public class SpiritWoodSword : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Duskwood Sword");
		}


        public override void SetDefaults()
        {
            item.damage = 15;
            item.useTime = 19;
            item.useAnimation = 19;
            item.melee = true;            
            item.width = 36;              
            item.height = 36;             
            item.useStyle = 1;        
            item.knockBack = 5;
            item.value = Terraria.Item.sellPrice(0, 0, 0, 0);
            item.rare = 0;
            item.UseSound = SoundID.Item1;   
            item.autoReuse = false;
            item.useTurn = true;
        }
       
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "SpiritWoodItem", 7);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}