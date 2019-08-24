using Terraria;
using System;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic
{
    public class HellStaff : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Firespark Staff");
			Tooltip.SetDefault("Shoots a flame that shatters into flametrails.");
		}


        public override void SetDefaults()
        {
            item.damage = 24;
            item.magic = true;
            item.mana = 15;
            item.width = 44;
            item.height = 44;
            item.useTime = 38;
            item.useAnimation = 38;
            item.useStyle = 5;
            Item.staff[item.type] = true;
            item.noMelee = true;
            item.knockBack = 4;
            item.value = 20000;
            item.rare = 3;
            item.UseSound = SoundID.Item20;
            item.autoReuse = false;
            item.shoot = mod.ProjectileType("Firespike");
            item.shootSpeed = 16f;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.HellstoneBar, 16);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
