using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.DonatorItems.MoonMan
{
	[AutoloadEquip(EquipType.Body)]
	class ChestplateOfTheWarrior : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chestplate of the Warrior");
		}

		public override void SetDefaults()
		{
			Item.width = 34;
			Item.height = 24;

			Item.value = Item.sellPrice(0, 0, 50, 0);
			Item.rare = ItemRarityID.Green;

			Item.vanity = true;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.IronBar, 12);
			recipe.AddIngredient(ItemID.SilverBar, 12);
			recipe.AddIngredient(ItemID.MeteoriteBar, 12);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
