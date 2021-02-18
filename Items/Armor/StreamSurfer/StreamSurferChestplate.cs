using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.StreamSurfer
{
	[AutoloadEquip(EquipType.Body)]
	public class StreamSurferChestplate : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stream Surfer Chestplate");
			Tooltip.SetDefault("9% increased magic damage\nIncreases maximum mana by 60");

		}
		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 20;
			item.value = Terraria.Item.sellPrice(0, 0, 80, 0);
			item.rare = ItemRarityID.Orange;
			item.defense = 7;
		}

		public override void UpdateEquip(Player player)
		{
			player.magicDamage += 0.09f;
			player.statManaMax2 += 60;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<TribalScale>(), 8);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}
