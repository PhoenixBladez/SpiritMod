using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Tool
{
    public class BismitePickaxe : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bismite Pickaxe");
		}


        public override void SetDefaults()
        {
            item.width = 38;
            item.height = 30;
            item.value = 1000;
            item.rare = 1;
            item.pick = 40;
            item.damage = 4;
            item.knockBack = 4;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.useTime = 16;
            item.useAnimation = 20;
            item.melee = true;
            item.useTurn = true;
            item.autoReuse = true;
            item.UseSound = SoundID.Item1;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null,"BismiteCrystal", 14);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
