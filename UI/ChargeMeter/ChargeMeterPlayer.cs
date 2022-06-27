using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.GameInput;
using System;
using System.Collections.Generic;
using System.IO;

namespace SpiritMod.UI.ChargeMeter
{
    public class ChargeMeterPlayer : ModPlayer
    {
        public struct ChargeMeter
        {
            public bool drawMeter;
            public float charge;
            public string meterTexture;
            public string barTexture;
        }
        public ChargeMeter chargeMeter;
        public override void ResetEffects()
        {
            chargeMeter.drawMeter = false;
            chargeMeter.charge = 0;
            chargeMeter.meterTexture = "SpiritMod/ChargeMeter/ChargeMeter";
            chargeMeter.barTexture = "SpiritMod/ChargeMeter/ChargeBar";
        }

        public override void ModifyDrawLayers(List<PlayerLayer> layers)
        {
            int body = layers.FindIndex(l => l == PlayerLayer.MiscEffectsFront);
            if (body < 0)
                return;

            layers.Insert(body - 1, new PlayerLayer(Mod.Name, "Body",
                delegate (PlayerDrawInfo drawInfo)
            {
                if (drawInfo.shadow != 0f)
                {
                    return;
                }

                if (chargeMeter.drawMeter)
                {
                    Player drawPlayer = drawInfo.drawPlayer;
                    Mod mod = ModLoader.GetMod("SpiritMod");
                    ChargeMeterPlayer modPlayer = drawPlayer.GetModPlayer<ChargeMeterPlayer>();
                    Vector2 Position = drawPlayer.position;
                    DrawData drawData = new DrawData();
                    SpriteEffects spriteEffects;
                    if (drawPlayer.gravDir == 1.0)
                    {
                        if (drawPlayer.direction == 1)
                            spriteEffects = SpriteEffects.None;
                        else
                            spriteEffects = SpriteEffects.FlipHorizontally;
                    }
                    else
                    {
                        if (drawPlayer.direction == 1)
                            spriteEffects = SpriteEffects.FlipVertically;
                        else
                            spriteEffects = SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically;
                    }

                    Texture2D ChargeMeter = ModContent.Request<Texture2D>(chargeMeter.meterTexture);
                    Texture2D ChargeBar = ModContent.Request<Texture2D>(chargeMeter.barTexture);
                    int num23 = !drawPlayer.setForbiddenCooldownLocked ? 1 : 0;
                    int num24 = (int)((drawPlayer.miscCounter / 300.0f * MathHelper.TwoPi).ToRotationVector2().Y * 4.0);
                    float num25 = ((float)(drawPlayer.miscCounter / 75.0 * 6.28318548202515)).ToRotationVector2().X * 4f;
                    Vector2 position = new Vector2((int)(Position.X - Main.screenPosition.X - (drawPlayer.bodyFrame.Width / 2) + (drawPlayer.width / 2)), (int)(Position.Y - Main.screenPosition.Y + drawPlayer.height - drawPlayer.bodyFrame.Height + 4.0)) + drawPlayer.bodyPosition + new Vector2(drawPlayer.bodyFrame.Width / 2, drawPlayer.bodyFrame.Height / 2) + new Vector2((-drawPlayer.direction), (num24 - 60));
                    drawData = new DrawData(ChargeMeter, position, new Microsoft.Xna.Framework.Rectangle?(), Color.White, drawPlayer.bodyRotation, ChargeMeter.Size() / 2f, 1f, spriteEffects, 0);
                    Main.playerDrawData.Add(drawData);

                    Rectangle rect = new Rectangle(0, 0, (int)(ChargeBar.Width * chargeMeter.charge), ChargeBar.Height);
                     drawData = new DrawData(ChargeBar, position + new Vector2(6,0), rect, Color.White, drawPlayer.bodyRotation, ChargeMeter.Size() / 2f, 1f, SpriteEffects.None, 0);
                    Main.playerDrawData.Add(drawData);
                }
            }));
        }
    }
}
