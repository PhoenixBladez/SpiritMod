using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace SpiritMod.Items.Material
{
    public class OddKeystone : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Odd Keystone");
			Tooltip.SetDefault("'It holds untold power'");
            ItemID.Sets.ItemIconPulse[item.type] = true;
        }


        public override void SetDefaults()
        {
            item.width = 26;
            item.height = 26;
            item.value = 100;
            item.rare = 1;
            item.maxStack = 1;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "KeystoneShard", 3);
            recipe.AddTile(null, "CreationAltarTile");
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}