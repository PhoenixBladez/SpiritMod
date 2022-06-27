using SpiritMod.Items.Sets.DonatorVanity;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.VinewrathDrops
{
	public class ReachBossBag : ModItem
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
			player.QuickSpawnItem(ItemID.GoldCoin, Main.rand.Next(4, 8));
			player.QuickSpawnItem(ModContent.ItemType<DeathRose>());

			int[] lootTable = {
				ModContent.ItemType<ThornBow>(),
				ModContent.ItemType<SunbeamStaff>(),
				ModContent.ItemType<ReachVineStaff>(),
				ModContent.ItemType<ReachBossSword>()
			};
			int loot = Main.rand.Next(lootTable.Length);
			player.QuickSpawnItem(lootTable[loot]);

			if (Main.rand.NextDouble() < 1 / 7f)
				player.QuickSpawnItem(ModContent.ItemType<ReachMask>());
			if (Main.rand.NextDouble() < 1 / 10f)
				player.QuickSpawnItem(ModContent.ItemType<Trophy5>());

			int[] vanityTable = {
				ModContent.ItemType<WaasephiVanity>(),
				ModContent.ItemType<MeteorVanity>(),
				ModContent.ItemType<PixelatedFireballVanity>(),
				ModContent.ItemType<LightNovasVanity>()
			};
			int vanityloot = Main.rand.Next(vanityTable.Length);
			if (Main.rand.NextBool(20))
				player.QuickSpawnItem(vanityTable[vanityloot]);
		}
	}
}
