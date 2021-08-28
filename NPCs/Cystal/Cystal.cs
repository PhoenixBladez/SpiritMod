using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;
using Terraria.ID;

namespace SpiritMod.NPCs.Cystal
{
	public class Cystal : ModNPC
	{
		public int colorNumber = 510;
		public int dustTimer = 0;
		public int healTimer = 0;
		public bool check = false;
		public static bool flag = false;
		public bool shieldSpawned = false;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cystal");
			Main.npcFrameCount[npc.type] = 8;
		}
		public override void SetDefaults()
		{
			npc.aiStyle = 0;
			npc.lifeMax = 82;
			npc.defense = 2;
			npc.value = 500f;
			npc.knockBackResist = 0.1f;
			npc.width = 40;
			npc.height = 40;
			npc.damage = 20;
			npc.lavaImmune = true;
			npc.noGravity = true;
			npc.noTileCollide = true;
			npc.HitSound = new Terraria.Audio.LegacySoundStyle(42, 193);
			npc.DeathSound = new Terraria.Audio.LegacySoundStyle(42, 182);
			/*banner = npc.type;
			bannerItem = mod.ItemType("Energy_Humanoid_Banner");*/
		}
		public override void AI()
		{
			dustTimer++;
			healTimer++;
			Player player = Main.player[npc.target];
			npc.spriteDirection = npc.direction;

			Lighting.AddLight(npc.position, 0.0149f / 4 * npc.life, 0.0142f / 4 * npc.life, 0.0207f / 4 * npc.life);
			if (npc.active && player.active && !shieldSpawned)
			{
				shieldSpawned = true;
				int shield1 = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y + (int)(npc.height / 2), mod.NPCType("Cystal_Shield"));
				Main.npc[shield1].Center += new Vector2(0, 150).RotatedBy(MathHelper.ToRadians(90));
				Main.npc[shield1].ai[1] = npc.whoAmI;

				int shield2 = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y + (int)(npc.height / 2), mod.NPCType("Cystal_Shield"));
				Main.npc[shield2].Center += new Vector2(0, 150).RotatedBy(MathHelper.ToRadians(180));
				Main.npc[shield2].ai[1] = npc.whoAmI;

				int shield3 = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y + (int)(npc.height / 2), mod.NPCType("Cystal_Shield"));
				Main.npc[shield3].Center += new Vector2(0, 150).RotatedBy(MathHelper.ToRadians(270));
				Main.npc[shield3].ai[1] = npc.whoAmI;

				int shield4 = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y + (int)(npc.height / 2), mod.NPCType("Cystal_Shield"));
				Main.npc[shield4].Center += new Vector2(0, 150).RotatedBy(MathHelper.ToRadians(360));
				Main.npc[shield4].ai[1] = npc.whoAmI;
			}
			if ((double)Vector2.Distance(npc.Center, player.Center) < (double)150f)
			{
				Filters.Scene.Activate("CystalTower", Main.player[Main.myPlayer].position);
				Filters.Scene.Activate("CystalBloodMoon", Main.player[Main.myPlayer].position);
				player.AddBuff(mod.BuffType("Crystallization"), 2);
			}
			else
			{
				Filters.Scene.Deactivate("CystalTower", Main.player[Main.myPlayer].position);
				Filters.Scene.Deactivate("CystalBloodMoon", Main.player[Main.myPlayer].position);
			}

			for (int i = 0; i < 200; i++)
			{
				NPC npc2 = Main.npc[i];
				if ((double)Vector2.Distance(npc.Center, npc2.Center) < (double)150f && healTimer % 60 == 0 && npc2.type != mod.NPCType("Cystal") && npc2.type != mod.NPCType("Cystal_Shield"))
				{
					if (npc2.life < npc2.lifeMax - 10)
					{
						npc2.life += 10;
						npc2.HealEffect(10, true);
					}
				}
				if ((double)Vector2.Distance(npc.Center, npc2.Center) < (double)1200f && npc2.type != mod.NPCType("Cystal") && npc2.type != mod.NPCType("Cystal_Shield") && npc2.life < npc2.lifeMax - 10 && npc2.type == Main.npc[(int)npc.ai[1]].type && npc2.active && !npc2.friendly)
				{
					npc.SimpleFlyMovement(npc.DirectionTo(Main.npc[(int)npc.ai[1]].Center + new Vector2(0.0f, -100f)) * 2.5f * 2f, 0.45f * 2f);
					return;
				}
			}

			if ((double)Vector2.Distance(player.Center, npc.Center) <= (double)150f && player.active)
			{
				Vector2 vector2 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
				float num2 = player.position.X + (float)(player.width / 2) - vector2.X;
				float num3 = player.position.Y + (float)(player.height / 2) - vector2.Y;
				float num4 = 8f / (float)Math.Sqrt((double)num2 * (double)num2 + (double)num3 * (double)num3);
				npc.velocity.X = num2 * num4 * -1 * (0.3f);
				npc.velocity.Y = num3 * num4 * -1 * (0.3f);
				npc.spriteDirection = -1;
				npc.direction = -1;
			}
			if ((double)Vector2.Distance(player.Center, npc.Center) <= (double)150f && (double)Vector2.Distance(player.Center, npc.Center) > (double)100f)
			{
				npc.velocity = Vector2.Zero;
			}

			if ((double)Vector2.Distance(player.Center, npc.Center) > (double)150f && player.active)
			{
				float num1 = 4f;
				float number2 = 0.25f;
				Vector2 vektor2 = new Vector2(npc.Center.X, npc.Center.Y);
				float number3 = Main.player[npc.target].Center.X - vektor2.X;
				float number4 = (float)((double)Main.player[npc.target].Center.Y - (double)vektor2.Y);
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
		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/CystalGore4"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/CystalGore3"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/CystalGore2"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/CystalGore1"), 1f);
				Filters.Scene.Deactivate("CystalTower", Main.player[Main.myPlayer].position);
				Filters.Scene.Deactivate("CystalBloodMoon", Main.player[Main.myPlayer].position);
			}
			for (int k = 0; k < 7; k++)
			{
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.ScourgeOfTheCorruptor, 2.5f * hitDirection, -2.5f, 0, default(Color), 1.2f);
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.ScourgeOfTheCorruptor, 2.5f * hitDirection, -2.5f, 0, default(Color), 0.5f);
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.PurpleCrystalShard, 2.5f * hitDirection, -2.5f, 0, default(Color), 0.7f);
			}
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (NPC.AnyNPCs(ModContent.NPCType<Cystal>()))
			{
				return 0f;
			}
			else
			{
				return SpawnCondition.Corruption.Chance * 0.065f;
			}
		}
		public override void NPCLoot()
		{
			/*if (Main.rand.Next(3) == 0)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("Demonicrystal"), Main.rand.Next(1, 3));
			}*/
			Filters.Scene.Deactivate("CystalTower", Main.player[Main.myPlayer].position);
			Filters.Scene.Deactivate("CystalBloodMoon", Main.player[Main.myPlayer].position);
			if (Main.rand.Next(2) == 0)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, 68, Main.rand.Next(1, 3));
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			if (colorNumber >= 510)
			{
				check = true;
			}
			else if (colorNumber <= 300)
			{
				check = false;
			}

			if (check)
			{
				colorNumber -= 10;
			}
			else if (!check)
			{
				colorNumber += 10;
			}
			var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame,
							 drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
			return false;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(mod.GetTexture("NPCs/Cystal/Cystal_Glow"), npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame,
							 new Color(colorNumber / 2, colorNumber / 2, colorNumber / 2), npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
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

			Player player = Main.player[npc.target];

			if (npc.active && player.active)
			{
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
				else if (npc.frameCounter < 48)
				{
					npc.frame.Y = Frame_8 * frameHeight;
				}
				else
				{
					npc.frameCounter = 0;
				}
			}
		}
	}
}