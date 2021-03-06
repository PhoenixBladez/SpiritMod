using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.OverseerArmor
{
	[AutoloadEquip(EquipType.Body)]
	public class ShadowSBody : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shadowspirit Breastplate");
			Tooltip.SetDefault("30% increased movement speed\nIncreases max life by 50\nMassively increases life regen'");

		}

		public override void SetDefaults()
		{
			item.width = 34;
			item.height = 24;
			item.value = Item.buyPrice(gold: 20);
			item.rare = ItemRarityID.Purple;
			item.defense = 30;
		}

		public override void UpdateEquip(Player player)
		{
			player.moveSpeed += 0.30f;
			player.statLifeMax2 += 50;
			player.lifeRegen += 10;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<EternityEssence>(), 5);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}
