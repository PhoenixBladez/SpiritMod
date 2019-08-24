using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Bow
{
    public class BloomBuster : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bloom Buster");
			Tooltip.SetDefault("Fires two arrows in quick succession");
		}



        public override void SetDefaults()
        {
            item.damage = 24;
            item.noMelee = true;
            item.ranged = true;
            item.width = 16;
            item.height = 32;
            item.useTime = 21;
            item.useAnimation = 27;
            item.useStyle = 5;
            item.shoot = 4;
            item.useAmmo = AmmoID.Arrow;
            item.knockBack = 3;
            item.useTurn = false;
            item.value = Terraria.Item.sellPrice(0, 1, 0, 0);
            item.rare = 3;
            item.UseSound = SoundID.Item5;
            item.autoReuse = false;
            item.shootSpeed = 6.7f;            
        } 
		public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Vine, 1);
			recipe.AddIngredient(ItemID.Stinger, 3);
            recipe.AddIngredient(ItemID.JungleSpores, 6);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
		}
    }
}