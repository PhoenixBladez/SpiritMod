using SpiritMod.Items.Material;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Tool
{
	public class ChitinHammer : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chitin Hammer");
		}


		public override void SetDefaults()
		{
			item.width = 44;
			item.height = 44;
			item.value = 10000;
			item.rare = ItemRarityID.Blue;
			item.hammer = 45;
			item.damage = 16;
			item.knockBack = 6;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = 26;
			item.useAnimation = 26;
			item.melee = true;
			item.useTurn = true;
			item.autoReuse = true;
			item.UseSound = SoundID.Item1;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<Chitin>(), 10);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}