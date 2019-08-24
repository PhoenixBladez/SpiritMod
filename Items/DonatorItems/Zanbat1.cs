using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.DonatorItems
{
    public class Zanbat1 : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Zanbat Sword");
			Tooltip.SetDefault("~Donator Item~");
		}


        int charger;
        public override void SetDefaults()
        {
            item.damage = 24;
            item.useTime = 15;
            item.useAnimation = 15;
            item.melee = true;
            item.width = 48;
            item.height = 48;
            item.useStyle = 1;
            item.knockBack = 6;
            item.value = 25700;
            item.rare = 2;
            item.shootSpeed = 6f;
            item.UseSound = SoundID.Item1;
            item.autoReuse = false;
            item.useTurn = true;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "GoldSword", 1);
            recipe.AddIngredient(ItemID.PlatinumBroadsword, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();

            ModRecipe recipe1 = new ModRecipe(mod);
            recipe1.AddIngredient(null, "PlatinumSword", 1);
            recipe1.AddIngredient(ItemID.GoldBroadsword, 1);
            recipe1.AddTile(TileID.Anvils);
            recipe1.SetResult(this, 1);
            recipe1.AddRecipe();

        }
    }
}