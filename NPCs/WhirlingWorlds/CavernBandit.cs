using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace SpiritMod.NPCs.WhirlingWorlds
{
	public class CavernBandit : ModNPC
	{
		public double timer = 0;
		public bool activated = false;
		public bool pickedFrame = false;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cavern Bandit");
			Main.npcFrameCount[npc.type] = 16;
			NPCID.Sets.TrailCacheLength[npc.type] = 20; 
			NPCID.Sets.TrailingMode[npc.type] = 0;
		}
		public override void SetDefaults()
		{
			npc.aiStyle = 3;
			npc.lifeMax = 40;
			npc.defense = 6;
			npc.value = 65f;
			aiType = NPCID.Skeleton;
			npc.knockBackResist = 0.7f;
			npc.width = 30;
			npc.height = 42;
			npc.damage = 15;
			npc.lavaImmune = false;
			npc.noTileCollide = false;
			npc.alpha = 0;
			npc.dontTakeDamage = false;
			npc.DeathSound = new Terraria.Audio.LegacySoundStyle(4, 1);
		}
		public override bool PreAI()
		{
			timer+= 0.05;
			Player player = Main.player[npc.target];

			npc.TargetClosest(true);
				
			if (activated)
			{
				if ((double)Vector2.Distance(player.Center, npc.Center) > (double)60f)
				{
					npc.aiStyle = 3;
				}
				else
				{
					npc.velocity.X = 0f;
				}
			}
			else if ((double)Vector2.Distance(player.Center, npc.Center) < (double)100f && !activated)
			{
				activated = true;
			}
				
			if (npc.life < npc.lifeMax)
				activated = true;
			
			Lighting.AddLight(new Vector2(npc.Center.X, npc.Center.Y - 40), 255*0.002f, 255*0.002f, 0*0.001f);
			
				
			if (npc.velocity.X < 0f)
			{
				npc.spriteDirection = 1;
			}
			else if (npc.velocity.X > 0f)
			{
				npc.spriteDirection = -1;
			}
			if (!activated)
			{
				return false;
			}
			return base.PreAI();
		}
		public override void HitEffect(int hitDirection, double damage)
		{		
			Main.PlaySound(3, (int)npc.position.X, (int)npc.position.Y, 4, 1f, 0f);
			if (npc.life <= 0)
			{
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/CavernBanditGore1"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/CavernBanditGore2"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/CavernBanditGore3"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/CavernBanditGore4"), 1f);
			}
			for (int k = 0; k < 7; k++)
			{
				Dust.NewDust(npc.position, npc.width, npc.height, 8, 2.5f * hitDirection, -2.5f, 0, default(Color), 1.2f);
				Dust.NewDust(npc.position, npc.width, npc.height, 8, 2.5f * hitDirection, -2.5f, 0, default(Color), 0.5f);
				Dust.NewDust(npc.position, npc.width, npc.height, 8, 2.5f * hitDirection, -2.5f, 0, default(Color), 0.7f);
			}
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return SpawnCondition.Cavern.Chance * 0.15f;
		}
		int frame = 0;
		public override void FindFrame(int frameHeight)
		{
			Player player = Main.player[npc.target];
			npc.frameCounter++;
			
			if (activated)
			{
				if ((double)Vector2.Distance(player.Center, npc.Center) >= (double)60f)
				{
					if(npc.frameCounter >= 7) {
						frame++;
						npc.frameCounter = 0;
					}
					if(frame >= 7) {
						frame = 0;
					}
				}
				else 
				{
					if(npc.frameCounter >= 5) {
					frame++;
					npc.frameCounter = 0;
					}
					if(frame >= 13) {
						frame = 7;
					}
					if(frame < 7) {
						frame = 7;
					}
					if (frame == 9 && npc.frameCounter == 4 && Collision.CanHitLine(npc.Center, 0, 0, Main.player[npc.target].Center, 0, 0))
						{
							player.Hurt(PlayerDeathReason.LegacyDefault(), (int)npc.damage * 2, npc.direction * -1, false, false, false, -1);
						}
				}
			}
			else
			{
				
				if (!pickedFrame)
				{
					frame = Main.rand.Next(3);
					pickedFrame = true;				
					switch (frame)
					{
						case 0:
							frame = 13;
							break;
						case 1:
							frame = 14;
							break;
						case 2:
							frame = 15;
							break;
						default:
							break;
					}			
				}
			}
			npc.frame.Y = frameHeight * frame;
		}
		/*public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			SpriteEffects spriteEffects = SpriteEffects.None;
			if (npc.spriteDirection == 1)
				spriteEffects = SpriteEffects.FlipHorizontally;
			Vector2 vector2_3 = new Vector2((float) (Main.npcTexture[npc.type].Width / 2), (float) (Main.npcTexture[npc.type].Height / Main.npcFrameCount[npc.type] / 2));
			Microsoft.Xna.Framework.Color color12 = Lighting.GetColor((int) ((double) npc.position.X + (double) npc.width * 0.5) / 16, (int) (((double) npc.position.Y + (double) npc.height * 0.5) / 16.0));
			Texture2D texture2D = mod.GetTexture("NPCs/WhirlingWorlds/CavernLantern");		
			Texture2D texture2DD = mod.GetTexture("Effects/Ripple");		
			int num21 = (int) ((double) ((float) ((double) timer / 100.0 * 6.28318548202515)).ToRotationVector2().Y * 6.0);
			(float) (int) ((double) npc.Center.Y + 360 - (double) Main.screenPosition.Y + (double) npc.height - (double) Main.npcTexture[npc.type].Height + 4.0)) + vector2_3 + new Vector2((float) (Main.npcTexture[npc.type].Width / 2), (float) (Main.npcTexture[npc.type].Height / 2)) + new Vector2((float) 0f, (float) (num21));
			Main.spriteBatch.Draw(texture2DD, position, new Microsoft.Xna.Framework.Rectangle?(), Color.Yellow, npc.rotation, texture2DD.Size() / 2f, 1f, spriteEffects, 0);
			Main.spriteBatch.Draw(texture2D, position, new Microsoft.Xna.Framework.Rectangle?(), color12, npc.rotation, texture2D.Size() / 2f, 1f, spriteEffects, 0);
			return true;
		}*/
		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			SpriteEffects spriteEffects = SpriteEffects.None;
			if (npc.spriteDirection == 1)
				spriteEffects = SpriteEffects.FlipHorizontally;
			int xpos = (int)((npc.Center.X + 59) - Main.screenPosition.X) - (int)(Main.npcTexture[npc.type].Width / 2);
			int ypos = (int)((npc.Center.Y - 60) - Main.screenPosition.Y) + (int)(Math.Sin(timer) * 12);
			Texture2D ripple = mod.GetTexture("Effects/Ripple");
			Texture2D lantern = mod.GetTexture("NPCs/WhirlingWorlds/CavernLantern");
			Main.spriteBatch.Draw(ripple, new Vector2(xpos,ypos), new Microsoft.Xna.Framework.Rectangle?(), Color.Yellow, npc.rotation, ripple.Size() / 2f, 1f, spriteEffects, 0);
			Main.spriteBatch.Draw(lantern, new Vector2(xpos,ypos), new Microsoft.Xna.Framework.Rectangle?(), Color.White, npc.rotation, lantern.Size() / 2f, 1f, spriteEffects, 0);
			return true;
		}
	}
}