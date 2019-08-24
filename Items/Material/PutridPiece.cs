using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Material
{
    public class PutridPiece : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Putrid Piece");
			Tooltip.SetDefault("'A shard of cursed power'");
		}


        public override void SetDefaults()
        {
            item.width = 38;
            item.height = 42;
            item.value = Terraria.Item.sellPrice(0, 0, 2, 0);
            item.rare = 4;

            item.maxStack = 999;
        }

		public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.SoulofNight, 2);
            recipe.AddIngredient(ItemID.CursedFlame);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
} 
