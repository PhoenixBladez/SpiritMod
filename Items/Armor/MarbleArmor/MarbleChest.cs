using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.MarbleArmor
{
    [AutoloadEquip(EquipType.Body)]
    public class MarbleChest : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Marble Guard");
            Tooltip.SetDefault("Increases critical strike chance by 4%\nIncreases movement speed by 5%");

        }
        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 24;
            item.value = 12100;
            item.rare = 2;
            item.defense = 4;
        }
        public override void UpdateEquip(Player player)
        {
            player.meleeCrit += 4;
            player.rangedCrit += 4;
            player.magicCrit += 4;
            player.thrownCrit += 4;
            player.maxRunSpeed += 0.05f;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "MarbleChunk", 12);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
