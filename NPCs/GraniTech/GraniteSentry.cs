using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SpiritMod.Utilities;
using SpiritMod.Particles;
using SpiritMod.Items.Sets.GranitechSet;
using System.IO;

namespace SpiritMod.NPCs.GraniTech
{
	class GraniteSentry : ModNPC
	{
		public float BaseState { get => npc.ai[0]; private set => npc.ai[0] = value; }

		private bool _firing;
		private bool Firing
		{
			get => _firing;
			set
			{
				if (_firing != value)
					npc.netUpdate = true;
				_firing = value;
			}
		}

		private Vector2 laserEdge = Vector2.Zero;
		private Vector2 laserOrigin = Vector2.Zero;
		private float chargeUp = 0;
		private float recoil = 0;
		private float scanTimer = 0;

		private const int STATE_SPAWNING = 0;
		private const int STATE_ABOVE = 1;
		private const int STATE_BELOW = 2;
		private const int STATE_FALLING = 3;

		private const int FIRING_CHARGE_TIME = 20;
		private const int FIRING_SHOOT_TIME = 6;

		private List<(float, int)> laserRotations = new List<(float, int)>();

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("GraniTec Turret");
			Main.npcFrameCount[npc.type] = 3;
		}

		public override void SetDefaults()
		{
			npc.width = 44; //Stats placeholder -->
			npc.height = 46;
			npc.damage = 0;
			npc.defense = 24;
			npc.lifeMax = 800; // <--
			npc.noGravity = true;
			npc.noTileCollide = true;
			npc.value = 800;
			npc.knockBackResist = 0f;
			npc.aiStyle = -1;
			npc.DeathSound = SoundID.NPCDeath37;
			npc.HitSound = SoundID.NPCHit4;

			for (int k = 0; k < npc.buffImmune.Length; k++)
				npc.buffImmune[k] = true;
		}

		private const int GroundDistance = 80; //Distance it'll scan to look for a valid wall

		public override void AI()
		{
			if (BaseState == STATE_SPAWNING)
				Spawn();
			else if (BaseState != STATE_FALLING)
			{
				if (chargeUp <= 6)
					scanTimer += 0.01f;

				if (!Firing)
					ScanningAI();
				else
					FiringAI();

				laserOrigin = npc.Center - new Vector2(0, 6);

				if (BaseState == STATE_ABOVE)
					npc.rotation = (float)(Math.Sin(scanTimer + recoil) * MathHelper.PiOver2) - MathHelper.PiOver2;
				else
					npc.rotation = (float)(Math.Sin(scanTimer + recoil) * MathHelper.PiOver2) + MathHelper.PiOver2;

				for (int i = 0; i < 1600; i += 8)
				{
					Vector2 toLookAt = laserOrigin + ((npc.rotation.ToRotationVector2().RotatedBy(3.14f)) * i);
					if (i >= 1590 || (Framing.GetTileSafely((int)(toLookAt.X / 16), (int)(toLookAt.Y / 16)).active() && Main.tileSolid[Framing.GetTileSafely((int)(toLookAt.X / 16), (int)(toLookAt.Y / 16)).type]))
					{
						laserEdge = toLookAt;
						break;
					}
				}
			}
			else
			{
				if (chargeUp <= 6)
					scanTimer += 0.02f;

				npc.velocity.Y += 0.2f;
				npc.rotation = (float)(Math.Sin(scanTimer + recoil) * MathHelper.PiOver2) - MathHelper.PiOver2;

				Vector2 adjPos = npc.position + new Vector2(2, npc.height - 8);
				if (Collision.SolidCollision(adjPos, npc.width - 4, 4))
				{
					npc.StrikeNPCNoInteraction((int)(npc.velocity.Y * 60), 0f, 0, true, false);

					if (npc.life > 0)
					{
						npc.velocity.Y = 0;

						BaseState = STATE_BELOW;
					}
				}
			}

			AnchorChecks();
		}

		private void AnchorChecks()
		{
			if (BaseState == STATE_ABOVE) //Ceiling check
			{
				Vector2 adjPos = npc.position + new Vector2(2, -18);
				if (!Collision.SolidCollision(adjPos, npc.width - 4, 4))
					BaseState = STATE_FALLING;
			}
			else if (BaseState == STATE_BELOW) //Ground check
			{
				Vector2 adjPos = npc.position + new Vector2(2, npc.height - 8);
				if (!Collision.SolidCollision(adjPos, npc.width - 4, 4))
					BaseState = STATE_FALLING;
			}
		}

		private void Spawn()
		{
			bool[] validGrounds = new bool[2] { false, false };

			Point tilePos = npc.position.ToTileCoordinates();

			for (int j = tilePos.Y; j < tilePos.Y + GroundDistance; j++) //below
				if (Framing.GetTileSafely(tilePos.X, j).active() && Main.tileSolid[Framing.GetTileSafely(tilePos.X, j).type])
					validGrounds[0] = true;

			for (int j = tilePos.Y; j > tilePos.Y - GroundDistance; j--) //above
				if (Framing.GetTileSafely(tilePos.X, j).active() && Main.tileSolid[Framing.GetTileSafely(tilePos.X, j).type])
					validGrounds[1] = true;

			if (!validGrounds.Any(x => x))
				npc.active = false; //Delete me if I don't have anchoring

			int index;
			int safety = 0;

			do //Choose a random placement
			{
				index = Main.rand.Next(validGrounds.Length);
				safety++;
			} while (validGrounds[index] && safety < 100);

			BaseState = index + 1;

			switch (BaseState)
			{
				case STATE_ABOVE:
					for (int i = (int)(npc.position.Y / 16f); i > (int)(npc.position.Y / 16f) - GroundDistance; --i) //above
					{
						if (Framing.GetTileSafely((int)(npc.position.X / 16f), i).active() && Main.tileSolid[Framing.GetTileSafely((int)(npc.position.X / 16f), i).type])
						{
							npc.position.Y = (i * 16f) + 28;
							break;
						}
					}
					break;
				case STATE_BELOW:
					for (int i = (int)(npc.position.Y / 16f); i < (int)(npc.position.Y / 16f) + GroundDistance; ++i) //below
					{
						if (Framing.GetTileSafely((int)(npc.position.X / 16f), i).active() && Main.tileSolid[Framing.GetTileSafely((int)(npc.position.X / 16f), i).type])
						{
							npc.position.Y = ((i - 2) * 16f) - 8;
							break;
						}
					}
					break;
				default:
					break;
			}
			npc.netUpdate = true;
		}

		private void ScanningAI()
		{
			if (laserRotations == null)
				laserRotations = new List<(float, int)>();

			Vector2 delta = laserEdge - laserOrigin;
			int length = (int)delta.Length();
			float rotation = delta.ToRotation();
			laserRotations.Add((rotation, length));

			while (laserRotations.Count > 16)
				laserRotations.RemoveAt(0);

			chargeUp = 0;
			ScanPlayers();
		}

		private void FiringAI()
		{
			chargeUp++;
			if (laserRotations != null)
				if (laserRotations.Count > 0)
					laserRotations.RemoveAt(0);

			//Once fully charged, shoot bullets every few ticks
			if (chargeUp > FIRING_CHARGE_TIME && chargeUp % FIRING_SHOOT_TIME == 0)
			{
				//Fire bullets and recoil
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					recoil = Main.rand.NextFloat(-0.1f, 0.1f);
					switch (BaseState)
					{
						case 1: //above
							npc.rotation = (float)(Math.Sin(scanTimer + recoil) * MathHelper.PiOver2) - MathHelper.PiOver2;
							break;

						case 2: //below
							npc.rotation = (float)(Math.Sin(scanTimer + recoil) * MathHelper.PiOver2) + MathHelper.PiOver2;
							break;
					}
					Projectile.NewProjectile(laserOrigin, (npc.rotation + 3.14f).ToRotationVector2() * 20, ModContent.ProjectileType<GraniteSentryBolt>(), NPCUtils.ToActualDamage(90), 3, Main.myPlayer);

					laserRotations = null;
					npc.netUpdate = true;
				}

				//Client side visuals and sound
				if (Main.netMode != NetmodeID.Server)
				{
					Main.PlaySound(SoundID.Item, (int)npc.Center.X, (int)npc.Center.Y, 91, 0.5f, 0.5f);

					Vector2 origin = laserOrigin + (npc.rotation + 3.14f).ToRotationVector2() * 20;
					ParticleHandler.SpawnParticle(new PulseCircle(npc, new Color(25, 132, 247) * 0.4f, 100, 15, PulseCircle.MovementType.OutwardsSquareRooted, origin)
					{
						Angle = npc.rotation + MathHelper.Pi,
						ZRotation = 0.6f,
						RingColor = new Color(25, 132, 247),
						Velocity = (npc.rotation + 3.14f).ToRotationVector2() * 3
					});
				}
			}

			recoil *= 0.99f;
			Firing = false;
			ScanPlayers(3);
		}

		private bool ScanPlayers(float widthMod = 1f)
		{
			for (int i = 0; i < Main.player.Length; i++)
			{
				Player player = Main.player[i];
				if (player.active && !player.dead && player != null)
				{
					float collisionPoint = 0f;
					if (Collision.CheckAABBvLineCollision(player.Hitbox.TopLeft(), player.Hitbox.Size(), laserOrigin, laserEdge, (npc.width + npc.height) * npc.scale * widthMod, ref collisionPoint))
					{
						Firing = true;
						return true;
					}
				}
			}
			return false;
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.WriteVector2(laserEdge);
			writer.WriteVector2(laserOrigin);
			writer.Write(chargeUp);
			writer.Write(recoil);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			laserEdge = reader.ReadVector2();
			laserOrigin = reader.ReadVector2();
			chargeUp = reader.ReadSingle();
			recoil = reader.ReadSingle();
		}

		public override void NPCLoot()
		{
			for (int i = 1; i <= 2; i++)
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot($"Gores/GraniTech/GraniteSentryGore{i}"), 1f);

			npc.DropItem(ModContent.ItemType<GranitechMaterial>(), Main.rand.Next(1, 4));
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) => (spawnInfo.spawnTileType == TileID.Granite) && spawnInfo.spawnTileY > Main.rockLayer && Main.hardMode ? 0.6f : 0f;

		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			if (BaseState != STATE_FALLING)
				DrawLaser(spriteBatch);

			Texture2D npcGlow = ModContent.GetTexture(Texture + "_glow");
			Vector2 realPos = npc.position - Main.screenPosition;
			Vector2 offset;
			Rectangle baseRect = new Rectangle(0, 32, 44, 18);
			float baseRotation;

			if (BaseState == STATE_ABOVE || BaseState == STATE_FALLING) //On ceiling
			{
				offset = new Vector2(44, 2);
				baseRotation = MathHelper.Pi;
			}
			else //On ground
			{
				offset = new Vector2(0, 24);
				baseRotation = 0;
			}

			spriteBatch.Draw(Main.npcTexture[npc.type], realPos + offset, baseRect, drawColor, baseRotation, new Vector2(), npc.scale, SpriteEffects.None, 0f);
			DrawAberration.DrawChromaticAberration(Vector2.UnitX, 2f, delegate (Vector2 aberrationOffset, Color colorMod)
			{
				spriteBatch.Draw(npcGlow, realPos + offset + aberrationOffset, baseRect, Color.White.MultiplyRGBA(colorMod), baseRotation, new Vector2(), npc.scale, SpriteEffects.None, 0f);
			});

			float rot = npc.rotation; //Rotation
			SpriteEffects s = SpriteEffects.None;

			if (BaseState == 1) s = SpriteEffects.FlipVertically;

			if (rot > Math.PI / 2f && rot < (Math.PI * 2) - (Math.PI / 2)) //Face the right direction
			{
				rot -= (float)Math.PI;
				s = SpriteEffects.FlipHorizontally;
				if (BaseState == 1) s = SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically;
			}

			Vector2 topOffset = new Vector2(22, 15);
			Rectangle topRect = new Rectangle(0, 0, 44, 30);
			Vector2 topOrigin = new Vector2(22, 15);

			spriteBatch.Draw(Main.npcTexture[npc.type], realPos + topOffset, topRect, drawColor, rot, topOrigin, npc.scale, s, 0f);
			DrawAberration.DrawChromaticAberration(Vector2.UnitX.RotatedBy(rot), 2f, delegate (Vector2 aberrationOffset, Color colorMod)
			{
				spriteBatch.Draw(npcGlow, realPos + topOffset + aberrationOffset, topRect, Color.White.MultiplyRGBA(colorMod), rot, topOrigin, npc.scale, s, 0f);
			});
			return false;
		}

		private void DrawLaser(SpriteBatch spriteBatch)
		{
			void DrawLaserIndividual(Color laserColor, float opacity, float k, float oldRot, float rotDifference, int oldLength, int currentLength)
			{
				float rot = k + oldRot;
				float lerper = Math.Abs(k / rotDifference);
				lerper *= lerper * lerper;
				Color color = Color.Lerp(laserColor, new Color(25, 132, 247), (lerper * lerper) / 2);
				color.A = 0;
				color *= opacity;
				float newLength = MathHelper.Lerp(oldLength, currentLength, lerper);
				spriteBatch.Draw(Main.magicPixel, laserOrigin - Main.screenPosition, new Rectangle(0, 0, 1, 1), color * lerper * 0.75f, rot, Vector2.Zero, new Vector2(newLength, 3), SpriteEffects.None, 0);
			}

			Vector2 delta = laserEdge - laserOrigin;
			float length = delta.Length();
			float rotation = delta.ToRotation();

			if (chargeUp < FIRING_CHARGE_TIME)
			{
				float progress = chargeUp / FIRING_CHARGE_TIME;
				//Cyan when about to fire, otherwise blue
				Color laserColor = Color.Lerp(new Color(25, 132, 247), new Color(99, 255, 229), progress);
				laserColor.A = 0;

				//Increase in opacity when about to fire, and fluctuate over time
				float opacity = MathHelper.Lerp(chargeUp / FIRING_CHARGE_TIME, 1, 0.12f);
				opacity *= ((float)Math.Sin(Main.GlobalTime * 6) * 0.15f) + 1;
				if (laserRotations != null && laserRotations.Count > 3)
				{
					float oldRot = laserRotations[0].Item1;
					int oldLength = laserRotations[0].Item2;
					float rotDifference = ((((rotation - oldRot) % 6.28f) + 9.42f) % 6.28f) - 3.14f;
					float step = 0.005f * Math.Sign(rotDifference);

					if (rotDifference > 0)
						for (float k = 0; k <= rotDifference; k += step)
							DrawLaserIndividual(laserColor, opacity, k, oldRot, rotDifference, oldLength, (int)length);
					else
						for (float k = rotDifference; k <= 0; k -= step)
							DrawLaserIndividual(laserColor, opacity, k, oldRot, rotDifference, oldLength, (int)length);
				}
				laserColor *= opacity;

				spriteBatch.Draw(Main.magicPixel, laserOrigin - Main.screenPosition, new Rectangle(0, 0, 1, 1), laserColor, rotation, new Vector2(0f, 1f), new Vector2(length, 3 - (chargeUp / 24f)), SpriteEffects.None, 0f);
			}
		}
	}

	public class GraniteSentryBolt : ModProjectile, IDrawAdditive
	{
		private readonly Color lightCyan = new Color(99, 255, 229);
		private readonly Color midBlue = new Color(25, 132, 247);
		private readonly Color darkBlue = new Color(20, 8, 189);

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Laser Bolt");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 8;
			ProjectileID.Sets.TrailingMode[projectile.type] = 2;
		}

		private float glow = 0;
		public override void SetDefaults()
		{
			projectile.penetrate = 3;
			projectile.tileCollide = false;
			projectile.hostile = true;
			projectile.friendly = false;
			projectile.width = 36;
			projectile.height = 18;
			projectile.tileCollide = true;
			projectile.alpha = 0;
			projectile.hide = true;
		}

		public override void AI()
		{
			if (glow == 0)
			{
				for (int i = 0; i < 12; i++)
					ParticleHandler.SpawnParticle(new GranitechParticle(projectile.Center + projectile.velocity,
						projectile.velocity.RotatedByRandom(MathHelper.Pi / 5) * Main.rand.NextFloat(0.66f), Main.rand.NextBool() ? lightCyan : midBlue,
						Main.rand.NextFloat(1f, 1.25f), 20));

				glow = Main.rand.NextFloat();
			}
			projectile.rotation = projectile.velocity.ToRotation() + 3.14f;
		}

		public void AdditiveCall(SpriteBatch spriteBatch)
		{
			Texture2D projTex = Main.projectileTexture[projectile.type];
			int trailLength = ProjectileID.Sets.TrailCacheLength[projectile.type];

			DrawAberration.DrawChromaticAberration(Vector2.Normalize(projectile.velocity), 2f, delegate (Vector2 offset, Color colorMod)
			{
				Color color = lightCyan.MultiplyRGB(colorMod);
				for (int i = 0; i < trailLength; i++)
				{
					float progress = i / (float)trailLength;
					float opacity = 1 - progress;
					opacity *= 0.66f;
					var trailColor = Color.Lerp(midBlue, darkBlue, progress);

					spriteBatch.Draw(projTex, projectile.oldPos[i] + (projectile.Size / 2) + offset - Main.screenPosition, null, trailColor * opacity,
						projectile.velocity.ToRotation(), projTex.Size() / 2, projectile.scale, SpriteEffects.None, 0);
				};

				spriteBatch.Draw(projTex, projectile.Center + offset - Main.screenPosition, null, color,
					projectile.velocity.ToRotation(), projTex.Size() / 2, projectile.scale, SpriteEffects.None, 0);
			});
		}

		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 14; i++)
				ParticleHandler.SpawnParticle(new GranitechParticle(projectile.Center, projectile.oldVelocity.RotatedByRandom(MathHelper.Pi / 12) * Main.rand.NextFloat(0.66f),
					Main.rand.NextBool() ? lightCyan : midBlue, Main.rand.NextFloat(1f, 1.25f), 20));
		}

		public override Color? GetAlpha(Color lightColor) => new Color(99, 255, 229, 0);
	}
}