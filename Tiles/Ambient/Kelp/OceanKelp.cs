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

            AddMapEntry(new Color(21, 92, 19));
            dustType = DustID.Grass;
            soundType = SoundID.Grass;
        }

        public override void NumDust(int i, int j, bool fail, ref int num) => num = 3;

        public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak) //Sets and randomizes tile frame
        {
            Tile t = Framing.GetTileSafely(i, j); //this tile :)
            if (t.frameX < 36 && t.frameY < 108) //Used for adjusting stem/top; does not include grown top or leafy stems
            {
                if (!Framing.GetTileSafely(i, j - 1).active()) //If top
                    t.frameX = 18;
                else //If stem
                    t.frameX = 0;
            }

            if (t.frameX == 0) //If stem
                t.frameY = (short)(Main.rand.Next(6) * 18); //Stem
            else if (t.frameX == 18)
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
            if (!Framing.GetTileSafely(i, j - 1).active() && Main.rand.Next(4) == 0 && t.liquid > 155 && t.frameX < 36 && t.frameY < 108) //Grows the kelp
                WorldGen.PlaceTile(i, j - 1, Type, true, false);

            if (t.frameX != 0 && t.frameY < 54 && Main.rand.Next(1) == 0 && t.liquid < 155) //Sprouts top
                t.frameY = (short)((Main.rand.Next(2) * 18) + 54);

            if (t.frameX == 0)
            {
                //if (Main.rand.NextBool(1))        Originally had a random chance in mind but it's pretty fair to just have a 100% per RandomUpdate I think
                //{
                    bool[] sides = new bool[2] { Framing.GetTileSafely(i - 1, j).active(), Framing.GetTileSafely(i + 1, j).active() };

                    if (!sides[0] || !sides[1]) //"Places" side (just changes frame [we do a LOT of deception])
                    {
                        t.frameY = (short)(18 * Main.rand.Next(8));
                        t.frameX = 44;
                    }
                //}
            }
        }

        public override void NearbyEffects(int i, int j, bool closer) //Dust effects
        {
            if (Main.rand.Next(1000) <= 5) //Spawns little bubbles
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
            Color col = Lighting.GetColor(i, j); //Lighting colour

            float xOff = (float)Math.Sin((Main.time + (i * 24) + (j * 19)) * (0.04f * (!Lighting.NotRetro ? 0f : 1))) * 2.3f; //Sin offset. Really ugly line I'm sorry
            if (Framing.GetTileSafely(i, j + 1).type != Type) //Adjusts the sine wave offset to make it look nicer when closer to ground
                xOff *= 0.25f;
            else if (Framing.GetTileSafely(i, j + 2).type != Type)
                xOff *= 0.5f;
            else if (Framing.GetTileSafely(i, j + 3).type != Type)
                xOff *= 0.75f;

            Rectangle source = new Rectangle(t.frameX, t.frameY, 16, 16); //Source rectangle used for drawing
            if (t.frameX == 44)
            {
                source = new Rectangle(t.frameX, t.frameY, 36, 16);
                xOff += 4; //Adjusts since the source is bigger here
            }

            Vector2 TileOffset = Lighting.lightMode > 1 ? Vector2.Zero : Vector2.One * 12; //Draw offset
            Vector2 drawPos = ((new Vector2(i, j) + TileOffset) * 16) - Main.screenPosition; //Draw position
            spriteBatch.Draw(tile, drawPos - new Vector2(xOff, 0), source, new Color(col.R, col.G, col.B, 255), 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
            return false; //don't draw the BORING, STUPID vanilla tile
        }
    }
}