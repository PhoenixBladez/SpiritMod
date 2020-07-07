using SpiritMod.Items.Material;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.StreamSurfer
{
	[AutoloadEquip(EquipType.Head)]
	public class StreamSurferHelmet : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stream Surfer Mask");
			Tooltip.SetDefault("5% increased critical strike chance and damage \nYou can breathe underwater");

		}

		public override void SetDefaults()
		{
			item.width = 22;
			item.height = 20;
			item.value = Item.buyPrice(silver: 30);
			item.rare = ItemRarityID.Blue;
			item.defense = 4;
		}
		public override void UpdateEquip(Player player)
		{
			player.magicCrit += 5;
			player.rangedCrit += 5;
			player.meleeCrit += 5;
			player.allDamage += 0.05f;
			player.gills = true;
		}
		public override void UpdateArmorSet(Player player)
        {
            var tapDir = Main.ReversedUpDownArmorSetBonuses ? "DOWN" : "UP";
            player.GetSpiritPlayer().surferSet = true;
			player.setBonus = $"Double tap {tapDir} to create a ridable waterspout that damages enemies\n7 second cooldown";
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<StreamSurferChestplate>() && legs.type == ModContent.ItemType<StreamSurferLeggings>();
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<TribalScale>(), 6);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}
