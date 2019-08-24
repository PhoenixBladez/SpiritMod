using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.CosmicArmor
{
    [AutoloadEquip(EquipType.Legs)]
    public class CometLegs : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cosmic Greaves");
			Tooltip.SetDefault("Increases throwing damage by 33% and throwing velocity by 30%");
		}

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 18;
            item.value = 190000;
            item.rare = 10;
            item.defense = 15;
        }
        public override void UpdateEquip(Player player)
        {

            player.thrownVelocity += .30f;
            player.thrownDamage += 0.33f;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "AccursedRelic", 1);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
