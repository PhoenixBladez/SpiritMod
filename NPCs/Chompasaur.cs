/*using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpiritMod.NPCs
{
	public class Chompasaur : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sand Biter");
			Main.npcFrameCount[npc.type] = 4;
            NPCID.Sets.TrailCacheLength[npc.type] = 3;
            NPCID.Sets.TrailingMode[npc.type] = 0;
        }

		public override void SetDefaults()
		{
			npc.width = 48;
			npc.height = 40;
			npc.damage = 13;
			npc.defense = 7;
			npc.lifeMax = 45;
			npc.HitSound = SoundID.NPCHit2;
			npc.DeathSound = SoundID.NPCDeath5;
			npc.value = 60f;
			npc.knockBackResist = 0.55f;
			npc.aiStyle = 3;
			aiType = NPCID.WalkingAntlion;
		}

		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += 0.25f;
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
		}
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame,
                             lightColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
            if (trailbehind)
            {
                Vector2 drawOrigin = new Vector2(Main.npcTexture[npc.type].Width * 0.5f, (npc.height / Main.npcFrameCount[npc.type]) * 0.5f);
                for (int k = 0; k < npc.oldPos.Length; k++)
                {
                    Vector2 drawPos = npc.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, npc.gfxOffY);
                    Color color = npc.GetAlpha(lightColor) * (float)(((float)(npc.oldPos.Length - k) / (float)npc.oldPos.Length) / 2);
                    spriteBatch.Draw(Main.npcTexture[npc.type], drawPos, new Microsoft.Xna.Framework.Rectangle?(npc.frame), color, npc.rotation, drawOrigin, npc.scale, effects, 0f);
                }
            }
            return false;
        }
        bool trailbehind;
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (SpawnCondition.OceanMonster.Chance > 0)
			{
				return 0f;
			}
			return spawnInfo.player.ZoneUndergroundDesert ? 0.04f : 0f;
		}
        int counter;
        bool wormAI;
		public override void AI()
		{
            counter++;
            if (wormAI)
            {
                trailbehind = true;
                npc.netUpdate = true;
                npc.noTileCollide = true;
                npc.behindTiles = true;
                npc.noGravity = true;
                int minTilePosX = (int)(npc.position.X / 16.0) - 1;
                int maxTilePosX = (int)((npc.position.X + npc.width) / 16.0) + 2;
                int minTilePosY = (int)(npc.position.Y / 16.0) - 1;
                int maxTilePosY = (int)((npc.position.Y + npc.height) / 16.0) + 2;
                if (minTilePosX < 0)
                    minTilePosX = 0;
                if (maxTilePosX > Main.maxTilesX)
                    maxTilePosX = Main.maxTilesX;
                if (minTilePosY < 0)
                    minTilePosY = 0;
                if (maxTilePosY > Main.maxTilesY)
                    maxTilePosY = Main.maxTilesY;

                bool collision = false;
                for (int i = minTilePosX; i < maxTilePosX; ++i)
                {
                    for (int j = minTilePosY; j < maxTilePosY; ++j)
                    {
                        if (Main.tile[i, j] != null && (Main.tile[i, j].nactive() && (Main.tileSolid[(int)Main.tile[i, j].type] || Main.tileSolidTop[(int)Main.tile[i, j].type] && (int)Main.tile[i, j].frameY == 0) || (int)Main.tile[i, j].liquid > 64) || Main.tile[i, j].type == TileID.Sand)
                        {
                            Vector2 vector2;
                            vector2.X = (float)(i * 16);
                            vector2.Y = (float)(j * 16);
                            if (npc.position.X + npc.width > vector2.X && npc.position.X < vector2.X + 16.0 && (npc.position.Y + npc.height > (double)vector2.Y && npc.position.Y < vector2.Y + 16.0))
                            {
                                collision = true;
                                if (Main.rand.Next(100) == 0 && Main.tile[i, j].nactive())
                                    WorldGen.KillTile(i, j, true, true, false);
                            }
                        }
                    }
                }
                if (!collision)
                {
                    Rectangle rectangle1 = new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height);
                    int maxDistance = 1000;
                    bool playerCollision = true;
                    for (int index = 0; index < 255; ++index)
                    {
                        if (Main.player[index].active)
                        {
                            Rectangle rectangle2 = new Rectangle((int)Main.player[index].position.X - maxDistance, (int)Main.player[index].position.Y - maxDistance, maxDistance * 2, maxDistance * 2);
                            if (rectangle1.Intersects(rectangle2))
                            {
                                playerCollision = false;
                                break;
                            }
                        }
                    }
                    if (playerCollision)
                        collision = true;
                }

                float speed = 6f;
                float acceleration = 0.0825f;

                Vector2 npcCenter = new Vector2(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
                float targetXPos = Main.player[npc.target].position.X + (Main.player[npc.target].width / 2);
                float targetYPos = Main.player[npc.target].position.Y + (Main.player[npc.target].height / 2);

                float targetRoundedPosX = (float)((int)(targetXPos / 16.0) * 16);
                float targetRoundedPosY = (float)((int)(targetYPos / 16.0) * 16);
                npcCenter.X = (float)((int)(npcCenter.X / 16.0) * 16);
                npcCenter.Y = (float)((int)(npcCenter.Y / 16.0) * 16);
                float dirX = targetRoundedPosX - npcCenter.X;
                float dirY = targetRoundedPosY - npcCenter.Y;

                float length = (float)Math.Sqrt(dirX * dirX + dirY * dirY);
                if (!collision)
                {
                    npc.TargetClosest(true);
                    npc.velocity.Y = npc.velocity.Y + 0.17f;
                    if (npc.velocity.Y > speed)
                        npc.velocity.Y = speed;
                    if (Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y) < speed * 0.4)
                    {
                        if (npc.velocity.X < 0.0)
                            npc.velocity.X = npc.velocity.X - acceleration * 1.1f;
                        else
                            npc.velocity.X = npc.velocity.X + acceleration * 1.1f;
                    }
                    else if (npc.velocity.Y == speed)
                    {
                        if (npc.velocity.X < dirX)
                            npc.velocity.X = npc.velocity.X + acceleration;
                        else if (npc.velocity.X > dirX)
                            npc.velocity.X = npc.velocity.X - acceleration;
                    }
                    else if (npc.velocity.Y > 4.0)
                    {
                        if (npc.velocity.X < 0.0)
                            npc.velocity.X = npc.velocity.X + acceleration * 0.9f;
                        else
                            npc.velocity.X = npc.velocity.X - acceleration * 0.9f;
                    }
                }
                else
                {
                    if (npc.soundDelay == 0)
                    {
                        float num1 = length / 40f;
                        if (num1 < 10.0)
                            num1 = 10f;
                        if (num1 > 20.0)
                            num1 = 20f;
                        npc.soundDelay = (int)num1;
                        Main.PlaySound(SoundID.Roar, (int)npc.position.X, (int)npc.position.Y, 1);
                    }
                    float absDirX = Math.Abs(dirX);
                    float absDirY = Math.Abs(dirY);
                    float newSpeed = speed / length;
                    dirX = dirX * newSpeed;
                    dirY = dirY * newSpeed;
                    if (npc.velocity.X > 0.0 && dirX > 0.0 || npc.velocity.X < 0.0 && dirX < 0.0 || (npc.velocity.Y > 0.0 && dirY > 0.0 || npc.velocity.Y < 0.0 && dirY < 0.0))
                    {
                        if (npc.velocity.X < dirX)
                            npc.velocity.X = npc.velocity.X + acceleration;
                        else if (npc.velocity.X > dirX)
                            npc.velocity.X = npc.velocity.X - acceleration;
                        if (npc.velocity.Y < dirY)
                            npc.velocity.Y = npc.velocity.Y + acceleration;
                        else if (npc.velocity.Y > dirY)
                            npc.velocity.Y = npc.velocity.Y - acceleration;
                        if (Math.Abs(dirY) < speed * 0.2 && (npc.velocity.X > 0.0 && dirX < 0.0 || npc.velocity.X < 0.0 && dirX > 0.0))
                        {
                            if (npc.velocity.Y > 0.0)
                                npc.velocity.Y = npc.velocity.Y + acceleration * 2f;
                            else
                                npc.velocity.Y = npc.velocity.Y - acceleration * 2f;
                        }
                        if (Math.Abs(dirX) < speed * 0.2 && (npc.velocity.Y > 0.0 && dirY < 0.0 || npc.velocity.Y < 0.0 && dirY > 0.0))
                        {
                            if (npc.velocity.X > 0.0)
                                npc.velocity.X = npc.velocity.X + acceleration * 2f;
                            else
                                npc.velocity.X = npc.velocity.X - acceleration * 2f;
                        }
                    }
                    else if (absDirX > absDirY)
                    {
                        if (npc.velocity.X < dirX)
                            npc.velocity.X = npc.velocity.X + acceleration * 1.14f;
                        else if (npc.velocity.X > dirX)
                            npc.velocity.X = npc.velocity.X - acceleration * 1.14f;
                        if (Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y) < speed * 0.5)
                        {
                            if (npc.velocity.Y > 0.0)
                                npc.velocity.Y = npc.velocity.Y + acceleration;
                            else
                                npc.velocity.Y = npc.velocity.Y - acceleration;
                        }
                    }
                    else
                    {
                        if (npc.velocity.Y < dirY)
                            npc.velocity.Y = npc.velocity.Y + acceleration * 1.14f;
                        else if (npc.velocity.Y > dirY)
                            npc.velocity.Y = npc.velocity.Y - acceleration * 1.14f;
                        if (Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y) < speed * 0.5)
                        {
                            if (npc.velocity.X > 0.0)
                                npc.velocity.X = npc.velocity.X + acceleration;
                            else
                                npc.velocity.X = npc.velocity.X - acceleration;
                        }
                    }
                }
                npc.rotation = npc.velocity.X * .06f;

                if (collision)
                {
                    if (npc.localAI[0] != 1)
                        npc.netUpdate = true;
                    npc.localAI[0] = 1f;
                }
                else
                {
                    if (npc.localAI[0] != 0.0)
                        npc.netUpdate = true;
                    npc.localAI[0] = 0.0f;
                }
                if ((npc.velocity.X > 0.0 && npc.oldVelocity.X < 0.0 || npc.velocity.X < 0.0 && npc.oldVelocity.X > 0.0 || (npc.velocity.Y > 0.0 && npc.oldVelocity.Y < 0.0 || npc.velocity.Y < 0.0 && npc.oldVelocity.Y > 0.0)) && !npc.justHit)
                    npc.netUpdate = true;
            }
            if (counter >= 200 && counter <= 500)
            {
                wormAI = true;
            }
            else
            {
                npc.rotation *= 0f;
                wormAI = false;
                trailbehind = false;
                npc.noTileCollide = false;
                npc.behindTiles = false;
                npc.noGravity = false;
                npc.aiStyle = 3;
                aiType = NPCID.WalkingAntlion;
            }
            if (counter >= 700)
            {
                counter = 0;
            }
			npc.spriteDirection = npc.direction;
		}

		public override void NPCLoot()
		{
			if (Main.rand.Next(25) == 1)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<FossilFlower>());
		}

		public override void HitEffect(int hitDirection, double damage)
		{
            for (int k = 0; k < 5; k++)
            {
                Dust.NewDust(npc.position, npc.width, npc.height, 0, hitDirection, -1f, 0, default(Color), 1f);
            }
            if (npc.life <= 0)
			{
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Chompasaur1"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Chompasaur2"), 1f);
			}
		}
	}
}*/
