using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Tool
{
    public class CobaltWarhammer : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cobalt Warhammer");
		}


        public override void SetDefaults()
        {
            item.width = 44;
            item.height = 40;
            item.value = 10000;
            item.rare = 4;

            item.hammer = 80;

            item.damage = 38;
            item.knockBack = 5.5f;

            item.useStyle = 1;
            item.useTime = 30;
            item.useAnimation = 30;

            item.melee = true;
            item.useTurn = true;
            item.autoReuse = true;

            item.UseSound = SoundID.Item1;
        }

        public override void AddRecipes()  
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.CobaltBar, 10);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}