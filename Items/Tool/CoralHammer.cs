using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Tool
{
    public class CoralHammer : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tidal Hammer");
		}


        public override void SetDefaults()
        {
            item.width = 34;
            item.height = 34;
            item.value = Terraria.Item.sellPrice(0, 0, 30, 0);
            item.rare = 3;
            item.hammer = 70;
            item.damage = 23;
            item.knockBack = 5.5f;
            item.useStyle = 1;
            item.useTime = 21;
            item.useAnimation = 30;
            item.melee = true;
            item.useTurn = true;
            item.autoReuse = true;
            item.UseSound = SoundID.Item1;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Coral, 3);
            recipe.AddIngredient(null, "PearlFragment", 12);
            recipe.AddIngredient(ItemID.BottledWater, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
