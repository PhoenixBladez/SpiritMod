
using SpiritMod.Items.Material;
using SpiritMod.Tiles.Furniture;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.BloomwindArmor
{
	[AutoloadEquip(EquipType.Head)]
	public class BloomwindHelmet : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bloomwind Helmet");
			Tooltip.SetDefault("Increases your max number of minions\n10% increased minion damage");
		}

		public override void SetDefaults()
		{
			item.width = 40;
			item.height = 30;
			item.value = Item.buyPrice(gold: 5);
			item.rare = 6;

			item.defense = 9;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<BloomwindChestguard>() && legs.type == ModContent.ItemType<BloomwindLeggings>();
		}

		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = "Increases your max number of minions\nYou are protected by a guardian of the wild";
			player.maxMinions += 1;
			player.GetSpiritPlayer().bloomwindSet = true;
		}

		public override void ArmorSetShadows(Player player)
		{
			player.armorEffectDrawShadow = true;
		}

		public override void UpdateEquip(Player player)
		{
			player.maxMinions += 1;
			player.minionDamage += 0.10f;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<PrimevalEssence>(), 8);
			recipe.AddTile(ModContent.TileType<EssenceDistorter>());
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}