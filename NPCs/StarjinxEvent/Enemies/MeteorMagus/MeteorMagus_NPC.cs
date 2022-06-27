using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod;
using SpiritMod.NPCs.StarjinxEvent.Enemies.Pathfinder;
using SpiritMod.Particles;
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

namespace SpiritMod.NPCs.StarjinxEvent.Enemies.MeteorMagus
{
	public class MeteorMagus_NPC : SpiritNPC, IStarjinxEnemy, IDrawAdditive
	{
		private const int IDLETIME = 210;
		private static Color Pink = new Color(255, 92, 211);
		private static Color Orange = new Color(255, 98, 74);
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Meteor Magus");
			Main.npcFrameCount[NPC.type] = 6;
		}

		public override bool IsLoadingEnabled(Mod mod) => false;

		//large chunk of this(attack pattern use and randomizing) directly ripped from haunted tome, TODO: reduce boilerplate a lot

		public override void SetDefaults()
		{
			NPC.Size = new Vector2(168, 118);
			NPC.lifeMax = 1500;
			NPC.damage = 56;
			NPC.defense = 28;
			NPC.noTileCollide = true;
			NPC.noGravity = true;
			NPC.aiStyle = -1;
			NPC.value = 1100;
			NPC.knockBackResist = .55f;
			NPC.HitSound = new LegacySoundStyle(SoundID.NPCHit, 55).WithPitchVariance(0.2f);
			NPC.DeathSound = SoundID.NPCDeath51;
		}

		public override void ScaleExpertStats(int numPlayers, float bossLifeScale) => NPC.lifeMax = (int)(NPC.lifeMax * 0.66f * bossLifeScale);

		public override bool CanHitPlayer(Player target, ref int cooldownSlot) => false;

		private ref float AiTimer => ref NPC.ai[0];

		private ref float AttackType => ref NPC.ai[1];

		private float _attackGlowStrength = 0f;
		private Color _attackGlowColor = Color.Transparent;

		private delegate void Attack(Player player, NPC npc);

		private enum Attacks
		{
			MortarComets = 1,
			CirclingStars = 2,
			MeteorShower = 3
		}

		private static readonly IDictionary<int, Attack> AttackDict = new Dictionary<int, Attack> {
			{ (int)Attacks.CirclingStars, delegate(Player player, NPC npc) { CirclingStars(player, npc); } },
			{ (int)Attacks.MortarComets, delegate(Player player, NPC npc) { MortarComets(player, npc); } },
			{ (int)Attacks.MeteorShower, delegate(Player player, NPC npc) { MeteorShower(player, npc); } },
		};

		private List<int> Pattern = new List<int>
		{
			(int)Attacks.MortarComets,
			(int)Attacks.CirclingStars,
			(int)Attacks.MeteorShower
		};

		public override void SendExtraAI(BinaryWriter writer)
		{
			foreach (int i in Pattern)
				writer.Write(i);
		}

		public override void ReceiveExtraAI(BinaryReader reader) => Pattern = Pattern.Select(i => reader.ReadInt32()).ToList();

		public override void AI()
		{
			Player player = Main.player[NPC.target];
			//npc.TargetClosest(true);
			StarjinxGlobalNPC.TargetClosestSjinx(NPC, true);
			NPC.spriteDirection = NPC.direction;

			bool empowered = NPC.GetGlobalNPC<PathfinderGNPC>().Buffed;

			if (++AiTimer < IDLETIME)
			{
				_attackGlowStrength = MathHelper.Lerp(_attackGlowStrength, 0f, 0.1f);
				_attackGlowColor = Color.Lerp(_attackGlowColor, Color.Transparent, 0.1f);
				Vector2 homeCenter = player.Center;

				//Dont go to players outside of the sjinx range
				if (player.DistanceSQ(player.GetModPlayer<StarjinxPlayer>().StarjinxPosition) > StarjinxMeteorite.EVENT_RADIUS * StarjinxMeteorite.EVENT_RADIUS)
				{
					NPC.velocity *= 0.98f;
					StarjinxGlobalNPC.TargetClosestSjinx(NPC, false);
				}
				else
				{
					float extraSpeed = empowered ? 1.2f : 1f;
					NPC.rotation = NPC.velocity.X * .035f;

					switch (Pattern[(int)AttackType])
					{
						case (int)Attacks.MortarComets:
							homeCenter.Y -= 300;
							NPC.AccelFlyingMovement(homeCenter, new Vector2(0.2f, 0.15f), new Vector2(0.07f, 0.1f), new Vector2(5, 3));
							break;

						default:
							homeCenter.Y -= 50;
							NPC.AccelFlyingMovement(homeCenter, new Vector2(0.15f, 0.1f), new Vector2(0.07f, 0.1f), new Vector2(2, 1));
							break;
					}
				}
			}

			if (AiTimer == IDLETIME && Main.netMode != NetmodeID.Server)
				SoundEngine.PlaySound(new LegacySoundStyle(SoundID.Item, 123).WithPitchVariance(0.2f).WithVolume(1.3f), NPC.Center);

			if (AiTimer > IDLETIME)
			{
				AttackDict[Pattern[(int)AttackType]].Invoke(Main.player[NPC.target], NPC);

				NPC.rotation = 0f;
			}
			else
			{
				UpdateYFrame(10, 0, 4);
				frame.X = 0;
			}

			NPC.localAI[0] = Math.Max(NPC.localAI[0] - 0.05f, 0);
		}

		private void ResetPattern()
		{
			AttackType++;
			AiTimer = 0;
			if (AttackType >= Pattern.Count)
			{
				AttackType = 0;
				Pattern.Randomize();
			}
			NPC.netUpdate = true;
		}

		public override void ModifyHitByProjectile(Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection) => knockback = (AiTimer < IDLETIME) ? knockback : 0;

		public override void ModifyHitByItem(Player player, Item item, ref int damage, ref float knockback, ref bool crit) => knockback = (AiTimer < IDLETIME) ? knockback : 0;

		private void PlayCastSound(Vector2 position)
		{
			if (Main.netMode != NetmodeID.Server)
				SoundEngine.PlaySound(Mod.GetLegacySoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/starCast").WithPitchVariance(0.3f).WithVolume(0.7f), position);
		}

		private void MakeMagicCircle(Color color, Vector2 offset, float size, int fadeTime, int totalLifetime, float ySpeed, float rotationalVelocity)
		{
			float zRotation = 1.05f * MathHelper.PiOver4;
			ParticleHandler.SpawnParticle(new MeteorMagus_Circle(NPC, offset, color, size, totalLifetime, fadeTime,
				zRotation, MathHelper.PiOver2, rotationalVelocity, -Vector2.UnitY * ySpeed, delegate (Particle p)
				{
					if (p.TimeActive < fadeTime)
						p.Velocity.Y *= 0.96f;

					else
						p.Velocity.Y = MathHelper.Lerp(p.Velocity.Y, (float)Math.Sin(p.TimeActive / 20) / 8, 0.1f);
				}));
		}

		private static void MortarComets(Player player, NPC npc)
		{
			var modnpc = npc.ModNPC as MeteorMagus_NPC;
			npc.velocity = Vector2.Lerp(npc.velocity, Vector2.Zero, 0.1f);
			modnpc._attackGlowColor = Color.Lerp(modnpc._attackGlowColor, Color.Cyan, 0.1f);
			modnpc._attackGlowStrength = MathHelper.Lerp(modnpc._attackGlowStrength, 1f, 0.1f);

			int attackStartTime = IDLETIME + 1;
			int attackDelayTime = 60;
			int attackLength = 120;

			modnpc.UpdateYFrame(8, 0, 3);
			modnpc.frame.X = 1;

			if ((modnpc.AiTimer == attackStartTime || modnpc.AiTimer == (attackStartTime + (attackDelayTime/4))) && !Main.dedServ)
			{
				//Movement for the circles: moves upwards, slowing down, until the npc's attack starts, then moves in sine pattern up/down
				//Spawns a bigger slower circle, then a smaller faster one

				bool firstCircle = modnpc.AiTimer == attackStartTime;
				Vector2 offset = new Vector2(0, (firstCircle) ? -20 : -30);
				float size = firstCircle ? 170 : 110;
				int fadeTime = attackDelayTime - (int)(modnpc.AiTimer - attackStartTime);
				int totalLifetime = (2 * fadeTime) + attackLength;
				float ySpeed = (modnpc.AiTimer == attackStartTime) ? 2 : 3f;
				float rotationalVelocity = firstCircle ? -0.05f : 0.08f;

				modnpc.MakeMagicCircle(Color.Cyan, offset, size, fadeTime, totalLifetime, ySpeed, rotationalVelocity);
			}
				
			if (modnpc.AiTimer % 15 == 0 && modnpc.AiTimer > (attackStartTime + attackDelayTime) && modnpc.AiTimer <= (attackStartTime + attackDelayTime + attackLength))
			{
				modnpc.PlayCastSound(npc.Center);

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					var pos = npc.Center + new Vector2(Main.rand.Next(-40, 40), Main.rand.Next(-120, -90));
					Projectile.NewProjectileDirect(pos, Vector2.Zero, ModContent.ProjectileType<MeteorMagus_Comet>(), npc.damage / 4, 1, Main.myPlayer, npc.whoAmI, player.whoAmI);
				}
			}

			if (modnpc.AiTimer > (attackStartTime + (2 * attackDelayTime) + attackLength))
				modnpc.ResetPattern();
		}

		private static void CirclingStars(Player player, NPC npc)
		{
			npc.velocity = Vector2.Lerp(npc.velocity, Vector2.Zero, 0.1f);

			bool empowered = npc.GetGlobalNPC<PathfinderGNPC>().Buffed;

			var modnpc = npc.ModNPC as MeteorMagus_NPC;
			modnpc.UpdateYFrame(10, 0, 5);
			modnpc.frame.X = 2;
			modnpc._attackGlowColor = Color.Lerp(modnpc._attackGlowColor, Pink, 0.1f);
			modnpc._attackGlowStrength = MathHelper.Lerp(modnpc._attackGlowStrength, 1f, 0.1f);

			int attackStartTime = IDLETIME + 1;
			int attackDelayTime = 10;
			int patternResetTime = 80;

			if (modnpc.AiTimer == attackStartTime + attackDelayTime)
			{
				modnpc.PlayCastSound(player.Center);

				int numStars = 3 + (empowered ? 1 : 0);
				int direction = Main.rand.NextBool() ? -1 : 1;
				Vector2 offset = new Vector2(0, 250).RotatedByRandom(MathHelper.TwoPi);

				for (int i = 0; i < numStars; i++)
				{
					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						var proj = Projectile.NewProjectileDirect(player.Center, Vector2.Zero, ModContent.ProjectileType<MeteorMagus_Star>(), npc.damage / 4, 1, Main.myPlayer, npc.whoAmI, player.whoAmI);

						var star = proj.ModProjectile as MeteorMagus_Star;
						star.Direction = direction;
						star.CirclingTime = empowered ? 50f : 60f;
						star.Offset_Rotation = offset.ToRotation() + MathHelper.TwoPi * i / numStars;
						star.Offset_Distance = offset.Length();

						if (Main.netMode == NetmodeID.Server)
							NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, proj.whoAmI);
					}
				}
			}

			if (modnpc.AiTimer > attackStartTime + attackDelayTime + patternResetTime)
				modnpc.ResetPattern();
		}

		private static void MeteorShower(Player player, NPC npc)
		{
			var modnpc = npc.ModNPC as MeteorMagus_NPC;
			npc.velocity = Vector2.Lerp(npc.velocity, Vector2.Zero, 0.1f);
			modnpc._attackGlowColor = Color.Lerp(modnpc._attackGlowColor, Orange, 0.1f);
			modnpc._attackGlowStrength = MathHelper.Lerp(modnpc._attackGlowStrength, 1f, 0.1f);

			int attackStartTime = IDLETIME + 1;
			int attackDelayTime = 40;
			int patternResetTime = 180;

			modnpc.UpdateYFrame(8, 0, 3);
			modnpc.frame.X = 1;

			//Make 3 circles around the npc, one by one
			if(modnpc.AiTimer < attackStartTime + attackDelayTime)
			{
				if((modnpc.AiTimer - attackStartTime) % ((attackDelayTime / 3) + 1) == 0)
				{
					bool firstCircle = modnpc.AiTimer == attackStartTime;
					bool secondCircle = modnpc.AiTimer == attackStartTime + (attackDelayTime / 3) + 1;

					Vector2 offset;
					float size;
					int fadeTime = attackDelayTime - (int)(modnpc.AiTimer - attackStartTime);
					int totalLifetime = fadeTime + patternResetTime;
					float rotationalVelocity;
					float ySpeed;

					//adjust properties depending on when it was spawned
					if(firstCircle)
					{
						offset = Vector2.Zero;
						size = 240;
						rotationalVelocity = Main.rand.NextBool() ? 0.05f : -0.05f;
						ySpeed = 1.5f;
					}
					else if(secondCircle)
					{
						offset = -Vector2.UnitY * 25;
						size = 120;
						rotationalVelocity = -0.1f;
						ySpeed = 1.75f;
					}
					else
					{
						offset = Vector2.UnitY * 20;
						size = 190;
						rotationalVelocity = 0.1f;
						ySpeed = 1.75f;
					}

					offset.Y += 20; //adjust offset due to vertical velocity
					modnpc.MakeMagicCircle(Orange, offset, size, fadeTime, totalLifetime, ySpeed, rotationalVelocity);
				}
			}

			if(modnpc.AiTimer == attackStartTime + attackDelayTime)
			{
				modnpc.PlayCastSound(npc.Center);
				if(Main.netMode != NetmodeID.MultiplayerClient)
				{
					int numMeteors = 5;
					for(int i = 0; i < numMeteors; i++)
					{
						bool firstMeteor = i == 0;
						Vector2 position = npc.Center;
						position.Y -= 1500;
						position.X += Main.rand.NextFloat(-600, 600);
						position.Y = Math.Max(position.Y, Main.minScreenH);

						Vector2 velocity = player.DirectionFrom(position) * Main.rand.NextFloat(2.5f, 3);
						float delay = 1; //should be 0, however projectile always has timer ticked past the time to spawn in its trail if at 0, too tired to fix at the moment of writing this

						if(!firstMeteor) //first meteor always aimed at player
						{
							velocity = velocity.RotatedByRandom(MathHelper.Pi / 12);
							delay = Main.rand.Next(30, 60);
						}

						int p = Projectile.NewProjectile(position, velocity, ModContent.ProjectileType<MeteorMagus_Meteor>(), npc.damage / 4, 1f, Main.myPlayer, 0, delay);
						if (Main.netMode != NetmodeID.SinglePlayer)
							NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, p);
					}
				}
			}

			if (modnpc.AiTimer == attackStartTime + attackDelayTime + patternResetTime)
				modnpc.ResetPattern();
		}

		public override void SafeFindFrame(int frameHeight) => NPC.frame.Width = 168;

		public override void OnHitKill(int hitDirection, double damage)
		{
			for (int i = 0; i < 6; i++)
				Gore.NewGore(NPC.Center, NPC.velocity * .5f, 99, Main.rand.NextFloat(.75f, 1f));
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			if (NPC.frame.Width > 168)
			{
				frame = new Point(0, 0);
				NPC.FindFrame();
			}

			float num395 = Main.mouseTextColor / 300f - 0.25f;
			num395 *= 0.2f;
			float num366 = num395 + 1.05f;

			Color color = _attackGlowColor * _attackGlowStrength;
			color.A = 0;
			DrawAfterImage(Main.spriteBatch, new Vector2(0f, 0f), 0.5f, color * .7f, color * .1f, 0.45f, num366, .75f);

			spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, NPC.Center - Main.screenPosition, NPC.frame, drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, SpriteEffects.None, 0);
			return false;
		}

		public void DrawPathfinderOutline(SpriteBatch spriteBatch) => PathfinderOutlineDraw.DrawAfterImage(spriteBatch, NPC, NPC.frame, Vector2.Zero, NPC.frame.Size() / 2);

		private void DrawGlowmask(SpriteBatch spriteBatch, Texture2D tex, Vector2 position, Color color, float opacity = 1) => spriteBatch.Draw(tex, position - Main.screenPosition, NPC.frame, color * opacity, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, SpriteEffects.None, 0);

		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			Texture2D baseGlow = ModContent.Request<Texture2D>(Texture + "_glow");
			Texture2D whiteGlow = ModContent.Request<Texture2D>(Texture + "_glowWhite");

			DrawGlowmask(spriteBatch, baseGlow, NPC.Center, Color.White);

			if(AiTimer <= IDLETIME)
			{
				for (int i = 0; i < 4; i++)
				{
					Vector2 drawpos = NPC.Center + new Vector2(0, 4 * (((float)Math.Sin(Main.GlobalTimeWrappedHourly * 4) / 2) + 0.5f)).RotatedBy(i * MathHelper.PiOver2);
					DrawGlowmask(spriteBatch, baseGlow, drawpos, Color.White, 0.3f);
				}
			}

			DrawGlowmask(spriteBatch, whiteGlow, NPC.Center, _attackGlowColor, _attackGlowStrength);
		}

		public void AdditiveCall(SpriteBatch sB)
		{
			Texture2D whiteGlow = ModContent.Request<Texture2D>(Texture + "_glowWhite");

			Color additiveGlow = _attackGlowColor;

			PulseDraw.DrawPulseEffect(PulseDraw.BloomConstant, 6, 6, delegate (Vector2 posOffset, float opacityMod)
			{
				DrawGlowmask(sB, whiteGlow, posOffset + NPC.Center, additiveGlow, opacityMod * _attackGlowStrength * 0.33f);
			});
			DrawGlowmask(sB, whiteGlow, NPC.Center, additiveGlow, _attackGlowStrength);
		}

		public void DrawAfterImage(SpriteBatch spriteBatch, Vector2 offset, float trailLengthModifier, Color color, float opacity, float startScale, float endScale) => DrawAfterImage(spriteBatch, offset, trailLengthModifier, color, color, opacity, startScale, endScale);

		public void DrawAfterImage(SpriteBatch spriteBatch, Vector2 offset, float trailLengthModifier, Color startColor, Color endColor, float opacity, float startScale, float endScale)
		{
			SpriteEffects spriteEffects = (NPC.spriteDirection == 1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			for (int i = 1; i < 10; i++)
			{
				Color color = Color.Lerp(startColor, endColor, i / 10f) * opacity;
				spriteBatch.Draw(Mod.Assets.Request<Texture2D>("NPCs/StarjinxEvent/Enemies/MeteorMagus/MeteorMagus_NPC_Afterimage").Value, new Vector2(NPC.Center.X, NPC.Center.Y) + offset - Main.screenPosition + new Vector2(0, NPC.gfxOffY) - NPC.velocity * (float)i * trailLengthModifier, NPC.frame, color, NPC.rotation, NPC.frame.Size() * 0.5f, MathHelper.Lerp(startScale, endScale, i / 10f), spriteEffects, 0f);
			}
		}
	}
}