using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory.Artifact
{
    public class HolyGrail : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Grail of the Righteous");
            Tooltip.SetDefault("'The water clears all things, physical and ethereal'\nPress a hotkey to create a powerful ward at the player position\nThe ward grants you 'Cleansed,' reducing damage taken and mana usage\nEnemies caught in the Ward suffer a 'Holy Burn'\nIncreases magic damage by 10%\n1 minute cooldown\nEmpowers Solus with 'Righteous Virtue,' increasing its damage by 10%");
        }


        public override void SetDefaults()
        {
            item.width = 36;
            item.height = 46;
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
            player.magicDamage += 0.1f;
            player.GetModPlayer<MyPlayer>(mod).HolyGrail = true;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "FrostLotus", 1);
            recipe.AddIngredient(null, "ChaosEmber", 1);
            recipe.AddIngredient(null, "FireLamp", 1);
            recipe.AddIngredient(ItemID.SorcererEmblem, 1);
            recipe.AddIngredient(null, "SpiritBar", 10);
			recipe.AddIngredient(null, "SoulShred", 10);
            recipe.AddIngredient(null, "PrimordialMagic", 75);
            recipe.AddTile(null, "CreationAltarTile");
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
