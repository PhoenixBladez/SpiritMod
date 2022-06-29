using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Sets.InfernonDrops;
using SpiritMod.Projectiles.Hostile;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.Infernon
{
	[AutoloadBossHead]
	public class InfernoSkull : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Infernus Skull");
			Main.npcFrameCount[NPC.type] = 4;
		}

		public override void SetDefaults()
		{
			NPC.width = 100;
			NPC.height = 80;
			NPC.knockBackResist = 0f;
			NPC.defense = 20;
			NPC.damage = 50;
			NPC.lifeMax = 5500;
			NPC.aiStyle = -1;
			NPC.buffImmune[BuffID.OnFire] = true;
			NPC.buffImmune[BuffID.CursedInferno] = true;
			NPC.npcSlots = 10;
			NPC.boss = true;
			NPC.noGravity = true;
			NPC.noTileCollide = true;

			bossBag = ModContent.ItemType<InfernonBag>();
			Music = Mod.GetSoundSlot(SoundType.Music, "Sounds/Music/Infernon");
		}

		public override void FindFrame(int frameHeight)
		{
			NPC.frameCounter += 0.10f;
			NPC.frameCounter %= Main.npcFrameCount[NPC.type];
			int frame = (int)NPC.frameCounter;
			NPC.frame.Y = frame * frameHeight;
		}

		public override bool PreKill()
		{
			MyWorld.downedInfernon = true;
			return true;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			var effects = NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, NPC.Center - Main.screenPosition + new Vector2(0, NPC.gfxOffY), NPC.frame, drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effects, 0);
			return false;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) => GlowmaskUtils.DrawNPCGlowMask(spriteBatch, NPC, Mod.Assets.Request<Texture2D>("NPCs/Boss/Infernon/InfernonSkull_Glow").Value);

		public override void OnKill()
		{
			if (Main.netMode != NetmodeID.MultiplayerClient)
			{
				int centerX = (int)(NPC.Center.X / 16);
				int centerY = (int)(NPC.Center.Y / 16);
				int halfLength = NPC.width / 2 / 16 + 1;
				for (int x = centerX - halfLength; x <= centerX + halfLength; x++)
				{
					for (int y = centerY - halfLength; y <= centerY + halfLength; y++)
					{
						Tile tile = Main.tile[x, y];
						if ((x == centerX - halfLength || x == centerX + halfLength || y == centerY - halfLength || y == centerY + halfLength) && !Main.tile[x, y].HasTile)
						{
							tile.TileType = TileID.HellstoneBrick;
							tile.HasTile = true;
						}
						tile.LiquidType = LiquidID.Lava;
						Main.tile[x, y].LiquidAmount = 0;

						if (Main.netMode == NetmodeID.Server)
							NetMessage.SendTileSquare(-1, x, y, 1);
						else
							WorldGen.SquareTileFrame(x, y, true);
					}
				}
			}
			if (Main.expertMode)
				NPC.DropBossBags();
		}

		int timer = 0;

		public override void AI()
		{
			if (NPC.localAI[0] == 0f)
			{
				NPC.localAI[0] = NPC.Center.Y;
				NPC.netUpdate = true; //localAI probably isnt affected by this... buuuut might as well play it safe
			}
			if (NPC.Center.Y >= NPC.localAI[0])
			{
				NPC.localAI[1] = -1f;
				NPC.netUpdate = true;
			}
			if (NPC.Center.Y <= NPC.localAI[0] - 10f)
			{
				NPC.localAI[1] = 1f;
				NPC.netUpdate = true;
			}

			NPC.velocity.Y = MathHelper.Clamp(NPC.velocity.Y + 0.05f * NPC.localAI[1], -2f, 2f);
			NPC.ai[0] += 1f;
			NPC.netUpdate = true;

			int damage = Main.expertMode ? 16 : 30;

			timer++;

			if (timer == 0 || timer == 200)
			{
				float spread = 45f * 0.0174f;
				double startAngle = Math.Atan2(1, 0) - spread / 2;
				double deltaAngle = spread / 8f;
				double offsetAngle;
				for (int i = 0; i < 4; i++)
				{
					offsetAngle = (startAngle + deltaAngle * (i + i * i) / 2f) + 32f * i;
					Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, (float)(Math.Sin(offsetAngle) * 5f), (float)(Math.Cos(offsetAngle) * 5f), ModContent.ProjectileType<InfernalWave>(), 28, 0, Main.myPlayer);
					Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, (float)(-Math.Sin(offsetAngle) * 5f), (float)(-Math.Cos(offsetAngle) * 5f), ModContent.ProjectileType<InfernalWave>(), 28, 0, Main.myPlayer);
					NPC.netUpdate = true;
				}
			}

			if (timer == 210 || timer == 220 || timer == 230 || timer == 240 || timer == 250 || timer == 260 || timer == 270 || timer == 280 || timer == 290 || timer == 300 || timer == 310 || timer == 320 || timer == 340 || timer == 350)
			{
				if (NPC.life >= (NPC.lifeMax / 3))
				{
					SoundEngine.PlaySound(SoundID.Item33, NPC.Center);
					Vector2 direction = Main.player[NPC.target].Center - NPC.Center;
					direction.Normalize();
					direction.X *= 9.5f;
					direction.Y *= 9.5f;

					int amountOfProjectiles = 1;
					for (int z = 0; z < amountOfProjectiles; ++z)
					{
						float A = Main.rand.Next(-200, 200) * 0.03f;
						float B = Main.rand.Next(-200, 200) * 0.03f;
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, direction.X + A, direction.Y + B, ModContent.ProjectileType<InfernalBlastHostile>(), damage, 1, Main.myPlayer, 0, 0);
					}
				}
			}

			if (timer == 400)
			{
				Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y + 200, 0f, 0f, ModContent.ProjectileType<Fireball>(), damage, 1, Main.myPlayer, 0, 0);
				Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X + 200, NPC.Center.Y, 0f, 0f, ModContent.ProjectileType<Fireball>(), damage, 1, Main.myPlayer, 0, 0);
				Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X - 200, NPC.Center.Y, 0f, 0f, ModContent.ProjectileType<Fireball>(), damage, 1, Main.myPlayer, 0, 0);
				Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y - 200, 0f, 0f, ModContent.ProjectileType<Fireball>(), damage, 1, Main.myPlayer, 0, 0);

				timer = 0;
			}
			else if (Main.rand.Next(90) == 1 && NPC.life <= (NPC.lifeMax / 3))
			{
				Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y + 500, 0f, 0f, ModContent.ProjectileType<Fireball>(), damage, 1, Main.myPlayer, 0, 0);
				Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X + 500, NPC.Center.Y, 0f, 0f, ModContent.ProjectileType<Fireball>(), damage, 1, Main.myPlayer, 0, 0);
				Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X - 500, NPC.Center.Y, 0f, 0f, ModContent.ProjectileType<Fireball>(), damage, 1, Main.myPlayer, 0, 0);
				Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y - 500, 0f, 0f, ModContent.ProjectileType<Fireball>(), damage, 1, Main.myPlayer, 0, 0);
			}
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 5; k++)
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Torch, hitDirection, -1f, 0, default, 1f);

			if (NPC.life <= 0)
			{
				NPC.position.X = NPC.position.X + (NPC.width / 2);
				NPC.position.Y = NPC.position.Y + (NPC.height / 2);
				NPC.width = 80;
				NPC.height = 80;
				NPC.position.X = NPC.position.X - (NPC.width / 2);
				NPC.position.Y = NPC.position.Y - (NPC.height / 2);

				for (int num621 = 0; num621 < 120; num621++)
				{
					int num622 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.Torch, 0f, 0f, 100, default, 2f);
					Main.dust[num622].velocity *= 3f;
					if (Main.rand.Next(2) == 0)
					{
						Main.dust[num622].scale = 0.5f;
						Main.dust[num622].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
					}
				}

				for (int num623 = 0; num623 < 200; num623++)
				{
					int num624 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.Torch, 0f, 0f, 100, default, 3f);
					Main.dust[num624].noGravity = true;
					Main.dust[num624].velocity *= 5f;
					num624 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.Torch, 0f, 0f, 100, default, 2f);
					Main.dust[num624].velocity *= 2f;
				}
			}
		}

		public override void BossLoot(ref string name, ref int potionType) => potionType = ItemID.GreaterHealingPotion;
	}
}