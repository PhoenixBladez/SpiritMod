using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Tool
{
    public class BismiteHamaxe : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bismite Hamaxe");
		}


        public override void SetDefaults()
        {
            item.width = 38;
            item.height = 30;
            item.value = 1000;
            item.rare = 2;
            item.hammer = 40;
			item.axe = 6;
            item.damage = 6;
            item.knockBack = 4;
            item.useStyle = 1;
            item.useTime = 18;
            item.useAnimation = 23;
            item.melee = true;
            item.useTurn = true;
            item.autoReuse = true;
            item.UseSound = SoundID.Item1;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null,"BismiteCrystal", 12);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
