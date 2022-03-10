
using SpiritMod.Items.Sets.DonatorVanity;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.AtlasDrops
{
	public class AtlasBag : ModItem
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
			player.QuickSpawnItem(ItemID.GoldCoin, Main.rand.Next(7, 12));
			player.QuickSpawnItem(ModContent.ItemType<AtlasEye>());
			player.QuickSpawnItem(ModContent.ItemType<ArcaneGeyser>(), Main.rand.Next(30, 46));

			int[] lootTable = {
				ModContent.ItemType<Mountain>(),
				ModContent.ItemType<TitanboundBulwark>(),
				ModContent.ItemType<CragboundStaff>(),
				ModContent.ItemType<QuakeFist>(),
				ModContent.ItemType<Earthshatter>()
			};
			int loot = Main.rand.Next(lootTable.Length);
			player.QuickSpawnItem(lootTable[loot]);

			if (Main.rand.NextDouble() < 1 / 7f)
				player.QuickSpawnItem(ModContent.ItemType<AtlasMask>());
			if (Main.rand.NextDouble() < 1 / 10f)
				player.QuickSpawnItem(ModContent.ItemType<Trophy8>());

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
