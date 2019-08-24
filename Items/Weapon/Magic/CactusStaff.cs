using Terraria;
using System;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic
{
    public class CactusStaff : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cactus Staff");
			Tooltip.SetDefault("Shoot a slow, thorny Cactus Ball at foes!");
		}


        public override void SetDefaults()
        {
            item.damage = 9;
            item.magic = true;
            item.mana = 11;
            item.width = 40;
            item.height = 40;
            item.useTime = 18;
            item.useAnimation = 18;
            item.useStyle = 5;
            Item.staff[item.type] = true;
            item.noMelee = true;
            item.knockBack = 0;
            item.value = 200;
            item.rare = 1;
            item.UseSound = SoundID.Item34;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("CactusProj");
            item.shootSpeed = 8f;
            item.autoReuse = false;
        }


      public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Cactus, 22);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
