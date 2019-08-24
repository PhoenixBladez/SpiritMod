using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor
{
    [AutoloadEquip(EquipType.Legs)]
    public class PestilentLeggings : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pestilent Leggings");
            Tooltip.SetDefault("9% increased movement speed and 5% increased ranged critical strike chance.");

        }
        public override void SetDefaults()
        {
            item.width = 34;
            item.height = 30;
            item.value = Terraria.Item.sellPrice(0, 0, 80, 0);
            item.rare = 4;
            item.defense = 8;
        }

        public override void UpdateEquip(Player player)
        {
            player.maxRunSpeed += 0.9f;
            player.rangedCrit += 5;
        } 
        
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "PutridPiece", 10);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
