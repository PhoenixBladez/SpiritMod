using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Yoyo
{
	public class Ployo : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pearlwood Throw");
		}


        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.WoodYoyo);
            item.damage = 20;
            item.value = 300;
            item.rare = 4;
            item.knockBack = 2;
            item.channel = true;
            item.useStyle = 5;
            item.useAnimation = 25;
            item.useTime = 28;
            item.shoot = mod.ProjectileType("PearlP");
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Pearlwood, 10);
            recipe.AddIngredient(ItemID.Cobweb, 20);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}