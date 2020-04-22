using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace SpiritMod.Items.DonatorItems.MoonMan
{
	class SwordOfRedwall : ModItem
	{
		public static readonly int _type;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sword of Redwall");
			Tooltip.SetDefault("~Donator Item~");
		}

		public override void SetDefaults()
		{
			item.width = 42;
			item.height = 42;
			item.useStyle = 1;
			item.UseSound = SoundID.Item1;

			item.value = 150000;
			item.rare = 3;
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
            recipe.AddRecipeGroup("LeadBars", 12);
            recipe.AddRecipeGroup("SilverBars", 12);
            recipe.AddIngredient(ItemID.MeteoriteBar, 12);
			recipe.AddIngredient(ItemID.Ruby, 1);
			recipe.AddIngredient(mod.ItemType("OldLeather"), 10);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
