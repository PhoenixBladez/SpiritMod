using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory.Artifact
{
    public class DarkBough : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bough of Corruption");
            Tooltip.SetDefault("'The voices within offer me wicked magics'\nIncreases maximum minions and sentries by 1\nMinion attacks on foes have a 10% chance to release multiple Nightmare Bolts in different directions\nMinions have an extremely low chance to return damage dealt as health\nIncreases summoning damage by 10%\nEmpowers Death Wind, causing scythes to implant exploding seeds upon hitting foes");
        }


        public override void SetDefaults()
        {
            item.width = 34;
            item.height = 30;
            item.rare = 6;
            item.value = Item.sellPrice(0, 3, 0, 0);
            item.accessory = true;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            TooltipLine line = new TooltipLine(mod, "ItemName", "Artifact Accessory");
            line.overrideColor = new Color(100, 0, 230);
            tooltips.Add(line);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.minionDamage += 0.1f;
            player.maxMinions += 1;
            player.maxTurrets += 1;
            player.GetModPlayer<MyPlayer>(mod).DarkBough = true;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "FrostLotus", 1);
            recipe.AddIngredient(null, "ChaosEmber", 1);
            recipe.AddIngredient(null, "FireLamp", 1);
            recipe.AddIngredient(ItemID.SummonerEmblem, 1);
            recipe.AddIngredient(null, "SpiritBar", 10);
			recipe.AddIngredient(null, "SoulShred", 10);
            recipe.AddIngredient(null, "PrimordialMagic", 75);
            recipe.AddTile(null, "CreationAltarTile");
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
