using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Returning
{
	public class BismiteChakra : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bismite Cutter");
			Tooltip.SetDefault("Occasionally poisons foes");
		}


		public override void SetDefaults()
		{
            item.damage = 9;            
            item.melee = true;
            item.width = 30;
            item.height = 28;
			item.useTime = 28;
			item.useAnimation = 25;
            item.noUseGraphic = true;
            item.useStyle = 1;
			item.knockBack = 2;
            item.value = Terraria.Item.sellPrice(0, 0, 90, 0);
            item.rare = 2;
			item.shootSpeed = 11f;
			item.shoot = mod.ProjectileType ("BismiteCutter");
			item.UseSound = SoundID.Item1;
			item.autoReuse = false;
		}
        public override bool CanUseItem(Player player)       //this make that you can shoot only 1 boomerang at once
        {
            for (int i = 0; i < 1000; ++i)
            {
                if (Main.projectile[i].active && Main.projectile[i].owner == Main.myPlayer && Main.projectile[i].type == item.shoot)
                {
                    return false;
                }
            }
            return true;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "BismiteCrystal", 10);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
		}
    }
}