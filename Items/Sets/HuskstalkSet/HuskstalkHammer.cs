using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.HuskstalkSet
{
	public class HuskstalkHammer : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Huskstalk Hammer");
		}


		public override void SetDefaults()
		{
			Item.width = 38;
			Item.height = 30;
			Item.rare = ItemRarityID.White;
			Item.hammer = 40;
			Item.damage = 8;
			Item.knockBack = 5.5f;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 20;
			Item.useAnimation = 25;
			Item.DamageType = DamageClass.Melee;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item1;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(1);
			recipe.AddIngredient(ModContent.ItemType<AncientBark>(), 8);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}
}
