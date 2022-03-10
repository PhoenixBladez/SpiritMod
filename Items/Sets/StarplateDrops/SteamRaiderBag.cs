using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Items.Sets.DonatorVanity;
namespace SpiritMod.Items.Sets.StarplateDrops
{
	public class SteamRaiderBag : ModItem
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
			player.QuickSpawnItem(ItemID.GoldCoin, Main.rand.Next(2, 4));
			player.QuickSpawnItem(ModContent.ItemType<StarMap>());
			player.QuickSpawnItem(ModContent.ItemType<CosmiliteShard>(), Main.rand.Next(6, 10));

			if (Main.rand.NextBool(7)) player.QuickSpawnItem(ModContent.ItemType<StarplateMask>());
			if (Main.rand.NextBool(10)) player.QuickSpawnItem(ModContent.ItemType<Trophy3>());

			int[] vanityTable = {
				ModContent.ItemType<WaasephiVanity>(),
				ModContent.ItemType<MeteorVanity>(),
				ModContent.ItemType<PixelatedFireballVanity>(),
				ModContent.ItemType<LightNovasVanity>()
			};
			int loot = Main.rand.Next(vanityTable.Length);
			if (Main.rand.NextBool(20))
				player.QuickSpawnItem(vanityTable[loot]);
		}
	}
}
