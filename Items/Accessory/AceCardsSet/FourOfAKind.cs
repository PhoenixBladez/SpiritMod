using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory.AceCardsSet
{
	public class FourOfAKind : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Four of a Kind");
			Tooltip.SetDefault("Critical hits deal more damage\nEnemies killed by a critical hit always drop a heart and more money\nCritical kills drop Diamond Aces, which empower damage");
		}

		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.value = Item.buyPrice(0, 5, 0, 0);
			Item.rare = ItemRarityID.Orange;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetSpiritPlayer().AceOfClubs = true;
			player.GetSpiritPlayer().AceOfHearts = true;
			player.GetSpiritPlayer().AceOfSpades = true;
			player.GetSpiritPlayer().AceOfDiamonds = true;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<AceOfClubs>());
            recipe.AddIngredient(ModContent.ItemType<AceOfHearts>());
            recipe.AddIngredient(ModContent.ItemType<AceOfSpades>());
			recipe.AddIngredient(ModContent.ItemType<AceOfDiamonds>());
            recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.Register();
		}
	}
}
