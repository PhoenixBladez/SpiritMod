using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Mechanics.QuestSystem;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.FallingAsteroid
{
	public class Falling_Asteroid : ModNPC
	{
		public int visualTimer = 0;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Falling Asteroid");
			NPCID.Sets.TrailCacheLength[npc.type] = 30;
			NPCID.Sets.TrailingMode[npc.type] = 0;
			Main.npcFrameCount[npc.type] = 5;
		}
		public override void SetDefaults()
		{
			npc.aiStyle = 0;
			npc.lifeMax = 115;
			npc.defense = 8;
			npc.value = 350f;
			npc.knockBackResist = 0f;
			npc.width = 30;
			npc.height = 50;
			npc.lavaImmune = true;
			npc.noGravity = true;
			npc.damage = 30;
			npc.buffImmune[BuffID.OnFire] = true;
			npc.buffImmune[BuffID.Confused] = true;
			npc.buffImmune[BuffID.Poisoned] = true;
			npc.HitSound = SoundID.NPCHit3;
			npc.DeathSound = SoundID.NPCDeath43;
			banner = npc.type;
			bannerItem = ModContent.ItemType<Items.Banners.FallingAsteroidBanner>();
		}

		public override void OnHitPlayer(Player target, int damage, bool crit) => target.AddBuff(24, 60 * 3);
		public override float SpawnChance(NPCSpawnInfo spawnInfo) => SpawnCondition.Meteor.Chance * 0.15f;

		public override void AI()
		{
			Player player = Main.player[npc.target];
			npc.spriteDirection = 1;

			npc.ai[0]++;
			if (npc.ai[0] <= 320 && npc.ai[0] >= 0)
				Movement();
			else if (npc.ai[0] > 360)
				DoExplosion();

			if (npc.ai[0] == 320 || npc.ai[0] == 360)
				npc.netUpdate = true;

			if (npc.ai[0] >= 580)
			{
				npc.ai[0] = 0;
				npc.netUpdate = true;
			}

			for (int i = 0; i < 2; ++i)
			{
				Dust dust = Main.dust[Dust.NewDust(npc.position, npc.width, npc.height, DustID.Fire, 0.0f, 0.0f, 0, new Color(), 1f)];
				dust.position = npc.Center + Vector2.Normalize(npc.velocity) / 2f;
				dust.velocity = npc.velocity.RotatedBy(MathHelper.PiOver2, new Vector2()) * 0.33f + npc.velocity / 120f;
				dust.position += npc.velocity.RotatedBy(MathHelper.PiOver2, new Vector2());
				dust.fadeIn = 0.5f;
				dust.noGravity = true;
			}

			if (player.dead)
				npc.velocity.Y -= 0.15f;
			Lighting.AddLight(new Vector2(npc.Center.X, npc.Center.Y), 0.5f, 0.25f, 0f);
		}

		public void DoExplosion()
		{
			npc.velocity.Y += 0.15f;
			npc.noTileCollide = false;
			if (npc.collideY && npc.ai[0] > 420)
			{
				for (int index = 0; index < 30; ++index)
				{
					Dust dust = Main.dust[Dust.NewDust(new Vector2(npc.position.X, npc.position.Y + 50), npc.width, npc.height, DustID.Granite, 0.0f, 0.0f, 0, new Color(), 1.2f)];
					dust.velocity.Y -= (float)(3.0 + 2 * 2.5);
					dust.velocity.Y *= Main.rand.NextFloat();
					dust.scale += 8 * 0.03f;
				}

				Main.PlaySound(SoundID.NPCKilled, (int)npc.position.X, (int)npc.position.Y, 14, 1f, 0.0f);
				npc.ai[0] = -90;
				npc.netUpdate = true;

				for (int k = 0; k < 10; k++)
					Gore.NewGore(npc.position, new Vector2(npc.velocity.X * 0.5f, -npc.velocity.Y * 0.5f), Main.rand.Next(61, 64), 1f);
				for (int k = 0; k < 20; k++)
					Dust.NewDust(npc.position, npc.width, npc.height, DustID.Fire, npc.velocity.X * 2f, -npc.velocity.Y * 2f, 150, new Color(), 1.2f);

				for (int i = 0; i < Main.npc.Length; i++)
				{
					NPC npc2 = Main.npc[i];
					if (Vector2.Distance(npc.Center, npc2.Center) <= (double)150f && !npc2.boss && npc2.knockBackResist != 0f)
						MoveEntity(npc2);
				}

				for (int i = 0; i < Main.item.Length; i++)
				{
					Item item = Main.item[i];
					if (Vector2.Distance(npc.Center, item.Center) <= 150f)
						MoveEntity(item);
				}

				for (int i = 0; i < Main.player.Length; i++)
				{
					Player patates = Main.player[i];
					if (Vector2.Distance(npc.Center, patates.Center) <= 150f)
						MoveEntity(patates);
				}

				for (int i = 0; i < Main.projectile.Length; i++)
				{
					Projectile aga = Main.projectile[i];
					if (Vector2.Distance(npc.Center, aga.Center) <= 150f)
						MoveEntity(aga);
				}
			}
		}

		public void MoveEntity(Entity t)
		{
			float num2 = npc.position.X + Main.rand.Next(-10, 10) + (npc.width / 2f) - t.Center.X;
			float num3 = npc.position.Y + Main.rand.Next(-10, 10) + (npc.height / 2f) - t.Center.Y;
			float num4 = 8f / (float)Math.Sqrt(num2 * num2 + num3 * num3);
			t.velocity.X = num2 * num4 * -1.7f;
			t.velocity.Y = num3 * num4 * -1.7f;
		}

		public void Movement()
		{
			const float MoveSpeed = 0.35f;

			Player player = Main.player[npc.target];

			npc.noTileCollide = true;
			float number3 = Main.player[npc.target].Center.X - npc.Center.X;
			float number4 = (float)(Main.player[npc.target].Center.Y - npc.Center.Y - 300.0);
			float num5 = (float)Math.Sqrt(number3 * number3 + number4 * number4);

			if (player == Main.player[npc.target])
			{
				float num6 = npc.velocity.X;
				float num7 = npc.velocity.Y;

				if (num5 >= 20)
				{
					float num8 = 4f / num5;
					num6 = number3 * num8;
					num7 = number4 * num8;
				}

				if (npc.velocity.X < num6)
				{
					npc.velocity.X += MoveSpeed;
					if (npc.velocity.X < 0 && num6 > 0)
						npc.velocity.X += MoveSpeed * 2f;
				}
				else if (npc.velocity.X > num6)
				{
					npc.velocity.X -= MoveSpeed;
					if (npc.velocity.X > 0 && num6 < 0)
						npc.velocity.X -= MoveSpeed * 2f;
				}
				if (npc.velocity.Y < num7)
				{
					npc.velocity.Y += MoveSpeed;
					if (npc.velocity.Y < 0 && num7 > 0)
						npc.velocity.Y += MoveSpeed * 2f;
				}
				else if (npc.velocity.Y > num7)
				{
					npc.velocity.Y -= MoveSpeed;
					if (npc.velocity.Y > 0 && num7 < 0)
						npc.velocity.Y -= MoveSpeed * 2f;
				}
			}

			if (++npc.ai[1] >= 5f)
			{
				npc.ai[1] = 0f;
				npc.netUpdate = true;
			}
		}

		public override void FindFrame(int frameHeight)
		{
			Player player = Main.player[npc.target];

			if (npc.active && player.active)
			{
				npc.frameCounter++;
				if (npc.frameCounter < 6)
					npc.frame.Y = 0 * frameHeight;
				else if (npc.frameCounter < 12)
					npc.frame.Y = 1 * frameHeight;
				else if (npc.frameCounter < 18)
					npc.frame.Y = 2 * frameHeight;
				else if (npc.frameCounter < 24)
					npc.frame.Y = 3 * frameHeight;
				else if (npc.frameCounter < 28)
					npc.frame.Y = 4 * frameHeight;
				else
					npc.frameCounter = 0;
			}
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
				for (int i = 1; i < 5; ++i)
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/FallenAsteroid/FallenAsteroidGore" + i), 1f);

			for (int k = 0; k < 7; k++)
			{
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.Fire, 2.5f * hitDirection, -2.5f, 0, default, 1.2f);
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.Fire, 2.5f * hitDirection, -2.5f, 0, default, 0.5f);
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.Fire, 2.5f * hitDirection, -2.5f, 0, default, 0.7f);
			}
		}

		public override void NPCLoot()
		{
			if (Main.rand.Next(10) == 0)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, 116, Main.rand.Next(2, 5));

			if (QuestManager.GetQuest<Mechanics.QuestSystem.Quests.StylistQuestMeteor>().IsActive && Main.rand.NextBool(3))
				Item.NewItem(npc.Center, ModContent.ItemType<Items.Sets.MaterialsMisc.QuestItems.MeteorDyeMaterial>());
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			if (npc.ai[0] > 360)
			{
				++visualTimer;

				Player player = Main.player[npc.target];

				bool flag2 = Vector2.Distance(npc.Center, player.Center) > 0f && npc.Center.Y == player.Center.Y;
				if (visualTimer >= 30f && flag2)
					visualTimer = 0;

				SpriteEffects spriteEffects = SpriteEffects.None;
				float addHeight = -4f;
				float addWidth = 0f;
				var vector2_3 = new Vector2(Main.npcTexture[npc.type].Width / 2, Main.npcTexture[npc.type].Height / Main.npcFrameCount[npc.type] / 2);

				if (npc.velocity.X == 0)
				{
					addHeight = 0f;
					addWidth = 0f;
				}

				Texture2D tex = Main.extraTexture[55];
				Vector2 origin = new Vector2(tex.Width / 2, tex.Height / 8 + 14);

				float num2 = -MathHelper.PiOver2 * npc.rotation;
				float amount = visualTimer / 45f;
				if (amount > 1f)
					amount = 1f;

				for (int index = 5; index >= 0; --index)
				{
					var color2 = Color.Lerp(Color.Lerp(Color.Gold, Color.OrangeRed, amount), Color.Blue, index / 12f);

					color2.A = (byte)(64.0 * amount);
					color2.R = (byte)(color2.R * (10 - index) / 20);
					color2.G = (byte)(color2.G * (10 - index) / 20);
					color2.B = (byte)(color2.B * (10 - index) / 20);
					color2.A = (byte)(color2.A * (10 - index) / 20);
					color2 *= amount;

					int frameY = (((visualTimer / 2) % 4) - index) % 4;
					if (frameY < 0)
						frameY += 4;

					Rectangle rectangle = tex.Frame(1, 4, 0, frameY);

					var pos = new Vector2(npc.oldPos[index].X + (npc.width / 2f) - Main.npcTexture[npc.type].Width * npc.scale / 2 + vector2_3.X + addWidth, npc.oldPos[index].Y + npc.height - Main.npcTexture[npc.type].Height * npc.scale / Main.npcFrameCount[npc.type] + 4 + vector2_3.Y + addHeight) * npc.scale;
					Main.spriteBatch.Draw(tex, pos - Main.screenPosition, rectangle, color2, num2, origin, MathHelper.Lerp(0.1f, 1.2f, ((10 - index) / 15f)), spriteEffects, 0.0f);
				}
			}
			return false;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			var effects = npc.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame, drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
			GlowmaskUtils.DrawNPCGlowMask(spriteBatch, npc, mod.GetTexture("NPCs/FallingAsteroid/Falling_Asteroid_Glow"));
		}
	}
}