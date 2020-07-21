using SpiritMod.Items.Material;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Tool
{
	public class HuskstalkHammer : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Huskstalk Hammer");
		}


		public override void SetDefaults()
		{
			item.width = 38;
			item.height = 30;
			item.rare = 0;
			item.hammer = 40;
			item.damage = 8;
			item.knockBack = 5.5f;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = 20;
			item.useAnimation = 25;
			item.melee = true;
			item.useTurn = true;
			item.autoReuse = true;
			item.UseSound = SoundID.Item1;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<AncientBark>(), 8);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}
