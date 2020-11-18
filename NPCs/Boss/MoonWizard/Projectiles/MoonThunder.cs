using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.MoonWizard.Projectiles
{
    public class MoonThunder : ModProjectile
    {
        private static readonly float MIN_MAX_ANGLE = 0.12f;

        private List<List<Line>> lines;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lightning");
        }

        public override void SetDefaults()
        {
            projectile.timeLeft = 4000;
            projectile.penetrate = 5;
            projectile.tileCollide = false;
            projectile.width = projectile.height = 40;
        }
        float fadeOutNum = 1f;
        public override bool PreAI()
        {
            if (projectile.ai[0] % 5 == 0)
            {
                MakeLightning();
            }
            projectile.ai[0]++;
            fadeOutNum -= .08f;
            if (projectile.ai[0] == 6)
            {
                SpiritMod.tremorTime = 3;
                for (int i = 0; i < 10; i++)
                {
                    int num = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y + 55), projectile.width, projectile.height, 226, 0f, -2f, 0, default(Color), 2f);
                    Main.dust[num].noGravity = true;
                    Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
                    Main.dust[num].position.Y += Main.rand.Next(11, 51) * .05f - 1.5f;
                    Main.dust[num].scale *= .25f;
                    if (Main.dust[num].position != projectile.Center)
                        Main.dust[num].velocity = projectile.DirectionTo(Main.dust[num].position) * -3f;
                }
            }
            if (projectile.ai[0] > 12)
            {
                projectile.Kill();
            }
            return false;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if (projectile.ai[0] <= 0f) return false;
            if (lines == null) return false;

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.LinearClamp, DepthStencilState.None, Main.instance.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            Rectangle screen = new Rectangle((int)Main.screenPosition.X, (int)Main.screenPosition.Y, Main.screenWidth, Main.screenHeight);

            for (int i = 0; i < lines.Count; i++)
            {
                List<Line> line = lines[i];
                for (int j = 0; j < line.Count; j++)
                {
                    DrawLine(spriteBatch, screen, line[j]);
                }
            }

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.instance.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            return false;
        }
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffID.Electrified, 180);
        }

        private void MakeLightning()
        {
            Vector2 start = new Vector2(projectile.Center.X, 0);
            Point endTile = start.ToTileCoordinates();
            Tile tile;
            do
            {
                endTile.Y++;
                tile = Main.tile[endTile.X, endTile.Y];
            }
            while (tile == null || (Main.tileSolid[tile.type] && !Main.tile[endTile.X, endTile.Y].active()));

            Vector2 end = endTile.ToVector2() * 16f + Vector2.UnitX * 8f;
            Line line = new Line(start, end);


            lines = new List<List<Line>>();
            lines.Add(new List<Line>());
            lines[0].Add(line);

            for (int steps = 0; steps < 6; steps++)
            {
                for (int index = 0; index < lines[0].Count; index += 2)
                {
                    JitterLine(index, lines[0]);
                }
            }

            int sincePrev = 0;
            for (int k = 0; k < lines[0].Count - 6; k++)
            {
                if (sincePrev <= 0 && Main.rand.Next(7) == 0)
                {
                    lines.Add(new List<Line>());
                    int index = lines.Count - 1;

                    Vector2 strandStart = lines[0][k].start;
                    Vector2 strandEnd = strandStart + new Vector2(
                        Main.rand.Next(2) == 0 ? Main.rand.NextFloat(-300f, -40f) : Main.rand.NextFloat(40f, 300f),
                        Main.rand.NextFloat(50f, 300f));
                    Line strand = new Line(strandStart, strandEnd);
                    lines[index].Add(strand);

                    for (int steps = 0; steps < 3; steps++)
                    {
                        for (int index2 = 0; index2 < lines[index].Count; index2 += 2)
                        {
                            JitterLine(index2, lines[index]);
                        }
                    }

                    sincePrev = Main.rand.Next(3, 7);
                }
                sincePrev--;
            }
        }

        private void JitterLine(int index, List<Line> lines)
        {
            Line line = lines[index];
            lines.RemoveAt(index);

            Vector2 midPoint;
            Point tile;
            int fails = 0;
            do
            {
                midPoint = line.start + ((line.end - line.start) * 0.5f).RotatedBy(Main.rand.NextFloat(-MIN_MAX_ANGLE, MIN_MAX_ANGLE));
                tile = midPoint.ToTileCoordinates();
                fails++;
            } while (fails < 20 && WorldGen.InWorld(tile.X, tile.Y) && Main.tile[tile.X, tile.Y] != null && Main.tile[tile.X, tile.Y].active());

            Line newLine1 = new Line(line.start, midPoint);
            Line newLine2 = new Line(midPoint, line.end);

            lines.Insert(index, newLine2);
            lines.Insert(index, newLine1);
        }

        private void DrawLine(SpriteBatch spriteBatch, Rectangle screen, Line line)
        {
            //if (!screen.Contains(line.start) && !screen.Contains(line.end)) return;

            Vector2 delta = line.end - line.start;
            Vector2 normalised = Vector2.Normalize(delta);
            float length = delta.Length();
            Texture2D texture = Main.projectileTexture[projectile.type];
            float rotation = delta.ToRotation();

            //draw main line
            spriteBatch.Draw(texture, line.start - Main.screenPosition, new Rectangle(16, 0, 2, 30), new Color(138, 235, 255) *fadeOutNum, rotation, new Vector2(0, 15f), new Vector2(length * 0.5f, 1.35f), SpriteEffects.None, 0f);
            //draw ends
            spriteBatch.Draw(texture, line.start - normalised * 16f - Main.screenPosition, new Rectangle(0, 0, 16, 30), new Color(138, 235, 255) * .5f * fadeOutNum, rotation, new Vector2(0, 15f), 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(texture, line.end - Main.screenPosition, new Rectangle(18, 0, 16, 30), Color.White * .85f * fadeOutNum, rotation, new Vector2(0, 15f), 1f, SpriteEffects.None, 0f);
        }

        private struct Line
        {
            public Vector2 start;
            public Vector2 end;

            public Line(Vector2 start, Vector2 end)
            {
                this.start = start;
                this.end = end;
            }
        }
    }
}