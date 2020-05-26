using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.IMArmor
{
    [AutoloadEquip(EquipType.Body)]
    public class IlluminantGarb : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Illuminant Robe");

        }

        public override void SetDefaults()
        {
            item.width = 34;
            item.height = 24;
            item.value = 120000;
            item.rare = 4;
            item.vanity = true;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Silk, 7);
            recipe.AddIngredient(ItemID.SoulofLight, 5);
            recipe.AddIngredient(mod.ItemType("Geode"), 3);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
