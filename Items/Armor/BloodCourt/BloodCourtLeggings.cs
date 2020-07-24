using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.BloodCourt
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
			item.width = 22;
			item.height = 18;
			item.value = 4000;
			item.rare = ItemRarityID.Green;
			item.defense = 2;
		}
		public override void UpdateEquip(Player player)
		{
			player.statManaMax2 += 30;
			player.moveSpeed += .1f;
			player.maxRunSpeed += .04f;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<BloodFire>(), 7);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}
