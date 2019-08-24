using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Flail
{
    public class Earthshaker : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Earthshaker");
            Tooltip.SetDefault("The flail detaches from its chain, bouncing around\nUpon hitting the ground, the flail releases a pulse of damaging dust");

        }


        public override void SetDefaults()
        {
            item.width = 44;
            item.height = 44;
            item.rare = 8;
            item.noMelee = true;
            item.useStyle = 5; 
            item.useAnimation = 36; 
            item.useTime = 36;
            item.knockBack = 8;
            item.value = Item.sellPrice(0, 2, 43, 0);
            item.damage = 85;
            item.noUseGraphic = true; 
            item.shoot = mod.ProjectileType("EarthshatterProj");
            item.shootSpeed = 16f;
            item.UseSound = SoundID.Item1;
            item.melee = true; 
            item.channel = true; 
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.TurtleShell, 3);
            recipe.AddIngredient(ItemID.Emerald, 5);
            recipe.AddIngredient(ItemID.HallowedBar, 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}