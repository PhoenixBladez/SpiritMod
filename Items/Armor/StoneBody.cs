using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor
{
	[AutoloadEquip(EquipType.Body)]
	public class StoneBody : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stone Plate");
			Tooltip.SetDefault("Decreases movement speed by 5%");

		}

		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 20;
			Item.value = 0;
			Item.rare = ItemRarityID.White;
			Item.defense = 3;
		}

		public override void UpdateEquip(Player player)
		{
			player.moveSpeed -= .05f;
			player.maxRunSpeed -= 0.05f;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(1);
			recipe.AddIngredient(ItemID.StoneBlock, 50);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}
}