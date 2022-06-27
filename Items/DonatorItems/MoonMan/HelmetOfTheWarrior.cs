using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.DonatorItems.MoonMan
{
	[AutoloadEquip(EquipType.Head)]
	class HelmetOfTheWarrior : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Helmet of the Warrior");
		}

		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 20;

			Item.value = Item.sellPrice(0, 0, 50, 0);
			Item.rare = ItemRarityID.Green;

			Item.vanity = true;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.IronBar, 8);
			recipe.AddIngredient(ItemID.SilverBar, 8);
			recipe.AddIngredient(ItemID.MeteoriteBar, 8);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
