using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Placeable.IceSculpture
{
	public class WinterbornSculpture : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Winterborn Sculpture");
        }


		public override void SetDefaults()
		{
            item.width = 30;
			item.height = 40;
            item.value = Item.sellPrice(0, 0, 15, 0); 

            item.maxStack = 99;

            item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = 10;
            item.useAnimation = 15;

            item.useTurn = true;
            item.autoReuse = true;
            item.consumable = true;

			item.createTile = mod.TileType("WinterbornDecor");
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null,"CreepingIce", 20);
            recipe.AddIngredient(null, "CryoliteBar", 2);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
			recipe.AddRecipe();            
        }
	}
}