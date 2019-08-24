using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.FrigidArmor
{
    [AutoloadEquip(EquipType.Legs)]
    public class FrigidLegs : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Frigid Greaves");
            Tooltip.SetDefault("Increases melee and magic damage by 3%\nIncreases movement speed by 3 % ");

        }


        int timer = 0;

        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 24;
            item.value = 1100;
            item.rare = 1;
            item.defense = 2;
        }
        public override void UpdateEquip(Player player)
        {
            player.meleeDamage += 0.03f;
            player.magicDamage += 0.03f;
            player.maxRunSpeed += 0.03f;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "FrigidFragment", 6);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
