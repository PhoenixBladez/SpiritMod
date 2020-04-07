using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Dungeon
{
	public class IllusionistSpectre : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Illusionist's Spectre");
			Main.npcFrameCount[npc.type] = 5;
            NPCID.Sets.TrailCacheLength[npc.type] = 3;
            NPCID.Sets.TrailingMode[npc.type] = 0;
        }

		public override void SetDefaults()
		{
			npc.width = 40;
			npc.height = 90;

			npc.lifeMax = 30;
			npc.defense = 0;
			npc.damage = 15;

			npc.HitSound = SoundID.NPCHit11;
			npc.DeathSound = SoundID.NPCDeath6;

			npc.knockBackResist = 0.75f;

			npc.noGravity = true;
			npc.netAlways = true;
			npc.chaseable = true;
            npc.noTileCollide = true;
			npc.lavaImmune = true;
		}

        int frame = 5;
        int timer = 0;
        int moveSpeed = 0;
        int moveSpeedY = 0;
        float HomeY = 100f;

        public override void AI()
		{
            npc.spriteDirection = npc.direction;
            Player target = Main.player[npc.target];
            {
                
                Player player = Main.player[npc.target];

                if (npc.Center.X >= player.Center.X && moveSpeed >= -30) // flies to players x position
                    moveSpeed--;

                if (npc.Center.X <= player.Center.X && moveSpeed <= 30)
                    moveSpeed++;

                npc.velocity.X = moveSpeed * 0.15f;

                if (npc.Center.Y >= player.Center.Y - HomeY && moveSpeedY >= -20) //Flies to players Y position
                {
                    moveSpeedY--;
                    HomeY = 125f;
                }

                if (npc.Center.Y <= player.Center.Y - HomeY && moveSpeedY <= 20)
                    moveSpeedY++;

                npc.velocity.Y = moveSpeedY * 0.1f;
                if (Main.rand.Next(180) == 1)
                {
                    HomeY = -25f;
                }

                timer++;
                if (timer == 4)
                {
                    frame++;
                    timer = 0;
                }
                if (frame >= 4)
                {
                    frame = 1;
                }
            }

            Lighting.AddLight((int)((npc.position.X + (float)(npc.width / 2)) / 16f), (int)((npc.position.Y + (float)(npc.height / 2)) / 16f), 0.3f, .3f, .3f);
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
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(255, 255, 255, 100);
        }
        public override void FindFrame(int frameHeight)
        {
            npc.frame.Y = frameHeight * frame;
        }
        public override void HitEffect(int hitDirection, double damage)
		{
			int d1 = 180;
		    for (int k = 0; k < 30; k++)
			{
				Dust.NewDust(npc.position, npc.width, npc.height, d1, 2.5f * hitDirection, -2.5f, 0, default(Color), .34f);
			}
            if (npc.life <= 0)
            {
                for (int k = 0; k < 30; k++)
                {
                    int d = Dust.NewDust(npc.position, npc.width, npc.height, d1, 2.5f * hitDirection, -4.5f, 0, default(Color), .34f);
                    Main.dust[d].noGravity = true;
                }
            }
        }
	}
}