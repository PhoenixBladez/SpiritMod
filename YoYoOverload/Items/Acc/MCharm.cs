using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.YoYoOverload.Items.Acc
{
	public class MCharm : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Necro Charm");
			Tooltip.SetDefault("Reduces damage taken by 5%");
		}


		public override void SetDefaults()
		{
			base.item.width = 18;
			base.item.height = 24;
			base.item.rare = 1;
			base.item.UseSound = SoundID.Item11;
			base.item.accessory = true;
			base.item.value = Item.buyPrice(0, 0, 30, 0);
			base.item.value = Item.sellPrice(0, 0, 6, 0);
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.endurance += 0.05f;
		}

		public override void AddRecipes()
		{
			ModRecipe modRecipe = new ModRecipe(base.mod);
			modRecipe.AddIngredient(154, 20);
			modRecipe.AddIngredient(85, 3);
			modRecipe.AddTile(16);
			modRecipe.SetResult(this, 1);
			modRecipe.AddRecipe();
		}
	}
}
