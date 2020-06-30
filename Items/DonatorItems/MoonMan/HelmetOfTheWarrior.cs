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
			item.width = 16;
			item.height = 20;

			item.value = Item.sellPrice(0, 0, 50, 0);
			item.rare = ItemRarityID.Orange;

			item.vanity = true;
		}

		public override void AddRecipes()
		{
			var recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.IronBar, 8);
			recipe.AddIngredient(ItemID.SilverBar, 8);
			recipe.AddIngredient(ItemID.MeteoriteBar, 8);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
