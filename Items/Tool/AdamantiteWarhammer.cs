using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Tool
{
    public class AdamantiteWarhammer : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Adamantite Warhammer");
		}


        public override void SetDefaults()
        {
            item.width = 48;
            item.height = 48;
            item.value = 10000;
            item.rare = 4;

            item.hammer = 85;

            item.damage = 49;
            item.knockBack = 6;

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
            recipe.AddIngredient(ItemID.AdamantiteBar, 12);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}