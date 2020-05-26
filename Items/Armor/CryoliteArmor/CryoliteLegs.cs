using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.CryoliteArmor
{
    [AutoloadEquip(EquipType.Legs)]
    public class CryoliteLegs : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cryolite Greaves");
			Tooltip.SetDefault("Increases melee speed by 9%");
		}

        public override void SetDefaults()
        {
            item.width = 38;
            item.height = 26;
            item.value = 10000;
            item.rare = 3;
            item.defense = 7;
        }

        public override void UpdateEquip(Player player)
        {
            player.meleeSpeed += .09f;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "CryoliteBar", 11);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
