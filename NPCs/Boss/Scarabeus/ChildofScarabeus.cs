using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Events;

namespace SpiritMod.NPCs.Boss.Scarabeus
{

	public class ChildofScarabeus : ModNPC
	{
		public static int _type;
		int moveSpeed = 0;
		int moveSpeedY = 0;
		float HomeY = 120f;
		private float SpeedMax = 33f;
		private float SpeedDistanceIncrease = 500f;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Child of Scarabeus");
			Main.npcFrameCount[npc.type] = 4;
		    NPCID.Sets.TrailCacheLength[npc.type] = 3;
            NPCID.Sets.TrailingMode[npc.type] = 0;
		}

		public override void SetDefaults()
		{
			npc.width = 60;
			npc.height = 40;
			npc.value = 0;
			npc.damage = 23;
			npc.noTileCollide = true;
			npc.defense = 7;
			npc.lifeMax = 40;
			npc.knockBackResist = 0f;
			npc.npcSlots = 10f;
			npc.HitSound = SoundID.NPCHit31;
			npc.DeathSound = SoundID.NPCDeath5;
			bossBag = mod.ItemType("BagOScarabs");
		}
		private int Counter;
		float frametimer = .25f;
		bool trailbehind;
		int frame = 0;
		int timer = 0;
		bool charge;
		bool jump;
		int npcCounter;		
		int jumpstacks;
        int shoottimer;
		public override bool PreAI()
		{
			npc.TargetClosest(true);
			npc.spriteDirection = npc.direction;
			Player player = Main.player[npc.target];
			bool expertMode = Main.expertMode;
            shoottimer++;
			{
				if(shoottimer >= 180)
				{
				         Main.PlaySound(2, (int)npc.Center.X, (int)npc.Center.Y, 20);
			        	Vector2 direction = Main.player[npc.target].Center - npc.Center;
						direction.Normalize();
						direction.X *= 8f;
						direction.Y *= 8f;

						int amountOfProjectiles = 1;
						for (int i = 0; i < amountOfProjectiles; ++i)
						{
    						int damage = expertMode ? 8 : 17;
							float A = (float)Main.rand.Next(-50, 50) * 0.02f;
							float B = (float)Main.rand.Next(-50, 50) * 0.02f;
							int p = Projectile.NewProjectile(npc.Center.X, npc.Center.Y, direction.X + A, direction.Y + B, mod.ProjectileType("MoltenGold"), damage, 1, Main.myPlayer, 0, 0);
							Main.projectile[p].hostile = true;
						}
						shoottimer = 0;
                    }
				}
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

					npc.velocity.X = moveSpeed * 0.1f;

					if (npc.Center.Y >= player.Center.Y - HomeY && moveSpeedY >= -27) //Flies to players Y position
					{
						moveSpeedY--;
						HomeY = 160f;
					}

					if (npc.Center.Y <= player.Center.Y - HomeY && moveSpeedY <= 27)
					{
						moveSpeedY++;
					}
					
					npc.velocity.Y = moveSpeedY * 0.12f;
				}				
			}
			return true;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame,
                             lightColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
			{
				Vector2 drawOrigin = new Vector2(Main.npcTexture[npc.type].Width * 0.5f, (npc.height/ Main.npcFrameCount[npc.type]) * 0.5f);
				for (int k = 0; k < npc.oldPos.Length; k++)
				{
					Vector2 drawPos = npc.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, npc.gfxOffY);
					Color color = npc.GetAlpha(lightColor) * (float)(((float)(npc.oldPos.Length - k) / (float)npc.oldPos.Length) / 2);
					spriteBatch.Draw(Main.npcTexture[npc.type], drawPos, new Microsoft.Xna.Framework.Rectangle?(npc.frame), color, npc.rotation, drawOrigin, npc.scale, effects, 0f);
				}
			}
            return false;
        }
       	public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            GlowmaskUtils.DrawNPCGlowMask(spriteBatch, npc, mod.GetTexture("NPCs/Boss/Scarabeus/ChildofScarabeus_Glow"));			
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 5; k++)
			{
				Dust.NewDust(npc.position, npc.width, npc.height, 5, hitDirection, -1f, 0, default(Color), 1f);
			}
			if (npc.life <= 0)
			{
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Scarabeus/LittleScarab1"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Scarabeus/LittleScarab5"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Scarabeus/LittleScarab2"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Scarabeus/LittleScarab3"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Scarabeus/LittleScarab4"), 1f);
                npc.position.X = npc.position.X + (float)(npc.width / 2);
				npc.position.Y = npc.position.Y + (float)(npc.height / 2);
				npc.width = 100;
				npc.height = 60;
				npc.position.X = npc.position.X - (float)(npc.width / 2);
				npc.position.Y = npc.position.Y - (float)(npc.height / 2);
				for (int num621 = 0; num621 < 30; num621++)
				{
					int randomDustType = Main.rand.Next(3);
					if (randomDustType == 0)
						randomDustType = 5;
					else if (randomDustType == 1)
						randomDustType = 36;
					else
						randomDustType = 32;

					int num622 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, randomDustType, 0f, 0f, 100, default(Color), .82f);
					Main.dust[num622].velocity *= 3f;
					if (Main.rand.Next(2) == 0)
					{
						Main.dust[num622].scale = 0.5f;
					}
				}
				for (int num623 = 0; num623 < 50; num623++)
				{
					int randomDustType = Main.rand.Next(3);
					if (randomDustType == 0)
						randomDustType = 5;
					else if (randomDustType == 1)
						randomDustType = 36;
					else
						randomDustType = 32;

					int num624 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, randomDustType, 0f, 0f, 100, default(Color), 1f);
					Main.dust[num624].noGravity = true;
					Main.dust[num624].velocity *= 5f;
					num624 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, randomDustType, 0f, 0f, 100, default(Color), .2f);
					Main.dust[num624].velocity *= 2f;
				}
			}
		}
		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += .25f;
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
		}
		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			npc.lifeMax = (int)(npc.lifeMax * 0.75f * bossLifeScale);
			npc.damage = (int)(npc.damage * 0.75f);
		}

	}
}