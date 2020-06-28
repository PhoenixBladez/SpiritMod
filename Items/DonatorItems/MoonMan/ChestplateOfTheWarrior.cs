using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.DonatorItems.MoonMan
{
	[AutoloadEquip(EquipType.Body)]
	class ChestplateOfTheWarrior : ModItem
	{
		public static readonly int _type;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chestplate of the Warrior");
		}

		public override void SetDefaults()
		{
			item.width = 34;
			item.height = 24;

			item.value = Item.sellPrice(0, 0, 50, 0);
			item.rare = ItemRarityID.Orange;

			item.defense = 10;
		}

		public override void AddRecipes()
		{
			var recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.IronBar, 12);
			recipe.AddIngredient(ItemID.SilverBar, 12);
			recipe.AddIngredient(ItemID.MeteoriteBar, 12);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
