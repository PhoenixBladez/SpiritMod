using Terraria;
using System;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic
{
    public class BloodStaff : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vessel Drainer");
			Tooltip.SetDefault("Calls upon a clot that splits into life-stealing blood cells.");
		}


        public override void SetDefaults()
        {
            item.damage = 42;
            item.magic = true;
            item.mana = 14;
            item.width = 52;
            item.height = 52;
            item.useTime = 50;
            item.useAnimation = 50;
            item.useStyle = 5;
            Item.staff[item.type] = true;
            item.noMelee = true;
            item.knockBack = 3;
            item.value = 20000;
            item.rare = 4;
            item.crit += 10;
            item.UseSound = SoundID.Item20;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("BloodVessel");
            item.shootSpeed = 9f;
        }

        public override void AddRecipes()
        {
            ModRecipe modRecipe = new ModRecipe(mod);
            modRecipe.AddIngredient(null, "CrimsonStaff", 1);
            modRecipe.AddIngredient(null, "JungleStaff", 1);
            modRecipe.AddIngredient(null, "DungeonStaff", 1);
            modRecipe.AddIngredient(null, "HellStaff", 1);
            modRecipe.AddTile(26);
            modRecipe.SetResult(this, 1);
            modRecipe.AddRecipe();
        }

    }
}
