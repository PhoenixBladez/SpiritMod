
using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
	[AutoloadEquip(EquipType.Balloon)]
	public class LumothLantern : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Luminous Lantern");
			Tooltip.SetDefault("Provides bright light\nWorks in the vanity slot, but less effectively\n'Adventure into the deepest caverns with a trusty lightsource!'");

		}

		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 38;
			Item.value = Item.buyPrice(0, 1, 0, 0);
			Item.rare = ItemRarityID.Green;
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			{
				Lighting.AddLight(player.position, 1.25f, 1.2f, 1.2f);
			}
		}
		public override void EquipFrameEffects(Player player, EquipType type)
		{
			Lighting.AddLight(player.position, .65f, .65f, .65f);
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(1);
			recipe.AddIngredient(ModContent.ItemType<Brightbulb>(), 1);
			recipe.AddIngredient(ItemID.SilverBar, 5);
			recipe.AddIngredient(ItemID.Wood, 20);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();

			Recipe recipe1 = CreateRecipe(1);
			recipe1.AddIngredient(ModContent.ItemType<Brightbulb>(), 1);
			recipe1.AddIngredient(ItemID.TungstenBar, 5);
			recipe1.AddIngredient(ItemID.Wood, 20);
			recipe1.AddTile(TileID.WorkBenches);
			recipe1.Register();
		}
	}
}
