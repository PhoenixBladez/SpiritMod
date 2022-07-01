using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod;
using SpiritMod.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Particles;
using SpiritMod.Items.Armor.StarjinxSet;

namespace SpiritMod.NPCs.StarjinxEvent.Enemies.StarWeaver
{
	public class StarWeaverNPC : SpiritNPC, IStarjinxEnemy
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Star Weaver");
			Main.npcFrameCount[NPC.type] = 6;
		}

		public override bool IsLoadingEnabled(Mod mod) => false;

		public override void SetDefaults()
		{
			NPC.Size = new Vector2(70, 68);
			NPC.lifeMax = 750;
			NPC.damage = 40;
			NPC.defense = 24;
			NPC.noTileCollide = true;
			NPC.noGravity = true;
			NPC.aiStyle = -1;
			NPC.value = 1100;
			NPC.knockBackResist = .4f;
			NPC.HitSound = new LegacySoundStyle(SoundID.NPCHit, 55).WithPitchVariance(0.2f);
			NPC.DeathSound = SoundID.NPCDeath51;
		}

		public override void ScaleExpertStats(int numPlayers, float bossLifeScale) => NPC.lifeMax = (int)(NPC.lifeMax * 0.66f * bossLifeScale);

		public override bool CanHitPlayer(Player target, ref int cooldownSlot) => false;

		public ref float AiTimer => ref NPC.ai[0];
		public ref float AiState => ref NPC.ai[1];

		private int _headIndex = -1;
		public Projectile Head => Main.projectile[_headIndex];

		private const int IDLETIME = 160;

		public const int STATE_IDLE = 0;
		public const int STATE_TELEPORT = 1;
		public const int STATE_STARBURST = 2;
		public const int STATE_STARGLOOP = 3;
		public const int STATE_TELEPORT_OOB = 4;

		public const float TELEPORT_DISTANCE = 400;
		public const int TELEPORT_STARTTIME = 30;
		public const int TELEPORT_ENDTIME = 45;

		public const int STARBURST_CHANNELTIME = 100;
		public const int STARGLOOP_TIME = 150;
		public const int STARGLOOP_NUMSHOTS = 3;

		public override void AI()
		{
			Player player = Main.player[NPC.target];
			NPC.TargetClosest(true);
			NPC.spriteDirection = NPC.direction;

			if(player.dead || !player.active) //dont attack if no players to attack
			{
				AiState = STATE_IDLE;
				AiTimer = 0;
			}

			switch (AiState)
			{
				case STATE_IDLE:
					frame.X = 0;
					NPC.AccelFlyingMovement(player.Center, 0.02f, 0.1f, 0.33f);
					NPC.position.Y += 0.66f * (float)Math.Sin(Main.GameUpdateCount / 12f);
					if (AiTimer > IDLETIME)
					{
						bool teleport = !Collision.CanHit(NPC.Center, 0, 0, player.Center, 0, 0) || NPC.Distance(player.Center) > TELEPORT_DISTANCE;
						AiState = teleport ? STATE_TELEPORT : Main.rand.NextBool() ? STATE_STARGLOOP : STATE_STARBURST;
						AiTimer = 0;
						NPC.netUpdate = true;
					}
					break;
				case STATE_TELEPORT:
					frame.X = 1;
					NPC.velocity = Vector2.Zero;

					if(AiTimer == TELEPORT_STARTTIME)
					{
						Vector2 desiredPos = player.Center + NPC.DirectionTo(player.Center) * (TELEPORT_DISTANCE * 0.75f * Main.rand.NextFloat(0.9f, 1.1f));
						float displacement = NPC.Distance(desiredPos);
						if (displacement < TELEPORT_DISTANCE)
							desiredPos += NPC.DirectionTo(player.Center) * (TELEPORT_DISTANCE - displacement);

						NPC.Center = desiredPos + new Vector2(0, 30);

						if (!Main.dedServ)
						{
							ParticleHandler.SpawnParticle(new ImpactLine(NPC.Center, Vector2.UnitY * 2, Color.White, new Vector2(0.1f, 1f), 10));
							ParticleHandler.SpawnParticle(new ImpactLine(NPC.Center, -Vector2.UnitY * 2, Color.White, new Vector2(0.1f, 1f), 10));
						}

						NPC.netUpdate = true;
					}

					if(AiTimer >= TELEPORT_ENDTIME + TELEPORT_STARTTIME)
					{
						if (Vector2.DistanceSquared(NPC.Center, player.GetModPlayer<StarjinxPlayer>().StarjinxPosition) > StarjinxMeteorite.EVENT_RADIUS * StarjinxMeteorite.EVENT_RADIUS)
							AiState = STATE_TELEPORT_OOB;
						else
							AiState = Main.rand.NextBool() ? STATE_STARGLOOP : STATE_STARBURST;
						AiTimer = 0;
						NPC.netUpdate = true;
					}
					break;
				case STATE_TELEPORT_OOB:
					frame.X = 1;
					NPC.velocity = Vector2.Zero;

					if (AiTimer == TELEPORT_STARTTIME / 2)
					{
						Vector2 desiredPos = player.Center + NPC.DirectionTo(player.Center).RotatedByRandom(MathHelper.PiOver4) * (TELEPORT_DISTANCE * 0.75f * Main.rand.NextFloat(0.9f, 1.1f));
						float displacement = NPC.Distance(desiredPos);
						if (displacement < TELEPORT_DISTANCE)
							desiredPos += NPC.DirectionTo(player.Center) * (TELEPORT_DISTANCE - displacement);

						NPC.Center = desiredPos + new Vector2(0, 30);

						if (!Main.dedServ)
						{
							ParticleHandler.SpawnParticle(new ImpactLine(NPC.Center, Vector2.UnitY * 2, Color.White, new Vector2(0.1f, 1f), 10));
							ParticleHandler.SpawnParticle(new ImpactLine(NPC.Center, -Vector2.UnitY * 2, Color.White, new Vector2(0.1f, 1f), 10));
						}

						NPC.netUpdate = true;
					}

					if (AiTimer >= TELEPORT_ENDTIME + (TELEPORT_STARTTIME / 2))
					{
						AiState = Main.rand.NextBool() ? STATE_STARGLOOP : STATE_STARBURST;
						AiTimer = 0;
						NPC.netUpdate = true;
					}
					break;
				case STATE_STARBURST:
					frame.X = 1;
					NPC.velocity = Vector2.Zero;

					if(AiTimer == 1)
					{
						if(Main.netMode != NetmodeID.MultiplayerClient)
							Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<WeaverStarChannel>(), NPCUtils.ToActualDamage(80, 1.5f), 1f, Main.myPlayer, NPC.whoAmI);

						if (!Main.dedServ)
							SoundEngine.PlaySound(Mod.GetLegacySoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/starCast").WithVolume(0.65f).WithPitchVariance(0.3f), NPC.Center);
					}

					if(AiTimer > STARBURST_CHANNELTIME)
					{
						AiTimer = -Main.rand.Next(60);
						AiState = STATE_IDLE;
						NPC.netUpdate = true;
					}
					break;

				case STATE_STARGLOOP: //Attack mostly handled by head projectile, this is just for controlling the body and setting back to another state when done
					frame.X = 1;
					NPC.velocity = Vector2.Zero;

					if (AiTimer > STARGLOOP_TIME)
					{
						AiTimer = -Main.rand.Next(60);
						AiState = STATE_IDLE;
						NPC.netUpdate = true;
					}
					break;
			}

			void SpawnHead()
			{
				if (Main.netMode == NetmodeID.MultiplayerClient)
					return;

				var proj = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Top - new Vector2(0, 20), Vector2.Zero, ModContent.ProjectileType<StarWeaverHead>(), NPCUtils.ToActualDamage(NPC.damage, 1), 1f, Main.myPlayer, NPC.whoAmI);
				_headIndex = proj.whoAmI;
				if (Main.netMode != NetmodeID.SinglePlayer)
					NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, _headIndex);

				NPC.netUpdate = true;
			}

			if (_headIndex < 0)
				SpawnHead();
			else if (!Head.active || Head.type != ModContent.ProjectileType<StarWeaverHead>())
				SpawnHead();

			++AiTimer;
			UpdateYFrame(10, 0, 5);
		}

		#region Drawing
		private float TeleportMaskOpacity()
		{
			if (AiState != STATE_TELEPORT || AiState != STATE_TELEPORT_OOB)
				return 0;

			bool start = AiTimer <= TELEPORT_STARTTIME;
			float progress = (start) ? (AiTimer / TELEPORT_STARTTIME) : (1 - ((AiTimer - TELEPORT_STARTTIME) / TELEPORT_ENDTIME));

			float speed = 2.5f;
			float opacity = 1 - MathHelper.Clamp(speed * (1 - progress), 0, 1); //reach 1 faster, while still being capped at 1

			return opacity;
		}

		private float TeleportWidth()
		{
			if (AiState != STATE_TELEPORT || AiState != STATE_TELEPORT_OOB)
				return 1;

			bool start = AiTimer <= TELEPORT_STARTTIME;
			float progress = (start) ? (AiTimer / TELEPORT_STARTTIME) : (1 - ((AiTimer - TELEPORT_STARTTIME) / TELEPORT_ENDTIME));

			float speed = 5;
			float width = MathHelper.Clamp(speed * (1 - progress), 0, 1);

			return width;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			if (NPC.frame.Width > 70)
			{
				frame = new Point(0, 0);
				NPC.FindFrame();
			}

			Vector2 scale = new Vector2(TeleportWidth(), 1) * NPC.scale;
			spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, NPC.Center - Main.screenPosition, NPC.frame, GetAlpha(drawColor).Value, NPC.rotation, NPC.frame.Size() / 2,
				scale, (NPC.spriteDirection > 0) ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);

			return false;
		}

		public override Color? GetAlpha(Color drawColor) => StarjinxGlobalNPC.GetColorBrightness(drawColor) * NPC.Opacity;

		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			Vector2 scale = new Vector2(TeleportWidth(), 1) * NPC.scale;

			Color bloomColor = Color.White;
			bloomColor.A = 0;
			float AttackProgress = GetBloomIntensity(this);

			PulseDraw.DrawPulseEffect(PulseDraw.BloomConstant, 12, 16, delegate (Vector2 posOffset, float opacityMod)
			{
				spriteBatch.Draw(ModContent.Request<Texture2D>(Texture + "_glow"), NPC.Center + posOffset - Main.screenPosition, NPC.frame, NPC.GetAlpha(bloomColor) * opacityMod * AttackProgress,
					NPC.rotation, NPC.frame.Size() / 2, scale, (NPC.spriteDirection > 0) ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
			});

			Color glowmaskColor = Color.Lerp(Color.White, bloomColor, AttackProgress);
			spriteBatch.Draw(ModContent.Request<Texture2D>(Texture + "_glow"), NPC.Center - Main.screenPosition, NPC.frame, NPC.GetAlpha(glowmaskColor), NPC.rotation, NPC.frame.Size() / 2,
				scale, (NPC.spriteDirection > 0) ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);

			spriteBatch.Draw(ModContent.Request<Texture2D>(Texture + "_mask"), NPC.Center - Main.screenPosition, NPC.frame, NPC.GetAlpha(Color.White) * TeleportMaskOpacity(), NPC.rotation, NPC.frame.Size() / 2,
				scale, (NPC.spriteDirection > 0) ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
		}

		public static float GetBloomIntensity(StarWeaverNPC starWeaver)
		{
			float AttackProgress = 0.1f;
			switch (starWeaver.AiState)
			{
				case STATE_STARBURST:
					AttackProgress = starWeaver.AiTimer / STARBURST_CHANNELTIME;
					AttackProgress = MathHelper.Min(AttackProgress, 1);
					AttackProgress = EaseFunction.EaseCubicIn.Ease(AttackProgress);
					break;
				case STATE_STARGLOOP:
					AttackProgress = starWeaver.AiTimer / STARGLOOP_TIME;
					AttackProgress = EaseFunction.EaseQuadOut.Ease(AttackProgress);
					AttackProgress *= 0.5f;
					break;
			}
			AttackProgress = Math.Max(AttackProgress * 0.75f, 0.125f);
			return AttackProgress;
		}

		public void DrawPathfinderOutline(SpriteBatch spriteBatch)
		{
			//Draw the bloom effect for the head in addition
			Texture2D headTex = TextureAssets.Projectile[Head.type].Value;
			PathfinderOutlineDraw.DrawAfterImage(spriteBatch, headTex, Head.Center, Head.DrawFrame(), Vector2.Zero, Head.Opacity, Head.rotation, Head.scale, Head.DrawFrame().Size()/2, 
				(Head.spriteDirection < 0) ? SpriteEffects.FlipHorizontally : SpriteEffects.None);

			PathfinderOutlineDraw.DrawAfterImage(spriteBatch, NPC, NPC.frame, Vector2.Zero, NPC.frame.Size() / 2);
		}

		#endregion

		public override void OnKill()
		{
			float chance = Main.expertMode ? 0.1f : 0.05f;
			NPC.DropItem(ModContent.ItemType<StargloopHead>(), chance);
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int i = 0; i < 12; i++)
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.VilePowder, 2.5f * hitDirection, -2.5f, 0, default, Main.rand.NextFloat(.45f, .75f));
		}

		public override void SendExtraAI(BinaryWriter writer) => writer.Write(_headIndex);
		public override void ReceiveExtraAI(BinaryReader reader) => _headIndex = reader.ReadInt32();
		public override void SafeFindFrame(int frameHeight) => NPC.frame.Width = 70;
	}
}