using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable.Potion
{
    public class SoulPotion : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Soulguard Potion");
			Tooltip.SetDefault("Getting hurt may cause all nearby enemies to suffer 'Soul Burn' for a short while\nIncreases melee damage by 5%");
		}


        public override void SetDefaults()
        {
            item.width = 20; 
            item.height = 30;
            item.rare = 5;
            item.maxStack = 30;

            item.useStyle = 2;
            item.useTime = item.useAnimation = 20;

            item.consumable = true;
            item.autoReuse = false;

            item.buffType = mod.BuffType("SoulPotionBuff");
            item.buffTime = 10800;

            item.UseSound = SoundID.Item3;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "SoulBloom", 1);
            recipe.AddIngredient(ItemID.IronOre, 1);
            recipe.AddIngredient(null, "SpiritOre", 3);
            recipe.AddIngredient(ItemID.BottledWater, 1);
            recipe.AddTile(TileID.Bottles);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
