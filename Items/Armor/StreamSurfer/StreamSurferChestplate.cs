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
			Tooltip.SetDefault("6% increased damage\nIncreases maximum mana by 40");

		}
		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 20;
			item.value = 6000;
			item.rare = ItemRarityID.Blue;
			item.defense = 7;
		}

		public override void UpdateEquip(Player player)
		{
			player.allDamage += 0.06f;
			player.statManaMax2 += 40;
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
