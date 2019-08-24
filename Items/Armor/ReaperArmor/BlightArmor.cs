using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.ReaperArmor
{
    [AutoloadEquip(EquipType.Body)]
    public class BlightArmor : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Reaper's Breastplate");
            Tooltip.SetDefault("Increases damage by 11% and critical strike chance by 6%");

        }

        public override void SetDefaults()
        {
            item.width = 34;
            item.height = 24;
            item.value = 120000;
            item.rare = 8;
            item.defense = 22;
        }

        public override void UpdateEquip(Player player)
        {
            player.magicDamage += 0.11f;
            player.meleeDamage += 0.11f;
            player.thrownDamage += 0.11f;
            player.rangedDamage += 0.11f;
            player.minionDamage += 0.11f;
			
			            player.magicCrit += 6;
						player.meleeCrit += 6;
						player.thrownCrit += 6;
						player.rangedCrit += 6;
						

        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "CursedFire", 16);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
