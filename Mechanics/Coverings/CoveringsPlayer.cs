using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;

using Terraria.World.Generation;

namespace SpiritMod.Mechanics.Coverings
{
    public class CoveringsPlayer : ModPlayer
    {
        public override void UpdateBiomeVisuals()
        {
            if (!Main.dedServ)
			{
				Overlays.Scene.Activate("SpiritMod:CoveringsEntities");
				Overlays.Scene.Activate("SpiritMod:CoveringsFGW");
				//Overlays.Scene.Activate("SpiritMod:CoveringsBGW");
			}

			// try adding snow coverings
			Point playerTile = player.Center.ToTileCoordinates();
            if (player.ZoneSnow && playerTile.Y < Main.worldSurface + 40)
            {
                bool blizzard = player.ZoneRain;
                int snowAttempts = blizzard ? 50 : 5; // change the values here to modify speed
                for (int i = 0; i < snowAttempts; i++) TryAddSnowCovering(playerTile);
            }

            /*
            var action = new Actions.Custom((x, y, args) =>
            {
                if (!WorldGen.SolidOrSlopedTile(Framing.GetTileSafely(x, y))) return false;

                int orientation = SpiritMod.Coverings.FullCoverOrientation(x, y);
                if (orientation > 0)
                {
                    SpiritMod.Coverings.SetData(x, y, Snow.MakeVariation(Main.rand.Next(3), 3), orientation, SpiritMod.Coverings.GetCoveringID<Snow>());
                    return true;
                }
                return false;
            });

            if (Main.mouseRight)
            {
                WorldUtils.Gen(new Point(Player.tileTargetX, Player.tileTargetY), new Shapes.Circle(12), action);
            }
            */
        }

        private void TryAddSnowCovering(Point aroundTile)
        {
            // pick a random tile on screen ish
            int randomX = aroundTile.X + Main.rand.Next(-80, 81);
            int randomY = aroundTile.Y + Main.rand.Next(-40, 41);

            // if none of our neighbours are air, return
            if (WorldGen.SolidOrSlopedTile(randomX - 1, randomY) && 
                WorldGen.SolidOrSlopedTile(randomX + 1, randomY) &&
                WorldGen.SolidOrSlopedTile(randomX, randomY - 1) &&
                WorldGen.SolidOrSlopedTile(randomX, randomY + 1)) return;

            // make sure we're not too high up
            if (randomY > 5)
            {
                int emptyUp = EmptyInDirection(randomX, randomY, new Point(0, -1), 60);
                int emptyHor = EmptyInDirection(randomX, randomY, new Point(-Math.Sign(Main.windSpeed), 0));

                bool coverUp = emptyUp >= 55;
                bool coverSide = emptyHor >= 15;
                if (coverUp || coverSide) // if either is high enough
                {
                    int snowCoveringID = SpiritMod.Coverings.GetCoveringID<Snow>();
                    CoverData data = SpiritMod.Coverings.GetData(randomX, randomY);
                    int alpha = 0;
                    int frameVariation = Main.rand.Next(3);
                    if (data.Orientation > 0)
                    {
                        alpha = Math.Min(3, Snow.GetAlphaFromVariation(data.Variation) + 1);
                        frameVariation = data.Variation % 4;
                    }
                    // set snow up
                    SpiritMod.Coverings.SetData(randomX, randomY, Snow.MakeVariation(frameVariation, alpha), SpiritMod.Coverings.GetOrientationFor(coverUp, Main.windSpeed > 0 && coverSide, Main.windSpeed < 0 && coverSide, false), snowCoveringID);
                }
            }
        }

        private int EmptyInDirection(int startX, int startY, Point offset, int max = 50)
        {
            int x = startX + offset.X;
            int y = startY + offset.Y;
            int empty = 0;
            while (WorldGen.InWorld(x, y) && !WorldGen.SolidOrSlopedTile(x, y) && Framing.GetTileSafely(x, y).wall == WallID.None)
            {
                empty++;
                x += offset.X;
                y += offset.Y;
                if (empty >= max) return empty;
            }
            return empty;
        }
    }
}
