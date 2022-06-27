
using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.SpiritSet
{
	[AutoloadEquip(EquipType.Wings)]
	public class SpiritWings : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spirit Wings");
			Tooltip.SetDefault("Allows for flight and slow fall.");
		}
		public override void SetDefaults()
		{
			Item.width = 47;
			Item.height = 37;
			Item.value = 60000;
			Item.rare = ItemRarityID.Pink;

			Item.accessory = true;

			Item.rare = ItemRarityID.Pink;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.wingTimeMax = 150;
			player.GetSpiritPlayer().BlueDust = true;
		}

		public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising,
ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
		{
			ascentWhenFalling = 0.75f;
			ascentWhenRising = 0.11f;
			maxCanAscendMultiplier = 1f;
			maxAscentMultiplier = 2.6f;
			constantAscend = 0.135f;
		}

		public override void HorizontalWingSpeeds(Player player, ref float speed, ref float acceleration)
		{

			speed = 7f;
			acceleration *= 2f;
		}
		public override void AddRecipes()
		{
			Recipe modRecipe = CreateRecipe(1);
			modRecipe.AddIngredient(ModContent.ItemType<SpiritBar>(), 14);
			modRecipe.AddIngredient(ItemID.SoulofFlight, 12);
			modRecipe.AddTile(TileID.MythrilAnvil);
			modRecipe.Register();
		}
	}
}