using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.IMArmor
{
    [AutoloadEquip(EquipType.Legs)]
    public class IlluminantLegs : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Illuminant Greaves");
			Tooltip.SetDefault("Increases life regeneration slightly");
		}
        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 16;
            item.value = 90000;
            item.rare = 7;
            item.defense = 17;
        }
        public override void UpdateEquip(Player player)
        {

            player.statLifeMax2 += 25;
            player.lifeRegen += 2;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "IlluminatedCrystal", 12);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
