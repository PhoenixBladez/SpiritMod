using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.ReachBoss
{
    [AutoloadEquip(EquipType.Legs)]
    public class ReachBossLegs : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Thornspeaker's Greaves");
            Tooltip.SetDefault("Increases minion damage by 4%");

        }


        int timer = 0;
        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 20;
            item.value = 30200;
            item.rare = 2;
            item.defense = 3;
        }
        public override void UpdateEquip(Player player)
        {
            player.minionDamage += 0.04f;
        }
		 public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "ReachFlowers", 6);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
