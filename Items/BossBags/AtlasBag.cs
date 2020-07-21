
using SpiritMod.Items.Accessory;
using SpiritMod.Items.Armor.Masks;
using SpiritMod.Items.Boss;
using SpiritMod.Items.Material;
using SpiritMod.Items.Weapon.Bow;
using SpiritMod.Items.Weapon.Magic;
using SpiritMod.Items.Weapon.Summon;
using SpiritMod.Items.Weapon.Swung;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.BossBags
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

		public override bool CanRightClick()
		{
			return true;
		}
		public override void RightClick(Player player)
		{
			player.QuickSpawnItem(ItemID.GoldCoin, Main.rand.Next(7, 12));
			player.QuickSpawnItem(ModContent.ItemType<AtlasEye>());
			player.QuickSpawnItem(ModContent.ItemType<ArcaneGeyser>(), Main.rand.Next(30, 46));

			int[] lootTable = {
				ModContent.ItemType<KingRock>(),
				ModContent.ItemType<Mountain>(),
				ModContent.ItemType<TitanboundBulwark>(),
				ModContent.ItemType<CragboundStaff>(),
				ModContent.ItemType<QuakeFist>(),
				ModContent.ItemType<Earthshatter>()
			};
			int loot = Main.rand.Next(lootTable.Length);
			player.QuickSpawnItem(lootTable[loot]);

			if (Main.rand.NextDouble() < 1d / 7)
				player.QuickSpawnItem(ModContent.ItemType<AtlasMask>());
			if (Main.rand.NextDouble() < 1d / 10)
				player.QuickSpawnItem(ModContent.ItemType<Trophy8>());
		}
	}
}
