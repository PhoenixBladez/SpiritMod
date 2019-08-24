using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.DepthArmor
{
    [AutoloadEquip(EquipType.Legs)]
    public class DepthGreaves : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Depth Walker's Greaves");
            Tooltip.SetDefault("Increases minion knockback by 7% and melee speed by 10%\nIncreases maximum number of minions by 1");

        }

        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 20;
            item.value = 6000;
            item.rare = 5;
            item.defense = 10;
        }

        public override void UpdateEquip(Player player)
        {
            player.minionKB += 0.07f;
            player.meleeSpeed += 0.10f;
            player.maxMinions += 1;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "DepthShard", 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
