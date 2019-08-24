using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Material
{
    public class MagalaScale : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gore Magala Scale");
			Tooltip.SetDefault("Maybe you'll get a plate next time.");
		}


        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 24;
            item.value = 2100;
            item.rare = 5;


            item.maxStack = 999;
        }
        
        public override void AddRecipes() 
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.SoulofNight, 2);
            recipe.AddIngredient(ItemID.HallowedBar);
            recipe.AddIngredient(ItemID.DefenderMedal);
            recipe.AddTile(TileID.AdamantiteForge);
            recipe.SetResult(this, 6);
            recipe.AddRecipe();
        }
    }
}