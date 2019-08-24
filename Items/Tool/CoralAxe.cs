using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Tool
{
    public class CoralAxe : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tidal Axe");
		}


        public override void SetDefaults()
        {
            item.width = 38;
            item.height = 30;
            item.value = Terraria.Item.sellPrice(0, 0, 32, 0);
            item.rare = 3;
            item.axe = 15;
            item.damage = 16;
            item.knockBack = 4;
            item.useStyle = 1;
            item.useTime = 20;
            item.useAnimation = 25;
            item.melee = true;
            item.useTurn = true;
            item.autoReuse = true;
            item.UseSound = SoundID.Item1;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Coral, 3);
            recipe.AddIngredient(null, "PearlFragment", 8);
            recipe.AddIngredient(ItemID.BottledWater, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
