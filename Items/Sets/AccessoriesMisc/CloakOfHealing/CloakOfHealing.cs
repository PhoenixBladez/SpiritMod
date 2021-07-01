using SpiritMod.Items.Material;
using SpiritMod.Items.Sets.BloodcourtSet;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.AccessoriesMisc.CloakOfHealing
{
	[AutoloadEquip(EquipType.Back)]
	public class CloakOfHealing : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cloak Of Healing");
			Tooltip.SetDefault("Minions have a small chance to return life");
		}
		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 28;
			item.rare = ItemRarityID.Green;
			item.value = Item.sellPrice(0, 1, 0, 0);
			item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetSpiritPlayer().HealCloak = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.HealingPotion, 1);
			recipe.AddIngredient(ModContent.ItemType<OldLeather>(), 5);
			recipe.AddIngredient(ModContent.ItemType<BloodFire>(), 5);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
