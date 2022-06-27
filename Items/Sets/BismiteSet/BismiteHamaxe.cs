using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.BismiteSet
{
	public class BismiteHamaxe : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bismite Hamaxe");
		}


		public override void SetDefaults()
		{
			Item.width = 38;
			Item.height = 30;
			Item.value = 1000;
			Item.rare = ItemRarityID.Blue;
			Item.hammer = 40;
			Item.axe = 6;
			Item.damage = 6;
			Item.knockBack = 4;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 18;
			Item.useAnimation = 23;
			Item.DamageType = DamageClass.Melee;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item1;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<BismiteCrystal>(), 12);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
