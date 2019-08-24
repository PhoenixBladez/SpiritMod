using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Material
{
    public class CursedFire : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ghoul Fire");
			Tooltip.SetDefault("'Cursed ghosts reside within this'");
		}


        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 36;
            item.value = Terraria.Item.sellPrice(0, 0, 20, 0);
            item.rare = 8;

            item.maxStack = 999;
        }

        public override void AddRecipes() 
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "PutridPiece", 3);
            recipe.AddIngredient(ItemID.Ectoplasm);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 5);
            recipe.AddRecipe();
        }
    }
}