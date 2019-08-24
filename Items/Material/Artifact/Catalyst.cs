using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace SpiritMod.Items.Material.Artifact
{
    public class Catalyst : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mysterious Catalyst");
			Tooltip.SetDefault("'It seems to originate from Ancient Aliens'");
            ItemID.Sets.ItemNoGravity[item.type] = true;
            ItemID.Sets.ItemIconPulse[item.type] = true;
        }


        public override void SetDefaults()
        {
            item.width = 42;
            item.height = 42;
            item.value = 500;
            item.rare = 10;
            item.maxStack = 10;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.LunarTabletFragment, 4);
            recipe.AddIngredient(ItemID.Ectoplasm, 6);
            recipe.AddRecipeGroup("CelestialFragment", 3);
            recipe.AddIngredient(null, "IcyEssence", 3);
            recipe.AddIngredient(null, "FieryEssence", 3);
            recipe.AddIngredient(null, "DuneEssence", 3);
            recipe.AddIngredient(null, "PrimevalEssence", 3);
            recipe.AddIngredient(null, "TidalEssence", 3);
            recipe.AddTile(null, "CreationAltarTile");
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}