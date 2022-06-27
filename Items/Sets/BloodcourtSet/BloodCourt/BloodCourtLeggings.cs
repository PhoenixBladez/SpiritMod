using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.BloodcourtSet.BloodCourt
{
	[AutoloadEquip(EquipType.Legs)]
	public class BloodCourtLeggings : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bloodcourt's Leggings");
			Tooltip.SetDefault("Increases movement speed by 10%\nIncreases maximum mana by 30");

		}
		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 18;
			Item.value = 4000;
			Item.rare = ItemRarityID.Green;
			Item.defense = 3;
		}
		public override void UpdateEquip(Player player)
		{
			player.statManaMax2 += 30;
			player.moveSpeed += .1f;
			player.maxRunSpeed += .04f;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(1);
			recipe.AddIngredient(ModContent.ItemType<DreamstrideEssence>(), 7);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
