using System;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
	public class TyrinaFury : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tyrina's Fury");
			Tooltip.SetDefault("Increases melee knockback\nIncreases melee damage by 18% and melee damage by 25% when under half health");
		}

		public override void SetDefaults()
		{
			item.width = 16;
			item.height = 16;
			item.rare = 5;
			item.value = 150000;
			item.defense = 3;
			item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			if (player.statLife <= player.statLifeMax2 / 2)
			{
				player.meleeDamage += 0.18f;
				player.meleeSpeed += .25f;
			}
			player.kbGlove = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.PowerGlove);
			recipe.AddIngredient(ItemID.WarriorEmblem);
			recipe.AddIngredient(ItemID.SoulofMight, 15);
			recipe.AddIngredient(mod.ItemType("SpiritBar"), 5);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
