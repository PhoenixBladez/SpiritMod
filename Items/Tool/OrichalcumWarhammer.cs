using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Tool
{
    public class OrichalcumWarhammer : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Orichalcum Warhammer");
		}


        public override void SetDefaults()
        {
            item.width = 42;
            item.height = 46;
            item.value = 10000;
            item.rare = 4;

            item.hammer = 85;

            item.damage = 47;
            item.knockBack = 7;

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
            recipe.AddIngredient(ItemID.OrichalcumBar, 12);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}