using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Yoyo
{
	public class Ebon : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ebonwood Throw");
		}


        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.WoodYoyo);
            item.damage = 11;
            item.value = 190;
            item.rare = 0;
            item.knockBack = 2;
            item.channel = true;
            item.useStyle = 5;
            item.useAnimation = 25;
            item.useTime = 28;
            item.shoot = mod.ProjectileType("EbonP");
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Ebonwood, 10);
            recipe.AddIngredient(ItemID.Cobweb, 20);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}