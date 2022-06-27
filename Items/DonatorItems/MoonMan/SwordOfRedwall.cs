using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.DonatorItems.MoonMan
{
	class SwordOfRedwall : ModItem
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sword of Redwall");
            Tooltip.SetDefault("Eulalia!");
		}

		public override void SetDefaults()
		{
			Item.width = 42;
			Item.height = 42;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.UseSound = SoundID.Item1;

			Item.value = 150000;
			Item.rare = ItemRarityID.Orange;
			Item.value = Item.sellPrice(0, 0, 50, 0);
			Item.damage = 28;
			Item.knockBack = 5.8f;
			Item.DamageType = DamageClass.Melee;

			Item.useTime = 26;
			Item.useAnimation = 26;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe();
			recipe.AddRecipeGroup(RecipeGroupID.IronBar, 12);
			recipe.AddRecipeGroup("SpiritMod:SilverBars", 12);
			recipe.AddIngredient(ItemID.MeteoriteBar, 12);
			recipe.AddIngredient(ItemID.Ruby, 1);
			recipe.AddIngredient(ModContent.ItemType<OldLeather>(), 10);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
