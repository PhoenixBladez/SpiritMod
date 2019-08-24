using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.SpiritCrystal
{
    [AutoloadEquip(EquipType.Body)]
    public class SpiritCrystalBody : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spirit Crystal Plate");
            Tooltip.SetDefault("Increases maximum number of minions by 1\nIncreases minion damage by 10%");

        }

        public override void SetDefaults()
        {
            item.width = 34;
            item.height = 24;
            item.value = 12000;
            item.rare = 5;
            item.defense = 11;
        }

        public override void UpdateEquip(Player player)
        {
            player.minionDamage += 0.10f;
            player.maxMinions += 1;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "SpiritCrystal", 12);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
