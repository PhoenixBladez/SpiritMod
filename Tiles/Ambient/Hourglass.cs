using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework;
namespace SpiritMod.Tiles.Ambient
{
	public class Hourglass : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
			TileObjectData.newTile.Height = 4;
			TileObjectData.newTile.Width = 4;
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16,
				16,
				16,
				16
			};
			TileObjectData.newTile.AnchorBottom = default(AnchorData);
			TileObjectData.newTile.AnchorTop = default(AnchorData);
			TileObjectData.newTile.AnchorWall = true;
			TileObjectData.addTile(Type);
			Terraria.ID.TileID.Sets.DisableSmartCursor[Type] = true;
			DustType -= 1;
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Hourglass");
			AddMapEntry(new Color(150, 150, 150), name);
		}

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}
        public override bool RightClick(int x, int y)
        {
            string text = "AM";
            //Get current weird time
            double time = Main.time;
            if (!Main.dayTime)
            {
                //if it's night add this number
                time += 54000.0;
            }
            //Divide by seconds in a day * 24
            time = time / 86400.0 * 24.0;
            //Dunno why we're taking 19.5. Something about hour formatting
            time = time - 7.5 - 12.0;
            //Format in readable time
            if (time < 0.0)
            {
                time += 24.0;
            }
            if (time >= 12.0)
            {
                text = "PM";
            }
            int intTime = (int)time;
            //Get the decimal points of time.
            double deltaTime = time - intTime;
            //multiply them by 60. Minutes, probably
            deltaTime = (int)(deltaTime * 60.0);
            //This could easily be replaced by deltaTime.ToString()
            string text2 = string.Concat(deltaTime);
            if (deltaTime < 10.0)
            {
                //if deltaTime is eg "1" (which would cause time to display as HH:M instead of HH:MM)
                text2 = "0" + text2;
            }
            if (intTime > 12)
            {
                //This is for AM/PM time rather than 24hour time
                intTime -= 12;
            }
            if (intTime == 0)
            {
                //0AM = 12AM
                intTime = 12;
            }
            //Whack it all together to get a HH:MM format
            var newText = string.Concat("Time: ", intTime, ":", text2, " ", text);
            Main.NewText(newText, 255, 240, 20);
            return true;
        }

        public override void NearbyEffects(int i, int j, bool closer)
        {
            if (closer)
            {
                Main.SceneMetrics.HasClock = true;
            }
        }
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(new Terraria.DataStructures.EntitySource_TileBreak(i, j), i * 16, j * 16, 32, 48, ModContent.ItemType<Items.Placeable.Furniture.HourglassItem>());
		}
	}
}