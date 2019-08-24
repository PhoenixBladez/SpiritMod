using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
    [AutoloadEquip(EquipType.Shield)]
    public class GoldShield : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gilded Shield");
			Tooltip.SetDefault("Provides Immunity to Knockback. As your health goes down, your life regeneration increases.");
		}
        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 28;
            item.rare = 4;
            item.value = 100000;

            item.defense = 4;
            item.accessory = true;
        }
		public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "GoldenApple", 1);
            recipe.AddIngredient(156, 1);
           recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.noKnockback = true;
            float defBoost = (float)(player.statLifeMax2 - player.statLife) / (float)player.statLifeMax2 * 20f;
            player.statDefense += (int)defBoost;
        }
    }
}
