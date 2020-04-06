using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Dungeon
{
	public class SpectralSkull : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spectral Skull");
			Main.npcFrameCount[npc.type] = 9;
            NPCID.Sets.TrailCacheLength[npc.type] = 3;
            NPCID.Sets.TrailingMode[npc.type] = 0;
        }

		public override void SetDefaults()
		{
			npc.width = 20;
			npc.height = 22;
			npc.damage = 26;
			npc.defense = 4;
			npc.noGravity = true;
			npc.lifeMax = 40;
			npc.HitSound = SoundID.NPCHit3;
			npc.DeathSound = SoundID.NPCDeath6;
			npc.value = 160f;
			npc.knockBackResist = .3f;
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
            int d1 = 156;
            for (int k = 0; k < 20; k++)
            {
                Dust.NewDust(npc.position, npc.width, npc.height, d1, 2.5f * hitDirection, -2.5f, 117, new Color(0, 255, 142), .6f);
            }
            if (npc.life <= 0)
            {
                Main.PlaySound(29, (int)npc.position.X, (int)npc.position.Y, 53);
                for (int i = 0; i < 40; i++)
                {
                    int num = Dust.NewDust(npc.position, npc.width, npc.height, 156, 0f, -2f, 117, new Color(0, 255, 142), .6f);
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
			npc.spriteDirection = npc.direction;
			Player target = Main.player[npc.target];
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(3, 252, 215, 100);
        }
		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
            npc.life = 0;
            Main.PlaySound(29, (int)npc.position.X, (int)npc.position.Y, 53);
            for (int i = 0; i < 40; i++)
            {
                int num = Dust.NewDust(npc.position, npc.width, npc.height, 156, 0f, -2f, 117, new Color(0, 255, 142), .6f);
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
			{
				target.AddBuff(BuffID.Cursed, 300);
			}
		}
        public override void NPCLoot()
        {
            if (Main.rand.Next(25) == 0)
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("ForbiddenKnowledgeTome"));
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
