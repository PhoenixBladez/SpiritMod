using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.AcidArmor
{
    [AutoloadEquip(EquipType.Legs)]
    public class AcidLegs : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Acid Leggings");
            Tooltip.SetDefault("Increases movement speed by 12%");

        }

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 18;
            item.value = 16000;
            item.rare = 5;
            item.defense = 7;
        }
		public override void UpdateEquip(Player player)
        {
            player.maxRunSpeed += 0.04f;
            player.runAcceleration += 0.12f;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "Acid", 6);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
