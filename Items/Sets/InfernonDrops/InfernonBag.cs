using SpiritMod.Items.Sets.DonatorVanity;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.InfernonDrops
{
	public class InfernonBag : ModItem
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
			player.QuickSpawnItem(ItemID.GoldCoin, Main.rand.Next(2, 5));
			player.QuickSpawnItem(ModContent.ItemType<HellsGaze>()); //expert drop
			player.QuickSpawnItem(ModContent.ItemType<InfernalAppendage>(), Main.rand.Next(25, 36));

			int[] lootTable = {
				ModContent.ItemType<InfernalJavelin>(),
				ModContent.ItemType<DiabolicHorn>(),
				ModContent.ItemType<SevenSins>(),
				ModContent.ItemType<InfernalSword>(),
				ModContent.ItemType<InfernalStaff>(),
				ModContent.ItemType<InfernalShield>(),
				ModContent.ItemType<EyeOfTheInferno>()
			};
			int loot = Main.rand.Next(lootTable.Length);
			player.QuickSpawnItem(lootTable[loot]);

			if (Main.rand.NextDouble() < 1d / 7)
				player.QuickSpawnItem(ModContent.ItemType<InfernonMask>());
			if (Main.rand.NextDouble() < 1d / 10)
				player.QuickSpawnItem(ModContent.ItemType<Trophy4>());

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
