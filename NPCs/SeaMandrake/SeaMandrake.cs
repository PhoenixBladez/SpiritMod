using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.SeaMandrake
{
	public class SeaMandrake : ModNPC
	{
		public bool hasGottenColor = false;
		public bool screamed = false;
		public int r = 255;
		public int g = 255;
		public int b = 255;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sea Mandrake");
			Main.npcFrameCount[npc.type] = 4;
			NPCID.Sets.TrailCacheLength[npc.type] = 20; 
			NPCID.Sets.TrailingMode[npc.type] = 0;
		}
		public override void SetDefaults()
		{
			npc.aiStyle = 16;
			npc.lifeMax = 50;
			npc.defense = 7;
			npc.value = 200f;
			aiType = 0;
			npc.knockBackResist = 1.2f;
			npc.width = 30;
			npc.height = 50;
			npc.damage = 35;
			npc.lavaImmune = false;
			npc.noTileCollide = false;
			npc.noGravity = true;
			npc.HitSound = SoundID.NPCHit25;
			npc.DeathSound = SoundID.NPCDeath28;
		}
		public override void AI()
		{
			Player player = Main.player[npc.target];
			npc.TargetClosest(true);
			
			if (Main.rand.Next(500)==0)
				Main.PlaySound(SoundLoader.customSoundType, (int)npc.position.X, (int)npc.position.Y, mod.GetSoundSlot(SoundType.Custom, "Sounds/Mandrake_Idle"));
			
			if (npc.velocity.X < 0f)
			{
				npc.spriteDirection = -1;
			}
			else if (npc.velocity.X > 0f)
			{
				npc.spriteDirection = 1;
			}
			if ((double)Vector2.Distance(player.Center, npc.Center) <= (double)45f)
			{
				npc.velocity.X = 0f;
			}
			
			if (Vector2.Distance(npc.Center, Main.player[npc.target].Center) <= (double) 80f && npc.wet)
				spawnInk();
			else if (Vector2.Distance(npc.Center, Main.player[npc.target].Center) > (double) 100f)
				screamed = false;
			
			if (npc.wet)
				movement();
			if (!npc.wet && !player.wet)
				npc.velocity.Y = 8f;
				
			if (!hasGottenColor)
			{
				hasGottenColor = true;
				if (Main.netMode != NetmodeID.Server) {
					if (MyWorld.luminousOcean)	{
						if (MyWorld.luminousType == 1) {
							r = 100;
							b = 100;
						}
						if (MyWorld.luminousType == 2) {
							r = 30;
							g = 100;
						}
						if (MyWorld.luminousType == 3) {
							r = 160;
							g = 30;
						}						
						
					}
					else {
						r = Main.rand.Next(1,255);
						g = Main.rand.Next(1,255);
						b = Main.rand.Next(1,255);
					}
				}
				npc.netUpdate = true;
			}
			Lighting.AddLight(new Vector2(npc.Center.X, npc.Center.Y), r*0.002f, g*0.002f, b*0.002f);
			
			if (!player.wet)
			{
				for(int i = 0; i < Main.projectile.Length; i++)
				{
					Projectile type = Main.projectile[i];

					if ((double) Vector2.Distance(type.Center, npc.Center) <= (double) 100f && (double) Vector2.Distance(type.Center, npc.Center) > (double) 20f && type.friendly && type.position.X > npc.position.X && npc.wet && type.active)
					{
						Vector2 vector2 = new Vector2(npc.position.X + (float) npc.width * 0.5f, npc.position.Y + (float) npc.height * 0.5f);
						float num2 = type.position.X + Main.rand.Next(-10,10) + (float) (type.width / 2) - vector2.X;
						float num3 = type.position.Y + Main.rand.Next(-10,10) + (float) (type.height / 2) - vector2.Y;
						float num4 = 8f / (float) Math.Sqrt((double) num2 * (double) num2 + (double) num3 * (double) num3);
						npc.velocity.X = num2 * num4 * -1 * (5f / 6);
						npc.velocity.Y = num3 * num4 * -1 * (5f / 6);
						npc.spriteDirection = -1;
						npc.direction = -1;
						npc.netUpdate = true;
					}
					else if ((double) Vector2.Distance(type.Center, npc.Center) <= (double) 100f && (double) Vector2.Distance(type.Center, npc.Center) > (double) 20f && type.friendly && type.position.X < npc.position.X && npc.wet && type.active)
					{
						Vector2 vector2 = new Vector2(npc.position.X + (float) npc.width * 0.5f, npc.position.Y + (float) npc.height * 0.5f);
						float num2 = type.position.X + Main.rand.Next(-10,10) + (float) (type.width / 2) - vector2.X;
						float num3 = type.position.Y + Main.rand.Next(-10,10) + (float) (type.height / 2) - vector2.Y;
						float num4 = 8f / (float) Math.Sqrt((double) num2 * (double) num2 + (double) num3 * (double) num3);
						npc.velocity.X = num2 * num4 * -1 * (5f / 6);
						npc.velocity.Y = num3 * num4 * -1 * (5f / 6);
						npc.spriteDirection = 1;
						npc.direction = 1;
						npc.netUpdate = true;
					}
				}
			}
		}
		
		private void movement()
		{
			if (!Main.player[npc.target].wet && npc.wet)
			{
				if (npc.direction == 0)
				{
					npc.TargetClosest(true);
				}
				bool flag30 = false;
				npc.TargetClosest(false);
				if (Main.player[npc.target].wet && !Main.player[npc.target].wet && !Main.player[npc.target].dead)
				{
					flag30 = true;
				}
				if (flag30)
				{
					if (npc.collideX)
					{
						npc.velocity.X *= -1f;
						npc.direction *= -1;
						npc.netUpdate = true;
					}
					if (npc.collideY)
					{
						npc.netUpdate = true;
						if (npc.velocity.Y > 0f)
						{
							npc.velocity.Y = -npc.velocity.Y;
							npc.directionY = -1;
							npc.ai[0] = -1f;
						}
						else if (npc.velocity.Y < 0f)
						{
							npc.velocity.Y = -npc.velocity.Y;
							npc.directionY = 1;
							npc.ai[0] = 1f;
						}
					}
				}
				{

					npc.velocity.X += (float)npc.direction * 0.1f * 2f;
					if (npc.velocity.X < -2f || npc.velocity.X > 2f)
					{
						npc.velocity.X *= 0.95f;
					}
					if (npc.ai[0] == -1f)
					{
						npc.velocity.Y -= 0.01f * 2f;
						if ((double)npc.velocity.Y < -0.3)
						{
							npc.ai[0] = 1f;
						}
					}
					else
					{
						npc.velocity.Y += 0.01f * 2f;
						if ((double)npc.velocity.Y > 0.3)
						{
							npc.ai[0] = -1f;
						}
					}
				}
				int num358 = (int)(npc.position.X + (float)(npc.width / 2)) / 16;
				int num359 = (int)(npc.position.Y + (float)(npc.height / 2)) / 16;
				if (Main.tile[num358, num359 - 1] == null)
				{
					Main.tile[num358, num359 - 1] = new Tile();
				}
				if (Main.tile[num358, num359 + 1] == null)
				{
					Main.tile[num358, num359 + 1] = new Tile();
				}
				if (Main.tile[num358, num359 + 2] == null)
				{
					Main.tile[num358, num359 + 2] = new Tile();
				}
				if (Main.tile[num358, num359 - 1].liquid > 128)
				{
					if (Main.tile[num358, num359 + 1].active())
					{
						npc.ai[0] = -1f;
					}
					else if (Main.tile[num358, num359 + 2].active())
					{
						npc.ai[0] = -1f;
					}
				}
			}
			else if (Main.player[npc.target].wet && npc.wet)
			{
				if (npc.collideX)
				{
					npc.velocity.X *= -1f;
					npc.direction *= -1;
					npc.netUpdate = true;
				}
				if (npc.collideY)
				{
					npc.netUpdate = true;
					if (npc.velocity.Y > 0f)
					{
						npc.velocity.Y = -npc.velocity.Y;
						npc.directionY = -1;
						npc.ai[0] = -1f;
					}
					else if (npc.velocity.Y < 0f)
					{
						npc.velocity.Y = -npc.velocity.Y;
						npc.directionY = 1;
						npc.ai[0] = 1f;
					}
				}
				if (Main.player[npc.target].position.X > npc.position.X)
				{
					npc.spriteDirection = -1;
					npc.direction = -1;
				}
				else if (Main.player[npc.target].position.X < npc.position.X)
				{
					npc.spriteDirection = 1;
					npc.direction = 1;
				}
				Player player = Main.player[npc.target];
				if (Vector2.Distance(npc.Center, Main.player[npc.target].Center) <= (double) 80f && npc.velocity.X != 0f)
				{
					Vector2 vector2 = new Vector2(npc.position.X + (float) npc.width * 0.5f, npc.position.Y + (float) npc.height * 0.5f);
					float num2 = player.position.X + Main.rand.Next(-10,10) + (float) (player.width / 2) - vector2.X;
					float num3 = player.position.Y + Main.rand.Next(-10,10) + (float) (player.height / 2) - vector2.Y;
					float num4 = 17f / (float) Math.Sqrt((double) num2 * (double) num2 + (double) num3 * (double) num3);
					npc.velocity.X = num2 * num4 * -1 * (2f / 5);
					npc.velocity.Y = num3 * num4 * -1 * (2f / 5);
				}
			}
			
			int x = (int)(npc.position.X + (float)(npc.width / 2)) / 16;
			int y = (int)(npc.position.Y + (float)(npc.height / 2)) / 16;
			if (npc.collideY && Main.tile[x, y].liquid < 128)
			{
				npc.position.Y += 4;
			}
			
			npc.rotation = npc.velocity.X * 0.2f;
			
			if (npc.collideX)
			{
				npc.netUpdate = true;
				npc.velocity.X = npc.oldVelocity.X * -0.7f * 3f;
				if ((double)npc.velocity.X > 0.0 && (double)npc.velocity.X < 6.0)
				{
					npc.velocity.X = 3f;
				}

				if ((double)npc.velocity.X < 0.0 && (double)npc.velocity.X > -6.0)
				{
					npc.velocity.X = -3f;
				}
			}
		}
		
		private void spawnInk()
		{
			npc.rotation = npc.velocity.X * 1.25f;
			Player player = Main.player[npc.target];
			if (!screamed)
			{
				//Main.PlaySound(SoundLoader.customSoundType, (int)npc.position.X, (int)npc.position.Y, mod.GetSoundSlot(SoundType.Custom, "Sounds/Mandrake_Scream"));
				screamed = true;
			}
			if ((double)Vector2.Distance(player.Center, npc.Center) <= (double)45f && player.position.Y < npc.position.Y + 100 && player.position.Y > npc.position.Y - 100 && Collision.CanHitLine(npc.Center, 0, 0, Main.player[npc.target].Center, 0, 0))  
			{
				player.AddBuff(80,2);
				player.AddBuff(22,2);
				player.AddBuff(163,2);
				player.Hurt(PlayerDeathReason.LegacyDefault(), (int)(npc.damage/1.5f), npc.direction, false, false, false, -1);
			}
			for (int i = 0; i < 1; i++)
			{
				int index2 = Dust.NewDust(npc.position, npc.width, npc.height, 261, 0.0f, 0.0f, 100, new Color(r,g,b), 1.1f);
				Main.dust[index2].alpha += Main.rand.Next(100);
				Main.dust[index2].noGravity = true;
				Main.dust[index2].velocity *= 0.3f;
				Main.dust[index2].velocity.X += (float)Main.rand.Next(-80, -40) * 0.025f * npc.velocity.X;
				Main.dust[index2].velocity.Y -= (float)(0.400000005960464 + (double)Main.rand.Next(-3, 14) * 0.150000005960464);
				Main.dust[index2].fadeIn = (float)(0.25 + (double)Main.rand.Next(15) * 0.150000005960464);
			}
			
			for (int i = 0; i < 1; i++)
			{
				int index2 = Dust.NewDust(npc.position, npc.width, npc.height, 261, 0.0f, 0.0f, 100, new Color(r,g,b), 2.4f);
				Main.dust[index2].alpha += Main.rand.Next(100);
				Main.dust[index2].noGravity = true;
				Main.dust[index2].velocity *= 0.3f;
				Main.dust[index2].velocity.X += (float)Main.rand.Next(-240, -180) * 0.025f * npc.velocity.X;
				Main.dust[index2].velocity.Y -= (float)(0.400000005960464 + (double)Main.rand.Next(-3, 14) * 0.150000005960464);
				Main.dust[index2].fadeIn = (float)(0.25 + (double)Main.rand.Next(10) * 0.150000005960464);
			}
		}
		public override void NPCLoot()
		{
			if (Main.rand.Next(25) == 0)
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("LuminanceSeacone"), 1);
			}
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				for (int index1 = 0; index1 < 26; ++index1)
				{
				  int index2 = Dust.NewDust(npc.position, npc.width, npc.height, 261, npc.velocity.X * 0.5f, npc.velocity.Y * 0.5f, 90, new Color(r,g,b), 2.5f);
				  Main.dust[index2].noGravity = true;
				  Main.dust[index2].fadeIn = 1f;
				  Main.dust[index2].velocity *= 4f;
				  Main.dust[index2].noLight = true;
				}
			}
			for (int k = 0; k < 10; k++)
			{
				Dust.NewDust(npc.position, npc.width, npc.height, 261, 2.5f * hitDirection, -2.5f, 0, new Color(r,g,b), 1.2f);
				Dust.NewDust(npc.position, npc.width, npc.height, 261, 2.5f * hitDirection, -2.5f, 0, new Color(r,g,b), 0.5f);
				Dust.NewDust(npc.position, npc.width, npc.height, 261, 2.5f * hitDirection, -2.5f, 0, new Color(r,g,b), 0.7f);
			}
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (NPC.AnyNPCs(ModContent.NPCType<SeaMandrake>()))
			{
				return 0f;
			}
			return SpawnCondition.OceanMonster.Chance * 0.11f;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			return false;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			SpriteEffects spriteEffects = SpriteEffects.None;
			if (npc.spriteDirection == 1)
				spriteEffects = SpriteEffects.FlipHorizontally;
			Vector2 vector2_3 = new Vector2((float) (Main.npcTexture[npc.type].Width / 2), (float) (Main.npcTexture[npc.type].Height / Main.npcFrameCount[npc.type] / 2));
			float addHeight = 13f;
			float addY = 0f;
			Microsoft.Xna.Framework.Color color1 = Lighting.GetColor((int) ((double) npc.position.X + (double) npc.width * 0.5) / 16, (int) (((double) npc.position.Y + (double) npc.height * 0.5) / 16.0));
			var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame,
							drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
			Main.spriteBatch.Draw(mod.GetTexture("NPCs/SeaMandrake/SeaMandrake_Glow"), npc.Bottom - Main.screenPosition + new Vector2((float) ((double) -Main.npcTexture[npc.type].Width * (double) npc.scale / 2.0 + (double) vector2_3.X * (double) npc.scale), (float) ((double) -Main.npcTexture[npc.type].Height * (double) npc.scale / (double) Main.npcFrameCount[npc.type] + 4.0 + (double) vector2_3.Y * (double) npc.scale) + addHeight + npc.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(npc.frame), new Microsoft.Xna.Framework.Color((int) r - npc.alpha, (int) byte.MaxValue - npc.alpha, (int) g - npc.alpha, (int) b - npc.alpha), npc.rotation, vector2_3, npc.scale, effects, 0.0f);
            float num = (float) (0.25 + (double) (npc.GetAlpha(color1).ToVector3() + new Vector3(2.5f)).Length() * 0.25);
            for (int index = 0; index < 8; ++index)
                Main.spriteBatch.Draw(mod.GetTexture("NPCs/SeaMandrake/SeaMandrake_Glow"), npc.Bottom - Main.screenPosition + new Vector2((float) ((double) -Main.npcTexture[npc.type].Width * (double) npc.scale / 2.0 + (double) vector2_3.X * (double) npc.scale), (float) ((double) -Main.npcTexture[npc.type].Height * (double) npc.scale / (double) Main.npcFrameCount[npc.type] + 4.0 + (double) vector2_3.Y * (double) npc.scale) + addHeight + npc.gfxOffY) + npc.velocity.RotatedBy((double) index * 47079637050629, new Vector2()) * num, new Microsoft.Xna.Framework.Rectangle?(npc.frame), new Microsoft.Xna.Framework.Color(r, g, b, 0), npc.rotation, vector2_3, npc.scale, effects, 0.0f);
		}
		public override void FindFrame(int frameHeight)
		{
			const int Frame_1 = 0;
			const int Frame_2 = 1;
			const int Frame_3 = 2;
			const int Frame_4 = 3;

			Player player = Main.player[npc.target];
			npc.frameCounter++;
			if (npc.frameCounter < 6)
			{
				npc.frame.Y = Frame_1 * frameHeight;
			}
			else if (npc.frameCounter < 12)
			{
				npc.frame.Y = Frame_2 * frameHeight;
			}
			else if (npc.frameCounter < 18)
			{
				npc.frame.Y = Frame_3 * frameHeight;
			}
			else if (npc.frameCounter < 24)
			{
				npc.frame.Y = Frame_4 * frameHeight;
			}
			else
			{
				npc.frameCounter = 0;
			}
		}
	}
}