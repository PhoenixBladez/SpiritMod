using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.MagalaArmor
{
    [AutoloadEquip(EquipType.Body)]
    public class MagalaPlate : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gore Magala Plate");
            Tooltip.SetDefault("Increases maximum health by 10 and damage dealt by 9% \n ~Donator item~");
            item.value = 3000;

        }


        int timer = 0;

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 20;
            item.value = 3000;
            item.rare = 5;
            item.defense = 20;
        }
        public override void UpdateEquip(Player player)
        {
            player.minionDamage += 0.09f;
            player.rangedDamage += 0.09f;
            player.meleeDamage += 0.09f;
            player.magicDamage += 0.09f;
            player.thrownDamage += 0.09f;
            player.statLifeMax2 += 10;
        }
		 public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "MagalaScale", 14);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
