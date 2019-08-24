using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.DonatorItems
{
    public class DodgeBall : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lightning Throw");
			Tooltip.SetDefault("Throw a very fast, mach speed dodgeball! \n ~Donator Item~");
		}


        public override void SetDefaults()
        {
            item.damage = 32;
            item.thrown = true;
            item.width = 30;
            item.height = 30;
            item.useTime = 15;
            item.useAnimation = 15;
            item.noUseGraphic = true;
            item.useStyle = 1;
            item.knockBack = 0;
            item.value = 35800;
            item.rare = 4;
            item.shootSpeed = 10f;
            item.shoot = mod.ProjectileType("Dodgeball");
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Silk, 20);
            recipe.AddIngredient(ItemID.SoulofNight, 4);
            recipe.AddIngredient(ItemID.SoulofLight, 4);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();

            ModRecipe recipe2 = new ModRecipe(mod);
            recipe2.AddIngredient(null, "DodgeBall1", 1);
            recipe2.SetResult(this);
            recipe2.AddRecipe();
        }
    }
}