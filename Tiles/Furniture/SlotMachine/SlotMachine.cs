using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria;
using Terraria.Enums;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SpiritMod.Tiles.Furniture.SlotMachine
{
	public class SlotMachine : ModItem
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Slot Machine");

		public override void SetDefaults()
		{
			item.width = 36;
			item.height = 34;
			item.value = Item.buyPrice(0, 50, 0, 0);
			item.maxStack = 99;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = 10;
			item.useAnimation = 15;
			item.useTurn = true;
			item.autoReuse = true;
			item.consumable = true;
			item.createTile = ModContent.TileType<SlotMachineTile>();
		}
	}

	public class SlotMachineTile : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;

			TileObjectData.newTile.UsesCustomCanPlace = true;
			TileObjectData.newTile.Width = 3;
			TileObjectData.newTile.Height = 3;
			TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16 };
			TileObjectData.newTile.CoordinateWidth = 16;
			TileObjectData.newTile.CoordinatePadding = 2;
			TileObjectData.newTile.Origin = new Point16(0, 1);
			TileObjectData.newTile.Direction = TileObjectDirection.PlaceLeft;
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.Table | AnchorType.SolidTile | AnchorType.SolidWithTop, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.StyleWrapLimit = 2;
			TileObjectData.newTile.StyleMultiplier = 2;
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceRight;
			TileObjectData.addAlternate(1);
			TileObjectData.addTile(Type);

			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Slot Machine");
			AddMapEntry(new Color(200, 200, 200), name);

			dustType = -1;
			animationFrameHeight = 54;
		}

		public override void AnimateTile(ref int frame, ref int frameCounter)
		{
			frameCounter++;
			if (frameCounter >= 10) //replace 10 with duration of frame in ticks
			{
				frameCounter = 0;
				frame++;
				frame %= 2;
			}
		}

		public override bool NewRightClick(int i, int j)
		{
			float minLength = float.MaxValue;
			Player nearestPlayer = Main.player[Main.myPlayer];

			foreach (Player player in Main.player)
			{
				Vector2 dist = player.Center - new Vector2(i * 16, j * 16);
				if (dist.LengthSquared() < minLength)
				{
					nearestPlayer = player;
					minLength = dist.LengthSquared();
				}
			}

			if (Main.player[Main.myPlayer] == nearestPlayer)
			{
				if (ModContent.GetInstance<SpiritMod>().SlotUserInterface.CurrentState is UISlotState currentSlotState && currentSlotState != null)
					return false;
				else
				{
					Main.PlaySound(SoundID.MenuOpen);
					ModContent.GetInstance<SpiritMod>().SlotUserInterface.SetState(new UISlotState(i, j, nearestPlayer));
				}
			}
			return true;
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY) => Item.NewItem(i * 16, j * 16, 64, 48, ModContent.ItemType<SlotMachine>());
	}
}