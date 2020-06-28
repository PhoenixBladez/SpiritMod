using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.QuicksilverArmor
{
	[AutoloadEquip(EquipType.Body)]
	public class QuicksilverBody : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Quicksilver Chestguard");
			Tooltip.SetDefault("10% increased melee speed\nSlightly increases the knockback of your minions\nIncreases your max number of minions by 2\nIncreases life regeneration slightly");

		}

		public override void SetDefaults()
		{
			item.width = 26;
			item.height = 12;
			item.value = Item.buyPrice(gold: 1);
			item.rare = 8;
			item.defense = 25;
		}

		public override void UpdateEquip(Player player)
		{
			player.meleeSpeed += 0.10f;
			player.minionKB += 0.5f;
			player.maxMinions += 2;
			player.lifeRegen += 2;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<Material.Material>(), 16);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}