using System;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace SpiritMod.Items.Weapon.Magic
{
    public class TerraStaff : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Terra Staff");
			Tooltip.SetDefault("'May the Wrath of the Elements consume your foes.'");
		}


        public override void SetDefaults()
        {
            item.damage = 78;
            item.magic = true;
            item.mana = 20;
            item.width = 58;
            item.height = 58;
            item.useTime = 14;
            item.useAnimation = 14;
            item.useStyle = 5;
            Item.staff[item.type] = true;
            item.noMelee = true;
            item.knockBack = 6;
            item.value = 200000;
            item.rare = 9;
            item.crit = 20;
            item.UseSound = SoundID.Item60;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("TerraProj");
            item.shootSpeed = 16f;
        }

        public override void AddRecipes()
        {
            ModRecipe modRecipe = new ModRecipe(mod);
            modRecipe.AddIngredient(null, "TrueDarkStaff", 1);
            modRecipe.AddIngredient(null, "TrueHallowedStaff", 1);
            modRecipe.AddTile(134);
            modRecipe.SetResult(this, 1);
            modRecipe.AddRecipe();
            
            modRecipe = new ModRecipe(mod);
            modRecipe.AddIngredient(null, "TrueBloodStaff", 1);
            modRecipe.AddIngredient(null, "TrueHallowedStaff", 1);
            modRecipe.AddTile(134);
            modRecipe.SetResult(this, 1);
            modRecipe.AddRecipe();
        }
    }
}
