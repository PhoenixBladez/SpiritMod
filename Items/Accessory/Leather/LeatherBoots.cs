
using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory.Leather
{
	[AutoloadEquip(EquipType.Shoes)]
	public class LeatherBoots : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Leather Striders");
			Tooltip.SetDefault("Slightly increases movement speed and acceleration");
		}
		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 20;
			Item.value = Item.buyPrice(0, 0, 4, 0);
			Item.rare = ItemRarityID.Blue;

			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.moveSpeed += 0.09f;
			player.runAcceleration += .045f;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(1);
			recipe.AddIngredient(ModContent.ItemType<OldLeather>(), 8);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
