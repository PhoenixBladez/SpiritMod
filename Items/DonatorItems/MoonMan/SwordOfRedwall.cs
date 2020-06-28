using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.DonatorItems.MoonMan
{
	class SwordOfRedwall : ModItem
	{
		public static readonly int _type;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sword of Redwall");
		}

		public override void SetDefaults()
		{
			item.width = 42;
			item.height = 42;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.UseSound = SoundID.Item1;

			item.value = 150000;
			item.rare = ItemRarityID.Orange;
			item.value = Item.sellPrice(0, 0, 50, 0);
			item.damage = 28;
			item.knockBack = 5.8f;
			item.melee = true;

			item.useTime = 26;
			item.useAnimation = 26;
		}

		public override void AddRecipes()
		{
			var recipe = new ModRecipe(mod);
			recipe.AddRecipeGroup(RecipeGroupID.IronBar, 12);
			recipe.AddRecipeGroup("SpiritMod:SilverBars", 12);
			recipe.AddIngredient(ItemID.MeteoriteBar, 12);
			recipe.AddIngredient(ItemID.Ruby, 1);
			recipe.AddIngredient(ModContent.ItemType<OldLeather>(), 10);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
