using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.GameContent.Events;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Vulture_Matriarch
{
	[AutoloadBossHead]
	public class Vulture_Matriarch : ModNPC
	{
			
		public bool gliding = false;
		
		public bool isFlashing = false;
		public bool justFlashed = false;
		public int flashingTimer = 0;
		
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vulture Matriarch");
			Main.npcFrameCount[npc.type] = 8;
			NPCID.Sets.TrailCacheLength[npc.type] = 20; 
			NPCID.Sets.TrailingMode[npc.type] = 0;
		}
		public override void SetDefaults()
		{
			npc.aiStyle = -1;
			npc.lifeMax = 3500;
			npc.defense = 24;
			npc.value = 0f;
			npc.damage = 60;
			npc.npcSlots = 6f;
			npc.knockBackResist = 0f;
			npc.width = 40;
			npc.height = 50;
			npc.lavaImmune = true;
			npc.noGravity = true;
			npc.noTileCollide = true;
			npc.HitSound = SoundID.NPCHit28;
			npc.DeathSound = SoundID.NPCDeath31;
			npc.friendly = false;
		}
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(flashingTimer);
			writer.Write(gliding);
			writer.Write(isFlashing);
			writer.Write(justFlashed);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			flashingTimer = reader.ReadInt32();
			gliding = reader.ReadBoolean();
			isFlashing = reader.ReadBoolean();
			justFlashed = reader.ReadBoolean();
		}
		public override void AI()
		{
			Player player = Main.player[npc.target];
			npc.TargetClosest(true);
			npc.rotation = npc.velocity.X * 0.05f;
			npc.spriteDirection = npc.direction;

			if (npc.position.Y > player.position.Y + 100)
				npc.velocity.Y = -8f;

			if (npc.Distance(player.Center) <= 140)			
			{
				if (npc.ai[0] == 0)
				{
					npc.ai[0] = 1;
					npc.netUpdate = true;
				}
			}	
			if (npc.ai[0] != 0)
			{
				if (npc.ai[2] != 1)
				{
					npc.ai[2] = 1;
					Main.NewText("The Vulture Matriarch has been disturbed!", 175, 75, 255);
					Main.PlaySound(3, (int)npc.position.X, (int)npc.position.Y, 28, 1.5f, -0.4f);
					npc.netUpdate = true;
				}
				npc.damage = 17;
				npc.noTileCollide = true;
				if (Main.rand.Next(355)==0 && !isFlashing && npc.ai[1] < 260 && !justFlashed)
				{
					isFlashing = true;
					justFlashed = true;
					npc.netUpdate = true;
				}
				else
				{
					circularGlideMovement();
					normalMovement();
				}
				
				if (isFlashing)
					flashing();
				
				if (!player.active || player.dead)
				{
					npc.velocity.Y = -5f;
				}
				
				npc.ai[3]++;
				if (npc.ai[1] < 260)
				{
					if (npc.life < npc.lifeMax/2)
					{
						if (npc.ai[3] % 55 == 0 && Main.netMode != NetmodeID.MultiplayerClient)
						{
							Main.PlaySound(SoundID.Item, (int)npc.position.X, (int)npc.position.Y, 73, 1f, -0.5f);
							Vector2 vector2_1 = new Vector2((float)player.Center.X, (float)player.Center.Y);
							Vector2 vector2_2 = Vector2.Normalize(vector2_1 - npc.Center) * 12f;
							int numberProjectiles = 3 + Main.rand.Next(4);
							for (int i = 0; i < numberProjectiles; i++)
							{
								Vector2 perturbedSpeed = new Vector2(vector2_2.X, vector2_2.Y).RotatedByRandom(MathHelper.ToRadians(32));
								float scale = 1f - (Main.rand.NextFloat() * .3f);
								perturbedSpeed = perturbedSpeed * scale;
								int p = Projectile.NewProjectile(npc.Center.X + 23 * npc.direction, npc.Center.Y + 16, perturbedSpeed.X, perturbedSpeed.Y, mod.ProjectileType("Sharp_Feather"), 22, 3f, 0);
							}
						}
					}
					else
					{
						if (npc.ai[3] % 110 == 0 && Main.netMode != NetmodeID.MultiplayerClient)
						{
							Main.PlaySound(SoundID.Item, (int)npc.position.X, (int)npc.position.Y, 73, 1f, -0.5f);
							Vector2 vector2_1 = new Vector2((float)player.Center.X, (float)player.Center.Y);
							Vector2 vector2_2 = Vector2.Normalize(vector2_1 - npc.Center) * 12f;
							int numberProjectiles = 2 + Main.rand.Next(4);
							for (int i = 0; i < numberProjectiles; i++)
							{
								Vector2 perturbedSpeed = new Vector2(vector2_2.X, vector2_2.Y).RotatedByRandom(MathHelper.ToRadians(32));
								float scale = 1f - (Main.rand.NextFloat() * .3f);
								perturbedSpeed = perturbedSpeed * scale;
								int p = Projectile.NewProjectile(npc.Center.X + 23 * npc.direction, npc.Center.Y + 16, perturbedSpeed.X, perturbedSpeed.Y, mod.ProjectileType("Sharp_Feather"), 22, 3f, 0);
							}
						}
					}
				}
			}
			else
			{
				npc.noTileCollide = false;
				npc.velocity.Y = 5f;
				npc.velocity.X = 0f;
				npc.damage = 0;		
			}
		}
		public void flashing()
		{
			npc.ai[1] = 0;
			Player player = Main.player[npc.target];
			flashingTimer++;
			npc.velocity = Vector2.Zero;
			if (player.position.X > npc.position.X)
				npc.spriteDirection = 1;
			else
				npc.spriteDirection = -1;
			if (flashingTimer >= 179)
			{
				flashingTimer = 0;
				isFlashing = false;
			}
			if (flashingTimer % 30 == 0 && flashingTimer != 180)
			{
				if (player.direction == -npc.spriteDirection && !player.HasBuff(mod.BuffType("Golden_Curse")) && Collision.CanHitLine(npc.Center, 0, 0, Main.player[npc.target].Center, 0, 0))
				{
					player.AddBuff(mod.BuffType("Golden_Curse"), 60*10);
					Main.PlaySound(42, (int)npc.position.X, (int)npc.position.Y, 50, 1f, -0.5f);
					for (int bb = 0; bb < 50; ++bb)
					{
						int bbb = Dust.NewDust(new Vector2(player.position.X, player.position.Y), player.width, player.height, 228, 0.0f, 0.0f, 100, new Color(), (float) Main.rand.Next(1, 3));
						Main.dust[bbb].velocity *= 3f;
						if ((double) Main.dust[bbb].scale > 1.0)
							Main.dust[bbb].noGravity = true;
					}
				}
				Main.PlaySound(42, (int)npc.position.X, (int)npc.position.Y, 41, 1f, 0f);
			}
		}
		public void normalMovement()
		{
			npc.knockBackResist = 0f;
			Player player = Main.player[npc.target];
			if (npc.ai[1] < 260 && npc.ai[1] > 1)
			{
				float num1 = 4f;
				float number2 = 0.25f;
				Vector2 vektor2 = new Vector2(npc.Center.X, npc.Center.Y);
				float number3 = Main.player[npc.target].Center.X - vektor2.X;
				float number4 = (float)((double)Main.player[npc.target].Center.Y - (double)vektor2.Y - 300.0);
				float num5 = (float)Math.Sqrt((double)number3 * (double)number3 + (double)number4 * (double)number4);
				float num6;
				float num7;
				if (player == Main.player[npc.target])
				{
					if ((double)num5 < 20.0)
					{
						num6 = npc.velocity.X;
						num7 = npc.velocity.Y;
					}
					else
					{
						float num8 = num1 / num5;
						num6 = number3 * num8;
						num7 = number4 * num8;
					}
					if ((double)npc.velocity.X < (double)num6)
					{
						npc.velocity.X += number2;
						if ((double)npc.velocity.X < 0.0 && (double)num6 > 0.0)
						{
							npc.velocity.X += number2 * 2f;
						}
					}
					else if ((double)npc.velocity.X > (double)num6)
					{
						npc.velocity.X -= number2;
						if ((double)npc.velocity.X > 0.0 && (double)num6 < 0.0)
						{
							npc.velocity.X -= number2 * 2f;
						}
					}
					if ((double)npc.velocity.Y < (double)num7)
					{
						npc.velocity.Y += number2;
						if ((double)npc.velocity.Y < 0.0 && (double)num7 > 0.0)
						{
							npc.velocity.Y += number2 * 2f;
						}
					}
					else if ((double)npc.velocity.Y > (double)num7)
					{
						npc.velocity.Y -= number2;
						if ((double)npc.velocity.Y > 0.0 && (double)num7 < 0.0)
						{
							npc.velocity.Y -= number2 * 2f;
						}
					}
				}
			}
		}
		public void circularGlideMovement()
		{
			npc.knockBackResist = 0f;
			Player player = Main.player[npc.target];
			npc.ai[1]++;
			if (npc.ai[1] >= 260 && npc.ai[1] < 360+80)
			{
				int num14 = 720;
				float num18 = 6.283185f / (float) (num14 / 2);
				npc.velocity = npc.velocity.RotatedBy(-(double) num18 * 2 * (double) npc.direction, new Vector2());
				
			}
			if (npc.ai[1] >= 360+80)
				flyTowardsPlayer();
		}
		
		public void flyTowardsPlayer()
		{
			if (npc.ai[1] == 361+80)
				Main.PlaySound(3, (int)npc.position.X, (int)npc.position.Y, 28, 1.5f, -0.4f);
			gliding = true;
			float num2 = 10f;
			float num5 = 30f;
			float num16 = num2;
			Vector2 center = npc.Center;
			npc.knockBackResist = 0.99f;
			Vector2 v = Main.player[npc.target].Center - center;
			Vector2 vector2_1 = v;
			float num17 = v.Length();
			v = Vector2.Normalize(v) * num16;
			Vector2 vector2_2 = Vector2.Normalize(vector2_1) * num16;
			npc.velocity.X = (npc.velocity.X * (num5 - 1f) + vector2_2.X) / num5;
			npc.velocity.Y = (npc.velocity.Y * (num5 - 1f) + vector2_2.Y) / num5;
			if (npc.ai[1] >= 500+80)
			{
				justFlashed = false;
				gliding = false;
				npc.ai[1] = 0;
				npc.netUpdate = true;
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			if (isFlashing)
			{
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
				Microsoft.Xna.Framework.Color color3 = new Microsoft.Xna.Framework.Color(211, 198, 111, 0) * 1.2f;
				Microsoft.Xna.Framework.Color color9 = new Microsoft.Xna.Framework.Color(211, 198, 111, 0) * 1.9f;
				Microsoft.Xna.Framework.Color color11 = new Microsoft.Xna.Framework.Color(211, 198, 111, 0) * 0.3f;
				Vector2 position3 = position1 + new Vector2(0.0f, -34f);
				Texture2D texture2D3 = mod.GetTexture("Effects/Ripple");
				Microsoft.Xna.Framework.Rectangle r3 = texture2D3.Frame(1, 1, 0, 0);
				origin = r3.Size() / 2f;
				Vector2 scale = new Vector2(0.75f, 1f + num16) * 1.5f;
				Vector2 scale2 = new Vector2(1f + num16, 0.75f) * 1.5f;
				float num17 = 1f + num13 * 0.75f;
				position3.Y -= 6f;
				Main.spriteBatch.Draw(texture2D3, position3, new Microsoft.Xna.Framework.Rectangle?(r3), color3 * num14, npc.rotation + 1.570796f, origin, scale, spriteEffects ^ SpriteEffects.FlipHorizontally, 0.0f);
				Main.spriteBatch.Draw(texture2D3, position3, new Microsoft.Xna.Framework.Rectangle?(r3), color3 * num14, npc.rotation + 1.570796f, origin, scale2, spriteEffects ^ SpriteEffects.FlipHorizontally, 0.0f);
				Main.spriteBatch.Draw(texture2D3, position3, new Microsoft.Xna.Framework.Rectangle?(r3), Microsoft.Xna.Framework.Color.Lerp(color9, Microsoft.Xna.Framework.Color.White, 0.7f), npc.rotation + 1.570796f, origin, 1.5f, spriteEffects ^ SpriteEffects.FlipHorizontally, 0.0f);
				Main.spriteBatch.Draw(texture2D3, position3, new Microsoft.Xna.Framework.Rectangle?(r3), Microsoft.Xna.Framework.Color.Lerp(color11, Microsoft.Xna.Framework.Color.White, 0.2f), npc.rotation + 1.570796f, origin, 3f, spriteEffects ^ SpriteEffects.FlipHorizontally, 0.0f);
			}
			return true;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			if (npc.ai[1] >= 360+80 || flashingTimer > 0)
			{	
				for (int i = 0; i<1; i++)
				{
					int num7 = 4;
					float num9 = 6f;
					float num8 = (float) (Math.Cos((double) Main.GlobalTime % 2.40000009536743 / 2.40000009536743 * 6.28318548202515) / 2.0 + 0.5);
					float amount1 = 0.5f;
					float num10 = 0.0f;
					float addY = 0f;
					float addHeight = -10f;
					SpriteEffects spriteEffects = SpriteEffects.None;
					if (npc.spriteDirection == 1)
						spriteEffects = SpriteEffects.FlipHorizontally;
					Texture2D texture = Main.npcTexture[npc.type];
					Vector2 vector2_3 = new Vector2((float) (Main.npcTexture[npc.type].Width / 2), (float) (Main.npcTexture[npc.type].Height / Main.npcFrameCount[npc.type] / 2));
					Vector2 position1 = npc.Center - Main.screenPosition - new Vector2((float) texture.Width, (float) (texture.Height / Main.npcFrameCount[npc.type])) * npc.scale / 2f + vector2_3 * npc.scale + new Vector2(0.0f, addY + addHeight + npc.gfxOffY);
					Microsoft.Xna.Framework.Color color2 = new Microsoft.Xna.Framework.Color((int) sbyte.MaxValue - npc.alpha, (int) sbyte.MaxValue - npc.alpha, (int) sbyte.MaxValue - npc.alpha, 0).MultiplyRGBA(Color.Gold);
					for (int index2 = 0; index2 < num7; ++index2)
					{
						Microsoft.Xna.Framework.Color newColor2 = color2;
						Microsoft.Xna.Framework.Color color3 = npc.GetAlpha(newColor2) * (1f-0.95f);
						Vector2 position2 = new Vector2 (npc.Center.X + 2 * npc.spriteDirection,npc.Center.Y) + ((float) ((double) index2 / (double) num7 * 6.28318548202515) + npc.rotation + num10).ToRotationVector2() * (float) (4.0 * (double) num8 + 2.0) - Main.screenPosition - new Vector2((float) texture.Width, (float) (texture.Height / Main.npcFrameCount[npc.type])) * npc.scale / 2f + vector2_3 * npc.scale + new Vector2(0.0f, addY + addHeight + npc.gfxOffY);
						Main.spriteBatch.Draw(Main.npcTexture[npc.type], position2, new Microsoft.Xna.Framework.Rectangle?(npc.frame), color3, npc.rotation, vector2_3, npc.scale*1.15f, spriteEffects, 0.0f);
					}
					Main.spriteBatch.Draw(Main.npcTexture[npc.type], position1, new Microsoft.Xna.Framework.Rectangle?(npc.frame), color2, npc.rotation, vector2_3, npc.scale*1.15f, spriteEffects, 0.0f);
				}
			}
		}
		
		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.ai[0] == 0)
			{
				npc.ai[0] = 1;
				npc.netUpdate = true;
			}
			if (npc.life <= 0)
			{
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/VultureMatriarch/VultureMatriarchGore1"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/VultureMatriarch/VultureMatriarchGore2"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/VultureMatriarch/VultureMatriarchGore2"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/VultureMatriarch/VultureMatriarchGore3"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/VultureMatriarch/VultureMatriarchGore3"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/VultureMatriarch/VultureMatriarchGore4"), 1f);
			}
		}
		public override void NPCLoot()
		{
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("Golden_Egg"), 1);

			if (Main.rand.Next(7) == 0)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("Vulture_Matriarch_Mask"), 1);
			}
		}
		public override void FindFrame(int frameHeight)
		{
			
			const int animationSpeed = 6;

			Player player = Main.player[npc.target];
				
			npc.frameCounter++;
			if (npc.ai[0] != 0)
			{
				if (!gliding)
				{	
					if (npc.frameCounter < animationSpeed * 1)
					{
						npc.frame.Y = 0 * frameHeight;
					}
					else if (npc.frameCounter < animationSpeed * 2)
					{
						npc.frame.Y = 1 * frameHeight;
					}
					else if (npc.frameCounter < animationSpeed * 3)
					{
						npc.frame.Y = 2 * frameHeight;
					}
					else if (npc.frameCounter < animationSpeed * 4)
					{
						npc.frame.Y = 3 * frameHeight;
					}
					else if (npc.frameCounter < animationSpeed * 5)
					{
						npc.frame.Y = 4 * frameHeight;
						Main.PlaySound(SoundID.Item, (int)npc.position.X, (int)npc.position.Y, 32, 1f, 0f);
					}
					else
					{
						npc.frameCounter = 0;
					}
				}
				else
				{	
					if (npc.frameCounter < animationSpeed * 1)
					{
						npc.frame.Y = 5 * frameHeight;
					}
					else if (npc.frameCounter < animationSpeed * 2)
					{
						npc.frame.Y = 6 * frameHeight;
					}
					else
					{
						npc.frameCounter = 0;
					}
				}
			}
			else
				npc.frame.Y = 7 * frameHeight;
		}
		public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
		{
			scale = 1.2f;
			return null;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (Main.tileSand[spawnInfo.spawnTileType] && Main.hardMode && !NPC.AnyNPCs(ModContent.NPCType<Vulture_Matriarch>()))
				return SpawnCondition.OverworldDayDesert.Chance * .145f;
			return 0;
		}
	}
}