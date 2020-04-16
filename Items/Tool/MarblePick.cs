using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Tool
{
	public class MarblePick : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gilded Pickaxe");
		}


		public override void SetDefaults()
		{
			item.width = 36;
			item.height = 38;
			item.value = Item.sellPrice(0, 1, 0, 0);
			item.rare = 2;

            item.pick = 70;

            item.damage = 16;
			item.knockBack = 5f;

			item.useStyle = 1;
			item.useTime = 19;
			item.useAnimation = 19;

			item.melee = true;
			item.autoReuse = true;

			item.UseSound = SoundID.Item1;
		}

		public override void AddRecipes()
		{
			ModRecipe modRecipe = new ModRecipe(mod);
			modRecipe.AddIngredient(null, "MarbleChunk", 16);
            modRecipe.AddTile(TileID.Anvils);
            modRecipe.SetResult(this);
			modRecipe.AddRecipe();
		}
	}
}
