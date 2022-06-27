using SpiritMod.Items.Sets.TideDrops;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.TideDrops.StreamSurfer
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
			Item.width = 22;
			Item.height = 18;
			Item.value = Terraria.Item.sellPrice(0, 0, 60, 0);
			Item.rare = ItemRarityID.Orange;
			Item.defense = 5;
		}
		public override void UpdateEquip(Player player)
		{
			player.magicDamage += .07f;
			player.manaCost -= 0.10f;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(1);
			recipe.AddIngredient(ModContent.ItemType<TribalScale>(), 5);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
