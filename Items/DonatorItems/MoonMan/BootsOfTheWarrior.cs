using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.DonatorItems.MoonMan
{
	[AutoloadEquip(EquipType.Legs)]
	class BootsOfTheWarrior : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Boots of the Warrior");
		}

		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 18;

			Item.value = Item.sellPrice(0, 0, 50, 0);
			Item.rare = ItemRarityID.Green;

			Item.vanity = true;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.IronBar, 6);
			recipe.AddIngredient(ItemID.SilverBar, 6);
			recipe.AddIngredient(ItemID.MeteoriteBar, 6);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
