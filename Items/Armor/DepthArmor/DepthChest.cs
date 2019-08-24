using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.DepthArmor
{
    [AutoloadEquip(EquipType.Body)]
    public class DepthChest : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Depth Walker's Platemail");
            Tooltip.SetDefault("Increases melee and summon damage by 12%\nIncreases maximum number of minions by 1");

        }

        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 20;
            item.value = 6000;
            item.rare = 5;
            item.defense = 16;
        }

        public override void UpdateEquip(Player player)
        {
            player.minionDamage += 0.12f;
            player.meleeDamage += 0.12f;
            player.maxMinions += 1;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "DepthShard", 12);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
