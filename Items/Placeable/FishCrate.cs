using SpiritMod.Items.Consumable;
using SpiritMod.Items.Consumable.Fish;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using BrownMushrooms = SpiritMod.Tiles.Ambient.SurfaceIce.TundraRock;

namespace SpiritMod.Items.Placeable
{
	public class FishCrate : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Packing Crate");
			Tooltip.SetDefault("'A logo from a popular fishing company can be seen'\nRight click to open\nContains different types of fish");
		}

		public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 20;
			Item.rare = ItemRarityID.Orange;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.createTile = ModContent.TileType<BrownMushrooms>();
			Item.maxStack = 999;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.consumable = true;
		}

		public override bool CanRightClick() => true;

		public override void Update(ref float gravity, ref float maxFallSpeed)
		{
			if (Item.wet)
			{
				gravity *= 0f;
				maxFallSpeed *= -.09f;
			}
			else
			{
				maxFallSpeed *= 1f;
				gravity *= 1f;
			}
		}

		public override void RightClick(Player player)
		{
			var source = player.GetSource_ItemUse(
				Item, "RightClick");

			if (Main.rand.NextBool(2))
				player.QuickSpawnItem(source, ModContent.ItemType<RawFish>());

			if (Main.rand.NextBool(4))
			{
				if (Main.rand.NextBool())
					player.QuickSpawnItem(source, ModContent.ItemType<FloaterItem>());
				else
					player.QuickSpawnItem(source, ModContent.ItemType<LuvdiscItem>());
			}

			int[] lootTable = { ItemID.Shrimp, ItemID.Salmon, ItemID.Bass, ItemID.RedSnapper, ItemID.Trout };
			player.QuickSpawnItem(source, lootTable[Main.rand.Next(lootTable.Length)], Main.rand.Next(3, 5));

			if (Main.rand.NextBool(4))
			{
				int[] lootTable3 = { ItemID.ArmoredCavefish, ItemID.Damselfish, ItemID.DoubleCod, ItemID.FrostMinnow };
				player.QuickSpawnItem(source, lootTable3[Main.rand.Next(lootTable3.Length)], Main.rand.Next(1, 2));
			}

			if (Main.rand.NextBool(27))
			{
				int[] lootTable4 = { ItemID.ReaverShark, ItemID.Swordfish, ItemID.SawtoothShark };
				player.QuickSpawnItem(source, lootTable4[Main.rand.Next(lootTable4.Length)]);
			}

			if (Main.rand.NextBool(14))
			{
				string[] lootTable2123 = { "DiverLegs", "DiverHead", "DiverBody" };
				int loot443 = Main.rand.Next(lootTable2123.Length);
				player.QuickSpawnItem(source, Mod.Find<ModItem>(lootTable2123[loot443]).Type);
			}

			if (Main.rand.NextBool(3))
			{
				int[] lootTable2 = { ItemID.FrostDaggerfish, ItemID.BombFish };
				player.QuickSpawnItem(source, lootTable2[Main.rand.Next(lootTable2.Length)], Main.rand.Next(9, 12));
			}

			if (Main.hardMode && Main.rand.NextBool(10))
			{
				int[] lootTable51 = { ItemID.FlarefinKoi, ItemID.Obsidifish, ItemID.Prismite, ItemID.PrincessFish };
				player.QuickSpawnItem(source, lootTable51[Main.rand.Next(lootTable51.Length)], Main.rand.Next(1, 3));
			}

			if (Main.rand.NextBool(3))
				player.QuickSpawnItem(source, ItemID.SilverCoin, Main.rand.Next(10, 90));

			if (Main.rand.NextBool(7))
				player.QuickSpawnItem(source, ItemID.GoldCoin, Main.rand.Next(1, 3));
		}
	}
}