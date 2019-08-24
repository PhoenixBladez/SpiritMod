using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Tool
{
    public class MythrilWarhammer : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mythril Warhammer");
		}


        public override void SetDefaults()
        {
            item.width = 38;
            item.height = 38;
            item.value = 10000;
            item.rare = 4;

            item.hammer = 83;

            item.damage = 44;
            item.knockBack = 6f;

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
            recipe.AddIngredient(ItemID.MythrilBar, 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}