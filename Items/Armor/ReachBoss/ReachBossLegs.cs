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
			DisplayName.SetDefault("Vinecaller's Greaves");
            Tooltip.SetDefault("Increases throwing velocity by 10%");

        }


        int timer = 0;
        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 20;
            item.value = 30200;
            item.rare = 2;
            item.defense = 4;
        }
        public override void UpdateEquip(Player player)
        {
            player.thrownVelocity += 0.1f;
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
