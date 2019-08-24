using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.MagalaArmor
{
    [AutoloadEquip(EquipType.Legs)]
    public class MagalaLegs : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gore Magala Greaves");
            Tooltip.SetDefault("Increases maximum health by 10, maximum number of minions by 1, and movement speed by 13% \n ~Donator item~");

        }


        int timer = 0;
        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 20;
            item.value = 3000;
            item.rare = 5;
            item.defense = 15;
        }
        public override void UpdateEquip(Player player)
        {
            player.moveSpeed += 0.13f;
            player.statLifeMax2 += 10;
            player.maxMinions += 1;
        }
		 public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "MagalaScale", 10);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
