using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.YoYoOverload.Items.Acc
{
	public class DCharm : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Demonite Charm");
			Tooltip.SetDefault("Increases melee critical strike chance by 5%");
		}


		public override void SetDefaults()
		{
			base.item.width = 16;
			base.item.height = 26;
			base.item.rare = 2;
			base.item.UseSound = SoundID.Item11;
			base.item.accessory = true;
			base.item.value = Item.buyPrice(0, 0, 30, 0);
			base.item.value = Item.sellPrice(0, 0, 6, 0);
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.meleeCrit += 5;
		}

		public override void AddRecipes()
		{
			ModRecipe modRecipe = new ModRecipe(base.mod);
			modRecipe.AddIngredient(57, 8);
			modRecipe.AddIngredient(85, 3);
			modRecipe.AddTile(16);
			modRecipe.SetResult(this, 1);
			modRecipe.AddRecipe();
		}
	}
}
