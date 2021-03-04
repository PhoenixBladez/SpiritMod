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

namespace SpiritMod.ChargeMeter
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

            layers.Insert(body - 1, new PlayerLayer(mod.Name, "Body",
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
                    SpriteEffects effect;
                    if ((double)drawPlayer.gravDir == 1.0)
                    {
                        if (drawPlayer.direction == 1)
                        {
                            spriteEffects = SpriteEffects.None;
                            effect = SpriteEffects.None;
                        }
                        else
                        {
                            spriteEffects = SpriteEffects.FlipHorizontally;
                            effect = SpriteEffects.FlipHorizontally;
                        }
                    }
                    else
                    {
                        if (drawPlayer.direction == 1)
                        {
                            spriteEffects = SpriteEffects.FlipVertically;
                            effect = SpriteEffects.FlipVertically;
                        }
                        else
                        {
                            spriteEffects = SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically;
                            effect = SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically;
                        }
                    }

                    Texture2D ChargeMeter = ModContent.GetTexture(chargeMeter.meterTexture);
                    Texture2D ChargeBar = ModContent.GetTexture(chargeMeter.barTexture);
                    int num23 = !drawPlayer.setForbiddenCooldownLocked ? 1 : 0;
                    int num24 = (int)((double)((float)((double)drawPlayer.miscCounter / 300.0 * 6.28318548202515)).ToRotationVector2().Y * 4.0);
                    float num25 = ((float)((double)drawPlayer.miscCounter / 75.0 * 6.28318548202515)).ToRotationVector2().X * 4f;
                    Vector2 position = new Vector2((float)(int)((double)Position.X - (double)Main.screenPosition.X - (double)(drawPlayer.bodyFrame.Width / 2) + (double)(drawPlayer.width / 2)), (float)(int)((double)Position.Y - (double)Main.screenPosition.Y + (double)drawPlayer.height - (double)drawPlayer.bodyFrame.Height + 4.0)) + drawPlayer.bodyPosition + new Vector2((float)(drawPlayer.bodyFrame.Width / 2), (float)(drawPlayer.bodyFrame.Height / 2)) + new Vector2((float)(-drawPlayer.direction), (float)(num24 - 60));
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
