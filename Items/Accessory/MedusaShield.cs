using System;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
    [AutoloadEquip(EquipType.Shield)]
    public class MedusaShield : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Medusa Shield");
			Tooltip.SetDefault("Provides immunity to knockback and the stoned debuff \n As your health goes down, your life regeneration increases");
		}
        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 36;
            item.rare = 5;
            item.value = 100000;
            item.accessory = true;
            item.defense = 6;
        }
		public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "GoldShield", 1);
            recipe.AddIngredient(ItemID.PocketMirror, 1);
            recipe.AddTile(114);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.noKnockback = true;
            float defBoost = (float)(player.statLifeMax2 - player.statLife) / (float)player.statLifeMax2 * 20f;
            player.statDefense += (int)defBoost;
            player.buffImmune[BuffID.Stoned] = true;
        }
    }
}
