using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Tool
{
    public class BlightedBattleaxe : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blighted Battleaxe");
		}


        public override void SetDefaults()
        {
            item.width = 60;
            item.height = 60;
            item.value = Terraria.Item.sellPrice(0, 0, 82, 0);
            item.rare = 4;

            item.axe = 15;

            item.damage = 34;
            item.knockBack = 5;

            item.useStyle = 1;
            item.useTime = 8;
            item.useAnimation = 29;

            item.melee = true;
            item.useTurn = true;
            item.autoReuse = true;

            item.UseSound = SoundID.Item1;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "PutridPiece", 5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}