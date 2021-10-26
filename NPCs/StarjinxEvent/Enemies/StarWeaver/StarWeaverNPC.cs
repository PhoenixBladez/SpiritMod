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
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Particles;

namespace SpiritMod.NPCs.StarjinxEvent.Enemies.StarWeaver
{
	public class StarWeaverNPC : SpiritNPC, IStarjinxEnemy
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Star Weaver");
			Main.npcFrameCount[npc.type] = 6;
		}

		public override void SetDefaults()
		{
			npc.Size = new Vector2(70, 68);
			npc.lifeMax = 750;
			npc.damage = 40;
			npc.defense = 24;
			npc.noTileCollide = true;
			npc.noGravity = true;
			npc.aiStyle = -1;
			npc.value = 1100;
			npc.knockBackResist = .4f;
			npc.HitSound = new LegacySoundStyle(SoundID.NPCHit, 55).WithPitchVariance(0.2f);
			npc.DeathSound = SoundID.NPCDeath51;
		}

		public override void ScaleExpertStats(int numPlayers, float bossLifeScale) => npc.lifeMax = (int)(npc.lifeMax * 0.66f * bossLifeScale);

		public override bool CanHitPlayer(Player target, ref int cooldownSlot) => false;

		public ref float AiTimer => ref npc.ai[0];
		public ref float AiState => ref npc.ai[1];

		private int _headIndex = -1;
		public Projectile Head => Main.projectile[_headIndex];

		private const int IDLETIME = 160;

		public const int STATE_IDLE = 0;
		public const int STATE_TELEPORT = 1;
		public const int STATE_STARBURST = 2;

		public const float TELEPORT_DISTANCE = 400;
		public const int TELEPORT_STARTTIME = 30;
		public const int TELEPORT_ENDTIME = 30;

		public const int STARBURST_CHANNELTIME = 100;

		public override void AI()
		{
			Player player = Main.player[npc.target];
			npc.TargetClosest(true);
			npc.spriteDirection = npc.direction;

			if(player.dead || !player.active) //dont attack if no players to attack
			{
				AiState = STATE_IDLE;
				AiTimer = 0;
			}

			switch (AiState)
			{
				case STATE_IDLE:
					frame.X = 0;
					npc.AccelFlyingMovement(player.Center, 0.02f, 0.1f, 0.33f);
					npc.position.Y += 0.66f * (float)Math.Sin(Main.GameUpdateCount / 12f);
					if (AiTimer > IDLETIME)
					{
						bool teleport = !Collision.CanHit(npc.Center, 0, 0, player.Center, 0, 0) || npc.Distance(player.Center) > TELEPORT_DISTANCE;
						AiState = teleport ? STATE_TELEPORT : STATE_STARBURST;
						AiTimer = 0;
						npc.netUpdate = true;
					}
					break;
				case STATE_TELEPORT:
					frame.X = 1;
					npc.velocity = Vector2.Zero;

					if(AiTimer == TELEPORT_STARTTIME)
					{
						Vector2 desiredPos = player.Center + npc.DirectionTo(player.Center) * (TELEPORT_DISTANCE * 0.75f * Main.rand.NextFloat(0.9f, 1.1f));
						float displacement = npc.Distance(desiredPos);
						float mindisplacement = TELEPORT_DISTANCE;
						if (displacement < mindisplacement)
							desiredPos += npc.DirectionTo(player.Center) * (mindisplacement - displacement);

						npc.Center = desiredPos + new Vector2(0, 30);
						if (!Main.dedServ)
						{
							ParticleHandler.SpawnParticle(new ImpactLine(npc.Center, Vector2.UnitY * 2, Color.White, new Vector2(0.1f, 1f), 10));
							ParticleHandler.SpawnParticle(new ImpactLine(npc.Center, -Vector2.UnitY * 2, Color.White, new Vector2(0.1f, 1f), 10));
						}

						npc.netUpdate = true;
					}

					if(AiTimer >= TELEPORT_ENDTIME + TELEPORT_STARTTIME)
					{
						AiState = STATE_STARBURST;
						AiTimer = 0;
						npc.netUpdate = true;
					}
					break;
				case STATE_STARBURST:
					frame.X = 1;
					npc.velocity = Vector2.Zero;

					if(AiTimer == 1)
					{
						if(Main.netMode != NetmodeID.MultiplayerClient)
							Projectile.NewProjectileDirect(npc.Center, Vector2.Zero, ModContent.ProjectileType<WeaverStarChannel>(), NPCUtils.ToActualDamage(80, 1.5f), 1f, Main.myPlayer, npc.whoAmI);

						if (!Main.dedServ)
							Main.PlaySound(mod.GetLegacySoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/starCast").WithVolume(0.65f).WithPitchVariance(0.3f), npc.Center);
					}

					if(AiTimer > STARBURST_CHANNELTIME)
					{
						AiTimer = -Main.rand.Next(60);
						AiState = STATE_IDLE;
						npc.netUpdate = true;
					}
					break;
			}

			void SpawnHead()
			{
				if (Main.netMode == NetmodeID.MultiplayerClient)
					return;

				Projectile proj = Projectile.NewProjectileDirect(npc.Top - new Vector2(0, 20), Vector2.Zero, ModContent.ProjectileType<StarWeaverHead>(), NPCUtils.ToActualDamage(npc.damage, 1), 1f, Main.myPlayer, npc.whoAmI);
				_headIndex = proj.whoAmI;
				if (Main.netMode != NetmodeID.SinglePlayer)
					NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, _headIndex);

				npc.netUpdate = true;
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
			if (AiState != STATE_TELEPORT)
				return 0;

			bool start = AiTimer <= TELEPORT_STARTTIME;
			float progress = (start) ? (AiTimer / TELEPORT_STARTTIME) : (1 - ((AiTimer - TELEPORT_STARTTIME) / TELEPORT_ENDTIME));

			float speed = 2.5f;
			float opacity = 1 - MathHelper.Clamp(speed * (1 - progress), 0, 1); //reach 1 faster, while still being capped at 1

			return opacity;
		}

		private float TeleportWidth()
		{
			if (AiState != STATE_TELEPORT)
				return 1;

			bool start = AiTimer <= TELEPORT_STARTTIME;
			float progress = (start) ? (AiTimer / TELEPORT_STARTTIME) : (1 - ((AiTimer - TELEPORT_STARTTIME) / TELEPORT_ENDTIME));

			float speed = 5;
			float width = MathHelper.Clamp(speed * (1 - progress), 0, 1);

			return width;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			if (npc.frame.Width > 70)
			{
				frame = new Point(0, 0);
				npc.FindFrame();
			}

			if (AiState == STATE_STARBURST)
			{
				float num395 = Main.mouseTextColor / 200f - 0.35f;
				num395 *= 0.2f;
				float num366 = num395 + 1.1f;
				DrawAfterImage(Main.spriteBatch, new Vector2(0f, 0f), 0.5f, new Color(255, 234, 0), new Color(255, 234, 0) * .3f, 0.45f, num366, .65f);
			}

			Vector2 scale = new Vector2(TeleportWidth(), 1) * npc.scale;
			spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition, npc.frame, drawColor, npc.rotation, npc.frame.Size() / 2,
				scale, (npc.spriteDirection > 0) ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);

			return false;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Vector2 scale = new Vector2(TeleportWidth(), 1) * npc.scale;
			spriteBatch.Draw(ModContent.GetTexture(Texture + "_glow"), npc.Center - Main.screenPosition, npc.frame, Color.White, npc.rotation, npc.frame.Size() / 2,
				scale, (npc.spriteDirection > 0) ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);

			spriteBatch.Draw(ModContent.GetTexture(Texture + "_mask"), npc.Center - Main.screenPosition, npc.frame, Color.White * TeleportMaskOpacity(), npc.rotation, npc.frame.Size() / 2,
				scale, (npc.spriteDirection > 0) ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
		}

		public void DrawPathfinderOutline(SpriteBatch spriteBatch) => PathfinderOutlineDraw.DrawAfterImage(spriteBatch, npc, npc.frame, Vector2.Zero, Color.White, 0.75f, 1, 1.4f, npc.frame.Size() / 2);

		public void DrawAfterImage(SpriteBatch spriteBatch, Vector2 offset, float trailLengthModifier, Color startColor, Color endColor, float opacity, float startScale, float endScale)
		{
			SpriteEffects spriteEffects = (npc.spriteDirection == 1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			for (int i = 1; i < 10; i++)
			{
				Color color = Color.Lerp(startColor, endColor, i / 10f) * opacity;
				spriteBatch.Draw(ModContent.GetTexture(Texture + "_mask"), new Vector2(npc.Center.X, npc.Center.Y) + offset - Main.screenPosition + new Vector2(0, npc.gfxOffY) - npc.velocity * (float)i * trailLengthModifier, npc.frame, color, npc.rotation, npc.frame.Size() * 0.5f, MathHelper.Lerp(startScale, endScale, i / 10f), spriteEffects, 0f);
			}
		}

		#endregion

		public override void SendExtraAI(BinaryWriter writer) => writer.Write(_headIndex);

		public override void ReceiveExtraAI(BinaryReader reader) => _headIndex = reader.ReadInt32();

		public override void SafeFindFrame(int frameHeight) => npc.frame.Width = 70;
	}
}