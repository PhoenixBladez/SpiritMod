using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.BismiteSet
{
	public class BismitePickaxe : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bismite Pickaxe");
		}


		public override void SetDefaults()
		{
			Item.width = 38;
			Item.height = 30;
			Item.value = 1000;
			Item.rare = ItemRarityID.Blue;
			Item.pick = 40;
			Item.damage = 4;
			Item.knockBack = 4;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 16;
			Item.useAnimation = 20;
			Item.DamageType = DamageClass.Melee;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item1;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<BismiteCrystal>(), 14);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
