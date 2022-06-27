using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Items.Sets.GraniteSet;

namespace SpiritMod.Items.Sets.GraniteSet.GraniteArmor
{
	[AutoloadEquip(EquipType.Legs)]
	public class GraniteLegs : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Granite Greaves");
			Tooltip.SetDefault("Increases jump height slightly");

		}
		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 24;
			Item.value = 1100;
			Item.rare = ItemRarityID.Green;
			Item.defense = 6;
		}
		public override void UpdateEquip(Player player)
		{
			player.jumpSpeedBoost += 0.5f;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<GraniteChunk>(), 13);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
