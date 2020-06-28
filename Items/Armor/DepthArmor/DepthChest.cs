using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.DepthArmor
{
	[AutoloadEquip(EquipType.Body)]
	public class DepthChest : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Depth Walker's Platemail");
			Tooltip.SetDefault("12% increased melee and minion damage\nIncreases your maximum number of minions");

		}

		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 20;
			item.value = Item.buyPrice(silver: 60);
			item.rare = ItemRarityID.Pink;
			item.defense = 16;
		}

		public override void UpdateEquip(Player player)
		{
			player.minionDamage += 0.12f;
			player.meleeDamage += 0.12f;
			player.maxMinions += 1;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<DepthShard>(), 12);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}
