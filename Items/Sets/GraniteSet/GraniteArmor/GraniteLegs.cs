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
			item.width = 28;
			item.height = 24;
			item.value = 1100;
			item.rare = ItemRarityID.Green;
			item.defense = 6;
		}
		public override void UpdateEquip(Player player)
		{
			player.jumpSpeedBoost += 0.5f;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<GraniteChunk>(), 13);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
