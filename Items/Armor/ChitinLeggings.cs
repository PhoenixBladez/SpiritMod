using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor
{
    [AutoloadEquip(EquipType.Legs)]
    public class ChitinLeggings : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chitin Leggings");
            Tooltip.SetDefault("Increases magic and summon damage by 4%\nIncreases movement speed by 5%");

        }
        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 18;
            item.value = 16000;
            item.rare = 2;
            item.defense = 3;
        }
		public override void UpdateEquip(Player player)
        {
			player.moveSpeed += 0.05f;
			player.magicDamage += 0.04f;
			player.minionDamage += 0.04f;
		}
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "Chitin", 12);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
