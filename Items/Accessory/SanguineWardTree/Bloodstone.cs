
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory.SanguineWardTree
{
	public class Bloodstone : ModItem
	{
		public override bool IsLoadingEnabled(Mod mod) => false;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bloodstone");
			Tooltip.SetDefault("A bloody ward surrounds you, inflicting Blood Corruption to nearby enemies\nKilling enemies within the aura restores some life\nHearts are more likely to drop from enemies");
		}

		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.value = Item.buyPrice(0, 1, 0, 0);
			Item.rare = ItemRarityID.Orange;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetSpiritPlayer().vitaStone = true;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<BloodWard>(), 1);
			recipe.AddIngredient(ModContent.ItemType<VitalityStone>(), 1);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.Register();
		}
	}
}
