using SpiritMod.Items.Sets.AvianDrops.ApostleArmor;
using SpiritMod.Items.Sets.DonatorVanity;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.AvianDrops
{
	public class FlyerBag : ModItem
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
            player.QuickSpawnItem(ModContent.ItemType<AvianHook>());
            player.QuickSpawnItem(ItemID.GoldCoin, Main.rand.Next(5, 9));

            int[] lootTable = {
                ModContent.ItemType<TalonBlade>(),
                ModContent.ItemType<Talonginus>(),
                ModContent.ItemType<SoaringScapula>(),
                ModContent.ItemType<TalonPiercer>(),
                ModContent.ItemType<SkeletalonStaff>()
            };
            int loot = Main.rand.Next(lootTable.Length);
            int[] lootTable1 = {
                ModContent.ItemType<TalonHeaddress>(),
                ModContent.ItemType<TalonGarb>()
            };
            int loot1 = Main.rand.Next(lootTable1.Length);
            player.QuickSpawnItem(lootTable[loot]);
            player.QuickSpawnItem(lootTable1[loot1]);

            if (Main.rand.NextDouble() < 1d / 7)
                player.QuickSpawnItem(ModContent.ItemType<FlierMask>());
            if (Main.rand.NextDouble() < 1d / 10)
                player.QuickSpawnItem(ModContent.ItemType<Trophy2>());

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