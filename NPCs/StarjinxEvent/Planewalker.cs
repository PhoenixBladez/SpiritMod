using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using System;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.StarjinxEvent
{
	public class Planewalker : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Planewalker");
			Main.npcFrameCount[npc.type] = 8;
			NPCID.Sets.TrailCacheLength[npc.type] = 20; 
			NPCID.Sets.TrailingMode[npc.type] = 0;
		}
		public override void SetDefaults()
		{
			npc.aiStyle = 44;
			npc.lifeMax = 300;
			npc.defense = 13;
			npc.value = 500f;
			npc.alpha = 0;
			npc.friendly = false;
			npc.knockBackResist = 0.5f;
			npc.width = 36;
			npc.height = 54;
			npc.damage = 45;
			npc.lavaImmune = true;
			npc.noGravity = true;
			npc.noTileCollide = true;
		}
		public override bool CheckDead()
		{
			return false;
		}
		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += 0.15f;
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
		}
		Vector2 laserRotation;
		int counter;
		public override void AI()
		{
			counter++;
			if (counter % 300 >= 200)
			{
				if (counter % 5 == 0)
				{
					Projectile.NewProjectile(npc.Center - new Vector2(0, 10), laserRotation * 6, ModContent.ProjectileType<PlanewalkerLaser>(), npc.damage, 2, npc.target);
					laserRotation = laserRotation.RotatedBy(0.314f);
				}
			}
			else
			{
				laserRotation = new Vector2(-1, 0);
				if (counter % 300 >= 100)
				{
					Dust dust = Dust.NewDustPerfect(npc.Center - new Vector2(0,10), 159);
					dust.velocity = Main.rand.NextFloat(6.28f).ToRotationVector2() * 2.5f;
					dust.position -= dust.velocity * 15;
					dust.position += npc.velocity * 15;
					dust.scale = (((counter % 300) - 100f) / 100f) + 0.5f;
				}
			}
		}
		 public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Vector2 center = new Vector2((float)(Main.npcTexture[npc.type].Width / 2), (float)(Main.npcTexture[npc.type].Height / Main.npcFrameCount[npc.type] / 2));
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
            for (int num103 = 0; num103 < 6; num103++)
            {
                Vector2 vector29 = new Vector2(npc.Center.X, npc.Center.Y) + ((float)num103 / (float)num108 * 6.28318548f + npc.rotation + num106).ToRotationVector2() * (4f * num107 + 2f) - Main.screenPosition + new Vector2(0, npc.gfxOffY);
                Main.spriteBatch.Draw(mod.GetTexture("NPCs/StarjinxEvent/PlanewalkerGlow"), vector29, npc.frame, color28, npc.rotation, npc.frame.Size() / 2f, npc.scale, spriteEffects3, 0f);
            }
        }
	}
}