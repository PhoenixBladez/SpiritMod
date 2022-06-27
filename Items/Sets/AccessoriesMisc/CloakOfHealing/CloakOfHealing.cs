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
			Item.width = 30;
			Item.height = 28;
			Item.rare = ItemRarityID.Green;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetSpiritPlayer().HealCloak = true;
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.HealingPotion, 1);
			recipe.AddIngredient(ModContent.ItemType<OldLeather>(), 5);
			recipe.AddIngredient(ModContent.ItemType<DreamstrideEssence>(), 5);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
