using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Armor.PrimalstoneArmor
{
    [AutoloadEquip(EquipType.Body)]
    public class PrimalstoneBreastplate : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Primalstone Breastplate");
			Tooltip.SetDefault("Increases life regeneration and melee critical strike chance by 10%\nIncreases melee damage by 5% and melee and magic critical strike chance by 7%");
		}

        public override void SetDefaults()
        {
            item.width = 34;
            item.height = 30;
            item.value = 10000;
            item.rare = 9;
            item.defense = 19;
        }

        public override void UpdateEquip(Player player)
        {
            player.lifeRegen += 2;
            player.meleeCrit += 10;
            player.meleeDamage += .05f;
            player.magicCrit += 7;
            player.meleeCrit += 7;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "ArcaneGeyser", 14);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}