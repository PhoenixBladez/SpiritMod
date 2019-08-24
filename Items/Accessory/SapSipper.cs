using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace SpiritMod.Items.Accessory
{
	public class SapSipper : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sap Sipper");
            Tooltip.SetDefault("A bloody ward surrounds you, inflicting Blood Corruption to nearby enemies\nIncreases life regeneration slightly\nProvides immunity to the 'Poisoned' buff\nIncreases maximum life by 10\nMagic attacks drench enemies in venom occasionally\nIncreases critical strike chance by 8%\nCritical hits may heal the player");

        }


		public override void SetDefaults()
		{
			item.width = 18;
            item.height = 18;
            item.value = Item.buyPrice(0, 3, 0, 0);
            item.rare = 9;
			item.accessory = true;
		}
        public override void UpdateAccessory(Player player, bool hideVisual)
		{
            player.lifeRegen += 3;
            player.GetModPlayer<MyPlayer>(mod).Ward1 = true;
            player.statLifeMax2 += 10;
            player.statDefense -= 1;
            player.buffImmune[BuffID.Poisoned] = true;
            player.meleeCrit += 8;
            player.thrownCrit += 8;
            player.magicCrit += 8;
            player.rangedCrit += 8;
            player.GetModPlayer<MyPlayer>(mod).ToxicExtract = true;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "PathogenWard", 1);
            recipe.AddIngredient(null, "ToxicExtract", 1);
            recipe.AddIngredient(null, "Veinstone", 8);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
