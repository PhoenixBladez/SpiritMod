using Terraria;
using System;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic
{
    public class CorruptStaff : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Putrid Staff");
			Tooltip.SetDefault("Shoots rapidfire diseased spit.");
		}


        public override void SetDefaults()
        {
            item.damage = 14;
            item.magic = true;
            item.mana = 8;
            item.width = 38;
            item.height = 38;
            item.useTime = 24;
            item.useAnimation = 38;
            item.useStyle = 5;
            Item.staff[item.type] = true;
            item.noMelee = true;
            item.knockBack = 3;
            item.value = 20000;
            item.rare = 2;
            item.UseSound = SoundID.Item20;
            item.autoReuse = false;
            item.shoot = mod.ProjectileType("Spit");
            item.shootSpeed = 20f;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.DemoniteBar, 12);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
