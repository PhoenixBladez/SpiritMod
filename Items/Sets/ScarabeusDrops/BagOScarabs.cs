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
using SpiritMod.Items.Sets.DonatorVanity;

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
			Item.width = 20;
			Item.height = 20;
			Item.rare = -2;

			Item.maxStack = 30;

			Item.expert = true;
		}

		public override bool CanRightClick() => true;

		public override void RightClick(Player player)
		{
			player.QuickSpawnItem(player.GetSource_OpenItem(Item.type, "RightClick"), ItemID.GoldCoin, Main.rand.Next(3, 5));
			player.QuickSpawnItem(player.GetSource_OpenItem(Item.type, "RightClick"), ModContent.ItemType<ScarabPendant>());
			player.QuickSpawnItem(player.GetSource_OpenItem(Item.type, "RightClick"), ModContent.ItemType<Chitin>(), Main.rand.Next(24, 37));

			int[] lootTable = {
				ModContent.ItemType<ScarabBow>(),
				ModContent.ItemType<LocustCrook.LocustCrook>(),
				ModContent.ItemType<RoyalKhopesh>(),
				ModContent.ItemType<RadiantCane.RadiantCane>()
			};
			int loot = Main.rand.Next(lootTable.Length);
			player.QuickSpawnItem(player.GetSource_OpenItem(Item.type, "RightClick"), lootTable[loot]);

			if (Main.rand.NextDouble() < 1 / 7f)
				player.QuickSpawnItem(player.GetSource_OpenItem(Item.type, "RightClick"), ModContent.ItemType<ScarabMask>());
			if (Main.rand.NextDouble() < 1 / 10f)
				player.QuickSpawnItem(player.GetSource_OpenItem(Item.type, "RightClick"), ModContent.ItemType<Trophy1>());

			if(Main.rand.NextBool(3))
				player.QuickSpawnItem(player.GetSource_OpenItem(Item.type, "RightClick"), ModContent.ItemType<SandsOfTime>());

			int[] vanityTable = {
				ModContent.ItemType<WaasephiVanity>(),
				ModContent.ItemType<MeteorVanity>(),
				ModContent.ItemType<PixelatedFireballVanity>(),
				ModContent.ItemType<LightNovasVanity>()
			};
			int vanityloot = Main.rand.Next(vanityTable.Length);
			if (Main.rand.NextBool(20))
				player.QuickSpawnItem(player.GetSource_OpenItem(Item.type, "RightClick"), vanityTable[vanityloot]);
		}
	}
}