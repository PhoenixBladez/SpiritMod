using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.ModLoader.Utilities;

namespace SpiritMod.NPCs.Mangrove_Defender
{
	public class Mangrove_Defender : ModNPC
	{

		public bool groundSlamming;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mangrove Defender");
			Main.npcFrameCount[NPC.type] = 14;
			NPCID.Sets.TrailCacheLength[NPC.type] = 20;
			NPCID.Sets.TrailingMode[NPC.type] = 0;
		}
		public override void SetDefaults()
		{
			NPC.aiStyle = -1;
			NPC.lifeMax = 240;
			NPC.defense = 25;
			NPC.value = 890f;
			AIType = 0;
			NPC.knockBackResist = 0.1f;
			NPC.width = 30;
			NPC.height = 62;
			NPC.damage = 45;
			NPC.lavaImmune = false;
			NPC.noTileCollide = false;
			NPC.alpha = 0;
			NPC.HitSound = new Terraria.Audio.LegacySoundStyle(6, 1);
			NPC.dontTakeDamage = false;
			NPC.DeathSound = SoundID.NPCDeath6;
		}
		/*public override void ModifyHitByItem(Player player, Item item, ref int damage, ref float knockback, ref bool crit)
		{
			if (player.HeldItem.axe > 0)		
				damage*=2;
		}*/
		public override void AI()
		{
			Player player = Main.player[NPC.target];

			NPC.TargetClosest(true);

			NPC.spriteDirection = NPC.direction;

			if (Vector2.Distance(NPC.Center, player.Center) < 600f && !groundSlamming)
			{
				NPC.ai[1]++;
			}
			if (NPC.ai[1] >= 280)
			{
				groundSlamming = true;
				slam();
			}
			else
			{
				Movement();
				groundSlamming = false;
			}
		}

		public void slam()
		{
			Player player = Main.player[NPC.target];
			NPC.ai[1]++;
			NPC.velocity.X = 0f;

			if (NPC.ai[1] == 280)
			{
				NPC.frameCounter = 0;
				NPC.netUpdate = true;
			}

			if (NPC.ai[1] == 281)
				SoundEngine.PlaySound(SoundID.Trackable, (int)NPC.position.X, (int)NPC.position.Y, 143, 1f, -0.9f);

			if (NPC.ai[1] >= 321)
			{
				Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, 4f * NPC.spriteDirection, 0f, ModContent.ProjectileType<Earth_Slam_Invisible>(), 0, 0, player.whoAmI);
				NPC.ai[1] = 0;
				NPC.netUpdate = true;
			}
		}

		public void Movement()
		{
			int num1 = 30;
			int num2 = 10;
			bool flag1 = false;
			bool flag2 = false;
			bool flag3 = false;
			if (NPC.velocity.Y == 0 && (NPC.velocity.X > 0.0 && NPC.direction < 0 || NPC.velocity.X < 0.0 && NPC.direction > 0))
			{
				flag2 = true;
				++NPC.ai[3];
			}
			if (NPC.position.X == NPC.oldPosition.X || NPC.ai[3] >= num1 || flag2)
			{
				++NPC.ai[3];
				flag3 = true;
			}
			else if (NPC.ai[3] > 0)
				--NPC.ai[3];

			if (NPC.ai[3] > (num1 * num2))
				NPC.ai[3] = 0.0f;

			if (NPC.justHit)
				NPC.ai[3] = 0.0f;

			if (NPC.ai[3] == num1)
				NPC.netUpdate = true;

			Vector2 vector2_1 = new Vector2(NPC.position.X + NPC.width * 0.5f, NPC.position.Y + NPC.height * 0.5f);
			float num3 = Main.player[NPC.target].position.X + Main.player[NPC.target].width * 0.5f - vector2_1.X;
			float num4 = Main.player[NPC.target].position.Y - vector2_1.Y;
			float num5 = (float)Math.Sqrt(num3 * num3 + num4 * num4);
			if (num5 < 200.0 && !flag3)
				NPC.ai[3] = 0.0f;

			if (NPC.ai[3] < num1)
				NPC.TargetClosest(true);
			else
			{
				if (NPC.velocity.X == 0.0)
				{
					if (NPC.velocity.Y == 0.0)
					{
						++NPC.ai[0];
						if (NPC.ai[0] >= 2.0)
						{
							NPC.direction *= -1;
							NPC.ai[0] = 0.0f;
						}
					}
				}
				else
					NPC.ai[0] = 0.0f;

				NPC.directionY = -1;
				if (NPC.direction == 0)
				{
					NPC.direction = 1;
				}
			}
			float num6 = 2.5f; //walking speed
			float num7 = 1.39f; //regular speed (x)
			if (!flag1 && ((double)NPC.velocity.Y == 0.0 || NPC.wet || (double)NPC.velocity.X <= 0.0 && NPC.direction < 0 || (double)NPC.velocity.X >= 0.0 && NPC.direction > 0))
			{
				if ((double)NPC.velocity.X < -(double)num6 || (double)NPC.velocity.X > (double)num6)
				{
					if ((double)NPC.velocity.Y == 0.0)
					{
						Vector2 vector2_2 = NPC.velocity * 0.5f; ///////SLIDE SPEED
						NPC.velocity = vector2_2;
					}
				}
				else if ((double)NPC.velocity.X < (double)num6 && NPC.direction == 1)
				{
					NPC.velocity.X += num7;
					if ((double)NPC.velocity.X > (double)num6)
					{
						NPC.velocity.X = num6;
					}
				}
				else if ((double)NPC.velocity.X > -(double)num6 && NPC.direction == -1)
				{
					NPC.velocity.X -= num7;
					if ((double)NPC.velocity.X < -(double)num6)
					{
						NPC.velocity.X = -num6;
					}
				}
			}
			if ((double)NPC.velocity.Y >= 0.0)
			{
				int num8 = 0;
				if ((double)NPC.velocity.X < 0.0)
				{
					num8 = -1;
				}

				if ((double)NPC.velocity.X > 0.0)
				{
					num8 = 1;
				}

				Vector2 position = NPC.position;
				position.X += NPC.velocity.X;
				int index1 = (int)(((double)position.X + (double)(NPC.width / 2) + (double)((NPC.width / 2 + 1) * num8)) / 16.0);
				int index2 = (int)(((double)position.Y + (double)NPC.height - 1.0) / 16.0);

				if ((double)(index1 * 16) < (double)position.X + (double)NPC.width && (double)(index1 * 16 + 16) > (double)position.X && (Main.tile[index1, index2].HasUnactuatedTile && !Main.tile[index1, index2].TopSlope && (!Main.tile[index1, index2 - 1].TopSlope && Main.tileSolid[(int)Main.tile[index1, index2].TileType]) && !Main.tileSolidTop[(int)Main.tile[index1, index2].TileType] || Main.tile[index1, index2 - 1].IsHalfBlock && Main.tile[index1, index2 - 1].HasUnactuatedTile) && ((!Main.tile[index1, index2 - 1].HasUnactuatedTile || !Main.tileSolid[(int)Main.tile[index1, index2 - 1].TileType] || Main.tileSolidTop[(int)Main.tile[index1, index2 - 1].TileType] || Main.tile[index1, index2 - 1].IsHalfBlock && (!Main.tile[index1, index2 - 4].HasUnactuatedTile || !Main.tileSolid[(int)Main.tile[index1, index2 - 4].TileType] || Main.tileSolidTop[(int)Main.tile[index1, index2 - 4].TileType])) && ((!Main.tile[index1, index2 - 2].HasUnactuatedTile || !Main.tileSolid[(int)Main.tile[index1, index2 - 2].TileType] || Main.tileSolidTop[(int)Main.tile[index1, index2 - 2].TileType]) && (!Main.tile[index1, index2 - 3].HasUnactuatedTile || !Main.tileSolid[(int)Main.tile[index1, index2 - 3].TileType] || Main.tileSolidTop[(int)Main.tile[index1, index2 - 3].TileType]) && (!Main.tile[index1 - num8, index2 - 3].HasUnactuatedTile || !Main.tileSolid[(int)Main.tile[index1 - num8, index2 - 3].TileType]))))
				{
					float num9 = (float)(index2 * 16);
					if (Main.tile[index1, index2].IsHalfBlock)
					{
						num9 += 8f;
					}

					if (Main.tile[index1, index2 - 1].IsHalfBlock)
					{
						num9 -= 8f;
					}

					if ((double)num9 < (double)position.Y + (double)NPC.height)
					{
						float num10 = position.Y + (float)NPC.height - num9;
						if ((double)num10 <= 16.1)
						{
							NPC.gfxOffY += NPC.position.Y + (float)NPC.height - num9;
							NPC.position.Y = num9 - (float)NPC.height;
							NPC.stepSpeed = (double)num10 >= 9.0 ? 4f : 2f;
						}
					}
				}
			}
			if ((double)NPC.velocity.Y == 0.0)
			{
				int index1 = (int)(((double)NPC.position.X + (double)(NPC.width / 2) + (double)(15 * NPC.direction)) / 16.0);
				int index2 = (int)(((double)NPC.position.Y + (double)NPC.height - 15.0) / 16.0);

				Tile tile = Main.tile[index1, index2 + 1];
				int spriteDirection = NPC.spriteDirection;
				if ((double)NPC.velocity.X < 0.0 || (double)NPC.velocity.X > 0.0)
				{
					if (NPC.height >= 24 && Main.tile[index1, index2 - 2].HasUnactuatedTile && Main.tileSolid[(int)Main.tile[index1, index2 - 2].TileType])
					{
						if (Main.tile[index1, index2 - 3].HasUnactuatedTile && Main.tileSolid[(int)Main.tile[index1, index2 - 3].TileType])
						{
							NPC.velocity.Y = -8f;
							NPC.netUpdate = true;
						}
						else
						{
							NPC.velocity.Y = -8f;
							NPC.netUpdate = true;
						}
					}
					else if (Main.tile[index1, index2 - 1].HasUnactuatedTile && Main.tileSolid[(int)Main.tile[index1, index2 - 1].TileType])
					{
						NPC.velocity.Y = -8f;
						NPC.netUpdate = true;
					}
					else if ((double)NPC.position.Y + (double)NPC.height - (double)(index2 * 16) > 10.0 && Main.tile[index1, index2].HasUnactuatedTile && (!Main.tile[index1, index2].TopSlope && Main.tileSolid[(int)Main.tile[index1, index2].TileType]))
					{
						NPC.velocity.Y = -8f;
						NPC.netUpdate = true;
					}
					else if (NPC.directionY < 0 && NPC.type != NPCID.Crab && (!Main.tile[index1, index2 + 1].HasUnactuatedTile || !Main.tileSolid[(int)Main.tile[index1, index2 + 1].TileType]) && (!Main.tile[index1 + NPC.direction, index2 + 1].HasUnactuatedTile || !Main.tileSolid[(int)Main.tile[index1 + NPC.direction, index2 + 1].TileType]))
					{
						NPC.velocity.Y = -8f;
						NPC.velocity.X *= 1.5f;
						NPC.netUpdate = true;
					}
					if ((double)NPC.velocity.Y == 0.0 && flag1 && (double)NPC.ai[3] == 1.0)
					{
						NPC.velocity.Y = -8f;
					}
				}
			}
			if (NPC.velocity.X < 0.3f && NPC.velocity.X > -0.3f)
			{
				NPC.velocity.Y = -5f;
				NPC.velocity.X += 3f * (float)NPC.direction;
			}
		}
		public override void OnKill()
		{

		}
		public override void HitEffect(int hitDirection, double damage)
		{
			for (int i = 0; i < 4; i++)
			{
				float goreScale = 0.01f * Main.rand.Next(20, 70);
				int a = Gore.NewGore(NPC.GetSource_OnHurt(null), new Vector2(NPC.position.X, NPC.position.Y + (Main.rand.Next(-50, 10))), new Vector2(hitDirection * 3f, 0f), 386, goreScale);
				Main.gore[a].timeLeft = 5;
			}
			for (int i = 0; i < 4; i++)
			{
				float goreScale = 0.01f * Main.rand.Next(20, 70);
				int a = Gore.NewGore(NPC.GetSource_OnHurt(null), new Vector2(NPC.position.X, NPC.position.Y + (Main.rand.Next(-50, 10))), new Vector2(hitDirection * 3f, 0f), 387, goreScale);
				Main.gore[a].timeLeft = 5;
			}
			if (NPC.life <= 0)
			{
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/MangroveDefender/ForestSentryGore1").Type, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/MangroveDefender/ForestSentryGore2").Type, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/MangroveDefender/ForestSentryGore3").Type, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/MangroveDefender/ForestSentryGore4").Type, 1f);
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			if (groundSlamming)
			{
				int num7 = 16;
				float num8 = (float)(Math.Cos((double)Main.GlobalTimeWrappedHourly % 2.4 / 2.4 * MathHelper.TwoPi) / 4.0 + 0.5);
				float num10 = 0.0f;
				SpriteEffects spriteEffects = SpriteEffects.None;
				if (NPC.spriteDirection == 1)
					spriteEffects = SpriteEffects.FlipHorizontally;
				Texture2D texture = TextureAssets.Npc[NPC.type].Value;
				Vector2 vector2_3 = new Vector2((float)(TextureAssets.Npc[NPC.type].Value.Width / 2), (float)(TextureAssets.Npc[NPC.type].Value.Height / Main.npcFrameCount[NPC.type] / 2));
				Vector2 position1 = NPC.Center - Main.screenPosition - new Vector2((float)texture.Width, (float)(texture.Height / Main.npcFrameCount[NPC.type])) * NPC.scale / 2f + vector2_3 * NPC.scale + new Vector2(0.0f, -10f + NPC.gfxOffY);
				Color color2 = new Color((int)sbyte.MaxValue - NPC.alpha, (int)sbyte.MaxValue - NPC.alpha, (int)sbyte.MaxValue - NPC.alpha, 0).MultiplyRGBA(Color.Green);
				for (int index2 = 0; index2 < num7; ++index2)
				{
					Color color3 = NPC.GetAlpha(color2) * (1f - num8) * 1.2f;
					Vector2 position2 = new Vector2(NPC.Center.X, NPC.Center.Y - 2) + ((float)(index2 / (double)num7 * 6.28318548202515) + NPC.rotation + num10).ToRotationVector2() * (float)(2.0 * (double)num8 + 2.0) - Main.screenPosition - new Vector2((float)texture.Width, (float)(texture.Height / Main.npcFrameCount[NPC.type])) * NPC.scale / 2f + vector2_3 * NPC.scale + new Vector2(0f, -10 + NPC.gfxOffY);
					Main.spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, position2, new Microsoft.Xna.Framework.Rectangle?(NPC.frame), color3, NPC.rotation, vector2_3, NPC.scale * 1.1f, spriteEffects, 0.0f);
				}
				Main.spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, position1, new Microsoft.Xna.Framework.Rectangle?(NPC.frame), color2, NPC.rotation, vector2_3, NPC.scale * 1.1f, spriteEffects, 0.0f);
			}
			return true;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			float addHeight = 0f;
			SpriteEffects spriteEffects = SpriteEffects.None;
			if (NPC.spriteDirection == 1)
				spriteEffects = SpriteEffects.FlipHorizontally;
			Vector2 vector2_3 = new Vector2((float)(TextureAssets.Npc[NPC.type].Value.Width / 2), (float)(TextureAssets.Npc[NPC.type].Value.Height / Main.npcFrameCount[NPC.type] / 2));
			Main.spriteBatch.Draw(Mod.Assets.Request<Texture2D>("NPCs/Mangrove_Defender/Mangrove_Defender_Glow").Value, new Vector2((float)((double)NPC.position.X - (double)Main.screenPosition.X + (double)(NPC.width / 2) - (double)TextureAssets.Npc[NPC.type].Value.Width * (double)NPC.scale / 2.0 + (double)vector2_3.X * (double)NPC.scale), (float)((double)NPC.position.Y - (double)Main.screenPosition.Y + (double)NPC.height - (double)TextureAssets.Npc[NPC.type].Value.Height * (double)NPC.scale / (double)Main.npcFrameCount[NPC.type] + 4.0 + (double)vector2_3.Y * (double)NPC.scale) + addHeight), new Microsoft.Xna.Framework.Rectangle?(NPC.frame), Microsoft.Xna.Framework.Color.White, NPC.rotation, vector2_3, NPC.scale, spriteEffects, 0.0f);
			if (NPC.velocity.Y != 0f)
			{
				for (int index = 1; index < 20; ++index)
				{
					Microsoft.Xna.Framework.Color color2 = new Microsoft.Xna.Framework.Color(255 - index * 10, 110 - index * 10, 110 - index * 10, 110 - index * 10);
					Main.spriteBatch.Draw(Mod.Assets.Request<Texture2D>("NPCs/Mangrove_Defender/Mangrove_Defender_Glow").Value, new Vector2((float)((double)NPC.position.X - (double)Main.screenPosition.X + (double)(NPC.width / 2) - (double)TextureAssets.Npc[NPC.type].Value.Width * (double)NPC.scale / 2.0 + (double)vector2_3.X * (double)NPC.scale), (float)((double)NPC.position.Y - (double)Main.screenPosition.Y + (double)NPC.height - (double)TextureAssets.Npc[NPC.type].Value.Height * (double)NPC.scale / (double)Main.npcFrameCount[NPC.type] + 4.0 + (double)vector2_3.Y * (double)NPC.scale) + addHeight) - NPC.velocity * (float)index * 0.5f, new Microsoft.Xna.Framework.Rectangle?(NPC.frame), color2, NPC.rotation, vector2_3, NPC.scale, spriteEffects, 0.0f);
				}
			}

			if (groundSlamming)
			{
				for (int i = 0; i < 1; i++)
				{
					float num8 = (float)(Math.Cos(Main.GlobalTimeWrappedHourly % 2.4 / 2.4 * MathHelper.TwoPi) / 4.0 + 0.5);
					float num10 = 0.0f;
					float addY = 0f;
					spriteEffects = SpriteEffects.None;
					if (NPC.spriteDirection == 1)
						spriteEffects = SpriteEffects.FlipHorizontally;
					Texture2D texture = TextureAssets.Npc[NPC.type].Value;
					vector2_3 = new Vector2((TextureAssets.Npc[NPC.type].Value.Width / 2), (TextureAssets.Npc[NPC.type].Value.Height / Main.npcFrameCount[NPC.type] / 2));
					Vector2 position1 = NPC.Center - Main.screenPosition - new Vector2(texture.Width, (texture.Height / Main.npcFrameCount[NPC.type])) * NPC.scale / 2f + vector2_3 * NPC.scale + new Vector2(0.0f, addY + addHeight + NPC.gfxOffY);
					Color color2 = Color.Green;
					for (int index2 = 0; index2 < 4; ++index2)
					{
						Color color3 = NPC.GetAlpha(color2) * (1f - num8);
						Vector2 position2 = new Vector2(NPC.Center.X + 2 * NPC.spriteDirection, NPC.Center.Y) + ((float)(index2 / 4 * 6.28318548202515) + NPC.rotation + num10).ToRotationVector2() * (float)(4.0 * (double)num8 + 2.0) - Main.screenPosition - new Vector2((float)texture.Width, (float)(texture.Height / Main.npcFrameCount[NPC.type])) * NPC.scale / 2f + vector2_3 * NPC.scale + new Vector2(0.0f, addY + -10 + NPC.gfxOffY);
						Main.spriteBatch.Draw(Mod.Assets.Request<Texture2D>("NPCs/Mangrove_Defender/Mangrove_Defender_Glow").Value, position2, new Microsoft.Xna.Framework.Rectangle?(NPC.frame), color3, NPC.rotation, vector2_3, NPC.scale * 1.15f, spriteEffects, 0.0f);
					}
					Main.spriteBatch.Draw(Mod.Assets.Request<Texture2D>("NPCs/Mangrove_Defender/Mangrove_Defender_Glow").Value, position1, new Microsoft.Xna.Framework.Rectangle?(NPC.frame), color2, NPC.rotation, vector2_3, NPC.scale * 1.15f, spriteEffects, 0.0f);
				}
			}
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			Player player = spawnInfo.Player;
			if (!(player.ZoneTowerSolar || player.ZoneTowerVortex || player.ZoneTowerNebula || player.ZoneTowerStardust) && ((!Main.pumpkinMoon && !Main.snowMoon) || spawnInfo.SpawnTileY > Main.worldSurface || Main.dayTime) && (!Main.eclipse || spawnInfo.SpawnTileY > Main.worldSurface || !Main.dayTime) && (SpawnCondition.GoblinArmy.Chance == 0))
				return spawnInfo.Player.GetSpiritPlayer().ZoneReach && Main.hardMode ? 2.1f : 0f;
			return 0f;
		}

		public override void FindFrame(int frameHeight)
		{
			NPC.frameCounter++;
			if (NPC.velocity.Y != 0f)
				NPC.frame.Y = 13 * frameHeight;
			else
			{
				if (groundSlamming)
				{
					if (NPC.frameCounter < 8)
						NPC.frame.Y = 6 * frameHeight;
					else if (NPC.frameCounter < 16)
						NPC.frame.Y = 7 * frameHeight;
					else if (NPC.frameCounter < 24)
						NPC.frame.Y = 8 * frameHeight;
					else if (NPC.frameCounter < 32)
						NPC.frame.Y = 9 * frameHeight;
					else if (NPC.frameCounter < 40)
						NPC.frame.Y = 10 * frameHeight;
					else if (NPC.frameCounter < 48)
						NPC.frame.Y = 11 * frameHeight;
					else if (NPC.frameCounter < 56)
						NPC.frame.Y = 12 * frameHeight;
					else
						NPC.frameCounter = 0;
				}
				else
				{
					if (NPC.frameCounter < 7)
						NPC.frame.Y = 0 * frameHeight;
					else if (NPC.frameCounter < 14)
						NPC.frame.Y = 1 * frameHeight;
					else if (NPC.frameCounter < 21)
						NPC.frame.Y = 2 * frameHeight;
					else if (NPC.frameCounter < 28)
						NPC.frame.Y = 3 * frameHeight;
					else if (NPC.frameCounter < 35)
						NPC.frame.Y = 4 * frameHeight;
					else if (NPC.frameCounter < 42)
						NPC.frame.Y = 5 * frameHeight;
					else
						NPC.frameCounter = 0;
				}
			}
		}
	}
}