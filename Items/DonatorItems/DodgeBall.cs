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
			Tooltip.SetDefault("Throw mach speed dodgeballs!");
		}


        public override void SetDefaults()
        {
            item.damage = 12;
            item.ranged = true;
            item.width = 30;
            item.height = 30;
            item.useTime = 22;
            item.useAnimation = 22;
            item.noUseGraphic = true;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.knockBack = 0;
            item.value = 4000;
            item.rare = 2;
            item.shootSpeed = 8f;
            item.shoot = ModContent.ProjectileType<Dodgeball>();
            item.UseSound = SoundID.Item1;
            item.autoReuse = false;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Silk, 11);
            recipe.AddIngredient(ModContent.ItemType<OldLeather>(), 8);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();

            ModRecipe recipe2 = new ModRecipe(mod);
            recipe2.AddIngredient(null, "DodgeBall1");
            recipe2.SetResult(this);
            recipe2.AddRecipe();
        }
    }
}