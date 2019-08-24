using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.FieryArmor
{
    [AutoloadEquip(EquipType.Body)]
    public class ObsidiusPlate : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fiery Chestplate");
            Tooltip.SetDefault("Increases throwing velocity by 9%");

        }

        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 20;
            item.value = Terraria.Item.sellPrice(0, 0, 35, 0);
            item.rare = 3;
            item.defense = 8;
        }

        public override void UpdateEquip(Player player)
        {
            player.thrownVelocity += 0.09f;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "CarvedRock", 17);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
