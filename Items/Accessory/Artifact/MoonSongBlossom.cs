using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory.Artifact
{
    public class MoonSongBlossom : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Moonsong Blossom");
            Tooltip.SetDefault("'Curious how it mimics the shimmer and glow of the moon itself'\nRanged Attacks may release a volley of Blossom Arrows toward foes\nBlossom Arrows may inflict 'Moon Burn'\nThe player is occasionally protected by a Blossom Petal, which regenerates every 7 seconds\nIncreases ranged damage by 10% and ranged critical strike chance by 7%\n Empowers Star Weaver: hitting enemies with Star Weaver's Star Cores may cause a moon to appear above struck enemies");
        }


        public override void SetDefaults()
        {
            item.width = 38;
            item.height = 34;
            item.rare = 6;
            item.value = Item.sellPrice(0, 3, 0, 0);
            item.accessory = true;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            TooltipLine line = new TooltipLine(mod, "ItemName", "Artifact Accessory");
            line.overrideColor = new Color(100, 60, 240);
            tooltips.Add(line);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.rangedDamage += 0.1f;
            player.rangedCrit += 7;
            player.GetModPlayer<MyPlayer>(mod).MoonSongBlossom = true;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "FrostLotus", 1);
            recipe.AddIngredient(null, "ChaosEmber", 1);
            recipe.AddIngredient(null, "FireLamp", 1);
            recipe.AddIngredient(ItemID.RangerEmblem, 1);
            recipe.AddIngredient(null, "SpiritBar", 10);
			recipe.AddIngredient(null, "SoulShred", 10);
            recipe.AddIngredient(null, "PrimordialMagic", 75);
            recipe.AddTile(null, "CreationAltarTile");
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
