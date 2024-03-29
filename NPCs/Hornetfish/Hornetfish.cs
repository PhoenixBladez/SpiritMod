﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Consumable.Quest;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Hornetfish
{
	public class Hornetfish : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hornetfish");
			Main.npcFrameCount[npc.type] = 8;
			NPCID.Sets.TrailCacheLength[npc.type] = 3;
		}

		public override void SetDefaults()
		{
			npc.width = 40;
			npc.height = 32;
			npc.damage = 20;
			npc.defense = 9;
			npc.lifeMax = 100;
			npc.noGravity = true;
			npc.buffImmune[BuffID.Poisoned] = true;
			npc.buffImmune[BuffID.Confused] = true;
			npc.value = 100f;
			npc.noTileCollide = false;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.DD2_LightningBugHurt;
		}

		int frame = 7;
		int timer = 0;
		bool trailing;

		public override void AI()
		{
			timer++;
			npc.spriteDirection = npc.direction;
			Player player = Main.player[npc.target];

			if (npc.life >= npc.lifeMax / 2)
			{
				npc.aiStyle = 16;
				npc.noGravity = true;
				aiType = 157;

				if (timer >= 6)
				{
					frame++;
					timer = 0;
				}

				if (frame >= 4)
					frame = 1;
			}
			else
			{
				npc.TargetClosest(true);

				++npc.ai[3];
				if (timer >= 4)
				{
					frame++;
					timer = 0;
				}

				if (frame >= 8)
					frame = 4;

				float num1 = 5f;
				float moveSpeed = 0.15f;
				Vector2 baseVel = Main.player[npc.target].Center - npc.Center + new Vector2(0.0f, Main.rand.NextFloat(-400f, -200f));
				float num2 = baseVel.Length();
				baseVel.Normalize();

				Vector2 desiredVelocity;

				if (num2 < 20.0)
					desiredVelocity = npc.velocity;
				else if (num2 < 40.0)
					desiredVelocity = baseVel * (num1 * 0.35f);
				else if (num2 < 80.0)
					desiredVelocity = baseVel * (num1 * 0.65f);
				else
					desiredVelocity = baseVel * num1;

				npc.SimpleFlyMovement(desiredVelocity, moveSpeed);

				if (npc.ai[3] >= 240 && !npc.collideY)
				{
					trailing = true;
					if (timer >= 4)
					{
						frame++;
						timer = 0;
					}

					if (frame >= 4)
						frame = 1;

					float num17 = Main.player[npc.target].Center.Y - npc.Center.Y;

					if (num17 > 0)
						npc.SimpleFlyMovement(npc.DirectionTo(player.Center + new Vector2((float)((double)npc.direction * 1000), npc.Center.Y + .001f)) * 17.5f, .5f);
					else
						npc.SimpleFlyMovement(npc.DirectionTo(player.Center + new Vector2((float)((double)npc.direction * 1000), npc.Center.Y + .001f)) * 17.5f, -.5f);

					npc.direction = npc.spriteDirection = npc.Center.X < player.Center.X ? 1 : -1;
				}

				if (npc.ai[3] >= 270)
				{
					trailing = false;
					npc.ai[3] = 0;
				}

				npc.SimpleFlyMovement(desiredVelocity, moveSpeed);

				npc.rotation = npc.velocity.X * 0.1f;
			}

			++npc.ai[2];

			if (npc.life <= npc.lifeMax / 2 && npc.ai[2] >= 140)
			{
				Main.PlaySound(SoundID.Item, npc.Center, 97);
				int distance = (int)Math.Sqrt((npc.Center.X - player.Center.X) * (npc.Center.X - player.Center.X) + (npc.Center.Y - player.Center.Y) * (npc.Center.Y - player.Center.Y));
				Projectile.NewProjectile(npc.Center.X, npc.position.Y, -(npc.position.X - player.position.X) / distance * 8, -(npc.position.Y - player.position.Y) / distance * 8, ProjectileID.Stinger, (int)((npc.damage / 2)), 0);
				npc.ai[2] = 0;
			}
		}

		public override void FindFrame(int frameHeight) => npc.frame.Y = frameHeight * frame;

		public override void NPCLoot()
		{
			if (!Main.LocalPlayer.HasItem(ModContent.ItemType<HornetfishQuest>()))
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<HornetfishQuest>());

			if (Main.rand.Next(150) == 5)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Compass);

			if (Main.rand.Next(100) == 1)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.RobotHat);

			if (Main.rand.Next(100) == 6)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Hook);

			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Stinger);
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame, lightColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);

			if (trailing)
			{
				Vector2 drawOrigin = new Vector2(Main.npcTexture[npc.type].Width * 0.5f, (npc.height / Main.npcFrameCount[npc.type]) * 0.5f);
				for (int k = 0; k < npc.oldPos.Length; k++)
				{
					Vector2 drawPos = npc.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, npc.gfxOffY);
					Color color = npc.GetAlpha(lightColor) * (((float)(npc.oldPos.Length - k) / npc.oldPos.Length) / 2);
					spriteBatch.Draw(Main.npcTexture[npc.type], drawPos, new Microsoft.Xna.Framework.Rectangle?(npc.frame), color, npc.rotation, drawOrigin, npc.scale, effects, 0f);
				}
			}
			return false;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 30; k++)
			{
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.Blood, 2.5f * hitDirection, -2.5f, 0, Color.White, Main.rand.NextFloat(.2f, .8f));
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.Blood, 2.5f * hitDirection, -2.5f, 0, default, 1.14f);
			}

			if (npc.life <= 0)
				for (int i = 1; i < 5; ++i)
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Hornetfish/Hornetfish" + i), 1f);
		}
	}
}