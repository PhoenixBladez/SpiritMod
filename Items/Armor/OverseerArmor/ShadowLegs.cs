using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.OverseerArmor
{
    [AutoloadEquip(EquipType.Legs)]
    public class ShadowLegs : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shadowspirit Treads");
			Tooltip.SetDefault("Increases invincibilty frames \n Increases critical strike chance by 22% \n Increases Max Life by 50");
		}

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 16;
            item.value = 200000;
            item.rare = 11;
            item.defense = 28;
        }
        public override void UpdateEquip(Player player)
        {

            player.longInvince = true;

            player.magicCrit += 22;
            player.meleeCrit += 22;
            player.rangedCrit += 22;
            player.thrownCrit += 22;

            player.statLifeMax2 += 50;

        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "EternityEssence", 4);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
