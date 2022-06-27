using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Items.Sets.CoilSet;

namespace SpiritMod.Items.Accessory.UnstableTeslaCoil
{
	public class Unstable_Tesla_Coil : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Unstable Tesla Coil");
			Tooltip.SetDefault("Electrocutes up to 3 nearby enemies\nIncreases pickup range for ores");
		}

		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 30;
			Item.value = Item.sellPrice(gold: 1);
			Item.rare = ItemRarityID.Orange;
			Item.defense = 2;
			Item.accessory = true;
		}
		
		public override void UpdateAccessory(Player player, bool hideVisual) => player.GetSpiritPlayer().teslaCoil = true;

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<TechDrive>(), 14);
			recipe.AddIngredient(ItemID.Wire, 4);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}