using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Tool
{
    public class BloodfireAxe : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bloodfire Axe");
		}


        public override void SetDefaults()
        {
            item.width = 38;
            item.height = 30;
            item.value = Terraria.Item.sellPrice(0, 0, 22, 0);
            item.rare = 2;
            item.axe = 10;
            item.damage = 10;
            item.knockBack = 4;
            item.useStyle = 1;
            item.useTime = 15;
            item.useAnimation = 20;
            item.melee = true;
            item.useTurn = true;
            item.autoReuse = true;
            item.UseSound = SoundID.Item1;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null,"BloodFire", 12);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
