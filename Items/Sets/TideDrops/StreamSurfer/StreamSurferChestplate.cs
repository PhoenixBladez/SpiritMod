using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.TideDrops.StreamSurfer
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
			Item.width = 30;
			Item.height = 20;
			Item.value = Item.sellPrice(0, 0, 80, 0);
			Item.rare = ItemRarityID.Orange;
			Item.defense = 7;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetDamage(DamageClass.Magic) += 0.09f;
			player.statManaMax2 += 60;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(1);
			recipe.AddIngredient(ModContent.ItemType<TribalScale>(), 8);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
