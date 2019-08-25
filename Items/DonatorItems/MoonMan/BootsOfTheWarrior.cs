using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace SpiritMod.Items.DonatorItems.MoonMan
{
	[AutoloadEquip(EquipType.Legs)]
	class BootsOfTheWarrior : ModItem
	{
		public static readonly int _type;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Boots of the Warrior");
			Tooltip.SetDefault("~Donator Item~");
		}

		public override void SetDefaults()
		{
			item.width = 22;
			item.height = 18;

			item.value = Item.sellPrice(0, 0, 50, 0);
			item.rare = 2;

			item.defense = 6;
		}

		public override void AddRecipes()
		{
			var recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.IronBar, 6);
			recipe.AddIngredient(ItemID.SilverBar, 6);
			recipe.AddIngredient(ItemID.MeteoriteBar, 6);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
