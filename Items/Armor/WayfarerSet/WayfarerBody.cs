using SpiritMod.Items.Material;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.WayfarerSet
{
	[AutoloadEquip(EquipType.Body)]
	public class WayfarerBody : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wayfarer's Rucksack");
			Tooltip.SetDefault("5% increased movement speed");
		}

		public override void SetDefaults()
		{
			item.width = 22;
			item.height = 20;
			item.value = Terraria.Item.sellPrice(0, 0, 40, 0);
			item.rare = ItemRarityID.Blue;
			item.defense = 3;
		}
		public override void DrawHands(ref bool drawHands, ref bool drawArms)
		{
			drawHands = true;
		}
		public override void UpdateEquip(Player player)
		{
			player.moveSpeed += 0.05f;
			player.runAcceleration += .01f;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<Items.Consumable.Quest.DurasilkSheaf>(), 1);
			recipe.AddIngredient(ItemID.IronBar, 2);
			recipe.anyIronBar = true;
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}
