using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.SnowArmor
{
    [AutoloadEquip(EquipType.Legs)]
    public class SnowRangerLegs : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hunter's Footwraps");
            Tooltip.SetDefault("Increases ranged damage by 3% and ranged critical strike chance by 2%");

        }


        int timer = 0;
        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 20;
            item.value = 3000;
            item.rare = 2;
            item.defense = 4;
        }
         public override void UpdateEquip(Player player)
        {
            player.rangedDamage += 0.03f;
            player.rangedCrit += 2;
        }
		
		 public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "SturdyFur", 4);
            recipe.AddIngredient(null, "FrigidFragment", 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
