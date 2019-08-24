using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor
{
    [AutoloadEquip(EquipType.Body)]
    public class PestilentPlatemail : ModItem
    {
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pestilent Platemail");
            Tooltip.SetDefault("Increases ranged damage by 5%, ranged critical strike chance by 6%, and 25% chance to not consume ammo");

        }
        public override void SetDefaults()
        {
            item.width = 34;
            item.height = 30;
            item.value = Terraria.Item.sellPrice(0, 0, 80, 0);
            item.rare = 4;
            item.defense = 9;
        }

        public override void UpdateEquip(Player player)
        {
            player.rangedDamage += 0.05f;
            player.rangedCrit += 6;
            player.ammoCost75 = true;
        }
        
        		        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "PutridPiece", 15);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
