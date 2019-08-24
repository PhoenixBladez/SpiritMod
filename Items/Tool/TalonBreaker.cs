using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Tool
{
    public class TalonBreaker : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Talon Breaker");
		}


        public override void SetDefaults()
        {
            item.width = 38;
            item.height = 38;
            item.value = 10000;
            item.rare = 2;
            item.hammer = 65;
            item.damage = 18;
            item.knockBack = 6;
            item.useStyle = 1;
            item.useTime = 29;
            item.useAnimation = 16;
            item.melee = true;
            item.useTurn = true;
            item.autoReuse = true;
            item.UseSound = SoundID.Item1;
        }
         public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null,"Talon", 10);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}