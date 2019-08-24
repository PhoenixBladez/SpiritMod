using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Placeable.Furniture
{
    public class CreationAltar : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Altar of Creation");
			Tooltip.SetDefault("'Where ancient energies converge'");
		}


        public override void SetDefaults()
        {
            item.width = 38;
            item.height = 58;
            item.maxStack = 1;
            item.rare = 6;

            item.useStyle = 1;
            item.useTime = item.useAnimation = 25;

            item.autoReuse = true;
            item.consumable = true;


            item.createTile = mod.TileType("CreationAltarTile");
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "OldLeather", 3);
            recipe.AddIngredient(null, "PrimordialMagic", 5);
            recipe.AddIngredient(ItemID.IronBar, 6);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();

            ModRecipe recipe1 = new ModRecipe(mod);
            recipe1.AddIngredient(null, "OldLeather", 3);
            recipe1.AddIngredient(null, "PrimordialMagic", 5);
            recipe1.AddIngredient(ItemID.LeadBar, 6);
            recipe1.AddTile(TileID.Anvils);
            recipe1.SetResult(this);
            recipe1.AddRecipe();
        }
    }
}
