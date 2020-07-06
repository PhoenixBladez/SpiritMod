using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.StreamSurfer
{
	[AutoloadEquip(EquipType.Legs)]
	public class StreamSurferLeggings : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stream Surfer Loincloth");
			Tooltip.SetDefault("4% increased damage \n10% reduced mana usage");

		}
		public override void SetDefaults()
		{
			item.width = 22;
			item.height = 18;
			item.value = Item.buyPrice(silver: 40);
			item.rare = ItemRarityID.Blue;
			item.defense = 5;
		}
		public override void UpdateEquip(Player player)
		{
			player.moveSpeed += .06f;
			player.manaCost -= 0.10f;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<TribalScale>(), 5);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}
