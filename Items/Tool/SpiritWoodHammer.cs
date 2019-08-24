using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Tool
{
    public class SpiritWoodHammer : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Duskwood Hammer");
		}


        public override void SetDefaults()
        {
            item.width = 34;
            item.height = 34;
            item.value = 0;
            item.rare = 0;

            item.hammer = 50;

            item.damage = 12;
            item.knockBack = 6;

            item.useStyle = 1;
            item.useTime = 28;
            item.useAnimation = 28;

            item.melee = true;
            item.useTurn = true;
            item.autoReuse = true;

            item.UseSound = SoundID.Item1;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "SpiritWoodItem", 8);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}