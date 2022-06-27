using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.CoilSet
{
	public class CoilEnergizerItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Coiled Energizer");
			Tooltip.SetDefault("Creates an electric field that energizes nearby players, greatly increasing movement speed and melee speed");
		}

		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 22;
			Item.value = Item.sellPrice(0, 0, 10, 0);
			Item.maxStack = 99;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 10;
			Item.useAnimation = 15;
			Item.rare = ItemRarityID.Green;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<CoilEnergizerTile>();
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<TechDrive>(), 10);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}