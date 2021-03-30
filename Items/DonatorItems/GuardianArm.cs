using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.DonatorItems
{
	[AutoloadEquip(EquipType.HandsOn)]
	public class GuardianArm : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Guardian's Arm");
			Tooltip.SetDefault("Increases sentry count by 1\nIncreases armor penetration by 5");
		}
		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 28;
			item.rare = ItemRarityID.Pink;
			item.value = Item.sellPrice(0, 2, 0, 0);
			item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.maxTurrets += 1;
			player.armorPenetration += 5;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.CrystalShard, 8);
			recipe.AddIngredient(ModContent.ItemType<MoonStone>(), 6);
			recipe.AddIngredient(ItemID.SoulofLight, 12);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
