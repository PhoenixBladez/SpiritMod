using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using System.Diagnostics;
using System.Runtime.CompilerServices;

using Terraria;
using Terraria.ModLoader;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpiritMod.Utilities
{
    public static class HookManager
    {
        internal static void Init()
        {
            On.Terraria.Main.DrawRain += Main_DrawRain;
            On.Terraria.Rain.NewRain += Rain_NewRain;
            On.Terraria.Rain.Update += Rain_Update;
            On.Terraria.Main.DrawBG += Main_DrawBG;
        }

        private static void Main_DrawBG(On.Terraria.Main.orig_DrawBG orig, Main self)
        {
            //FIX RAIN HACK
            if (Main.gameMenu)
            {
                for (int i = 0; i < Main.rain.Length; i++)
                {
                    if (Main.rain[i].type >= 6)
                    {
                        Main.rain[i].type = (byte)(Main.rain[i].type % 3);
                    }
                }
            }

            orig(self);
        }

        private static void Rain_Update(On.Terraria.Rain.orig_Update orig, Rain self)
        {
            self.position += self.velocity;
            if (Collision.SolidCollision(self.position, 2, 2) || self.position.Y > Main.screenPosition.Y + (float)Main.screenHeight + 100f || Collision.WetCollision(self.position, 2, 2))
            {
                self.active = false;
                if (Main.rand.Next(100) < Main.gfxQuality * 100f)
                {
                    Color color;
                    int dustType = SpiritMod.GetRainDustType(self.type, out color);

                    Vector2 vector2 = self.position - self.velocity;

                    Dust dust = Main.dust[Dust.NewDust(vector2, 2, 2, dustType, 0f, 0f, 0, color, 1f)];
                    dust.position.X -= 2f;
                    dust.alpha = 38;
                    dust.velocity *= 0.1f;
                    dust.velocity += (-self.velocity * 0.025f);
                    dust.scale = 0.75f;
                }
            }
        }

        private static int Rain_NewRain(On.Terraria.Rain.orig_NewRain orig, Vector2 Position, Vector2 Velocity)
        {
            int id = orig(Position, Velocity);
            Rain rain = Main.rain[id];
            Player player = Main.LocalPlayer;

            if (Main.bloodMoon)
            {
                rain.type = Main.rand.Next(2) == 0 ? (byte)Main.rand.Next(3, 6) : (byte)Main.rand.Next(21, 24);
            }
            else if (player.ZoneHoly)
            {
                rain.type = (byte)Main.rand.Next(12, 15);
            }
            else if (player.ZoneCorrupt)
            {
                rain.type = (byte)Main.rand.Next(15, 18);
            }
            else if (player.ZoneCrimson)
            {
                rain.type = (byte)Main.rand.Next(18, 21);
            }
            return id;
        }

        public static void Main_DrawRain(On.Terraria.Main.orig_DrawRain orig, Main self)
        {
            bool isActive = self.IsActive;
            Rectangle[] rectangle = new Rectangle[24];
            for (int i = 0; i < rectangle.Length; i++)
            {
                rectangle[i] = new Rectangle(i << 2, 0, 2, 40);
            }
            for (int j = 0; j < Main.maxRain; j++)
            {
                Rain rain = Main.rain[j];
                if (rain.active)
                {
                    Texture2D texture = rain.type < 6 ? Main.rainTexture : SpiritMod.RainTexture;
                    Rectangle source = rectangle[rain.type < 6 ? rain.type : rain.type - 6];
                    Main.spriteBatch.Draw(texture, rain.position - Main.screenPosition, source, Lighting.GetColor((int)(rain.position.X + 4f) >> 4, (int)(rain.position.Y + 4f) >> 4) * 0.85f, rain.rotation, Vector2.Zero, rain.scale, SpriteEffects.None, 0f);

                    if (isActive)
                    {
                        rain.Update();
                    }
                }
            }
            TimeLogger.DetailedDrawTime(23);
        }
    }
}
