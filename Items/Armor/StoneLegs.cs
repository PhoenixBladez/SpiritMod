using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor
{
    [AutoloadEquip(EquipType.Legs)]
    public class StoneLegs : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stone Greaves");
			Tooltip.SetDefault("Increases melee damage by 7%");
		}

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 18;
            item.value = 400;
            item.rare = 1;
            item.defense = 2;
        }

        public override void UpdateEquip(Player player)
        {
            player.meleeDamage += 0.07f;
        }

        public override void AddRecipes()  //How to craft this item
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.StoneBlock, 40);
            recipe.AddTile(TileID.Anvils);   //at work bench
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}