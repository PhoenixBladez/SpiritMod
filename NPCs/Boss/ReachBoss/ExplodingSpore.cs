using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod;
using SpiritMod.Effects;
using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.ReachBoss
{
	public class ExplodingSpore : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Exploding Spore");
			NPCID.Sets.TrailCacheLength[npc.type] = 2;
			NPCID.Sets.TrailingMode[npc.type] = 0;
		}

		public override void SetDefaults()
		{
			npc.width = 10;
			npc.aiStyle = -1;
			npc.height = 14;
			npc.damage = 0;
			npc.defense = 0;
			npc.knockBackResist = 0.2f;
			npc.lifeMax = 15;
			npc.value = 0f;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.noGravity = true;
			npc.noTileCollide = true;
		}

		public override void AI()
		{
			npc.rotation = npc.velocity.X * .1f;
			Lighting.AddLight(npc.Center, .237f, .191f, .040f);
			npc.ai[2]++;
            if (npc.ai[2] < 120)
            {
                int num1 = ModContent.NPCType<ReachBoss>();
                float num2 = 210f;
                float x = 0.08f;
                float y = 0.1f;
                if (npc.ai[0] < num2)
                {
                    NPC vwb = Main.npc[(int)npc.ai[1]];
                    if (vwb.active && vwb.type == num1)
                    {
                        npc.position += vwb.position - vwb.oldPos[1]; 
                        npc.velocity = npc.velocity + new Vector2(Math.Sign(npc.DirectionTo(vwb.Center).X), Math.Sign(npc.DirectionTo(vwb.Center).Y)) * new Vector2(x, y);
                    }

                    else
                        npc.ai[0] = num2;
                }    
            }
            else
            {
                npc.velocity *= .97f;
            }
			if (npc.ai[2] > 270)
			{
				Explode();
				npc.netUpdate = true;
			}		
		}
		public void Explode()
		{
			Main.PlaySound(new LegacySoundStyle(2, 14).WithPitchVariance(0.2f), npc.Center);
			npc.life = 0;
			npc.active = false;
			DustHelper.DrawStar(new Vector2(npc.Center.X, npc.Center.Y), DustID.GoldCoin, pointAmount: 5, mainSize: 8f, dustDensity: 2.5f, dustSize: .75f, pointDepthMult: 0.4f, noGravity: true);
			if (Main.netMode != NetmodeID.MultiplayerClient)
			{
				int p = Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0f, 0f, ModContent.ProjectileType<SporeExplosion>(), 24, 1, Main.myPlayer, 0, 0);	
			}	
			Vector2 spinningpoint1 = ((float)Main.rand.NextDouble() * 6.283185f).ToRotationVector2();
			Vector2 spinningpoint2 = spinningpoint1;
			float dagada = (float)(Main.rand.Next(3, 6) * 2);
			int num2 = 10;
			float num3 = Main.rand.Next(2) == 0 ? 1f : -1f;
			bool flag = true;
			for (int index1 = 0; (double)index1 < (double)num2 * (double)dagada; ++index1)
			{
				if (index1 % num2 == 0)
				{
					spinningpoint2 = spinningpoint2.RotatedBy((double)num3 * (6.28318548202515 / (double)dagada), new Vector2());
					spinningpoint1 = spinningpoint2;
					flag = !flag;
				}
				else
				{
					float num4 = 6.283185f / ((float)num2 * dagada);
					spinningpoint1 = spinningpoint1.RotatedBy((double)num4 * (double)num3 * 3.0, new Vector2());
				}
				float adada = MathHelper.Lerp(1f, 4f, (float)(index1 % num2) / (float)num2);
				int index2 = Dust.NewDust(new Vector2(npc.Center.X, npc.Center.Y), 6, 6, DustID.GoldCoin, 0.0f, 0.0f, 100, new Color(), 1.4f);
				Main.dust[index2].velocity *= 0.1f;
				Main.dust[index2].velocity += spinningpoint1 * adada;
				if (flag)	
				{
					Main.dust[index2].scale = 0.9f;
				}	
					Main.dust[index2].noGravity = true;
			}									
		}
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame,
							 drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
			Vector2 vector2_3 = new Vector2((float)(Main.npcTexture[npc.type].Width / 2), (float)(Main.npcTexture[npc.type].Height / Main.npcFrameCount[npc.type] / 2));
			Vector2 drawOrigin = new Vector2(Main.npcTexture[npc.type].Width * 0.5f, (npc.height / Main.npcFrameCount[npc.type]) * 0.5f);

			SpriteEffects spriteEffects = SpriteEffects.None;
			if (npc.spriteDirection == 1)
				spriteEffects = SpriteEffects.FlipHorizontally;
			Texture2D texture2D1 = Main.npcTexture[npc.type];
			Vector2 position1 = npc.Bottom - Main.screenPosition;
			Microsoft.Xna.Framework.Rectangle r1 = texture2D1.Frame(1, 1, 0, 0);
			Vector2 origin = r1.Size() / 2f;
			float num11 = (float) (Math.Cos((double) Main.GlobalTime % 2.40000009536743 / 2.40000009536743 * 6.28318548202515) / 2 + 0.5);
			float num12 = num11;
			if ((double) num12 > 0.5)
			  num12 = 1f - num11;
			if ((double) num12 < 0.0)
			  num12 = 0.0f;
			float num13 = (float) (((double) num11 + 0.5) % 1.0);
			float num14 = num13;
			if ((double) num14 > 0.5)
			  num14 = 1f - num13;
			if ((double) num14 < 0.0)
			  num14 = 0.0f;
			float num16 = 1f + num13 * 0.75f;
			Microsoft.Xna.Framework.Color color3 = Color.Gold * 1.2f;
			Microsoft.Xna.Framework.Color color9 = Color.Khaki * 1.9f;
			Microsoft.Xna.Framework.Color color11 = Color.Yellow * 0.3f;
			Vector2 position3 = position1 + new Vector2(0.0f, -10f);
			Texture2D texture2D3 = mod.GetTexture("Effects/Ripple");
			Microsoft.Xna.Framework.Rectangle r3 = texture2D3.Frame(1, 1, 0, 0);
			origin = r3.Size() / 2f;
			Vector2 scale = new Vector2(0.75f, 1f + num16) * 0.45f * npc.scale;
			Vector2 scale2 = new Vector2(1f + num16, 0.75f) * 0.45f * npc.scale;
			float num17 = 1f + num13 * 0.75f;
			position3.Y -= 6f;
			Main.spriteBatch.Draw(texture2D3, position3, new Microsoft.Xna.Framework.Rectangle?(r3), color3 * num14, npc.rotation + 1.570796f, origin, scale, spriteEffects ^ SpriteEffects.FlipHorizontally, 0.0f);
			Main.spriteBatch.Draw(texture2D3, position3, new Microsoft.Xna.Framework.Rectangle?(r3), color3 * num14, npc.rotation + 1.570796f, origin, scale2, spriteEffects ^ SpriteEffects.FlipHorizontally, 0.0f);
			return false;
		}
        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			GlowmaskUtils.DrawNPCGlowMask(spriteBatch, npc, mod.GetTexture("NPCs/Boss/ReachBoss/ExplodingSpore_Glow"));
		}
	}
}
