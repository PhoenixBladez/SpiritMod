using SpiritMod.Items.Armor.Masks;
using SpiritMod.Items.Boss;
using SpiritMod.Items.Equipment;
using SpiritMod.Items.Equipment.ScarabExpertDrop;
using SpiritMod.Items.Material;
using SpiritMod.Items.Weapon.Bow.AdornedBow;
using SpiritMod.Items.Weapon.Summon;
using SpiritMod.Items.Weapon.Swung.Khopesh;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.BossBags
{
	public class BagOScarabs : ModItem
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
			player.QuickSpawnItem(ItemID.GoldCoin, Main.rand.Next(2, 3));
			player.QuickSpawnItem(ModContent.ItemType<ScarabPendant>());
			player.QuickSpawnItem(ModContent.ItemType<Chitin>(), Main.rand.Next(25, 36));

			int[] lootTable = {
				ModContent.ItemType<ScarabBow>(),
				ModContent.ItemType<OrnateStaff>(),
				ModContent.ItemType<RoyalKhopesh>()
			};
			int loot = Main.rand.Next(lootTable.Length);
			player.QuickSpawnItem(lootTable[loot]);

			if (Main.rand.NextDouble() < 1d / 7)
				player.QuickSpawnItem(ModContent.ItemType<ScarabMask>());
			if (Main.rand.NextDouble() < 1d / 10)
				player.QuickSpawnItem(ModContent.ItemType<Trophy1>());

			if(Main.rand.NextBool(3))
				player.QuickSpawnItem(ModContent.ItemType<DesertSnowglobe>());
		}
	}
}