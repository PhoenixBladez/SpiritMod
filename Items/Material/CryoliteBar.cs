using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Material
{
    public class CryoliteBar : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cryolite Bar");
		}


        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 24;
            item.value = 100;
			item.useStyle = 1;
            item.rare = 4;
			  item.consumable = true;
            item.rare = 3;
            item.maxStack = 999;
			item.createTile = mod.TileType("CryoliteBar");
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
        }
        public override void AddRecipes() 
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "CryoliteOre", 3);
            recipe.AddTile(TileID.Furnaces);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}