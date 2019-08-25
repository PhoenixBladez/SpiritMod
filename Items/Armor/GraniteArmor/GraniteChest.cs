using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.GraniteArmor
{
    [AutoloadEquip(EquipType.Body)]
    public class GraniteChest : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Granite Breastplate");
            Tooltip.SetDefault("Reduces movement speed by 15%");

        }

        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 24;
            item.value = 1100;
            item.rare = 2;
            item.defense = 12;
        }
        public override void UpdateEquip(Player player)
        {
            player.maxRunSpeed -= 0.15f;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "GraniteChunk", 12);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
