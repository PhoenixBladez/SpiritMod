using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Spear
{
    public class HuskstalkSpear : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Huskstalk Spear");
			Tooltip.SetDefault("Inflicts withering leaf");
		}


        public override void SetDefaults()
        {
            item.useStyle = 5;
            item.width = 24;
            item.height = 24;
            item.noUseGraphic = true;
            item.UseSound = SoundID.Item1;
            item.melee = true;
            item.noMelee = true;
            item.useAnimation = 27;
            item.useTime = 27;
            item.shootSpeed = 4f;
            item.knockBack = 4f;
            item.damage = 19;
            item.value = Item.sellPrice(0, 0, 10, 0);
            item.rare = 1;
            item.shoot = mod.ProjectileType("HuskstalkSpearProj");
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "AncientBark", 8);
            recipe.AddIngredient(null, "EnchantedLeaf", 4);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}