using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace SpiritMod.Items.Accessory
{
	public class MageCharm : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Flame of the Magus");
            Tooltip.SetDefault("Increases magic damage by 7%\nIncreases magic critical strike chance by 10% and reduces mana cost by 6%\nIncreases maximum mana by 50 and increases mana regeneration\nYou emit blue light at all times and magic critical hits may deal extra damage\nMagic attacks may spawn sparks that deal more damage the less mana you have \nMagic attacks may slow down enemies.");
            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(8, 5));
            ItemID.Sets.ItemNoGravity[item.type] = true;
        }


		public override void SetDefaults()
		{
			item.width = 22;
            item.height = 54;
            item.value = Item.sellPrice(0, 5, 0, 0);
            item.rare = 8;
            item.defense = 1;
			item.accessory = true;
		}
        public override void UpdateAccessory(Player player, bool hideVisual)
		{
            player.GetModPlayer<MyPlayer>(mod).eyezorEye = true;
            player.GetModPlayer<MyPlayer>(mod).manaWings = true;
            player.GetModPlayer<MyPlayer>(mod).winterbornCharmMage = true;
            player.statManaMax2 += 50;
            player.magicDamage += 0.07f;
            player.magicCrit += 10;
            player.manaRegenBonus += 2;
            Lighting.AddLight(player.position, 0.0f, .75f, 1.25f);
            player.manaCost -= 0.06f;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "ManaFlame", 1);
            recipe.AddIngredient(null, "WintryCharmMage", 1);
            recipe.AddIngredient(ItemID.ManaRegenerationBand, 1);
            recipe.AddIngredient(null, "Cogflower", 1);
            recipe.AddIngredient(null, "FallenAngel", 1);
            recipe.AddIngredient(null, "Eyezor", 1);
            recipe.AddIngredient(ItemID.Ectoplasm, 5);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();

        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
    }
}
