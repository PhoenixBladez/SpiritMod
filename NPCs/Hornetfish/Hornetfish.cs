using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Consumable.Quest;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Hornetfish
{
	public class Hornetfish : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hornetfish");
			Main.npcFrameCount[NPC.type] = 8;
			NPCID.Sets.TrailCacheLength[NPC.type] = 3;
		}

		public override void SetDefaults()
		{
			NPC.width = 40;
			NPC.height = 32;
			NPC.damage = 20;
			NPC.defense = 9;
			NPC.lifeMax = 100;
			NPC.noGravity = true;
			NPC.buffImmune[BuffID.Poisoned] = true;
			NPC.buffImmune[BuffID.Confused] = true;
			NPC.value = 100f;
			NPC.noTileCollide = false;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.DD2_LightningBugHurt;
		}

		int frame = 7;
		int timer = 0;
		bool trailing;

		public override void AI()
		{
			timer++;
			NPC.spriteDirection = NPC.direction;
			Player player = Main.player[NPC.target];

			if (NPC.life >= NPC.lifeMax / 2)
			{
				NPC.aiStyle = 16;
				NPC.noGravity = true;
				AIType = 157;

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
				NPC.TargetClosest(true);

				++NPC.ai[3];
				if (timer >= 4)
				{
					frame++;
					timer = 0;
				}

				if (frame >= 8)
					frame = 4;

				float num1 = 5f;
				float moveSpeed = 0.15f;
				Vector2 baseVel = Main.player[NPC.target].Center - NPC.Center + new Vector2(0.0f, Main.rand.NextFloat(-400f, -200f));
				float num2 = baseVel.Length();
				baseVel.Normalize();

				Vector2 desiredVelocity;

				if (num2 < 20.0)
					desiredVelocity = NPC.velocity;
				else if (num2 < 40.0)
					desiredVelocity = baseVel * (num1 * 0.35f);
				else if (num2 < 80.0)
					desiredVelocity = baseVel * (num1 * 0.65f);
				else
					desiredVelocity = baseVel * num1;

				NPC.SimpleFlyMovement(desiredVelocity, moveSpeed);

				if (NPC.ai[3] >= 240 && !NPC.collideY)
				{
					trailing = true;
					if (timer >= 4)
					{
						frame++;
						timer = 0;
					}

					if (frame >= 4)
						frame = 1;

					float num17 = Main.player[NPC.target].Center.Y - NPC.Center.Y;

					if (num17 > 0)
						NPC.SimpleFlyMovement(NPC.DirectionTo(player.Center + new Vector2((float)((double)NPC.direction * 1000), NPC.Center.Y + .001f)) * 17.5f, .5f);
					else
						NPC.SimpleFlyMovement(NPC.DirectionTo(player.Center + new Vector2((float)((double)NPC.direction * 1000), NPC.Center.Y + .001f)) * 17.5f, -.5f);

					NPC.direction = NPC.spriteDirection = NPC.Center.X < player.Center.X ? 1 : -1;
				}

				if (NPC.ai[3] >= 270)
				{
					trailing = false;
					NPC.ai[3] = 0;
				}

				NPC.SimpleFlyMovement(desiredVelocity, moveSpeed);

				NPC.rotation = NPC.velocity.X * 0.1f;
			}

			++NPC.ai[2];

			if (NPC.life <= NPC.lifeMax / 2 && NPC.ai[2] >= 140)
			{
				SoundEngine.PlaySound(SoundID.Item, NPC.Center, 97);
				int distance = (int)Math.Sqrt((NPC.Center.X - player.Center.X) * (NPC.Center.X - player.Center.X) + (NPC.Center.Y - player.Center.Y) * (NPC.Center.Y - player.Center.Y));
				Projectile.NewProjectile(NPC.Center.X, NPC.position.Y, -(NPC.position.X - player.position.X) / distance * 8, -(NPC.position.Y - player.position.Y) / distance * 8, ProjectileID.Stinger, (int)((NPC.damage / 2)), 0);
				NPC.ai[2] = 0;
			}
		}

		public override void FindFrame(int frameHeight) => NPC.frame.Y = frameHeight * frame;

		public override void OnKill()
		{
			if (!Main.LocalPlayer.HasItem(ModContent.ItemType<HornetfishQuest>()))
				Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ModContent.ItemType<HornetfishQuest>());

			if (Main.rand.Next(150) == 5)
				Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ItemID.Compass);

			if (Main.rand.Next(100) == 1)
				Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ItemID.RobotHat);

			if (Main.rand.Next(100) == 6)
				Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ItemID.Hook);

			Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ItemID.Stinger);
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			var effects = NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, NPC.Center - Main.screenPosition + new Vector2(0, NPC.gfxOffY), NPC.frame, lightColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effects, 0);

			if (trailing)
			{
				Vector2 drawOrigin = new Vector2(TextureAssets.Npc[NPC.type].Value.Width * 0.5f, (NPC.height / Main.npcFrameCount[NPC.type]) * 0.5f);
				for (int k = 0; k < NPC.oldPos.Length; k++)
				{
					Vector2 drawPos = NPC.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, NPC.gfxOffY);
					Color color = NPC.GetAlpha(lightColor) * (((float)(NPC.oldPos.Length - k) / NPC.oldPos.Length) / 2);
					spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, drawPos, new Microsoft.Xna.Framework.Rectangle?(NPC.frame), color, NPC.rotation, drawOrigin, NPC.scale, effects, 0f);
				}
			}
			return false;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 30; k++)
			{
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, 2.5f * hitDirection, -2.5f, 0, Color.White, Main.rand.NextFloat(.2f, .8f));
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, 2.5f * hitDirection, -2.5f, 0, default, 1.14f);
			}

			if (NPC.life <= 0)
				for (int i = 1; i < 5; ++i)
					Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/Hornetfish/Hornetfish" + i).Type, 1f);
		}
	}
}