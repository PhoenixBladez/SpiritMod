using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.ReaperArmor
{
    [AutoloadEquip(EquipType.Legs)]
    public class BlightLegs : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Reaper's Greaves");
			Tooltip.SetDefault("Increases movement speed by 25% and increases max life by 30");
		}
        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 16;
            item.value = Terraria.Item.sellPrice(0, 2, 0, 0);
            item.rare = 8;
            item.defense = 18;
        }
        public override void UpdateEquip(Player player)
        {

            player.moveSpeed += 0.25f;
            player.maxRunSpeed += 1f;
            player.statLifeMax2 += 30;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "CursedFire", 12);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
