using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace SpiritMod.Items.Accessory
{
	public class StarCharm : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Starplate Signet");
			Tooltip.SetDefault("Causes electrical stars to fall and increases length of invincibility after taking damage\nIncreases critical strike chance by 7% and movement speed by 12% \n'The stars shine in your favor'");
		}


		public override void SetDefaults()
		{
			item.width = 32;
            item.height = 32;
            item.defense = 1;
			item.value = Item.sellPrice(0, 15, 0, 0);
            item.defense = 2;
            item.rare = 2;
            item.expert = true;
            item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
		{
            player.GetModPlayer<MyPlayer>(mod).starCharm = true;
            player.longInvince = true;
            player.moveSpeed += .12f;
            player.meleeCrit += 7;
            player.magicCrit += 7;
            player.thrownCrit += 7;
            player.rangedCrit += 7;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.StarVeil, 1);
            recipe.AddIngredient(null, "StarMap", 1);
            recipe.AddIngredient(null, "StarPiece", 3);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
