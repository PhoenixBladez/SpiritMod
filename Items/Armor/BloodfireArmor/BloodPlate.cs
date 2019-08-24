using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.BloodfireArmor
{
    [AutoloadEquip(EquipType.Body)]
    public class BloodPlate : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bloodfire Scalemail");
            Tooltip.SetDefault("Reduces mana cost by 5%");

        }


        int timer = 0;
        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 18;
            item.value = 15000;
            item.rare = 2;
            item.defense = 6;
        }
        public override void UpdateEquip(Player player)
        {
            player.manaCost -= .05f;
        }
		public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "BloodFire", 10);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}