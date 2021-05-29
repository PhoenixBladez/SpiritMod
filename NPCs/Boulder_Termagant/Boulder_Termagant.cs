using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boulder_Termagant
{
	public class Boulder_Termagant : ModNPC
	{
		public bool hasGottenColor = false;
		public bool resetFrames = false;
		public bool isRoaring = false;
		public int r = 0;
		public int g = 0;
		public int b = 0;
		public int randomColor = 0;
		public int boulderTimer = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Boulder Behemoth");
			Main.npcFrameCount[npc.type] = 11;
			NPCID.Sets.TrailCacheLength[npc.type] = 30; 
			NPCID.Sets.TrailingMode[npc.type] = 0;
		}
		
		public override void SetDefaults()
		{
			npc.aiStyle = 3;
			npc.lifeMax = 365;
			npc.defense = 28;
			npc.value = 1250f;
			aiType = 0;
			npc.knockBackResist = 0.1f;
			npc.width = 50;
			npc.height = 38;
			npc.damage = 70;
			npc.lavaImmune = false;
			npc.noTileCollide = false;
			npc.HitSound = new Terraria.Audio.LegacySoundStyle(3, 6);
			npc.DeathSound = new Terraria.Audio.LegacySoundStyle(4, 5);
		}
		public override void AI()
		{
			Player player = Main.player[npc.target];

			npc.TargetClosest(true);
			npc.spriteDirection = npc.direction;
			
			if (!hasGottenColor)
			{
				hasGottenColor = true;
				randomColor = Main.rand.Next(6);
				if (randomColor == 0)
				{
					r = 219;
					g = 97;
					b = 255;
				}
				if (randomColor == 1)
				{
					r = 255;
					g = 198;
					b = 0;
				}
				if (randomColor == 2)
				{
					r = 23;
					g = 147;
					b = 234;
				}
				if (randomColor == 3)
				{
					r = 33;
					g = 184;
					b = 115;
				}
				if (randomColor == 4)
				{
					r = 238;
					g = 51;
					b = 53;
				}
				if (randomColor == 5)
				{
					r = 223;
					g = 230;
					b = 238;
				}
			}
			Lighting.AddLight(new Vector2(npc.Center.X, npc.Center.Y), r*0.002f, g*0.002f, b*0.002f);
			
			if (Vector2.Distance(npc.Center, player.Center) < 800f && Collision.CanHitLine(npc.Center, 0, 0, Main.player[npc.target].Center, 0, 0))
			{
				boulderTimer++;
			}
			if (boulderTimer > 299 && boulderTimer < 421)
			{
				if (boulderTimer == 300)
				{
					Main.PlaySound(42, (int)npc.position.X, (int)npc.position.Y, 180, 1f, -0.9f);
				}
				npc.aiStyle = -1;
				npc.velocity.X = 0f;
				isRoaring = true;
				npc.defense = 60;
				if (!resetFrames)
				{
					npc.netUpdate = true;
					npc.frameCounter = 0;
					resetFrames = true;
				}
				if (boulderTimer == 420)
				{
					npc.netUpdate = true;
					boulderTimer = 0;
					resetFrames = false;
					isRoaring = false;
					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						if (player.GetModPlayer<MyPlayer>().ZoneGranite)
						{
							for (int i = 0; i < 5; i++)
							{
								Projectile.NewProjectile(player.Center.X - Main.rand.Next(-300,300), player.Center.Y - Main.rand.Next(800,1200), 0f, 2f + (float)Main.rand.Next(1,3), mod.ProjectileType("Granite_Boulder"), 15, 0, player.whoAmI);
							}
						}
						else if (player.GetModPlayer<MyPlayer>().ZoneMarble)
						{
							for (int i = 0; i < 5; i++)
							{
								Projectile.NewProjectile(player.Center.X - Main.rand.Next(-300,300), player.Center.Y - Main.rand.Next(800,1200), 0f, 2f + (float)Main.rand.Next(1,3), mod.ProjectileType("Marble_Boulder"), 15, 0, player.whoAmI);
							}
						}
						else
						{
							for (int i = 0; i < 5; i++)
							{
								Projectile.NewProjectile(player.Center.X - Main.rand.Next(-300,300), player.Center.Y - Main.rand.Next(800,1200), 0f, 2f + (float)Main.rand.Next(1,3), mod.ProjectileType("Cavern_Boulder"), 15, 0, player.whoAmI);
							}
						}
					}
				}
			}
			else
			{
				npc.aiStyle = 3;
				npc.defense = 28;
			}
		}
		public override void NPCLoot()
		{
			if (r == 219)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, 181, Main.rand.Next(1,3));
			if (r == 255)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, 180, Main.rand.Next(1,3));
			if (r == 23)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, 177, Main.rand.Next(1,3));
			if (r == 33)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, 179, Main.rand.Next(1,3));
			if (r == 238)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, 178, Main.rand.Next(1,3));
			if (r == 223)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, 182, Main.rand.Next(1,3));
		
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/BoulderTermagent/RockTermagantGore4"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/BoulderTermagent/RockTermagantGore3"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/BoulderTermagent/RockTermagantGore2"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/BoulderTermagent/RockTermagantGore1"), 1f);
			}
			for (int k = 0; k < 7; k++)
			{
				Dust.NewDust(npc.position, npc.width, npc.height, 1, 2.5f * hitDirection, -2.5f, 0, default(Color), 1.2f);
				Dust.NewDust(npc.position, npc.width, npc.height, 1, 2.5f * hitDirection, -2.5f, 0, default(Color), 0.5f);
				Dust.NewDust(npc.position, npc.width, npc.height, 1, 2.5f * hitDirection, -2.5f, 0, default(Color), 0.7f);
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			return false;
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo) => !spawnInfo.playerSafe && spawnInfo.spawnTileY > Main.rockLayer && Main.hardMode ? 0.07f : 0f;

		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			SpriteEffects spriteEffects = SpriteEffects.None;
			if (npc.spriteDirection == 1)
				spriteEffects = SpriteEffects.FlipHorizontally;
			Vector2 vector2_3 = new Vector2((float) (Main.npcTexture[npc.type].Width / 2), (float) (Main.npcTexture[npc.type].Height / Main.npcFrameCount[npc.type] / 2));
			float addHeight = 4f;
			float addY = 0f;
			Microsoft.Xna.Framework.Color color1 = Lighting.GetColor((int) ((double) npc.position.X + (double) npc.width * 0.5) / 16, (int) (((double) npc.position.Y + (double) npc.height * 0.5) / 16.0));
			var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(Main.npcTexture[npc.type],npc.Bottom - Main.screenPosition + new Vector2((float) ((double) -Main.npcTexture[npc.type].Width * (double) npc.scale / 2.0 + (double) vector2_3.X * (double) npc.scale), (float) ((double) -Main.npcTexture[npc.type].Height * (double) npc.scale / (double) Main.npcFrameCount[npc.type] + 4.0 + (double) vector2_3.Y * (double) npc.scale) + addHeight + npc.gfxOffY), npc.frame,
							drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
			Main.spriteBatch.Draw(mod.GetTexture("NPCs/Boulder_Termagant/Boulder_Termagant_Glow"), npc.Bottom - Main.screenPosition + new Vector2((float) ((double) -Main.npcTexture[npc.type].Width * (double) npc.scale / 2.0 + (double) vector2_3.X * (double) npc.scale), (float) ((double) -Main.npcTexture[npc.type].Height * (double) npc.scale / (double) Main.npcFrameCount[npc.type] + 4.0 + (double) vector2_3.Y * (double) npc.scale) + addHeight + npc.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(npc.frame), new Microsoft.Xna.Framework.Color((int) r - npc.alpha, (int) byte.MaxValue - npc.alpha, (int) g - npc.alpha, (int) b - npc.alpha), npc.rotation, vector2_3, npc.scale, effects, 0.0f);
            float num = (float) (0.25 + (double) (npc.GetAlpha(color1).ToVector3() - new Vector3(1.25f)).Length() * 0.25);
            for (int index = 0; index < 16; ++index)
                Main.spriteBatch.Draw(mod.GetTexture("NPCs/Boulder_Termagant/Boulder_Termagant_Glow"), npc.Bottom - Main.screenPosition + new Vector2((float) ((double) -Main.npcTexture[npc.type].Width * (double) npc.scale / 2.0 + (double) vector2_3.X * (double) npc.scale), (float) ((double) -Main.npcTexture[npc.type].Height * (double) npc.scale / (double) Main.npcFrameCount[npc.type] + 4.0 + (double) vector2_3.Y * (double) npc.scale) + addHeight + npc.gfxOffY) + npc.velocity.RotatedBy((double) index * 47079637050629, new Vector2()) * num, new Microsoft.Xna.Framework.Rectangle?(npc.frame), new Microsoft.Xna.Framework.Color(r, g, b, 0), npc.rotation, vector2_3, npc.scale, effects, 0.0f);
		}
		public override void FindFrame(int frameHeight)
		{
			const int Frame_1 = 0;
			const int Frame_2 = 1;
			const int Frame_3 = 2;
			const int Frame_4 = 3;
			const int Frame_5 = 4;
			const int Frame_6 = 5;
			const int Frame_7 = 6;
			const int Frame_8 = 7;
			const int Frame_9 = 8;
			const int Frame_10 = 9;
			const int Frame_11 = 10;

			Player player = Main.player[npc.target];
			npc.frameCounter++;
			if (!isRoaring)
			{
				if (npc.velocity.Y == 0f)
				{
					if (npc.velocity.X != 0f)
					{
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
						else if (npc.frameCounter < 30)
						{
							npc.frame.Y = Frame_5 * frameHeight;
						}
						else if (npc.frameCounter < 36)
						{
							npc.frame.Y = Frame_6 * frameHeight;
						}
						else if (npc.frameCounter < 42)
						{
							npc.frame.Y = Frame_7 * frameHeight;
						}
						else
						{
							npc.frameCounter = 0;
						}
					}
				}
				else
				{
					npc.frame.Y = Frame_5 * frameHeight;
				}
			}
			else
			{
				if (npc.frameCounter < 15)
				{
					npc.frame.Y = Frame_8 * frameHeight;
				}
				else if (npc.frameCounter < 30)
				{
					npc.frame.Y = Frame_9 * frameHeight;
				}
				else if (npc.frameCounter < 45)
				{
					npc.frame.Y = Frame_10 * frameHeight;
				}
				else if (npc.frameCounter < 120)
				{
					npc.frame.Y = Frame_11 * frameHeight;
				}
				else
				{
					npc.frameCounter = 0;
				}
			}
		}
	}
}