using Terraria;
using System;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic
{
    public class CrimsonStaff : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bloodletter");
			Tooltip.SetDefault("Shoots Clusters of Blood");
		}


        public override void SetDefaults()
        {
            item.damage = 15;
            item.magic = true;
            item.mana = 9;
            item.width = 36;
            item.height = 42;
            item.useTime = 28;
            item.useAnimation = 28;
            item.useStyle = 5;
            Item.staff[item.type] = true;
            item.noMelee = true;
            item.knockBack = 5;
            item.value = 20000;
            item.rare = 2;
            item.crit = 9;
            item.UseSound = SoundID.Item20;
            item.autoReuse = false;
            item.shoot = mod.ProjectileType("Blood");
            item.shootSpeed = 13f;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.CrimtaneBar, 12);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
