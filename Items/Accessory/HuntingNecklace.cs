using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
    public class HuntingNecklace : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Briarhunt Charm");
            Tooltip.SetDefault("Increases melee damage by 4% and melee speed by 3%\nIncreases melee critical srike chance by 9% and ranged critical strike chance by 7%\nIncreases magic and thrown critical strike chance by 5% and maximum life by 10");

        }


        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 42;
            item.value = Item.buyPrice(0, 1, 20, 0);
            item.rare = 4;
            item.defense = 3;

            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.magicCrit += 5;
            player.meleeCrit += 9;
            player.thrownCrit += 5;
            player.rangedCrit += 7;
            player.meleeDamage += 0.04f;
            player.statLifeMax2 += 10;
            player.meleeSpeed += 0.03f;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "ReachBrooch", 1);
            recipe.AddIngredient(null, "CleftHorn", 1);
            recipe.AddIngredient(null, "FloranCharm", 1);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
