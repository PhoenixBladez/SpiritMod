using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.MoltenCore
{
	public class Molten_Core : ModNPC
	{
		public int spawnedProjectiles = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Molten Core");
			Main.npcFrameCount[npc.type] = 5;
		}
		public override void SetDefaults()
		{
			npc.aiStyle = -1;
			npc.lifeMax = 65;
			npc.defense = 6;
			npc.value = 350f;
			npc.knockBackResist = 0.5f;
			npc.width = 24;
			npc.height = 24;
			npc.damage = 25;
			npc.lavaImmune = true;
			npc.noGravity = true;
			npc.noTileCollide = true;
			npc.buffImmune[BuffID.OnFire] = true;
			npc.buffImmune[BuffID.Confused] = true;
			npc.HitSound = SoundID.NPCHit3;
			npc.DeathSound = SoundID.NPCDeath43;
			banner = npc.type;
			bannerItem = ModContent.ItemType<Items.Banners.MoltenCoreBanner>();
		}
		public override void OnHitPlayer (Player target, int damage, bool crit)
		{
			target.AddBuff(24, 60*3);
		}
		public override void AI()
		{
			Player player = Main.player[npc.target];
			npc.spriteDirection = npc.direction;
			movement();
			CheckPlatform();
			
			if (Main.rand.Next(15) == 0)
			{
				int index = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, DustID.Fire, 0.0f, 0.0f, 100, new Color(), 1f);
				Main.dust[index].noGravity = true;
				Main.dust[index].velocity.X = (float) Main.rand.Next(-3,3);
				Main.dust[index].velocity.Y = 2f;
			}
			
			if (npc.ai[2] < 3)
			{
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					npc.ai[2]++;
					int p = Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0f, 0f, mod.ProjectileType("Molten_Core_Projectile"), 8, 0, 0);
					Main.projectile[p].ai[1] = npc.whoAmI;
				}
				npc.netUpdate = true;
			}
		}
		private void CheckPlatform()
		{
			bool onplatform = true;
			for (int i = (int)npc.position.X; i < npc.position.X + npc.width; i += npc.width / 4) {
				Tile tile = Framing.GetTileSafely(new Point((int)npc.position.X / 16, (int)(npc.position.Y + npc.height + 8) / 16));
				if (!TileID.Sets.Platforms[tile.type])
					onplatform = false;
			}
			if (onplatform)
				npc.noTileCollide = true;
			else
				npc.noTileCollide = false;
		}
		private void movement()
		{
			npc.noGravity = true;
			if (!npc.noTileCollide)
			{
				if (npc.collideX)
				{
					npc.velocity.X = npc.oldVelocity.X * -0.5f;
					if (npc.direction == -1 && (double)npc.velocity.X > 0.0 && (double)npc.velocity.X < 2.0)
					{
						npc.velocity.X = 2f;
					}

					if (npc.direction == 1 && (double)npc.velocity.X < 0.0 && (double)npc.velocity.X > -2.0)
					{
						npc.velocity.X = -2f;
					}
				}
				if (npc.collideY)
				{
					npc.velocity.Y = npc.oldVelocity.Y * -0.5f;
					if ((double)npc.velocity.Y > 0.0 && (double)npc.velocity.Y < 1.0)
					{
						npc.velocity.Y = 1f;
					}

					if ((double)npc.velocity.Y < 0.0 && (double)npc.velocity.Y > -1.0)
					{
						npc.velocity.Y = -1f;
					}
				}
			}
			npc.TargetClosest(true);
			if (Collision.CanHit(npc.position, npc.width, npc.height, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height))
			{
				if ((double)npc.ai[1] > 0.0 && !Collision.SolidCollision(npc.position, npc.width, npc.height))
				{
					npc.ai[1] = 0.0f;
					npc.ai[0] = 0.0f;
					npc.netUpdate = true;
				}
			}
			else if ((double)npc.ai[1] == 0.0)
			{
				++npc.ai[0];
			}

			if ((double)npc.ai[0] >= 300.0)
			{
				npc.ai[1] = 1f;
				npc.ai[0] = 0.0f;
				npc.netUpdate = true;
			}
			if ((double)npc.ai[1] == 0.0)
			{
				npc.alpha = 0;
				npc.noTileCollide = false;
			}
			else
			{
				npc.wet = false;
				npc.alpha = 200;
				npc.noTileCollide = true;
			}
			npc.TargetClosest(true);
			if (npc.direction == -1 && (double)npc.velocity.X > -1.5 && (double)npc.position.X > (double)Main.player[npc.target].position.X + (double)Main.player[npc.target].width)
			{
				npc.velocity.X -= 0.08f;
				if ((double)npc.velocity.X > 1.5)
				{
					npc.velocity.X -= 0.04f;
				}
				else if ((double)npc.velocity.X > 0.0)
				{
					npc.velocity.X -= 0.2f;
				}

				if ((double)npc.velocity.X < -1.5)
				{
					npc.velocity.X = -1.5f;
				}
			}
			else if (npc.direction == 1 && (double)npc.velocity.X < 1.5 && (double)npc.position.X + (double)npc.width < (double)Main.player[npc.target].position.X)
			{
				npc.velocity.X += 0.08f;
				if ((double)npc.velocity.X < -1.5)
				{
					npc.velocity.X += 0.04f;
				}
				else if ((double)npc.velocity.X < 0.0)
				{
					npc.velocity.X += 0.2f;
				}

				if ((double)npc.velocity.X > 1.5)
				{
					npc.velocity.X = 1.5f;
				}
			}
			if (npc.directionY == -1 && (double)npc.velocity.Y > -1.5 && (double)npc.position.Y > (double)Main.player[npc.target].position.Y + (double)Main.player[npc.target].height)
			{
				npc.velocity.Y -= 0.1f;
				if ((double)npc.velocity.Y > 1.5)
				{
					npc.velocity.Y -= 0.05f;
				}
				else if ((double)npc.velocity.Y > 0.0)
				{
					npc.velocity.Y -= 0.15f;
				}

				if ((double)npc.velocity.Y < -1.5)
				{
					npc.velocity.Y = -1.5f;
				}
			}
			else if (npc.directionY == 1 && (double)npc.velocity.Y < 1.5 && (double)npc.position.Y + (double)npc.height < (double)Main.player[npc.target].position.Y)
			{
				npc.velocity.Y += 0.1f;
				if ((double)npc.velocity.Y < -1.5)
				{
					npc.velocity.Y += 0.05f;
				}
				else if ((double)npc.velocity.Y < 0.0)
				{
					npc.velocity.Y += 0.15f;
				}

				if ((double)npc.velocity.Y > 1.5)
				{
					npc.velocity.Y = 1.5f;
				}
			}
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/MoltenCore/MoltenCoreGore1"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/MoltenCore/MoltenCoreGore2"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/MoltenCore/MoltenCoreGore3"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/MoltenCore/MoltenCoreGore4"), 1f);
			}
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
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, 116, 1);
			}
			if (Main.rand.Next(33) == 0)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("MeteoriteSpewer"), 1);
			}
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return SpawnCondition.Meteor.Chance * 0.15f;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame,
							 drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
			return false;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			GlowmaskUtils.DrawNPCGlowMask(spriteBatch, npc, mod.GetTexture("NPCs/MoltenCore/MoltenCore_Glow"));
		}
		public override void FindFrame(int frameHeight)
		{
			const int Frame_1 = 0;
			const int Frame_2 = 1;
			const int Frame_3 = 2;
			const int Frame_4 = 3;
			const int Frame_5 = 4;
			int animationSpeed;

			Player player = Main.player[npc.target];
			npc.frameCounter++;
			animationSpeed = 5;
			if (npc.frameCounter < animationSpeed * 1)
			{
				npc.frame.Y = Frame_1 * frameHeight;
			}
			else if (npc.frameCounter < animationSpeed * 2)
			{
				npc.frame.Y = Frame_2 * frameHeight;
			}
			else if (npc.frameCounter < animationSpeed * 3)
			{
				npc.frame.Y = Frame_3 * frameHeight;
			}
			else if (npc.frameCounter < animationSpeed * 4)
			{
				npc.frame.Y = Frame_4 * frameHeight;
			}
			else if (npc.frameCounter < animationSpeed * 5)
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