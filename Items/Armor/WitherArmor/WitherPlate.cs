using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.WitherArmor
{
    [AutoloadEquip(EquipType.Body)]
    public class WitherPlate : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wither Chestplate");
            Tooltip.SetDefault("Increases damage by 18% and movement speed by 10%\nIncreases life regeneration");
        }
        public override void SetDefaults()
        {
            item.width = 24;
            item.height = 24;
            item.value = 120000;
            item.rare = 8;
            item.defense = 23;
        }

        public override void UpdateEquip(Player player)
        {
            player.magicDamage += 0.10f;
            player.meleeDamage += 0.10f;
            player.thrownDamage += 0.10f;
            player.rangedDamage += 0.10f;
			player.minionDamage += 0.10f;
            player.maxRunSpeed += 0.1f;
			
			player.lifeRegen += 3;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "NightmareFuel", 16);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
