using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.BloodCourt
{
    [AutoloadEquip(EquipType.Body)]
    public class BloodCourtChestplate : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bloodcourt's Vestments");
            Tooltip.SetDefault("Increases damage dealt by 7%");

        }
        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 20;
            item.value = 6000;
            item.rare = 2;
            item.defense = 3;
        }

        public override void UpdateEquip(Player player)
        {
            player.magicDamage += 0.07f;
            player.meleeDamage += 0.07f;
            player.rangedDamage += 0.07f;
			player.minionDamage += 0.07f;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "BloodFire", 10);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
