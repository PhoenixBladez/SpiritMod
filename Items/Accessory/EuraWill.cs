using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
	public class EuraWill : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Eura's Will");
			Tooltip.SetDefault("'The Arcane speaks to me...'\nIncreases magic damage by 18%\nIncreases maximum mana by 40\nYou restore mana when damaged\n~Donator item~");
		}


		public override void SetDefaults()
		{
            item.width = 24;
            item.height = 26;
			item.value = Item.sellPrice(0, 2, 0, 0);
			item.rare = 5;

			item.accessory = true;

			item.defense = 2;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
            player.magicDamage += 0.18f;
            player.statManaMax2 += 40;
            player.magicCuffs = true;

		}
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.MagicCuffs, 1);
            recipe.AddIngredient(ItemID.SorcererEmblem, 1);
            recipe.AddIngredient(ItemID.ManaCrystal, 2);
            recipe.AddIngredient(null, "SpiritBar", 5);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
