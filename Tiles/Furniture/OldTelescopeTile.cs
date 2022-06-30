
using Microsoft.Xna.Framework;
using SpiritMod.Items.Placeable.Furniture;
using Terraria;
using Terraria.Chat;
using Terraria.Enums;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.ID;
using Terraria.Localization;

namespace SpiritMod.Tiles.Furniture
{
	public class OldTelescopeTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			AnimationFrameHeight = 54;
			TileObjectData.newTile.UsesCustomCanPlace = true;
			TileObjectData.newTile.Width = 3;
			TileObjectData.newTile.Height = 3;
			TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16 };
			TileObjectData.newTile.CoordinateWidth = 16;
			TileObjectData.newTile.CoordinatePadding = 2;
			TileObjectData.newTile.Origin = new Point16(0, 1);
			TileObjectData.newTile.Direction = TileObjectDirection.PlaceLeft;
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.Table | AnchorType.SolidTile | AnchorType.SolidWithTop, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.StyleWrapLimit = 2; //not really necessary but allows me to add more subtypes of chairs below the example chair texture
			TileObjectData.newTile.StyleMultiplier = 2; //same as above
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceRight; //allows me to place example chairs facing the same way as the player
			TileObjectData.addAlternate(1); //facing right will use the second texture style
			TileObjectData.addTile(Type);
			DustType = -1;
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Old Telescope");
			AddMapEntry(new Color(200, 200, 200), name);
		}
		public override void AnimateTile(ref int frame, ref int frameCounter)
		{
			frameCounter++;
			if (frameCounter >= 10) //replace 10 with duration of frame in ticks
			{
				frameCounter = 0;
				frame++;
				frame %= 14;
			}
		}
		public override void KillMultiTile(int i, int j, int frameX, int frameY) => Item.NewItem(new Terraria.DataStructures.EntitySource_TileBreak(i, j), i * 16, j * 16, 64, 48, ModContent.ItemType<OldTelescope>());

		public override bool RightClick(int i, int j)
		{
			if (Main.netMode == NetmodeID.SinglePlayer)
				Main.NewText(GetMoonPhase(), Color.AntiqueWhite);
			else
				ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(GetMoonPhase()), Color.AntiqueWhite);
			return true;
		}

		public string GetMoonPhase()
		{
			switch (Main.moonPhase)
			{
				case 0:
					return Language.GetTextValue("GameUI.FullMoon");
				case 1:
					return Language.GetTextValue("GameUI.WaningGibbous");
				case 2:
					return Language.GetTextValue("GameUI.ThirdQuarter");
				case 3:
					return Language.GetTextValue("GameUI.ThirdQuarter");
				case 4:
					return Language.GetTextValue("GameUI.NewMoon");
				case 5:
					return Language.GetTextValue("GameUI.WaxingCrescent");
				case 6:
					return Language.GetTextValue("GameUI.FirstQuarter");
				default:
					return Language.GetTextValue("GameUI.WaxingGibbous");
			}
		}
	}
}