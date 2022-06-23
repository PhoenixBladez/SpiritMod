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
			item.width = 24;
			item.height = 30;
			item.value = Item.sellPrice(gold: 1);
			item.rare = ItemRarityID.Orange;
			item.defense = 2;
			item.accessory = true;
		}
		
		public override void UpdateAccessory(Player player, bool hideVisual) => player.GetSpiritPlayer().teslaCoil = true;

		public override void AddRecipes()
		{
			var recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<TechDrive>(), 14);
			recipe.AddIngredient(ItemID.Wire, 4);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}