using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.DonatorItems.Folv
{
    [AutoloadEquip(EquipType.Balloon)]
    public class FolvStaff2 : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Folv's Staff of Defense");
			Tooltip.SetDefault("Grants 3% increased magic damage, +20 maximum mana, and immunity to knockback \n ~Donator Item~");
		}
        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 30;
            item.rare = 4;
            item.value = 5000;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.noKnockback = true;
            player.statManaMax2 += 20;
            player.magicDamage *= 1.03f;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.CobaltShield, 1);
            recipe.AddIngredient(ItemID.Amber, 1);
            recipe.AddIngredient(ItemID.Diamond, 1);
            recipe.AddIngredient(ItemID.Emerald, 1);
            recipe.AddIngredient(ItemID.Ruby, 1);
            recipe.AddIngredient(ItemID.Sapphire, 1);
            recipe.AddIngredient(ItemID.Topaz, 1);
            recipe.AddIngredient(ItemID.Amethyst, 1);
            recipe.AddIngredient(null, "FolvStaff1", 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
