using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable
{
    public class AcidVial : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Acid Vial");
			Tooltip.SetDefault("Throwing weapons may inflict Acid Burn");
		}


        public override void SetDefaults()
        {
            item.width = 20; 
            item.height = 30;
            item.rare = 5;
            item.maxStack = 99;

            item.useStyle = 5;
            item.useTime = item.useAnimation = 45;

            item.noMelee = true;
            item.consumable = true;
            item.autoReuse = false;

            item.buffType = mod.BuffType("AcidImbue");
            item.buffTime = 72000;

            item.UseSound = SoundID.Item3;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "Acid", 4);
            recipe.AddIngredient(ItemID.BottledWater, 1);
            recipe.AddTile(TileID.ImbuingStation);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
