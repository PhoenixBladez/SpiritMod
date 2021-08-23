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
			item.width = 28;
			item.height = 24;
			item.value = 0;
			item.rare = ItemRarityID.White;
			item.defense = 2;
		}
		public override void DrawHands(ref bool drawHands, ref bool drawArms)
		{
			drawHands = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<AncientBark>(), 30);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
