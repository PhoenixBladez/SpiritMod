using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.BloodCourt
{
    [AutoloadEquip(EquipType.Legs)]
    public class BloodCourtLeggings : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bloodcourt's Leggings");
            Tooltip.SetDefault("Increases damage dealt by 6%\nIncreases minion knockback by 12%");

        }
        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 18;
            item.value = 4000;
            item.rare = 2;
            item.defense = 3;
        }
        public override void UpdateEquip(Player player)
        {
		   player.minionKB += 0.12f;
           player.magicDamage += 0.06f;
           player.meleeDamage += 0.06f;
           player.rangedDamage += 0.06f;
		   player.minionDamage += 0.06f;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "BloodFire", 7);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
