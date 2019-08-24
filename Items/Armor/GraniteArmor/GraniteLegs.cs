using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.GraniteArmor
{
    [AutoloadEquip(EquipType.Legs)]
    public class GraniteLegs : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Granite Greaves");
            Tooltip.SetDefault("Reduces damage taken by 3%");

        }


        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 24;
            item.value = 1100;
            item.rare = 2;
            item.defense = 7;
        }
        public override void UpdateEquip(Player player)
        {
            player.endurance += 0.03f;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "GraniteChunk", 10);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
