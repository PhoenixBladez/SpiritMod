using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace SpiritMod.Items.Accessory
{
    public class TwilightStone : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Twilight Stone");
            Tooltip.SetDefault("Increases melee and ranged damage by 8%\nIncreases melee and ranged critical strike chance as health wanes\nMelee attacks may inflict 'Solar Burn,' and ranged attacks may inflict 'Moon Burn'\nIncreases damage dealt by 4% at daytime, and increases life regen at nighttime");
        }


        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.value = Item.buyPrice(0, 18, 0, 0);
            item.rare = 7;
            item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.meleeDamage += 0.08f;
            player.rangedDamage += .08f;
            if (Main.dayTime)
            {
                player.meleeDamage += 0.04f;
                player.rangedDamage += .04f;
                player.thrownDamage += .04f;
                player.magicDamage += .04f;
                player.minionDamage += .04f;
            }
            else
            {
                player.lifeRegen += 1;
            }
            float defBoost = (float)(player.statLifeMax2 - player.statLife) / (float)player.statLifeMax2 * 7f;
            player.rangedCrit += (int)defBoost;
            float defBoost1 = (float)(player.statLifeMax2 - player.statLife) / (float)player.statLifeMax2 * 7f;
            player.meleeCrit += (int)defBoost1;
            player.GetModPlayer<MyPlayer>(mod).moonStone = true;
            player.GetModPlayer<MyPlayer>(mod).sunStone = true;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "DawnStone", 1);
            recipe.AddIngredient(null, "DuskStone1", 1);
            recipe.AddIngredient(null, "InfernalAppendage", 5);
            recipe.AddIngredient(null, "DuskStone", 5);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
