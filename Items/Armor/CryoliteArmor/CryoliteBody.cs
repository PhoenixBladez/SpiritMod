using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.CryoliteArmor
{
	[AutoloadEquip(EquipType.Body)]
	public class CryoliteBody : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cryolite Chestplate");
			Tooltip.SetDefault("10% increased melee damage\nMelee attacks may slow down hit enemies");
		}

		public override void SetDefaults()
		{
			item.width = 38;
			item.height = 26;
			item.value = Item.buyPrice(gold: 1);
			item.rare = ItemRarityID.Orange;
			item.defense = 6;
		}

		public override void UpdateEquip(Player player)
		{
			player.meleeDamage += 0.1f;
			player.GetSpiritPlayer().cryoChestplate = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<CryoliteBar>(), 15);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
