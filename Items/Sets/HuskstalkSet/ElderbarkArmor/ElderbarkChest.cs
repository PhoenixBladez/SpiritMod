using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.HuskstalkSet.ElderbarkArmor
{
	[AutoloadEquip(EquipType.Body)]
	public class ElderbarkChest : ModItem
	{
		public override void SetStaticDefaults()
			=> DisplayName.SetDefault("Elderbark Breastplate");

		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 24;
			Item.value = 0;
			Item.rare = ItemRarityID.White;
			Item.defense = 2;
		}
		public override void DrawHands(ref bool drawHands, ref bool drawArms)
		{
			drawHands = true;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<AncientBark>(), 30);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}
}
