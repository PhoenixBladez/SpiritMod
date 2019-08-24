using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor
{
    [AutoloadEquip(EquipType.Legs)]
    public class LihzahrdLegs : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lihzahrd Leggings");
			Tooltip.SetDefault("Increased throwing damage by 25%");
		}

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 18;
            item.value = 100000;
            item.rare = 8;
            item.defense = 15;
        }

        public override void UpdateEquip(Player player)
        {
			player.thrownDamage += 0.25f;
        }

        public override void AddRecipes()  //How to craft this item
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "SunShard", 12);
            recipe.AddTile(TileID.MythrilAnvil);   //at work bench
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}