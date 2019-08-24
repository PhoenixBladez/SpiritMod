using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.YoYoOverload.Items.Acc
{
	public class HCharm : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hellstone Charm");
			Tooltip.SetDefault("Increases melee damage by 6%");
		}


		public override void SetDefaults()
		{
			base.item.width = 14;
			base.item.height = 24;
			base.item.rare = 2;
			base.item.UseSound = SoundID.Item11;
			base.item.accessory = true;
			base.item.value = Item.buyPrice(0, 1, 0, 0);
			base.item.value = Item.sellPrice(0, 0, 50, 0);
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.meleeDamage += 0.06f;
		}

		public override void AddRecipes()
		{
			ModRecipe modRecipe = new ModRecipe(base.mod);
			modRecipe.AddIngredient(175, 8);
			modRecipe.AddIngredient(85, 3);
			modRecipe.AddTile(16);
			modRecipe.SetResult(this, 1);
			modRecipe.AddRecipe();
		}
	}
}
