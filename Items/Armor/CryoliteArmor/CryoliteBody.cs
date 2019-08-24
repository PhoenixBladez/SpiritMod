using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.CryoliteArmor
{
    [AutoloadEquip(EquipType.Body)]
    public class CryoliteBody : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cryolite Chestplate");
			Tooltip.SetDefault("Increases ranged damage by 8% and ranged critical strike chance by 4%");
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
            player.rangedDamage += 0.08f;
            player.rangedCrit += 4;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "CryoliteBar", 15);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
