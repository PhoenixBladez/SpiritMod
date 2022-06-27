using SpiritMod.Items.Sets.TideDrops;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.TideDrops.StreamSurfer
{
	[AutoloadEquip(EquipType.Head)]
	public class StreamSurferHelmet : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stream Surfer Mask");
			Tooltip.SetDefault("8% increased magic critical strike chance\nYou can breathe underwater");
		}

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 20;
			Item.value = Terraria.Item.sellPrice(0, 0, 60, 0);
			Item.rare = ItemRarityID.Orange;
			Item.defense = 4;
		}

		public override void UpdateEquip(Player player)
		{
			player.magicCrit += 8;
			player.gills = true;
		}

		public override void UpdateArmorSet(Player player)
		{
			string tapDir = Language.GetTextValue(Main.ReversedUpDownArmorSetBonuses ? "Key.UP" : "Key.DOWN");
			player.GetSpiritPlayer().surferSet = true;
			player.setBonus = $"Double tap {tapDir} to create a ridable waterspout that damages enemies\n7 second cooldown";
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
			=> body.type == ModContent.ItemType<StreamSurferChestplate>()
			&& legs.type == ModContent.ItemType<StreamSurferLeggings>();

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(1);
			recipe.AddIngredient(ModContent.ItemType<TribalScale>(), 6);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
