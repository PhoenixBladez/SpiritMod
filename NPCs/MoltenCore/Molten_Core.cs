using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Mechanics.QuestSystem;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace SpiritMod.NPCs.MoltenCore
{
	public class Molten_Core : ModNPC
	{
		public int spawnedProjectiles = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Molten Core");
			Main.npcFrameCount[NPC.type] = 5;
		}

		public override void SetDefaults()
		{
			NPC.aiStyle = -1;
			NPC.lifeMax = 65;
			NPC.defense = 6;
			NPC.value = 350f;
			NPC.knockBackResist = 0.5f;
			NPC.width = 24;
			NPC.height = 24;
			NPC.damage = 25;
			NPC.lavaImmune = true;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.buffImmune[BuffID.OnFire] = true;
			NPC.buffImmune[BuffID.Confused] = true;
			NPC.HitSound = SoundID.NPCHit3;
			NPC.DeathSound = SoundID.NPCDeath43;
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<Items.Banners.MoltenCoreBanner>();
		}

		public override void OnHitPlayer(Player target, int damage, bool crit) => target.AddBuff(24, 60 * 3);

		public override void AI()
		{
			Player player = Main.player[NPC.target];
			NPC.spriteDirection = NPC.direction;
			Movement();
			CheckPlatform();
			
			if (Main.rand.Next(15) == 0)
			{
				int index = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.Torch, 0.0f, 0.0f, 100, new Color(), 1f);
				Main.dust[index].noGravity = true;
				Main.dust[index].velocity.X = (float) Main.rand.Next(-3,3);
				Main.dust[index].velocity.Y = 2f;
			}
			
			if (NPC.ai[2] < 3)
			{
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					NPC.ai[2]++;
					int p = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, 0f, 0f, ModContent.ProjectileType<Molten_Core_Projectile>(), 8, 0, 0);
					Main.projectile[p].ai[1] = NPC.whoAmI;
				}
				NPC.netUpdate = true;
			}
		}

		private void CheckPlatform()
		{
			bool onplatform = true;
			for (int i = (int)NPC.position.X; i < NPC.position.X + NPC.width; i += NPC.width / 4) {
				Tile tile = Framing.GetTileSafely(new Point((int)NPC.position.X / 16, (int)(NPC.position.Y + NPC.height + 8) / 16));
				if (!TileID.Sets.Platforms[tile.TileType])
					onplatform = false;
			}
			if (onplatform)
				NPC.noTileCollide = true;
			else
				NPC.noTileCollide = false;
		}

		private void Movement()
		{
			NPC.noGravity = true;
			if (!NPC.noTileCollide)
			{
				if (NPC.collideX)
				{
					NPC.velocity.X = NPC.oldVelocity.X * -0.5f;
					if (NPC.direction == -1 && (double)NPC.velocity.X > 0.0 && (double)NPC.velocity.X < 2.0)
					{
						NPC.velocity.X = 2f;
					}

					if (NPC.direction == 1 && (double)NPC.velocity.X < 0.0 && (double)NPC.velocity.X > -2.0)
					{
						NPC.velocity.X = -2f;
					}
				}
				if (NPC.collideY)
				{
					NPC.velocity.Y = NPC.oldVelocity.Y * -0.5f;
					if ((double)NPC.velocity.Y > 0.0 && (double)NPC.velocity.Y < 1.0)
					{
						NPC.velocity.Y = 1f;
					}

					if ((double)NPC.velocity.Y < 0.0 && (double)NPC.velocity.Y > -1.0)
					{
						NPC.velocity.Y = -1f;
					}
				}
			}
			NPC.TargetClosest(true);
			if (Collision.CanHit(NPC.position, NPC.width, NPC.height, Main.player[NPC.target].position, Main.player[NPC.target].width, Main.player[NPC.target].height))
			{
				if ((double)NPC.ai[1] > 0.0 && !Collision.SolidCollision(NPC.position, NPC.width, NPC.height))
				{
					NPC.ai[1] = 0.0f;
					NPC.ai[0] = 0.0f;
					NPC.netUpdate = true;
				}
			}
			else if ((double)NPC.ai[1] == 0.0)
			{
				++NPC.ai[0];
			}

			if ((double)NPC.ai[0] >= 300.0)
			{
				NPC.ai[1] = 1f;
				NPC.ai[0] = 0.0f;
				NPC.netUpdate = true;
			}
			if ((double)NPC.ai[1] == 0.0)
			{
				NPC.alpha = 0;
				NPC.noTileCollide = false;
			}
			else
			{
				NPC.wet = false;
				NPC.alpha = 200;
				NPC.noTileCollide = true;
			}
			NPC.TargetClosest(true);
			if (NPC.direction == -1 && (double)NPC.velocity.X > -1.5 && (double)NPC.position.X > (double)Main.player[NPC.target].position.X + (double)Main.player[NPC.target].width)
			{
				NPC.velocity.X -= 0.08f;
				if ((double)NPC.velocity.X > 1.5)
				{
					NPC.velocity.X -= 0.04f;
				}
				else if ((double)NPC.velocity.X > 0.0)
				{
					NPC.velocity.X -= 0.2f;
				}

				if ((double)NPC.velocity.X < -1.5)
				{
					NPC.velocity.X = -1.5f;
				}
			}
			else if (NPC.direction == 1 && (double)NPC.velocity.X < 1.5 && (double)NPC.position.X + (double)NPC.width < (double)Main.player[NPC.target].position.X)
			{
				NPC.velocity.X += 0.08f;
				if ((double)NPC.velocity.X < -1.5)
				{
					NPC.velocity.X += 0.04f;
				}
				else if ((double)NPC.velocity.X < 0.0)
				{
					NPC.velocity.X += 0.2f;
				}

				if ((double)NPC.velocity.X > 1.5)
				{
					NPC.velocity.X = 1.5f;
				}
			}
			if (NPC.directionY == -1 && (double)NPC.velocity.Y > -1.5 && (double)NPC.position.Y > (double)Main.player[NPC.target].position.Y + (double)Main.player[NPC.target].height)
			{
				NPC.velocity.Y -= 0.1f;
				if ((double)NPC.velocity.Y > 1.5)
				{
					NPC.velocity.Y -= 0.05f;
				}
				else if ((double)NPC.velocity.Y > 0.0)
				{
					NPC.velocity.Y -= 0.15f;
				}

				if ((double)NPC.velocity.Y < -1.5)
				{
					NPC.velocity.Y = -1.5f;
				}
			}
			else if (NPC.directionY == 1 && (double)NPC.velocity.Y < 1.5 && (double)NPC.position.Y + (double)NPC.height < (double)Main.player[NPC.target].position.Y)
			{
				NPC.velocity.Y += 0.1f;
				if ((double)NPC.velocity.Y < -1.5)
				{
					NPC.velocity.Y += 0.05f;
				}
				else if ((double)NPC.velocity.Y < 0.0)
				{
					NPC.velocity.Y += 0.15f;
				}

				if ((double)NPC.velocity.Y > 1.5)
				{
					NPC.velocity.Y = 1.5f;
				}
			}
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (NPC.life <= 0)
			{
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("MoltenCoreGore1").Type, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("MoltenCoreGore2").Type, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("MoltenCoreGore3").Type, 1f);
			}
			for (int k = 0; k < 7; k++)
			{
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Torch, 2.5f * hitDirection, -2.5f, 0, default, 1.2f);
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Torch, 2.5f * hitDirection, -2.5f, 0, default, 0.5f);
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Torch, 2.5f * hitDirection, -2.5f, 0, default, 0.7f);
			}
		}

		//public override void OnKill()
		//{
		//	if (QuestManager.GetQuest<Mechanics.QuestSystem.Quests.StylistQuestMeteor>().IsActive && Main.rand.NextBool(3))
		//		Item.NewItem(npc.Center, ModContent.ItemType<Items.Sets.MaterialsMisc.QuestItems.MeteorDyeMaterial>());
		//}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.AddCommon<Items.Sets.GunsMisc.MeteoriteSpewer.Meteorite_Spewer>(33);
			npcLoot.AddCommon(116, 10);
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) => SpawnCondition.Meteor.Chance * 0.15f;

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			var effects = NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, NPC.Center - Main.screenPosition + new Vector2(0, NPC.gfxOffY), NPC.frame, drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effects, 0);
			return false;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) => GlowmaskUtils.DrawNPCGlowMask(spriteBatch, NPC, Mod.Assets.Request<Texture2D>("NPCs/MoltenCore/MoltenCore_Glow").Value, screenPos);

		public override void FindFrame(int frameHeight)
		{
			const int AnimationSpeed = 5;

			NPC.frameCounter++;
			if (NPC.frameCounter < AnimationSpeed * 1)
				NPC.frame.Y = 0 * frameHeight;
			else if (NPC.frameCounter < AnimationSpeed * 2)
				NPC.frame.Y = 1 * frameHeight;
			else if (NPC.frameCounter < AnimationSpeed * 3)
				NPC.frame.Y = 2 * frameHeight;
			else if (NPC.frameCounter < AnimationSpeed * 4)
				NPC.frame.Y = 3 * frameHeight;
			else if (NPC.frameCounter < AnimationSpeed * 5)
				NPC.frame.Y = 4 * frameHeight;
			else
				NPC.frameCounter = 0;
		}
	}
}