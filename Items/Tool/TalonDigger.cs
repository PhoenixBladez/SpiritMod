using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Tool
{
    public class TalonDigger : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Talon Digger");
		}


        public override void SetDefaults()
        {
            item.width = 36;
            item.height = 36;
            item.value = 1000;
            item.rare = 2;
            item.pick = 75;
            item.damage = 19;
            item.knockBack = 3;
            item.useStyle = 1;
            item.useTime = 18;
            item.useAnimation = 18;
            item.melee = true;
            item.useTurn = true;
            item.autoReuse = true;
            item.UseSound = SoundID.Item1;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null,"Talon", 14);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}