using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.BismiteArmor
{
    [AutoloadEquip(EquipType.Body)]
    public class BismiteChestplate : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bismite Chestplate");
            Tooltip.SetDefault("Increases damage dealt and critical strike chance by 4%");

        }
        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 20;
            item.value = 6000;
            item.rare = 1;
            item.defense = 3;
        }

        public override void UpdateEquip(Player player)
        {
            player.magicDamage += 0.04f;
            player.meleeDamage += 0.04f;
            player.rangedDamage += 0.04f;
			player.minionDamage += 0.04f;
            player.magicCrit += 4;
            player.meleeCrit += 4;
            player.rangedCrit += 4;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "BismiteCrystal", 10);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
