using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.BotanistSet
{
	[AutoloadEquip(EquipType.Legs)]
	public class BotanistLegs : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Botanist Leggings");
			Tooltip.SetDefault("10% increased move speed");
		}

		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 20;
			Item.value = Item.sellPrice(0, 0, 10, 0);
			Item.rare = ItemRarityID.White;
			Item.defense = 1;
		}

		public override void UpdateEquip(Player player) => player.moveSpeed += 0.1f;

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Sunflower, 1);
			recipe.AddIngredient(ItemID.FallenStar, 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}
}
