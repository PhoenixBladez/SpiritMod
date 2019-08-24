using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Tool
{
    public class CoralPickaxe : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tidal Pickaxe");
			Tooltip.SetDefault("Can mine Cobalt and Palladium");
		}


        public override void SetDefaults()
        {
            item.width = 36;
            item.height = 36;
            item.value = Terraria.Item.sellPrice(0, 3, 0, 0);
            item.rare = 3;
            item.pick = 100;
            item.damage = 22;
            item.knockBack = 2;
            item.useStyle = 1;
            item.useTime = 14;
            item.useAnimation = 20;
            item.melee = true;
            item.useTurn = true;
            item.autoReuse = true;
            item.UseSound = SoundID.Item1;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Coral, 3);
            recipe.AddIngredient(null, "PearlFragment", 15);
            recipe.AddIngredient(ItemID.BottledWater, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
