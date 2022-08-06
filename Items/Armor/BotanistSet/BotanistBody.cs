using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.BotanistSet
{
	[AutoloadEquip(EquipType.Body, EquipType.Waist)]
	public class BotanistBody : ModItem
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Botanist Apron");

		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 20;
			Item.value = Item.sellPrice(0, 0, 10, 0);
			Item.rare = ItemRarityID.White;
			Item.defense = 1;
		}

		public override void UpdateEquip(Player player)
		{
			if (player.armor[11].IsAir)
				player.waist = (sbyte)EquipLoader.GetEquipSlot(Mod, nameof(BotanistBody), EquipType.Waist);
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.Silk, 6)
				.AddIngredient(ItemID.FallenStar, 5)
				.AddTile(TileID.Loom)
				.Register();
		}
	}
}
