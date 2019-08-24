using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Tool
{
    public class TalonAxe : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Talon Axe");
		}


        public override void SetDefaults()
        {
            item.width = 46;
            item.height = 46;
            item.value = 10000;
            item.rare = 2;
            item.axe = 24;
            item.damage = 12;
            item.knockBack = 6;
            item.useStyle = 1;
            item.useTime = 22;
            item.useAnimation = 22;
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