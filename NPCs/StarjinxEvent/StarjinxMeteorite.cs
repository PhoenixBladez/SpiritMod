using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.Graphics.Shaders;
using SpiritMod.NPCs.StarjinxEvent.Comets;
using System.Linq;

namespace SpiritMod.NPCs.StarjinxEvent
{
    [AutoloadBossHead]
    public class StarjinxMeteorite : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Starjinx");
            NPCID.Sets.TrailCacheLength[npc.type] = 20;
            NPCID.Sets.TrailingMode[npc.type] = 0;
        }
        public override void SetDefaults()
        {
            npc.aiStyle = -1;
            npc.lifeMax = 8000;
            npc.defense = 20;
            npc.value = 0f;
            aiType = 0;
            npc.dontTakeDamage = true;
            npc.knockBackResist = 0f;
            npc.width = 60;
            npc.height = 60;
            npc.damage = 0;
            npc.lavaImmune = false;
            npc.noTileCollide = true;
            npc.noGravity = true;
            npc.HitSound = SoundID.NPCHit7;
            npc.DeathSound = SoundID.Item89;
            npc.alpha = 255;
        }

        float sinCounter;
        bool spawnedComets = false;
        int[] comets = new int[5];

        public override void AI()
        {
            npc.velocity.X = 0;
            npc.velocity.Y = (float)Math.Sin(sinCounter) * 0.75f;
            sinCounter += 0.03f;

            if (npc.alpha > 0)
                npc.alpha -= 10;
            else
                npc.alpha = 0;

            if (!spawnedComets)
            {
                spawnedComets = true;
                for (int i = 0; i < 2; i++)
                {
                    int c = i == 1 ? 1 : -1;
                    float a = (int)npc.Center.X - 150 * c;
                    float b = (int)npc.Center.Y + (int)(npc.height / 2) + Main.rand.Next(-300, 100);
                    comets[i] = NPC.NewNPC((int)a, (int)b, ModContent.NPCType<SmallComet>(), 0, npc.whoAmI, Main.rand.NextFloat(10));
                }
                for (int i = 0; i < 2; i++)
                {
                    int c = i == 1 ? 1 : -1;
                    float a = (int)npc.Center.X - 300 * c;
                    float b = (int)npc.Center.Y + (int)(npc.height / 2) + Main.rand.Next(-300, 100);
                    comets[i + 2] = NPC.NewNPC((int)a, (int)b, ModContent.NPCType<MediumComet>(), 0, npc.whoAmI, Main.rand.NextFloat(10));
                }
                for (int i = 0; i < 1; i++)
                {
                    float a = (int)npc.Center.X - Main.rand.Next(Main.rand.Next(-700, -500), Main.rand.Next(500, 700));
                    float b = (int)npc.Center.Y + (int)(npc.height / 2) + Main.rand.Next(-300, 100);
                    comets[i + 4] = NPC.NewNPC((int)a, (int)b, ModContent.NPCType<LargeComet>(), 0, npc.whoAmI, Main.rand.NextFloat(10));
                }
            }
            if (spawnedComets && npc.dontTakeDamage)
            {
                for (int i = 0; i < comets.Length; i++)
                {
                    NPC comet = Main.npc[comets[i]];
                    int[] cometTypes = new int[] { ModContent.NPCType<LargeComet>(), ModContent.NPCType<SmallComet>(), ModContent.NPCType<MediumComet>() };
                    if (comet.active && comet.life > 0 && cometTypes.Contains(comet.type))
                        break;
                    if (i == comets.Length - 1)
                        npc.dontTakeDamage = false;
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            return false;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Vector2 center = new Vector2((float)(Main.npcTexture[npc.type].Width / 2), (float)(Main.npcTexture[npc.type].Height / Main.npcFrameCount[npc.type] / 2));
            float sineAdd = (float)Math.Sin(sinCounter * 1.33f);

            //Weird shader stuff, dont touch yuyutsu
            #region shader
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
            Vector4 yellow = new Vector4(1f, 0.89f, 0.63f, 1f);
            Vector4 pink = new Vector4(0.95f, 0.45f, 0.78f, 1f);
            Vector4 colorMod = Vector4.Lerp(yellow, pink, 0.5f - (sineAdd / 2));
            SpiritMod.StarjinxNoise.Parameters["distance"].SetValue(2.9f - (sineAdd / 10));
			SpiritMod.StarjinxNoise.Parameters["colorMod"].SetValue(colorMod);
			SpiritMod.StarjinxNoise.Parameters["noise"].SetValue(mod.GetTexture("Textures/noise"));
			SpiritMod.StarjinxNoise.Parameters["rotation"].SetValue(sinCounter / 5);
			SpiritMod.StarjinxNoise.Parameters["opacity2"].SetValue(0.3f - (sineAdd / 10));
			SpiritMod.StarjinxNoise.CurrentTechnique.Passes[0].Apply();
            Main.spriteBatch.Draw(mod.GetTexture("Effects/Masks/Extra_49"), (npc.Center - Main.screenPosition), null, npc.GetAlpha(Color.White), 0f, new Vector2(50, 50), 2.1f - (sineAdd / 9), SpriteEffects.None, 0f);

			SpiritMod.StarjinxNoise.Parameters["opacity2"].SetValue(1);
			SpiritMod.StarjinxNoise.Parameters["rotation"].SetValue((sinCounter + 3) / 4);
            colorMod = Vector4.Lerp(yellow, pink, 0.5f + (sineAdd / 2));
			SpiritMod.StarjinxNoise.Parameters["colorMod"].SetValue(colorMod);
			SpiritMod.StarjinxNoise.CurrentTechnique.Passes[0].Apply();

            Main.spriteBatch.Draw(mod.GetTexture("Effects/Masks/Extra_49"), (npc.Center - Main.screenPosition), null, npc.GetAlpha(Color.White), 0f, new Vector2(50, 50), 1.3f + (sineAdd / 7), SpriteEffects.None, 0f);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);
            #endregion


            Main.spriteBatch.Draw(mod.GetTexture("NPCs/StarjinxEvent/StarjinxMeteorite"), npc.Center - Main.screenPosition, null, Color.White, 0f, center, 1, SpriteEffects.None, 0f);

            /*Color pureWhite = new Color((int) sbyte.MaxValue, (int) sbyte.MaxValue, (int) sbyte.MaxValue, 0).MultiplyRGBA(Color.White);
			for (float i = 0; i < 6.28f; i+= 0.78f)
			{
				Vector2 offset = (i + npc.rotation).ToRotationVector2() * ((sineAdd + 2.5f) * 1.5f);
				Color transColor = npc.GetAlpha(pureWhite) * (0.4f - (sineAdd / 6));
				Main.spriteBatch.Draw(mod.GetTexture("NPCs/StarjinxEvent/StarjinxMeteoriteGlowOutline"), npc.Center - Main.screenPosition + offset, null, transColor, 0f, center, 1, SpriteEffects.None, 0f);
				Main.spriteBatch.Draw(mod.GetTexture("NPCs/StarjinxEvent/StarjinxMeteoriteGlow"), npc.Center - Main.screenPosition + (offset / 2), null, transColor, 0f, center, 1, SpriteEffects.None, 0f);
			}*/
            float num341 = 0f;
            float num340 = npc.height;
            float num108 = 4;
            float num107 = (float)Math.Cos((double)(Main.GlobalTime % 2.4f / 2.4f * 6.28318548f)) / 2f + 0.5f;
            float num106 = 0f;

            Texture2D texture2D6 = Main.npcTexture[npc.type];
            Vector2 vector15 = new Vector2((float)(Main.npcTexture[npc.type].Width / 2), (float)(Main.npcTexture[npc.type].Height / Main.npcFrameCount[npc.type] / 2));
            SpriteEffects spriteEffects3 = (npc.spriteDirection == 1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Vector2 vector33 = new Vector2(npc.Center.X, npc.Center.Y) - Main.screenPosition + new Vector2(0, npc.gfxOffY) - npc.velocity;
            Microsoft.Xna.Framework.Color color29 = new Microsoft.Xna.Framework.Color(127 - npc.alpha, 127 - npc.alpha, 127 - npc.alpha, 0).MultiplyRGBA(Microsoft.Xna.Framework.Color.White);
            Microsoft.Xna.Framework.Color color28 = color29;
            color28 = npc.GetAlpha(color28);
            color28 *= 1f - num107;

            Microsoft.Xna.Framework.Color color30 = color29;
            color30 = npc.GetAlpha(color28);
            color30 *= 1.18f - num107;

            Main.spriteBatch.Draw(mod.GetTexture("NPCs/StarjinxEvent/StarjinxMeteorite"), npc.Center - Main.screenPosition, null, npc.GetAlpha(Color.White), 0f, center, 1, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(mod.GetTexture("NPCs/StarjinxEvent/StarjinxMeteoriteGlowOutline"), npc.Center - Main.screenPosition, null, npc.GetAlpha(Color.White * .4f), 0f, center, 1, SpriteEffects.None, 0f);


            for (int num103 = 0; num103 < 6; num103++)
            {
                Vector2 vector29 = new Vector2(npc.Center.X, npc.Center.Y) + ((float)num103 / (float)num108 * 6.28318548f + npc.rotation + num106).ToRotationVector2() * (4f * num107 + 2f) - Main.screenPosition + new Vector2(0, npc.gfxOffY) - npc.velocity * (float)num103;
                Main.spriteBatch.Draw(mod.GetTexture("NPCs/StarjinxEvent/StarjinxMeteoriteGlow"), vector29, npc.frame, color28, npc.rotation, npc.frame.Size() / 2f, npc.scale, spriteEffects3, 0f);
                Main.spriteBatch.Draw(mod.GetTexture("NPCs/StarjinxEvent/StarjinxMeteoriteGlowOutline"), vector29, npc.frame, color28, npc.rotation, npc.frame.Size() / 2f, npc.scale, spriteEffects3, 0f);
            }
        }

        public override void NPCLoot()
        {
            StarjinxEventWorld.StarjinxActive = false;
            int stackstodrop = (Main.expertMode) ? 9 : 7;
            for (int i = 0; i < 7; i++)
            {
                Item.NewItem(new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height), mod.ItemType("Starjinx"), Main.rand.Next(3, 6));
            }
            Main.NewText("The asteroids return to their tranquil state...", 252, 150, 255);
        }

        public override bool CheckActive()
        {
            return false;
        }
    }
}