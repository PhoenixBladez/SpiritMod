
using SpiritMod.Items.Accessory;
using SpiritMod.Items.Armor.Masks;
using SpiritMod.Items.Boss;
using SpiritMod.Items.Material;
using SpiritMod.Items.Weapon.Bow;
using SpiritMod.Items.Weapon.Magic;
using SpiritMod.Items.Weapon.Swung;
using SpiritMod.Items.Weapon.Thrown;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.BossBags
{
	public class DuskingBag : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Treasure Bag");
			Tooltip.SetDefault("Consumable\nRight Click to open");
		}


		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 20;
			item.rare = -2;

			item.maxStack = 30;

			item.expert = true;
		}

		public override bool CanRightClick()
		{
			return true;
		}

		public override void RightClick(Player player)
		{
			player.QuickSpawnItem(ItemID.GoldCoin, Main.rand.Next(3, 7));
			player.QuickSpawnItem(ModContent.ItemType<DuskPendant>());
			player.QuickSpawnItem(ModContent.ItemType<DuskStone>(), Main.rand.Next(25, 36));

			int[] lootTable = {
				ModContent.ItemType<CrystalShadow>(),
				ModContent.ItemType<ShadowflameSword>(),
				ModContent.ItemType<UmbraStaff>(),
				ModContent.ItemType<ShadowSphere>(),
				ModContent.ItemType<Shadowmoor>()
			};
			int loot = Main.rand.Next(lootTable.Length);
			if (loot == 0)
				player.QuickSpawnItem(lootTable[0], Main.rand.Next(29, 49));
			else
				player.QuickSpawnItem(lootTable[loot]);

			if (Main.rand.NextDouble() < 1d / 7)
				player.QuickSpawnItem(ModContent.ItemType<DuskingMask>());
			if (Main.rand.NextDouble() < 1d / 10)
				player.QuickSpawnItem(ModContent.ItemType<Trophy6>());
		}
	}
}
