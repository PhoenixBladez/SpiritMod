using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor
{
    [AutoloadEquip(EquipType.Body)]
    public class CoiledChestplate : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Coiled Chestplate");
            Tooltip.SetDefault("Increases throwing critical strike chance by 5%\nIncreases throwing damage by 7%");

        }
        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 20;
            item.value = Terraria.Item.sellPrice(0, 0, 18, 0);
            item.rare = 2;
            item.defense = 6;
        }

        public override void UpdateEquip(Player player)
        {
            player.thrownCrit += 5;
			player.thrownDamage += 0.07f;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "TechDrive", 16);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
