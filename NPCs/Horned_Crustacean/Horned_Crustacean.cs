using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;
using SpiritMod.Items.Consumable.Fish;
using SpiritMod.Items.Sets.ReefhunterSet;
using SpiritMod.Mechanics.QuestSystem;
using Terraria.ModLoader.Utilities;

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
			Main.npcFrameCount[NPC.type] = 10;
			NPCID.Sets.TrailCacheLength[NPC.type] = 10;
			NPCID.Sets.TrailingMode[NPC.type] = 0;
		}

		public override void SetDefaults()
		{
			NPC.aiStyle = -1;
			NPC.lifeMax = 40;
			NPC.defense = 5;
			NPC.value = 200f;
			NPC.knockBackResist = 0.9f;
			NPC.width = 20;
			NPC.height = 40;
			NPC.damage = 30;
			NPC.lavaImmune = false;
			NPC.noTileCollide = false;
			NPC.HitSound = new Terraria.Audio.LegacySoundStyle(3, 31);
			NPC.dontTakeDamage = false;
			NPC.DeathSound = new Terraria.Audio.LegacySoundStyle(4, 32);
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
			Player player = Main.player[NPC.target];

			if (NPC.velocity.X < 0f)
				NPC.spriteDirection = -1;
			else if (NPC.velocity.X > 0f)
				NPC.spriteDirection = 1;

			if (Vector2.Distance(player.Center, NPC.Center) <= 45f)
				NPC.velocity.X = 0f;

			if (NPC.wet && !player.wet)
			{
				NPC.noGravity = true;
				NPC.aiStyle = 16;
				AIType = NPCID.Goldfish;
				NPC.TargetClosest(false);
			}
			else
			{
				NPC.noGravity = false;
				NPC.aiStyle = 0;
			}

			if (player.wet && NPC.wet)
				Movement();

			if (!hasGottenColor)
			{
				hasGottenColor = true;
				r = Main.rand.Next(1, 255);
				g = Main.rand.Next(1, 255);
				b = Main.rand.Next(1, 255);
			}

			Lighting.AddLight(new Vector2(NPC.Center.X, NPC.Center.Y), r * 0.002f, g * 0.002f, b * 0.002f);

			if (!player.wet)
			{
				for (int i = 0; i < Main.projectile.Length; i++)
				{
					Projectile type = Main.projectile[i];

					if (Vector2.Distance(type.Center, NPC.Center) <= 100f && Vector2.Distance(type.Center, NPC.Center) > 20f && type.friendly && type.position.X > NPC.position.X && NPC.wet && type.active)
					{
						Vector2 vector2 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
						float num2 = type.position.X + Main.rand.Next(-10, 10) + (float)(type.width / 2) - vector2.X;
						float num3 = type.position.Y + Main.rand.Next(-10, 10) + (float)(type.height / 2) - vector2.Y;
						float num4 = 8f / (float)Math.Sqrt(num2 * num2 + num3 * num3);
						NPC.velocity.X = num2 * num4 * -1 * (5f / 6);
						NPC.velocity.Y = num3 * num4 * -1 * (5f / 6);
						NPC.spriteDirection = -1;
						NPC.direction = -1;
					}
					else if (Vector2.Distance(type.Center, NPC.Center) <= 100f && Vector2.Distance(type.Center, NPC.Center) > 20f && type.friendly && type.position.X < NPC.position.X && NPC.wet && type.active)
					{
						Vector2 vector2 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
						float num2 = type.position.X + Main.rand.Next(-10, 10) + (float)(type.width / 2) - vector2.X;
						float num3 = type.position.Y + Main.rand.Next(-10, 10) + (float)(type.height / 2) - vector2.Y;
						float num4 = 8f / (float)Math.Sqrt(num2 * num2 + num3 * num3);
						NPC.velocity.X = num2 * num4 * -1 * (5f / 6);
						NPC.velocity.Y = num3 * num4 * -1 * (5f / 6);
						NPC.spriteDirection = 1;
						NPC.direction = 1;
					}
				}
			}
		}

		private void Movement()
		{
			NPC.aiStyle = -1;
			NPC.noGravity = true;
			if (!NPC.noTileCollide)
			{
				if (NPC.collideX)
				{
					NPC.velocity.X = NPC.oldVelocity.X * -0.5f;
					if (NPC.direction == -1 && NPC.velocity.X > 0 && NPC.velocity.X < 2)
						NPC.velocity.X = 2f;

					if (NPC.direction == 1 && NPC.velocity.X < 0 && NPC.velocity.X > -2)
						NPC.velocity.X = -2f;
				}

				if (NPC.collideY)
				{
					NPC.velocity.Y = NPC.oldVelocity.Y * -0.5f;
					if (NPC.velocity.Y > 0 && NPC.velocity.Y < 1)
						NPC.velocity.Y = 1f;

					if (NPC.velocity.Y < 0 && NPC.velocity.Y > -1)
						NPC.velocity.Y = -1f;
				}
			}

			NPC.TargetClosest(true);

			if (Collision.CanHit(NPC.position, NPC.width, NPC.height, Main.player[NPC.target].position, Main.player[NPC.target].width, Main.player[NPC.target].height))
			{
				if (NPC.ai[1] > 0 && !Collision.SolidCollision(NPC.position, NPC.width, NPC.height))
				{
					NPC.ai[1] = 0.0f;
					NPC.ai[0] = 0.0f;
					NPC.netUpdate = true;
				}
			}
			else if (NPC.ai[1] == 0)
				++NPC.ai[0];

			if (NPC.ai[0] >= 300)
			{
				NPC.ai[1] = 1f;
				NPC.ai[0] = 0.0f;
				NPC.netUpdate = true;
			}

			if (NPC.ai[1] == 0)
			{
				NPC.alpha = 0;
				NPC.noTileCollide = false;
			}
			else
			{
				NPC.alpha = 200;
				NPC.noTileCollide = true;
			}

			if (NPC.direction == -1 && NPC.velocity.X > -4 && NPC.position.X > Main.player[NPC.target].position.X + Main.player[NPC.target].width)
			{
				NPC.velocity.X -= 0.08f;

				if (NPC.velocity.X > 4)
					NPC.velocity.X -= 0.04f;
				else if (NPC.velocity.X > 0.0)
					NPC.velocity.X -= 0.2f;

				if (NPC.velocity.X < -4)
					NPC.velocity.X = -4f;
			}
			else if (NPC.direction == 1 && NPC.velocity.X < 4 && NPC.position.X + NPC.width < Main.player[NPC.target].position.X)
			{
				NPC.velocity.X += 0.08f;

				if (NPC.velocity.X < -4)
					NPC.velocity.X += 0.04f;
				else if (NPC.velocity.X < 0.0)
					NPC.velocity.X += 0.2f;

				if (NPC.velocity.X > 4)
					NPC.velocity.X = 4f;
			}
			if (NPC.directionY == -1 && NPC.velocity.Y > -4 && NPC.position.Y > Main.player[NPC.target].position.Y + Main.player[NPC.target].height)
			{
				NPC.velocity.Y -= 0.1f;

				if (NPC.velocity.Y > 4)
					NPC.velocity.Y -= 0.05f;
				else if (NPC.velocity.Y > 0.0)
					NPC.velocity.Y -= 0.15f;

				if (NPC.velocity.Y < -4)
					NPC.velocity.Y = -4f;
			}
			else if (NPC.directionY == 1 && NPC.velocity.Y < 4 && NPC.position.Y + NPC.height < Main.player[NPC.target].position.Y)
			{
				NPC.velocity.Y += 0.1f;

				if (NPC.velocity.Y < -4)
					NPC.velocity.Y += 0.05f;
				else if (NPC.velocity.Y < 0.0)
					NPC.velocity.Y += 0.15f;

				if (NPC.velocity.Y > 4)
					NPC.velocity.Y = 4f;
			}
		}

		public override void OnKill()
		{
			if (Main.rand.Next(25) == 0)
				Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, Mod.Find<ModItem>("LuminanceSeacone").Type, 1);
			if (Main.rand.Next(2) == 1)
				Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ModContent.ItemType<RawFish>(), 1);

			Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ModContent.ItemType<IridescentScale>(), Main.rand.Next(3, 6));
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (NPC.life <= 0)
			{
				for (int index1 = 0; index1 < 13; ++index1)
				{
					int index2 = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.AncientLight, NPC.velocity.X * 0.5f, NPC.velocity.Y * 0.5f, 90, new Color(r, g, b), 2.5f);
					Main.dust[index2].noGravity = true;
					Main.dust[index2].fadeIn = 1f;
					Main.dust[index2].velocity *= 4f;
					Main.dust[index2].noLight = true;
				}
			}

			for (int k = 0; k < 5; k++)
			{
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.AncientLight, 2.5f * hitDirection, -2.5f, 0, new Color(r, g, b), 1.2f);
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.AncientLight, 2.5f * hitDirection, -2.5f, 0, new Color(r, g, b), 0.5f);
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.AncientLight, 2.5f * hitDirection, -2.5f, 0, new Color(r, g, b), 0.7f);
			}
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) => SpawnCondition.OceanMonster.Chance * 0.08f;
		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) => false;

		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			Vector2 vector2_3 = new Vector2(TextureAssets.Npc[NPC.type].Value.Width / 2f, TextureAssets.Npc[NPC.type].Value.Height / Main.npcFrameCount[NPC.type] / 2f);

			float addHeight = 10f;

			Color color1 = Lighting.GetColor((int)(NPC.position.X + NPC.width * 0.5) / 16, (int)((NPC.position.Y + NPC.height * 0.5) / 16.0));

			var effects = NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, NPC.Center - Main.screenPosition + new Vector2(0, NPC.gfxOffY), NPC.frame, drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effects, 0);

			Main.spriteBatch.Draw(Mod.GetTexture("NPCs/Horned_Crustacean/Horned_Crustacean_Glow"), NPC.Bottom - Main.screenPosition + new Vector2((float)(-TextureAssets.Npc[NPC.type].Value.Width * NPC.scale / 2.0 + vector2_3.X * NPC.scale), (float)(-TextureAssets.Npc[NPC.type].Value.Height * NPC.scale / Main.npcFrameCount[NPC.type] + 4.0 + vector2_3.Y * NPC.scale) + addHeight + NPC.gfxOffY), new Microsoft.Xna.Framework.Rectangle?(NPC.frame), new Microsoft.Xna.Framework.Color((int)r - NPC.alpha, (int)byte.MaxValue - NPC.alpha, (int)g - NPC.alpha, (int)b - NPC.alpha), NPC.rotation, vector2_3, NPC.scale, effects, 0.0f);

			float num = 0.25f + (NPC.GetAlpha(color1).ToVector3() - new Vector3(4f)).Length() * 0.25f;
			for (int index = 0; index < 4; ++index)
				Main.spriteBatch.Draw(Mod.GetTexture("NPCs/Horned_Crustacean/Horned_Crustacean_Glow"), NPC.Bottom - Main.screenPosition + new Vector2((float)(-TextureAssets.Npc[NPC.type].Value.Width * NPC.scale / 2.0 + vector2_3.X * NPC.scale), (float)(-TextureAssets.Npc[NPC.type].Value.Height * NPC.scale / Main.npcFrameCount[NPC.type] + 4.0 + vector2_3.Y * NPC.scale) + addHeight + NPC.gfxOffY) + NPC.velocity.RotatedBy(index * 47079637050629, new Vector2()) * num, new Microsoft.Xna.Framework.Rectangle?(NPC.frame), new Color(r, g, b, 0), NPC.rotation, vector2_3, NPC.scale, effects, 0.0f);
		}

		public override void FindFrame(int frameHeight)
		{
			Player player = Main.player[NPC.target];

			NPC.frameCounter++;
			if (Vector2.Distance(player.Center, NPC.Center) <= 45f && NPC.velocity.X == 0f)
			{
				if (NPC.frameCounter == 24 && Collision.CanHitLine(NPC.Center, 0, 0, Main.player[NPC.target].Center, 0, 0))
				{
					player.Hurt(PlayerDeathReason.LegacyDefault(), (int)(NPC.damage * 1.5f), NPC.direction, false, false, false, -1);
					NPC.frame.Y = 9 * frameHeight;
				}

				NPC.velocity.X = 0f;

				if (NPC.frameCounter < 5)
					NPC.frame.Y = 5 * frameHeight;
				else if (NPC.frameCounter < 10)
					NPC.frame.Y = 6 * frameHeight;
				else if (NPC.frameCounter < 15)
					NPC.frame.Y = 7 * frameHeight;
				else if (NPC.frameCounter < 20)
					NPC.frame.Y = 8 * frameHeight;
				else if (NPC.frameCounter < 25)
					NPC.frame.Y = 9 * frameHeight;
				else
					NPC.frameCounter = 0;
			}
			else
			{
				if (NPC.frameCounter < 6)
					NPC.frame.Y = 0 * frameHeight;
				else if (NPC.frameCounter < 12)
					NPC.frame.Y = 1 * frameHeight;
				else if (NPC.frameCounter < 18)
					NPC.frame.Y = 2 * frameHeight;
				else if (NPC.frameCounter < 24)
					NPC.frame.Y = 3 * frameHeight;
				else if (NPC.frameCounter < 30)
					NPC.frame.Y = 4 * frameHeight;
				else
					NPC.frameCounter = 0;
			}
		}
	}
}