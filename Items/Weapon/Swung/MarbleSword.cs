using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Swung
{
    public class MarbleSword : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gilded Blade");
			Tooltip.SetDefault("Right click to parry, dealing much more damage");
		}


        public override void SetDefaults()
        {
            item.damage = 24;            
            item.melee = true;            
            item.width = 44;              
            item.height = 44;
            item.useTime = 25;
            item.useAnimation = 25;     
            item.useStyle = 1;        
            item.knockBack = 5;
            item.value = Terraria.Item.sellPrice(0, 2, 0, 0);
            item.rare = 2;
            item.shootSpeed = 0f;
            item.UseSound = SoundID.Item1;        
            item.autoReuse = true;
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }


        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                item.useStyle = 5;
                item.damage = 40;
                item.useTime = 46;
                item.knockBack = 4;
                item.useAnimation = 46;
            }
            else
            {
                item.useStyle = 1;
                item.useTime = 24;
                item.damage = 20;
                item.UseSound = SoundID.Item1;
                item.useAnimation = 24;
                item.knockBack = 5;
                item.shoot = 0;
            }
            return base.CanUseItem(player);
        }
		public override Vector2? HoldoutOffset()
        {
				{
				return new Vector2(-5, 0);
				}
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "MarbleChunk", 16);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}