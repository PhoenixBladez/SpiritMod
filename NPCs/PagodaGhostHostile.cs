using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace SpiritMod.NPCs
{
	public class PagodaGhostHostile : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Disturbed Yurei");
			Main.npcFrameCount[npc.type] = 4;
            NPCID.Sets.TrailCacheLength[npc.type] = 3;
            NPCID.Sets.TrailingMode[npc.type] = 0;
        }

		public override void SetDefaults()
		{
            if (NPC.downedBoss2)
            {
                npc.width = 30;
                npc.height = 40;
                npc.damage = 25;
                npc.noGravity = true;
                npc.defense = 8;
                npc.lifeMax = 80;
            }
            if (NPC.downedBoss3)
            {
                npc.width = 30;
                npc.height = 40;
                npc.damage = 30;
                npc.noGravity = true;
                npc.defense = 11;
                npc.lifeMax = 130;
            }
            else
            {
                npc.width = 30;
                npc.height = 40;
                npc.damage = 23;
                npc.noGravity = true;
                npc.defense = 4;
                npc.lifeMax = 50;
            }
            npc.HitSound = SoundID.NPCHit3;
			npc.DeathSound = SoundID.NPCDeath6;
			npc.value = 120f;
			npc.knockBackResist = .1f;
            npc.noTileCollide = true;
			npc.aiStyle = 44;
			aiType = NPCID.FlyingAntlion;
		}

		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += 0.15f;
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
            int d1 = 91;
            for (int k = 0; k < 20; k++)
            {
                Dust.NewDust(npc.position, npc.width, npc.height, d1, 2.5f * hitDirection, -2.5f, 117, new Color(0, 255, 142), .6f);
            }
            if (npc.life <= 0)
            {
                Gore.NewGore(npc.position, npc.velocity, 99);
                Gore.NewGore(npc.position, npc.velocity, 99);
                Gore.NewGore(npc.position, npc.velocity, 99);
                for (int i = 0; i < 40; i++)
                {
                    int num = Dust.NewDust(npc.position, npc.width, npc.height, 91, 0f, -2f, 117, new Color(0, 255, 142), .6f);
                    Main.dust[num].noGravity = true;
                    Dust expr_62_cp_0 = Main.dust[num];
                    expr_62_cp_0.position.X = expr_62_cp_0.position.X + ((float)(Main.rand.Next(-50, 51) / 20) - 1.5f);
                    Dust expr_92_cp_0 = Main.dust[num];
                    expr_92_cp_0.position.Y = expr_92_cp_0.position.Y + ((float)(Main.rand.Next(-50, 51) / 20) - 1.5f);
                    if (Main.dust[num].position != npc.Center)
                    {
                        Main.dust[num].velocity = npc.DirectionTo(Main.dust[num].position) * 6f;
                    }
                }
            }
        }

		public override void AI()
		{
            Player player = Main.player[npc.target];
            npc.alpha += 1;
            if (npc.alpha >= 180)
            {
                npc.position.X = player.position.X + Main.rand.Next(-60, 60);
                npc.position.Y = player.position.Y + Main.rand.Next(-160, -100);
                Gore.NewGore(npc.position, npc.velocity, 99);
                Gore.NewGore(npc.position, npc.velocity, 99);
                Gore.NewGore(npc.position, npc.velocity, 99);
                npc.alpha = 0;
                Main.PlaySound(4, (int)npc.position.X, (int)npc.position.Y, 6);
            }
			npc.spriteDirection = npc.direction;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(100 + npc.alpha, 100 + npc.alpha, 100 + npc.alpha, 100 + npc.alpha);
        }
        public override void OnHitPlayer(Player target, int damage, bool crit)
		{
            if (Main.rand.Next(4) == 0)
			{
                target.AddBuff(BuffID.Cursed, 180);
			}
		}
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Vector2 drawOrigin = new Vector2(Main.npcTexture[npc.type].Width * 0.5f, (npc.height * 0.5f));
            for (int k = 0; k < npc.oldPos.Length; k++)
            {
                var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
                Vector2 drawPos = npc.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, npc.gfxOffY);
                Color color = npc.GetAlpha(lightColor) * (float)(((float)(npc.oldPos.Length - k) / (float)npc.oldPos.Length) / 2);
                spriteBatch.Draw(Main.npcTexture[npc.type], drawPos, new Microsoft.Xna.Framework.Rectangle?(npc.frame), color, npc.rotation, drawOrigin, npc.scale, effects, 0f);
            }
            return true;
        }
    }
}
