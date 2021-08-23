using SpiritMod.Items.Sets.ScarabeusDrops;
using SpiritMod.Items.Sets.ScarabeusDrops.Khopesh;
using SpiritMod.Items.Sets.ScarabeusDrops.LocustCrook;
using SpiritMod.Items.Sets.ScarabeusDrops.ScarabExpertDrop;
using SpiritMod.Items.Sets.ScarabeusDrops.AdornedBow;
using SpiritMod.Items.Sets.ScarabeusDrops.RadiantCane;
using SpiritMod.Items.Equipment;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.ScarabeusDrops
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

		public override bool CanRightClick() => true;

		public override void RightClick(Player player)
		{
			player.QuickSpawnItem(ItemID.GoldCoin, Main.rand.Next(3, 5));
			player.QuickSpawnItem(ModContent.ItemType<ScarabPendant>());
			player.QuickSpawnItem(ModContent.ItemType<Chitin>(), Main.rand.Next(24, 37));

			int[] lootTable = {
				ModContent.ItemType<ScarabBow>(),
				ModContent.ItemType<LocustCrook.LocustCrook>(),
				ModContent.ItemType<RoyalKhopesh>(),
				ModContent.ItemType<RadiantCane.RadiantCane>()
			};
			int loot = Main.rand.Next(lootTable.Length);
			player.QuickSpawnItem(lootTable[loot]);

			if (Main.rand.NextDouble() < 1 / 7f)
				player.QuickSpawnItem(ModContent.ItemType<ScarabMask>());
			if (Main.rand.NextDouble() < 1 / 10f)
				player.QuickSpawnItem(ModContent.ItemType<Trophy1>());

			if(Main.rand.NextBool(3))
				player.QuickSpawnItem(ModContent.ItemType<SandsOfTime>());
		}
	}
}