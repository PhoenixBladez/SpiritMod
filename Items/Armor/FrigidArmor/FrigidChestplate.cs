using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.FrigidArmor
{
    [AutoloadEquip(EquipType.Body)]
    public class FrigidChestplate : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Frigid Plate");
            Tooltip.SetDefault("Increases melee speed by 4%\nIncreases magic critical strike chance by 3%");

        }


        int timer = 0;

        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 24;
           item.value = 1100;
            item.rare = 1;
            item.defense = 3;
        }
        public override void UpdateEquip(Player player)
        {
            player.meleeSpeed += 0.04f;
            player.magicCrit += 3;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "FrigidFragment", 10);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
