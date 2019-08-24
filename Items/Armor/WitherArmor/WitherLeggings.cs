using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.WitherArmor
{
    [AutoloadEquip(EquipType.Legs)]
    public class WitherLeggings : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wither Greaves");
			Tooltip.SetDefault("Increases movement speed by 10%\n5% increased damage and critical strike chance");
		}
        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 18;
            item.value = 90000;
            item.rare = 8;
            item.defense = 15;
        }
        public override void UpdateEquip(Player player)
        {

			            player.magicDamage += 0.05f;
            player.meleeDamage += 0.05f;
            player.thrownDamage += 0.05f;
            player.rangedDamage += 0.05f;
			player.minionDamage += 0.05f;
            player.maxRunSpeed += 0.05f;
			
            player.magicCrit += 5;
            player.meleeCrit += 5;
            player.rangedCrit += 5;
            player.thrownCrit += 5;
			
			player.moveSpeed += 0.1f;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "NightmareFuel", 12);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
