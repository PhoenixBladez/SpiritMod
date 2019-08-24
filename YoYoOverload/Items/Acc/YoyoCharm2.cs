using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.YoYoOverload.Items.Acc
{
	public class YoyoCharm2 : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Amazonian Charm");
			Tooltip.SetDefault("Increases melee speed by 7%");
		}


		public override void SetDefaults()
		{
			base.item.width = 16;
			base.item.height = 22;
			base.item.rare = 2;
			base.item.UseSound = SoundID.Item11;
			base.item.accessory = true;
			base.item.value = Item.buyPrice(0, 0, 30, 0);
			base.item.value = Item.sellPrice(0, 0, 6, 0);
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.meleeSpeed += 0.07f;
		}

		public override void AddRecipes()
		{
			ModRecipe modRecipe = new ModRecipe(base.mod);
			modRecipe.AddIngredient(331, 10);
			modRecipe.AddIngredient(210, 1);
			modRecipe.AddTile(16);
			modRecipe.SetResult(this, 1);
			modRecipe.AddRecipe();
		}
	}
}
