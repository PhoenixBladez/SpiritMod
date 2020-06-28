
using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory
{
	[AutoloadEquip(EquipType.HandsOn)]
	public class ShadowGauntlet : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shadow Gauntlet");
			Tooltip.SetDefault("Melee attacks may inflict Shadowflame\n10% increased melee damage and speed\nIncreases melee knockback");
		}
		public override void SetDefaults()
		{
			item.width = 16;
			item.height = 16;
			item.rare = ItemRarityID.Yellow;
			item.value = Item.buyPrice(gold: 15);
			item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetSpiritPlayer().shadowGauntlet = true;
			player.meleeSpeed += 0.1f;
			player.meleeDamage += 0.1f;
			player.kbGlove = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.FireGauntlet);
			recipe.AddIngredient(ModContent.ItemType<DuskStone>(), 18);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
