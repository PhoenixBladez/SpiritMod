
﻿using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Swung
{
    public class TitanicBlade : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Titanic Blade");
			Tooltip.SetDefault("Shoots out a mass of slowing water");
		}


        public override void SetDefaults()
        {
            item.width = 54;
            item.height = 50;
            item.rare = 6;

            item.crit += 4;
            item.damage = 57;
            item.knockBack = 6;

            item.useStyle = 1;
            item.useTime = item.useAnimation = 19;

            item.melee = true;
            item.autoReuse = true;

            item.shoot = mod.ProjectileType("WaterMass");
            item.shootSpeed = 12;
            item.UseSound = SoundID.Item1;
            item.value = Terraria.Item.sellPrice(0, 6, 0, 0);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "TidalEssence", 16);
            recipe.AddTile(null, "EssenceDistorter");
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}