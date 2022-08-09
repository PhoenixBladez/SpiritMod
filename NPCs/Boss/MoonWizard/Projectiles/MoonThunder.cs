using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.GameContent;
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
            Projectile.timeLeft = 4000;
            Projectile.penetrate = 5;
            Projectile.tileCollide = false;
            Projectile.width = Projectile.height = 30;
        }
        float fadeOutNum = 1f;
        public override bool PreAI()
        {
            if (Projectile.ai[0] == 0)
            {
                MakeLightning();
            }
            Projectile.ai[0]++;
            fadeOutNum -= .04f;
            if (Projectile.ai[0] > 18)
            {
                Projectile.Kill();
            }
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.ai[0] <= 0f) return false;
            if (lines == null) return false;

            Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.LinearClamp, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            Rectangle screen = new Rectangle((int)Main.screenPosition.X, (int)Main.screenPosition.Y, Main.screenWidth, Main.screenHeight);

            for (int i = 0; i < lines.Count; i++)
            {
                List<Line> line = lines[i];
                for (int j = 0; j < line.Count; j++)
                {
                    DrawLine(Main.spriteBatch, screen, line[j]);
                }
            }

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            return false;
        }
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffID.Electrified, 180);
        }

        private void MakeLightning()
        {
            Vector2 start = new Vector2(Projectile.Center.X, 0);
            Point endTile = start.ToTileCoordinates();
            Tile tile;
            do
            {
                endTile.Y++;
                tile = Main.tile[endTile.X, endTile.Y];
            }
            while (tile == null || (Main.tileSolid[tile.TileType] && !Main.tile[endTile.X, endTile.Y].HasTile) || TileID.Sets.Platforms[tile.TileType]);

            Vector2 end = endTile.ToVector2() * 16f + Vector2.UnitX * 8f;
            Line line = new Line(start, end);


            lines = new List<List<Line>>();
            lines.Add(new List<Line>());
            lines[0].Add(line);

            for (int steps = 0; steps < 5; steps++)
            {
                for (int index = 0; index < lines[0].Count; index += 2)
                {
                    JitterLine(index, lines[0]);
                }
            }

            int sincePrev = 0;
            for (int k = 0; k < lines[0].Count - 6; k++)
            {
                if (sincePrev <= 0 && Main.rand.NextBool(7))
                {
                    lines.Add(new List<Line>());
                    int index = lines.Count - 1;

                    Vector2 strandStart = lines[0][k].start;
                    Vector2 strandEnd = strandStart + new Vector2(
                        Main.rand.NextBool(2) ? Main.rand.NextFloat(-300f, -40f) : Main.rand.NextFloat(40f, 300f),
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
            } while (fails < 20 && WorldGen.InWorld(tile.X, tile.Y) && Main.tile[tile.X, tile.Y] != null && Main.tile[tile.X, tile.Y].HasTile);

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
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            float rotation = delta.ToRotation();

            //draw main line
            spriteBatch.Draw(texture, line.start - Main.screenPosition, new Rectangle(16, 0, 2, 30), new Color(138, 235, 255) *fadeOutNum, rotation, new Vector2(0, 15f), new Vector2(length * 0.5f, 1.55f), SpriteEffects.None, 0f);
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