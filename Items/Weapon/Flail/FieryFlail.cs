using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Flail
{
    public class FieryFlail : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fiery Flail");
			Tooltip.SetDefault("Burns foes in a nearby area");
		}


        public override void SetDefaults()
        {
            item.width = 44;
            item.height = 44;
            item.rare = 3;
            item.noMelee = true;
            item.useStyle = 5; 
            item.useAnimation = 34; 
            item.useTime = 34;
            item.knockBack = 6;
            item.value = Item.sellPrice(0, 0, 43, 0);
            item.damage = 26;
            item.noUseGraphic = true; 
            item.shoot = mod.ProjectileType("FieryFlailProj");
            item.shootSpeed = 16f;
            item.UseSound = SoundID.Item1;
            item.melee = true; 
            item.channel = true; 
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "CarvedRock", 14);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}