using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.DonatorItems
{
    public class Terravolt : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Terravolt");
			Tooltip.SetDefault("Launches a beam of electricity \n~Donator Item~");
		}


        int charger;
        public override void SetDefaults()
        {
            item.damage = 80;
            item.useTime = 25;
            item.useAnimation = 25;
            item.melee = true;
            item.width = 48;
            item.height = 48;
            item.useStyle = 1;
            item.knockBack = 6;
            item.value = item.value = Item.sellPrice(0, 2, 50, 0);;
            item.rare = 8;
			item.shoot = mod.ProjectileType("ElectricityBolt");
            item.shootSpeed = 25f;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.useTurn = true;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "SunShard", 5);
            recipe.AddIngredient(671, 1);
			recipe.AddIngredient(1508, 1);
			recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();

            

        }
    }
}