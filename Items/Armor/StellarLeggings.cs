using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor
{

    [AutoloadEquip(EquipType.Legs)]
    public class StellarLeggings : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stellar Leggings");
            Tooltip.SetDefault("12% increased movement speed and 8% increased ranged damage");

        }

        public override void SetDefaults()
        {
            item.width = 34;
            item.height = 30;
            item.value = Item.sellPrice(0, 1, 0, 0);
            item.rare = 5;
            item.defense = 10;
        }

        public override void UpdateEquip(Player player)
        {
            player.moveSpeed += 0.12f;
			player.rangedDamage += 0.08f;
        } 
        
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "StellarBar", 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
