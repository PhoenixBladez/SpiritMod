
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.Daybloom
{
	[AutoloadEquip(EquipType.Body)]
	public class DaybloomBody : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sunflower Garb");
			Tooltip.SetDefault("Increases magic damage by 1");
		}
		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 20;
			Item.value = Terraria.Item.sellPrice(0, 0, 10, 0);
			Item.rare = ItemRarityID.White;
			Item.defense = 2;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetSpiritPlayer().daybloomGarb = true;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Sunflower, 2);
			recipe.AddIngredient(ItemID.FallenStar, 2);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}
}
