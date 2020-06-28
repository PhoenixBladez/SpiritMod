using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.OverseerArmor
{
	[AutoloadEquip(EquipType.Legs)]
	public class ShadowLegs : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shadowspirit Treads");
			Tooltip.SetDefault("Increases length of invincibility after taking damage\n22% increased critical strike chance\nIncreases Max Life by 50");
		}

		public override void SetDefaults()
		{
			item.width = 22;
			item.height = 16;
			item.value = Item.buyPrice(gold: 20);
			item.rare = ItemRarityID.Purple;
			item.defense = 28;
		}
		public override void UpdateEquip(Player player)
		{

			player.longInvince = true;

			player.magicCrit += 22;
			player.meleeCrit += 22;
			player.rangedCrit += 22;

			player.statLifeMax2 += 50;

		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<EternityEssence>(), 4);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}
