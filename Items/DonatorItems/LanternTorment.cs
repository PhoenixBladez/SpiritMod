
using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.DonatorItems
{
	//[AutoloadEquip(EquipType.Back)]
	//[AutoloadEquip(EquipType.HandsOn)]
	public class LanternTorment : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Silva'S Soul");
			Tooltip.SetDefault("Minions have a chance to spawn Tormented Soldiers ");
		}
		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 28;
			item.rare = ItemRarityID.LightRed;
			item.value = Terraria.Item.sellPrice(0, 1, 0, 0);
			item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetSpiritPlayer().TormentLantern = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Sapphire, 5);
			recipe.AddIngredient(ItemID.SoulofNight, 5);
			recipe.AddIngredient(ItemID.Topaz, 5);
			recipe.AddIngredient(ModContent.ItemType<Chitin>(), 10);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
