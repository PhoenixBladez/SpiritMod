using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.CosmicArmor
{
    [AutoloadEquip(EquipType.Body)]
    public class CometArmor : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cosmic Chestplate");
            Tooltip.SetDefault("Increases movement speed by 20%, throwing velocity by 10%, and throwing critical strike chance by 15%");

        }
        public override void SetDefaults()
        {
            item.width = 24;
            item.height = 24;
            item.value = 120000;
            item.rare = 10;
            item.defense = 27;
        }

        public override void UpdateEquip(Player player)
        {
            player.thrownVelocity += .10f;
            player.moveSpeed += 0.20f;
            player.thrownCrit += 15;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "AccursedRelic", 2);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
