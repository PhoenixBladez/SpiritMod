using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor
{
    [AutoloadEquip(EquipType.Body)]
    public class StellarPlate : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stellar Plate");
            Tooltip.SetDefault("8% increased ranged damage and critical strike chace/nReduces ammo consumption by 25%");

        }
        public override void SetDefaults()
        {
            item.width = 34;
            item.height = 30;
            item.value = Item.sellPrice(0, 1, 0, 0);
            item.rare = 5;
            item.defense = 14;
        }

        public override void UpdateEquip(Player player)
        {
            player.rangedDamage += 0.08f;
            player.rangedCrit += 08;
			player.ammoCost75 = true;
        }
        
		public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "StellarBar", 16);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
