using SpiritMod.Items.Accessory;
using SpiritMod.Items.Armor.Masks;
using SpiritMod.Items.Boss;
using SpiritMod.Items.Material;
using SpiritMod.Items.Weapon.Bow;
using SpiritMod.Items.Weapon.Magic;
using SpiritMod.Items.Weapon.Swung;
using SpiritMod.Items.Weapon.Flail;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.BossBags
{
	public class OverseerBag : ModItem
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
			player.QuickSpawnItem(ItemID.GoldCoin, Main.rand.Next(30, 54));
			player.QuickSpawnItem(ModContent.ItemType<EternityCharm>());
			player.QuickSpawnItem(ModContent.ItemType<EternityEssence>(), Main.rand.Next(18, 28));

			int[] lootTable = {
				ModContent.ItemType<Eternity>(),
				ModContent.ItemType<SoulExpulsor>(),
				ModContent.ItemType<EssenseTearer>(),
				ModContent.ItemType<AeonRipper>()
			};
			int loot = Main.rand.Next(lootTable.Length);
			player.QuickSpawnItem(lootTable[loot]);

			if (Main.rand.NextDouble() < 1d / 7)
				player.QuickSpawnItem(ModContent.ItemType<OverseerMask>());
			if (Main.rand.NextDouble() < 1d / 10)
				player.QuickSpawnItem(ModContent.ItemType<Trophy9>());
		}
	}
}
