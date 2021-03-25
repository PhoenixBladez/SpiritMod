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
			Tooltip.SetDefault("7% increased magic damage\n10% reduced mana usage");

		}
		public override void SetDefaults()
		{
			item.width = 22;
			item.height = 18;
			item.value = Terraria.Item.sellPrice(0, 0, 60, 0);
			item.rare = ItemRarityID.Orange;
			item.defense = 5;
		}
		public override void UpdateEquip(Player player)
		{
			player.magicDamage += .07f;
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
