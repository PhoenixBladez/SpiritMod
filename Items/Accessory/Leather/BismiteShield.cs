using System;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory.Leather
{
    [AutoloadEquip(EquipType.Shield)]
    public class BismiteShield : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Noxious Targe");
			Tooltip.SetDefault("Being struck by an enemy poisones them\nIncreases defense by 1 for every poisoned enemy near the player\nThis effect stacks three times");
		}

        public override void SetDefaults()
        {
            item.width = 24;
            item.height = 28;
            item.rare = 2;
            item.defense = 1;
            item.melee = true;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
           player.GetModPlayer<MyPlayer>(mod).bismiteShield = true;

           player.statDefense += 1 * player.GetModPlayer<MyPlayer>(mod).bismiteShieldStacks;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("LeatherShield"), 1);
            recipe.AddIngredient(null, "BismiteCrystal", 6);
            recipe.AddRecipeGroup("EvilMaterial1", 4);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
