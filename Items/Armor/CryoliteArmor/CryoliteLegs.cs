using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.CryoliteArmor
{
	[AutoloadEquip(EquipType.Legs)]
	public class CryoliteLegs : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cryolite Greaves");
			Tooltip.SetDefault("9% increased melee speed");
		}

		public override void SetDefaults()
		{
			item.width = 38;
			item.height = 26;
			item.value = Item.buyPrice(gold: 1);
			item.rare = ItemRarityID.Orange;
			item.defense = 7;
		}

		public override void UpdateEquip(Player player)
		{
			player.meleeSpeed += .09f;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<CryoliteBar>(), 11);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
