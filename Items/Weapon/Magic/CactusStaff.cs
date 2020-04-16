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
			Tooltip.SetDefault("Shoots two cactus needles at foes\nThese pins stick to enemies and poison them");
		}


        public override void SetDefaults()
        {
            item.damage = 5;
            item.magic = true;
            item.mana = 7;
            item.width = 40;
            item.height = 40;
            item.useTime = 11;
            item.useAnimation = 22;
            item.useStyle = 5;
            Item.staff[item.type] = true;
            item.noMelee = true;
            item.knockBack = 2f;
            item.value = 200;
            item.rare = 1;
            item.UseSound = new Terraria.Audio.LegacySoundStyle(2, 8);
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
