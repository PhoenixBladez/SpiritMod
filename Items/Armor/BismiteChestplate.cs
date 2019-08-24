using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor
{
    [AutoloadEquip(EquipType.Body)]
    public class BismiteChestplate : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bismite Chestplate");
            Tooltip.SetDefault("Increases damage dealt by 2%");

        }
        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 20;
            item.value = 6000;
            item.rare = 1;
            item.defense = 5;
        }

        public override void UpdateEquip(Player player)
        {
            player.magicDamage += 0.02f;
            player.meleeDamage += 0.02f;
            player.thrownDamage += 0.02f;
            player.rangedDamage += 0.02f;
			player.minionDamage += 0.02f;
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
