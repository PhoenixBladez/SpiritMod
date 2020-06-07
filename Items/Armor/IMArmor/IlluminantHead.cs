using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.IMArmor
{
    [AutoloadEquip(EquipType.Head)]
    public class IlluminantHead : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Illuminant Cowl");
			Tooltip.SetDefault("'It resembles a familiar figure'");
		}
        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 24;
            item.value = 100000;
            item.rare = 4;
            item.vanity = true;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Silk, 5);
            recipe.AddIngredient(ItemID.SoulofLight, 5);
            recipe.AddIngredient(ModContent.ItemType<Geode>(), 2);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}