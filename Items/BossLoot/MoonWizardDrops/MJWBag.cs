using SpiritMod.Items.BossLoot.MoonWizardDrops.JellynautHelmet;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Items.Consumable;
using SpiritMod.Items.Sets.DonatorVanity;

namespace SpiritMod.Items.BossLoot.MoonWizardDrops
{
	public class MJWBag : ModItem
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

		public override bool CanRightClick()
		{
			return true;
		}

		public override void RightClick(Player player)
		{
			player.QuickSpawnItem(player.GetSource_OpenItem(Item.type, "RightClick"), ItemID.GoldCoin, Main.rand.Next(4, 5));
			if (Main.rand.NextDouble() < 1d / 7)
				player.QuickSpawnItem(player.GetSource_OpenItem(Item.type, "RightClick"), ModContent.ItemType<MJWMask>());
			if (Main.rand.NextDouble() < 1d / 10)
				player.QuickSpawnItem(player.GetSource_OpenItem(Item.type, "RightClick"), ModContent.ItemType<MJWTrophy>());
            player.QuickSpawnItem(player.GetSource_OpenItem(Item.type, "RightClick"), ModContent.ItemType<Cornucopion>());
            int[] lootTable = {
                ModContent.ItemType<Moonshot>(),
                ModContent.ItemType<Moonburst>(),
                ModContent.ItemType<JellynautBubble>(),
                ModContent.ItemType<MoonjellySummonStaff>()
            };
            int loot = Main.rand.Next(lootTable.Length);
			int lunazoastack = Main.rand.Next(10, 12);
			player.QuickSpawnItem(player.GetSource_OpenItem(Item.type, "RightClick"), lootTable[loot]);
			if (lootTable[loot] == ModContent.ItemType<Moonshot>())
				lunazoastack += Main.rand.Next(10, 20);

			player.QuickSpawnItem(player.GetSource_OpenItem(Item.type, "RightClick"), ModContent.ItemType<TinyLunazoaItem>(), lunazoastack);

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
