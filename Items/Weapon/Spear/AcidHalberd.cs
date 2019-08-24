using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Spear
{
    public class AcidHalberd : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gastro Glaive");
            Tooltip.SetDefault("Inflicts 'Acid Burn', which deals more damage as enemies are hit");

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
            item.shootSpeed = 5f;
            item.knockBack = 8f;
            item.damage = 48;
            item.value = Item.sellPrice(0, 0, 70, 0);
            item.rare = 5;
            item.shoot = mod.ProjectileType("Halberd");
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "HuskstalkSpear", 1);
            recipe.AddIngredient(null, "Acid", 6);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}