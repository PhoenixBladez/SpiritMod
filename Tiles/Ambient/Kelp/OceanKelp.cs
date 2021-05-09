using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Tiles.Ambient.Kelp
{
    internal class OceanKelp : ModTile
    {
        private static string TexName = "";

		private const int ClumpFrameOffset = 76; //so I don't have to magic number 76 constantly

        public override bool Autoload(ref string name, ref string texture)
        {
            TexName = texture; //Autoloads the texture :)
            return base.Autoload(ref name, ref texture);
        }

        public override void SetDefaults()
        {
            Main.tileSolid[Type] = false; //Non solid
            Main.tileMergeDirt[Type] = false; //Don't merge with dirt (or anything else ever)
            Main.tileBlockLight[Type] = false; //Don't block light
            Main.tileCut[Type] = true; //Cut by player projectiles and items
			Main.tileFrameImportant[Type] = true;

            AddMapEntry(new Color(21, 92, 19));
            dustType = DustID.Grass;
            soundType = SoundID.Grass;
        }

        public override void NumDust(int i, int j, bool fail, ref int num) => num = 3;

        public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak) //Sets and randomizes tile frame
        {
            Tile t = Framing.GetTileSafely(i, j); //this tile :)
			int realFrameX = t.frameX;
			int totalOffset = t.frameX / ClumpFrameOffset;
			if (realFrameX > ClumpFrameOffset) realFrameX -= ClumpFrameOffset;
			if (realFrameX > ClumpFrameOffset) realFrameX -= ClumpFrameOffset; //repeat twice to adjust properly

            if (realFrameX < 36 && t.frameY < 108) //Used for adjusting stem/top; does not include grown top or leafy stems
			{
                if (!Framing.GetTileSafely(i, j - 1).active()) //If top
                    t.frameX = (short)(18 + (ClumpFrameOffset * totalOffset));
                else //If stem
                    t.frameX = (short)(0 + (ClumpFrameOffset * totalOffset));
            }

            if (realFrameX == 0) //If stem
                t.frameY = (short)(Main.rand.Next(6) * 18); //Stem
            else if (realFrameX == 18)
            {
                if (t.frameY >= 108) //If grown top
                    t.frameY = (short)((Main.rand.Next(4) * 18) + 108);
                else //If ungrown top
                    t.frameY = (short)(Main.rand.Next(6) * 18);
            }
            else //Leafy stem
                t.frameY = (short)(18 * Main.rand.Next(8));

            return false;
        }

        public override void RandomUpdate(int i, int j) //Used for growing and "growing"
        {
            Tile t = Framing.GetTileSafely(i, j);

			int totalOffset = t.frameX / ClumpFrameOffset;
			int realFrameX = t.frameX;
			if (realFrameX > ClumpFrameOffset) realFrameX -= ClumpFrameOffset;
			if (realFrameX > ClumpFrameOffset) realFrameX -= ClumpFrameOffset; //repeat twice to adjust properly

			if (!Framing.GetTileSafely(i, j - 1).active() && Main.rand.Next(4) == 0 && t.liquid > 155 && t.frameX < 36 && t.frameY < 108) //Grows the kelp
                WorldGen.PlaceTile(i, j - 1, Type, true, false);

            if (realFrameX == 18 && t.frameY < 54 && t.liquid < 155) //Sprouts top
                t.frameY = (short)((Main.rand.Next(2) * 18) + 54);

			if (realFrameX == 0 && Main.rand.NextBool(3)) //"Places" side (just changes frame [we do a LOT of deception])
			{
				t.frameY = (short)(18 * Main.rand.Next(8));
				t.frameX = (short)(44 + (totalOffset * ClumpFrameOffset));
			}

			bool validGrowthBelow = Framing.GetTileSafely(i, j + 1).type != Type || (Framing.GetTileSafely(i, j + 1).type == Type && Framing.GetTileSafely(i, j + 1).frameX >= ClumpFrameOffset);
			if (realFrameX == 0 && t.frameX < ClumpFrameOffset * 2 && validGrowthBelow) //grows "clumps"
			{
				if (Framing.GetTileSafely(i, j + 1).frameX == ClumpFrameOffset && t.frameX < ClumpFrameOffset) //Grows clump 1
					t.frameX += ClumpFrameOffset;
				else if (Framing.GetTileSafely(i, j + 1).frameX >= ClumpFrameOffset * 2 && t.frameX >= ClumpFrameOffset * 2) //Grows clump 2
					t.frameX += ClumpFrameOffset;
			}
        }

        public override void NearbyEffects(int i, int j, bool closer) //Dust effects
        {
			int totalOffset = Framing.GetTileSafely(i, j).frameX / ClumpFrameOffset;

			if (Main.rand.Next(1000) <= 3 * totalOffset) //Spawns little bubbles
                Dust.NewDustPerfect(new Vector2(i * 16, j * 16) + new Vector2(2 + Main.rand.Next(12), Main.rand.Next(16)), 34, new Vector2(Main.rand.NextFloat(-0.08f, 0.08f), Main.rand.NextFloat(-0.2f, -0.02f)));

            if (!Framing.GetTileSafely(i, j + 1).active()) //KILL ME if there's no ground below me
                WorldGen.KillTile(i, j);
            if (Framing.GetTileSafely(i, j + 1).liquid < 155 && Framing.GetTileSafely(i, j).liquid < 155) //Kill me if I'm thirsty (aka kill if there's no water)
                WorldGen.KillTile(i, j);
        }

        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem) //Kills whole stack when...killed
        {
            if (Framing.GetTileSafely(i, j - 1).active() && Framing.GetTileSafely(i, j -1).type == Type && Framing.GetTileSafely(i, j).frameY < 108) //If ungrounded, kill me
                WorldGen.KillTile(i, j - 1, false, false, false);
        }

        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch) //Drawing woo
        {
            Tile t = Framing.GetTileSafely(i, j); //ME!
            Texture2D tile = ModContent.GetTexture(TexName); //Associated texture - loaded automatically

			int realFrameX = t.frameX;
			if (realFrameX > ClumpFrameOffset) realFrameX -= ClumpFrameOffset;
			if (realFrameX > ClumpFrameOffset) realFrameX -= ClumpFrameOffset; //repeat twice to adjust properly

			float xOff = GetOffset(i, j, t.frameX); //Sin offset.

			var source = new Rectangle(t.frameX, t.frameY, 16, 16); //Source rectangle used for drawing
            if (realFrameX == 44)
                source = new Rectangle(t.frameX, t.frameY, 26, 16);

            Vector2 TileOffset = Lighting.lightMode > 1 ? Vector2.Zero : Vector2.One * 12; //Draw offset
            Vector2 drawPos = ((new Vector2(i, j) + TileOffset) * 16) - Main.screenPosition; //Draw position

			int totalOffset = t.frameX / ClumpFrameOffset; //draws clumps

			bool[] hasClumps = new bool[] { GetKelpTile(i, j - 1) >= ClumpFrameOffset, GetKelpTile(i, j - 1) >= ClumpFrameOffset * 2 }; //Checks for if there's a grown clump above this clump

			for (int v = totalOffset; v >= 0; --v) 
			{
				Rectangle realSource = source;
				Color col = Color.White; //Lighting colour
				Vector2 realPos = drawPos;

				if (v == 0)
					xOff = GetOffset(i, j, realSource.X); //Grab offset properly
				else if (v == 1) 
				{
					realPos.X -= 8f + (i % 2); //Plain visual offset
					if (realSource.Y < 108) //"Randomzies" frame (it's consistent but we do a little more deception)
					{
						realSource.Y += 18;
						if (realSource.Y >= 108)
							realSource.Y -= 108;
					}
					col = new Color(169, 169, 169); //Makes it darker for depth

					if (!hasClumps[0]) //Adjust frame so it's a kelp top
					{
						realSource.X = 18;
						realSource.Width = 16;
						if (realSource.Y >= 108)
							realSource.Y -= 36;
					}

					xOff = GetOffset(i, j, realSource.X, -0.75f + (i % 4 * 0.4f));
				}
				else if (v == 2) //Repeat lines 150-167 but slight offsets
				{
					realPos.X += 6f + (i % 2);
					if (realSource.Y < 108) 
					{
						realSource.Y -= 18;
						if (realSource.Y < 0)
							realSource.Y += 108;
					}
					col = new Color(140, 140, 140);

					if (!hasClumps[1]) //Adjust frame to be a kelp top
					{
						realSource.X = 18;
						realSource.Width = 16;
						if (realSource.Y >= 108)
							realSource.Y -= 36;
					}

					xOff = GetOffset(i, j, realSource.X, -1.55f + (i % 4 * 0.4f));
				}

				col = Lighting.GetColor(i, j, col); 
				spriteBatch.Draw(tile, realPos - new Vector2(xOff, 0), realSource, new Color(col.R, col.G, col.B, 255), 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
			}
            return false; //don't draw the BORING, STUPID vanilla tile
        }

		public float GetOffset(int i, int j, int frameX, float sOffset = 0f)
		{
			float sin = (float)Math.Sin((Main.time + (i * 24) + (j * 19)) * (0.04f * (!Lighting.NotRetro ? 0f : 1)) + sOffset) * 2.3f;
			if (Framing.GetTileSafely(i, j + 1).type != Type) //Adjusts the sine wave offset to make it look nicer when closer to ground
				sin *= 0.25f;
			else if (Framing.GetTileSafely(i, j + 2).type != Type)
				sin *= 0.5f;
			else if (Framing.GetTileSafely(i, j + 3).type != Type)
				sin *= 0.75f;

			if (frameX > ClumpFrameOffset) frameX -= ClumpFrameOffset;
			if (frameX > ClumpFrameOffset) frameX -= ClumpFrameOffset; //repeat twice to adjust properly

			if (frameX == 44)
				sin += 4; //Adjusts since the source is bigger here

			return sin;
		}

		public int GetKelpTile(int i, int j)
		{
			if (Framing.GetTileSafely(i, j).active() && Framing.GetTileSafely(i, j).type == Type)
				return Framing.GetTileSafely(i, j).frameX;
			return -1;
		}
	}
}