using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor
{
    [AutoloadEquip(EquipType.Body)]
    public class LihzahrdPlate : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lihzahrd Plate");
            Tooltip.SetDefault("Increased throwing velocity by 25% and movement speed by 15%\nIncreases throwing damage by 10%");

        }

        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 20;
            item.value = 80000;
            item.rare = 8;
            item.defense = 24;
        }

        public override void UpdateEquip(Player player)
        {
            player.moveSpeed += 0.15f;
			player.maxRunSpeed += 1f;
			player.thrownDamage += .1f;
			player.thrownVelocity += 0.25f;
        }
        public override void AddRecipes()  
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "SunShard", 14);
            recipe.AddTile(TileID.MythrilAnvil);   
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}