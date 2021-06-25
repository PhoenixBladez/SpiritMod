using SpiritMod.Items.Sets.MoonWizardDrops.JellynautHelmet;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Items.Consumable;

namespace SpiritMod.Items.Sets.MoonWizardDrops
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
			player.QuickSpawnItem(ItemID.GoldCoin, Main.rand.Next(4, 5));
			if (Main.rand.NextDouble() < 1d / 7)
				player.QuickSpawnItem(ModContent.ItemType<MJWMask>());
			if (Main.rand.NextDouble() < 1d / 10)
				player.QuickSpawnItem(ModContent.ItemType<MJWTrophy>());
            player.QuickSpawnItem(ModContent.ItemType<Cornucopion>());
            int[] lootTable = {
                ModContent.ItemType<Moonshot>(),
                ModContent.ItemType<Moonburst>(),
                ModContent.ItemType<JellynautBubble>(),
                ModContent.ItemType<MoonjellySummonStaff>()
            };
            int loot = Main.rand.Next(lootTable.Length);
			int lunazoastack = Main.rand.Next(10, 12);
			player.QuickSpawnItem(lootTable[loot]);
			if (lootTable[loot] == ModContent.ItemType<Moonshot>())
				lunazoastack += Main.rand.Next(10, 20);

			player.QuickSpawnItem(ModContent.ItemType<TinyLunazoaItem>(), lunazoastack);
		}
	}
}
