using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.SwordsMisc.AlphaBladeTree
{
	public class AlphaBlade : ModItem
	{
		public override bool IsLoadingEnabled(Mod mod) => false;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Alpha Blade");
			Tooltip.SetDefault("'The power of the universe sides with you'");
		}

		public override void SetDefaults()
		{
			Item.damage = 200;
			Item.DamageType = DamageClass.Melee;
			Item.width = 70;
			Item.height = 76;
			Item.useTime = 16;
			Item.useAnimation = 16;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 6;
			Item.value = Item.sellPrice(1, 0, 0, 0);
			Item.rare = 12;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<SpiritStar>(), 1);
			recipe.AddIngredient(ItemID.LunarBar, 10);
			recipe.AddIngredient(ItemID.FragmentVortex, 4);
			recipe.AddIngredient(ItemID.FragmentNebula, 4);
			recipe.AddIngredient(ItemID.FragmentSolar, 4);
			recipe.AddIngredient(ItemID.FragmentStardust, 4);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.Register();
		}
	}
}