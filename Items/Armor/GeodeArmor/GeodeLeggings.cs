using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.GeodeArmor
{
    [AutoloadEquip(EquipType.Legs)]
    public class GeodeLeggings : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Geode Leggings");
		}

        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 22;
            item.value = Terraria.Item.sellPrice(0, 0, 75, 0);
            item.rare = 4;

            item.vanity = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "Geode", 6);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
