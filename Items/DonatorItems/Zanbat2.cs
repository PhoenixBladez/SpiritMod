using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.DonatorItems
{
    public class Zanbat2 : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Zanbat Sword");
			Tooltip.SetDefault("~Donator Item~");
		}


        int charger;
        public override void SetDefaults()
        {
            item.damage = 32;
            item.useTime = 13;
            item.useAnimation = 13;
            item.melee = true;
            item.width = 48;
            item.height = 48;
            item.useStyle = 1;
            item.knockBack = 6;
            item.value = 25700;
            item.rare = 3;
            item.crit = 2;
            item.shootSpeed = 6f;
            item.UseSound = SoundID.Item1;
            item.autoReuse = false;
            item.useTurn = true;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "Zanbat1", 1);
            recipe.AddIngredient(ItemID.HellstoneBar, 15);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();

        }
    }
}