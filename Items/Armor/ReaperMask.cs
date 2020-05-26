using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class ReaperMask : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Reaper's Mask");
        }

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 20;
            item.value = 3000;
            item.rare = 5;
            item.vanity = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "BloodFire", 5);
            recipe.AddIngredient(ItemID.SoulofNight, 1);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
