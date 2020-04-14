using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Events;

namespace SpiritMod.NPCs.BloodMoon
{

	public class Phantom : ModNPC
	{
		public static int _type;
		int moveSpeed = 0;
		int moveSpeedY = 0;
		float HomeY = 120f;
		private float SpeedMax = 33f;
		private float SpeedDistanceIncrease = 500f;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Phantom");
			Main.npcFrameCount[npc.type] = 5;
		    NPCID.Sets.TrailCacheLength[npc.type] = 3;
            NPCID.Sets.TrailingMode[npc.type] = 0;
		}

		public override void SetDefaults()
		{
			npc.width = 60;
			npc.height = 48;
			npc.value = 140;
			npc.damage = 23;
			npc.noTileCollide = true;
			npc.defense = 6;
			npc.lifeMax = 50;
			npc.knockBackResist = 0.45f;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
		}
		bool trailbehind;
		bool noise;
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return spawnInfo.spawnTileY < Main.rockLayer && (Main.bloodMoon) && NPC.downedBoss1 ? 0.14f : 0f;
        }
        public override bool PreAI()
		{
			npc.TargetClosest(true);
			npc.spriteDirection = npc.direction;
			Player player = Main.player[npc.target];
			bool expertMode = Main.expertMode;
			{
				{
					if (npc.Center.X >= player.Center.X && moveSpeed >= -30) // flies to players x position
					{
						moveSpeed--;
					}

					if (npc.Center.X <= player.Center.X && moveSpeed <= 30)
					{
						moveSpeed++;
					}

					npc.velocity.X = moveSpeed * 0.18f;

					if (npc.Center.Y >= player.Center.Y - HomeY && moveSpeedY >= -27) //Flies to players Y position
					{
						moveSpeedY--;
						HomeY = 220f;
					}

					if (npc.Center.Y <= player.Center.Y - HomeY && moveSpeedY <= 27)
					{
						moveSpeedY++;
					}
					npc.velocity.Y = moveSpeedY * 0.12f;
                    if (player.velocity.Y != 0)
                    {
                        HomeY = -60f;
                        trailbehind = true;
                        npc.velocity.Y = moveSpeedY * 0.16f;
                        int d = Dust.NewDust(npc.position, npc.width, npc.height, 173, 0f, -2.5f, 0, default(Color), 0.6f);
                        if (!noise)
                        {
                            Main.PlaySound(29, (int)npc.position.X, (int)npc.position.Y, 7);
                            noise = true;
                        }
                        npc.rotation = npc.velocity.X * .1f;
                    }
                    else
                    {
                        if (HomeY < 220f)
                        {
                            HomeY += 8f;
                        }
                        npc.rotation = 0f;
                        noise = false;
                        trailbehind = false;
                    }
                }				
			}
            if (Main.dayTime)
            {
                Main.PlaySound(4, (int)npc.position.X, (int)npc.position.Y, 6);
                Gore.NewGore(npc.position, npc.velocity, 99);
                Gore.NewGore(npc.position, npc.velocity, 99);
                Gore.NewGore(npc.position, npc.velocity, 99);
                npc.active = false;
            }
            return true;
		}
        public override void NPCLoot()
        {
            if (Main.rand.Next(12) == 0)
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("PhantomEgg"));
            }
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("BloodFire"));
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            {
                var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
                spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame,
                                 lightColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
                {
                    Vector2 drawOrigin = new Vector2(Main.npcTexture[npc.type].Width * 0.5f, (npc.height / Main.npcFrameCount[npc.type]) * 0.5f);
                    for (int k = 0; k < npc.oldPos.Length; k++)
                    {
                        Vector2 drawPos = npc.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, npc.gfxOffY);
                        Color color = npc.GetAlpha(lightColor) * (float)(((float)(npc.oldPos.Length - k) / (float)npc.oldPos.Length) / 2);
                        spriteBatch.Draw(Main.npcTexture[npc.type], drawPos, new Microsoft.Xna.Framework.Rectangle?(npc.frame), color, npc.rotation, drawOrigin, npc.scale, effects, 0f);
                    }
                }
            }
            return false;
        }
       	public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            if (trailbehind)
            {
                GlowmaskUtils.DrawNPCGlowMask(spriteBatch, npc, mod.GetTexture("NPCs/BloodMoon/Phantom_Glow"));
            }
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 20; k++)
			{
				Dust.NewDust(npc.position, npc.width, npc.height, 5, hitDirection * 2, -1f, 0, default(Color), 1f);
			}
            if (trailbehind)
            {
                for (int k = 0; k < 20; k++)
                {
                    Dust.NewDust(npc.position, npc.width, npc.height, 173, hitDirection * 2, -1f, 0, default(Color), 1f);
                }
            }
            if (npc.life <= 0)
            {
                for (int k = 0; k < 20; k++)
                {
                    Dust.NewDust(npc.position, npc.width, npc.height, 173, hitDirection * 2, -1f, 0, default(Color), 1f);
                }
                for (int k = 0; k < 20; k++)
                {
                    Dust.NewDust(npc.position, npc.width, npc.height, 5, hitDirection * 2, -1f, 0, default(Color), 1f);
                }
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Phantom/Phantom1"));
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Phantom/Phantom2"));
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Phantom/Phantom2"));
            }
        }
		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += .25f;
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
		}
	}
}