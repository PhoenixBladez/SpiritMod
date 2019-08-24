using System;
using System.Collections.Generic;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
	[AutoloadEquip(EquipType.Shield)]
	public class SonaDetermination : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sona's Determination");
			Tooltip.SetDefault("\n'I will never back down'\nIncreases maximum life by 30\nReduces damage taken by 10%\nGrants immunity to knockback\nEnemies are more likely to target you\n~Donator Item~");
		}

		public override void SetDefaults()
		{
			item.width = 16;
			item.height = 16;
			item.rare = 5;
			item.value = 150000;
			item.defense = 5;
			item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.aggro += 1000;
			player.statLifeMax2 += 30;
			player.endurance += .10f;
			player.noKnockback = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.ObsidianShield);
			recipe.AddIngredient(ItemID.WarriorEmblem);
			recipe.AddIngredient(ItemID.LifeCrystal, 15);
			recipe.AddIngredient(mod.ItemType("SpiritBar"), 5);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
