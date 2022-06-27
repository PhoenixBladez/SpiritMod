using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SpiritMod.Items.Placeable.Furniture;
using Terraria.Enums;
using Terraria.ObjectData;

namespace SpiritMod.Items.Sets.FloatingItems
{
	public class SunkenTreasure : FloatingItem
	{
		public override float SpawnWeight => 0.001f;
		public override float Weight => base.Weight * 0.9f;
		public override float Bouyancy => base.Bouyancy * 1.08f;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sunken Treasure");
			Tooltip.SetDefault("Right-click to open\n'Perhaps it is still full of valuable treasure''");
		}
		public override void SetDefaults()
		{
			Item.width = Item.height = 16;
			Item.rare = ItemRarityID.LightRed;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.maxStack = 99;
			Item.createTile = ModContent.TileType<SunkenTreasureTile>();
			Item.useTime = Item.useAnimation = 20;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.noMelee = true;
			Item.autoReuse = false;
		}

		public override bool CanRightClick() => true;

		public override void RightClick(Player player)
		{
			if (Main.rand.Next(3) == 0)
			{
				int[] lootTable = {
					ItemID.FishHook,
					ItemID.ShipsWheel,
					ItemID.ShipsWheel,
					ItemID.ShipsWheel,
					ItemID.TreasureMap,
					ItemID.CompassRose,
					ItemID.CompassRose,
					ItemID.ShipInABottle,
					ItemID.Sextant
				};
				int loot = Main.rand.Next(lootTable.Length);
				player.QuickSpawnItem(lootTable[loot]);
			}

			if (Main.rand.Next(4) == 0)
			{
				int[] lootTable2 = {
					ItemID.GoldBar,
					ItemID.SilverBar,
					ItemID.TungstenBar,
					ItemID.PlatinumBar
				};
				int loot2 = Main.rand.Next(lootTable2.Length);
				int Booty = Main.rand.Next(6, 10);

				for (int j = 0; j < Booty; j++)
					player.QuickSpawnItem(lootTable2[loot2]);
			}

			if (Main.rand.Next(6) == 1)
			{
				int Gems = Main.rand.Next(5, 7);
				for (int I = 0; I < Gems; I++)
				{
					int[] lootTable3 = {
						ItemID.Ruby,
						ItemID.Emerald,
						ItemID.Topaz,
						ItemID.Amethyst,
						ItemID.Diamond,
						ItemID.Sapphire,
						ItemID.Amber
					};
					int loot3 = Main.rand.Next(lootTable3.Length);
					player.QuickSpawnItem(lootTable3[loot3]);
				}
			}

			if (Main.rand.Next(2) == 0)
			{
				int Coins = Main.rand.Next(1, 3);
				for (int K = 0; K < Coins; K++)
					player.QuickSpawnItem(ItemID.GoldCoin);
			}
			else
			{
				int cobweb = Main.rand.Next(8, 12);
				for (int K = 0; K < cobweb; K++)
					player.QuickSpawnItem(ItemID.Cobweb);
			}
			Item.NewItem(player.Center, Vector2.Zero, ModContent.ItemType<Items.Weapon.Thrown.ExplosiveRum.ExplosiveRum>(), Main.rand.Next(45, 70));

		}
	}

	public class SunkenTreasureTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
			TileObjectData.newTile.Direction = TileObjectDirection.PlaceLeft;
			TileObjectData.newTile.StyleWrapLimit = 2; //not really necessary but allows me to add more subtypes of chairs below the example chair texture
			TileObjectData.newTile.StyleMultiplier = 2; //same as above
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceRight; //allows me to place example chairs facing the same way as the player
			TileObjectData.addAlternate(1); //facing right will use the second texture style
			TileObjectData.addTile(Type);
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Sunken Treasure");
			DustType = -1;
			AddMapEntry(new Color(133, 106, 56), name);
		}

		public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY) => offsetY = 2;
		public override void KillMultiTile(int i, int j, int frameX, int frameY) => Item.NewItem(i * 16, j * 16, 48, 48, ModContent.ItemType<SunkenTreasure>());
	}
}