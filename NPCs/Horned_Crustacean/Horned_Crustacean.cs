using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;
using SpiritMod.Items.Consumable.Fish;

namespace SpiritMod.NPCs.Horned_Crustacean
{
	public class Horned_Crustacean : ModNPC
	{
		public bool hasGottenColor = false;
		public int r = 0;
		public int g = 0;
		public int b = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Luminous Prowler");
			Main.npcFrameCount[npc.type] = 10;
			NPCID.Sets.TrailCacheLength[npc.type] = 10; 
			NPCID.Sets.TrailingMode[npc.type] = 0;
		}
		public override void SetDefaults()
		{
			npc.aiStyle = -1;
			npc.lifeMax = 40;
			npc.defense = 5;
			npc.value = 200f;
			npc.knockBackResist = 0.9f;
			npc.width = 20;
			npc.height = 40;
			npc.damage = 30;
			npc.lavaImmune = false;
			npc.noTileCollide = false;
			npc.HitSound = new Terraria.Audio.LegacySoundStyle(3, 31);
			npc.dontTakeDamage = false;
			npc.DeathSound = new Terraria.Audio.LegacySoundStyle(4, 32);
		}
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(r);
			writer.Write(g);
			writer.Write(b);
			writer.Write(hasGottenColor);

		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			r = reader.ReadInt32();
			g = reader.ReadInt32();
			b = reader.ReadInt32();
			hasGottenColor = reader.ReadBoolean();

		}
		public override void AI()
		{
			Player player = Main.player[npc.target];
			if (npc.velocity.X < 0f)
			{
				npc.spriteDirection = -1;
			}
			else if (npc.velocity.X > 0f)
			{
				npc.spriteDirection = 1;
			}
			if (Vector2.Distance(player.Center, npc.Center) <= 45f)
			{
				npc.velocity.X = 0f;
			}
			
			if (npc.wet && !player.wet)
			{
				npc.noGravity = true;
				npc.aiStyle = 16;
				aiType = NPCID.Goldfish;
				npc.TargetClosest(false);
			}
			else
			{
				npc.noGravity = false;
				npc.aiStyle = 0;
			}
			
			if (player.wet && npc.wet)
				Movement();
			
			if (!hasGottenColor)
			{
				hasGottenColor = true;
				r = Main.rand.Next(1,255);
				g = Main.rand.Next(1,255);
				b = Main.rand.Next(1,255);
			}
			Lighting.AddLight(new Vector2(npc.Center.X, npc.Center.Y), r*0.002f, g*0.002f, b*0.002f);
			
			if (!player.wet)
			{
				for (int i = 0; i < Main.projectile.Length; i++)
				{
					Projectile type = Main.projectile[i];

					if (Vector2.Distance(type.Center, npc.Center) <= 100f && Vector2.Distance(type.Center, npc.Center) > 20f && type.friendly && type.position.X > npc.position.X && npc.wet && type.active)
					{
						Vector2 vector2 = new Vector2(npc.position.X + (float) npc.width * 0.5f, npc.position.Y + (float) npc.height * 0.5f);
						float num2 = type.position.X + Main.rand.Next(-10,10) + (float) (type.width / 2) - vector2.X;
						float num3 = type.position.Y + Main.rand.Next(-10,10) + (float) (type.height / 2) - vector2.Y;
						float num4 = 8f / (float) Math.Sqrt(num2 * num2 + num3 * num3);
						npc.velocity.X = num2 * num4 * -1 * (5f / 6);
						npc.velocity.Y = num3 * num4 * -1 * (5f / 6);
						npc.spriteDirection = -1;
						npc.direction = -1;
					}
					else if (Vector2.Distance(type.Center, npc.Center) <= 100f && Vector2.Distance(type.Center, npc.Center) > 20f && type.friendly && type.position.X < npc.position.X && npc.wet && type.active)
					{
						Vector2 vector2 = new Vector2(npc.position.X + (float) npc.width * 0.5f, npc.position.Y + (float) npc.height * 0.5f);
						float num2 = type.position.X + Main.rand.Next(-10,10) + (float) (type.width / 2) - vector2.X;
						float num3 = type.position.Y + Main.rand.Next(-10,10) + (float) (type.height / 2) - vector2.Y;
						float num4 = 8f / (float) Math.Sqrt(num2 * num2 + num3 * num3);
						npc.velocity.X = num2 * num4 * -1 * (5f / 6);
						npc.velocity.Y = num3 * num4 * -1 * (5f / 6);
						npc.spriteDirection = 1;
						npc.direction = 1;
					}
				}
			}
		}
		private void Movement()
		{
			npc.aiStyle = -1;
			npc.noGravity = true;
			if (!npc.noTileCollide)
			{
				if (npc.collideX)
				{
					npc.velocity.X = npc.oldVelocity.X * -0.5f;
					if (npc.direction == -1 && npc.velocity.X > 0.0 && npc.velocity.X < 2.0)
					{
						npc.velocity.X = 2f;
					}

					if (npc.direction == 1 && npc.velocity.X < 0.0 && npc.velocity.X > -2.0)
					{
						npc.velocity.X = -2f;
					}
				}
				if (npc.collideY)
				{
					npc.velocity.Y = npc.oldVelocity.Y * -0.5f;
					if (npc.velocity.Y > 0.0 && npc.velocity.Y < 1.0)
					{
						npc.velocity.Y = 1f;
					}

					if (npc.velocity.Y < 0.0 && npc.velocity.Y > -1.0)
					{
						npc.velocity.Y = -1f;
					}
				}
			}
			npc.TargetClosest(true);
			if (Collision.CanHit(npc.position, npc.width, npc.height, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height))
			{
				if (npc.ai[1] > 0.0 && !Collision.SolidCollision(npc.position, npc.width, npc.height))
				{
					npc.ai[1] = 0.0f;
					npc.ai[0] = 0.0f;
					npc.netUpdate = true;
				}
			}
			else if (npc.ai[1] == 0.0)
			{
				++npc.ai[0];
			}

			if (npc.ai[0] >= 300.0)
			{
				npc.ai[1] = 1f;
				npc.ai[0] = 0.0f;
				npc.netUpdate = true;
			}
			if (npc.ai[1] == 0.0)
			{
				npc.alpha = 0;
				npc.noTileCollide = false;
			}
			else
			{
				npc.alpha = 200;
				npc.noTileCollide = true;
			}
			npc.TargetClosest(true);
			if (npc.direction == -1 && npc.velocity.X > -4 && npc.position.X > Main.player[npc.target].position.X + Main.player[npc.target].width)
			{
				npc.velocity.X -= 0.08f;
				if (npc.velocity.X > 4)
				{
					npc.velocity.X -= 0.04f;
				}
				else if (npc.velocity.X > 0.0)
				{
					npc.velocity.X -= 0.2f;
				}

				if (npc.velocity.X < -4)
				{
					npc.velocity.X = -4f;
				}
			}
			else if (npc.direction == 1 && npc.velocity.X < 4 && npc.position.X + npc.width < Main.player[npc.target].position.X)
			{
				npc.velocity.X += 0.08f;
				if (npc.velocity.X < -4)
				{
					npc.velocity.X += 0.04f;
				}
				else if (npc.velocity.X < 0.0)
				{
					npc.velocity.X += 0.2f;
				}

				if (npc.velocity.X > 4)
				{
					npc.velocity.X = 4f;
				}
			}
			if (npc.directionY == -1 && npc.velocity.Y > -4 && npc.position.Y > Main.player[npc.target].position.Y + Main.player[npc.target].height)
			{
				npc.velocity.Y -= 0.1f;
				if (npc.velocity.Y > 4)
				{
					npc.velocity.Y -= 0.05f;
				}
				else if (npc.velocity.Y > 0.0)
				{
					npc.velocity.Y -= 0.15f;
				}

				if (npc.velocity.Y < -4)
				{
					npc.velocity.Y = -4f;
				}
			}
			else if (npc.directionY == 1 && npc.velocity.Y < 4 && npc.position.Y + npc.height < Main.player[npc.target].position.Y)
			{
				npc.velocity.Y += 0.1f;
				if (npc.velocity.Y < -4)
				{
					npc.velocity.Y += 0.05f;
				}
				else if (npc.velocity.Y < 0.0)
				{
					npc.velocity.Y += 0.15f;
				}

				if (npc.velocity.Y > 4)
				{
					npc.velocity.Y = 4f;
				}
			}
		}
		public override void NPCLoot()
		{
			if (Main.rand.Next(25) == 0)
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("LuminanceSeacone"), 1);
			}
			if (Main.rand.Next(2) == 1)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<RawFish>(), 1);
			}
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				for (int index1 = 0; index1 < 13; ++index1)
				{
				  int index2 = Dust.NewDust(npc.position, npc.width, npc.height, DustID.AncientLight, npc.velocity.X * 0.5f, npc.velocity.Y * 0.5f, 90, new Color(r,g,b), 2.5f);
				  Main.dust[index2].noGravity = true;
				  Main.dust[index2].fadeIn = 1f;
				  Main.dust[index2].velocity *= 4f;
				  Main.dust[index2].noLight = true;
				}
			}
			for (int k = 0; k < 5; k++)
			{
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.AncientLight, 2.5f * hitDirection, -2.5f, 0, new Color(r,g,b), 1.2f);
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.AncientLight, 2.5f * hitDirection, -2.5f, 0, new Color(r,g,b), 0.5f);
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.AncientLight, 2.5f * hitDirection, -2.5f, 0, new Color(r,g,b), 0.7f);
			}
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo) => SpawnCondition.OceanMonster.Chance * 0.08f;
		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor) => false;
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Vector2 vector2_3 = new Vector2((float) (Main.npcTexture[npc.type].Width / 2), (float) (Main.npcTexture[npc.type].Height / Main.npcFrameCount[npc.type] / 2));

			float addHeight = 10f;

			Color color1 = Lighting.GetColor((int) (npc.position.X + npc.width * 0.5) / 16, (int) ((npc.position.Y + npc.height * 0.5) / 16.0));

			var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame, drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);

			Main.spriteBatch.Draw(mod.GetTexture("NPCs/Horned_Crustacean/Horned_Crustacean_Glow"), npc.Bottom - Main.screenPosition + new Vector2((float) (-Main.npcTexture[npc.type].Width * npc.scale / 2.0 + vector2_3.X * npc.scale), (float) (-Main.npcTexture[npc.type].Height * npc.scale / Main.npcFrameCount[npc.type] + 4.0 + vector2_3.Y * npc.scale) + addHeight + npc.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(npc.frame), new Microsoft.Xna.Framework.Color((int) r - npc.alpha, (int) byte.MaxValue - npc.alpha, (int) g - npc.alpha, (int) b - npc.alpha), npc.rotation, vector2_3, npc.scale, effects, 0.0f);
          
			float num = (float) (0.25f + (npc.GetAlpha(color1).ToVector3() - new Vector3(4f)).Length() * 0.25f);
            for (int index = 0; index < 4; ++index)
                Main.spriteBatch.Draw(mod.GetTexture("NPCs/Horned_Crustacean/Horned_Crustacean_Glow"), npc.Bottom - Main.screenPosition + new Vector2((float) (-Main.npcTexture[npc.type].Width * npc.scale / 2.0 + vector2_3.X * npc.scale), (float) (-Main.npcTexture[npc.type].Height * npc.scale / Main.npcFrameCount[npc.type] + 4.0 + vector2_3.Y * npc.scale) + addHeight + npc.gfxOffY) + npc.velocity.RotatedBy(index * 47079637050629, new Vector2()) * num, new Microsoft.Xna.Framework.Rectangle?(npc.frame), new Color(r, g, b, 0), npc.rotation, vector2_3, npc.scale, effects, 0.0f);
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

			Player player = Main.player[npc.target];
			npc.frameCounter++;
			if (Vector2.Distance(player.Center, npc.Center) <= 45f && npc.velocity.X == 0f)
			{
				if (npc.frameCounter == 24 && Collision.CanHitLine(npc.Center, 0, 0, Main.player[npc.target].Center, 0, 0))
				{
					player.Hurt(PlayerDeathReason.LegacyDefault(), (int)(npc.damage * 1.5f), npc.direction, false, false, false, -1);
					npc.frame.Y = Frame_10 * frameHeight;
				}
				npc.velocity.X = 0f; ;
				if (npc.frameCounter < 5)
				{
					npc.frame.Y = Frame_6 * frameHeight;
				}
				else if (npc.frameCounter < 10)
				{
					npc.frame.Y = Frame_7 * frameHeight;
				}
				else if (npc.frameCounter < 15)
				{
					npc.frame.Y = Frame_8 * frameHeight;
				}
				else if (npc.frameCounter < 20)
				{
					npc.frame.Y = Frame_9 * frameHeight;
				}
				else if (npc.frameCounter < 25)
				{
					npc.frame.Y = Frame_10 * frameHeight;
				}
				else
				{
					npc.frameCounter = 0;
				}
			}
			else
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
				else
				{
					npc.frameCounter = 0;
				}
			}
		}
	}
}
