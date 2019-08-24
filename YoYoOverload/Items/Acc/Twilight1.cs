using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.YoYoOverload.Items.Acc
{
	public class Twilight1 : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Twilight Talisman");
			Tooltip.SetDefault("'A must have for any Yoyoer'\nIncreases melee speed by 7%, melee damage by 6%, melee critical strike chance by 5%\nReduces damage taken by 6%");
		}


		public override void SetDefaults()
		{
			base.item.width = 14;
			base.item.height = 24;
			base.item.rare = 3;
			base.item.UseSound = SoundID.Item11;
			base.item.accessory = true;
			base.item.value = Item.buyPrice(0, 2, 30, 0);
			base.item.value = Item.sellPrice(0, 1, 6, 0);
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.meleeDamage += 0.06f;
			player.meleeCrit += 5;
			player.endurance += 0.05f;
			player.meleeSpeed += 0.07f;
		}

		public override void AddRecipes()
		{
			ModRecipe modRecipe = new ModRecipe(base.mod);
			modRecipe.AddIngredient(null, "YoyoCharm2", 1);
			modRecipe.AddIngredient(null, "MCharm", 1);
			modRecipe.AddIngredient(null, "DCharm", 1);
			modRecipe.AddIngredient(null, "HCharm", 1);
			modRecipe.AddTile(26);
			modRecipe.SetResult(this, 1);
			modRecipe.AddRecipe();
		}
	}
}
