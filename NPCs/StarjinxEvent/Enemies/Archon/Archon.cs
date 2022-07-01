using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Mechanics.BackgroundSystem;
using SpiritMod.NPCs.StarjinxEvent.Enemies.Archon.Projectiles;
using SpiritMod.NPCs.StarjinxEvent.Enemies.Starachnid;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Particles;
using Terraria.Utilities;
using SpiritMod.Effects.Stargoop;

namespace SpiritMod.NPCs.StarjinxEvent.Enemies.Archon
{
	public class Archon : SpiritNPC, IDrawAdditive, IGalaxySprite
	{
		// CONSTS ----------

		// FG/BG Related
		public const int MaxForegroundTransitionTime = 1200;

		// Stage IDs
		public const int InitStage = 0;
		public const int EnchantAttackStage = 1;
		public const int VulnerableStage = 2;

		// Stage Lengths
		public const int EnchantMaxTime = 1200;

		// Fields
		internal int stage = InitStage;

		private ref Player Target => ref Main.player[NPC.target];

		// FG/BG Related
		internal bool inFG = true;
		private bool transitionFG = false;
		private int fgTime = 0;
		private Vector2 fgPos = new Vector2();
		private ArchonBG Background = null;

		// Enchantment
		internal Enchantment enchantment = Enchantment.None;

		// ATTACKS --------------------
		internal bool waitingOnAttack = false;
		private AttackType attack = AttackType.None;
		private float attackTimeMax = 0;

		// Basic Slash
		private Vector2 cachedAttackPos = new Vector2();
		private bool starlightDoubleSlash = true;

		// Starlight Constellation
		private readonly List<StarThread> threads = new List<StarThread>();
		private int currentThread = 0;

		// Meteor Dash
		private Vector2 meteorDashOffset = Vector2.Zero;
		private Vector2 meteorDashBegin = Vector2.Zero;
		private float meteorDashFactor = 0f;

		// Misc
		private readonly Dictionary<string, int> timers = new Dictionary<string, int>() { { "ENCHANT", 0 }, { "ATTACK", 0 }, { "CONSTELLATION", 0 }, { "DUO", 0 } };

		private Vector2 HeadPosition
		{
			get
			{
				GetRotation(out float rot, out _);
				return NPC.Center - new Vector2(10, 44).RotatedBy(rot);
			}
		}

		private Texture2D SwordExtra => enchantment == Enchantment.Void ? ModContent.Request<Texture2D>(Texture + "_Sword_Void") : ModContent.Request<Texture2D>(Texture + "_Sword");

		public void SetDuo(float d) => timers["DUO"] = (int)d;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Archon");
			Main.npcFrameCount[NPC.type] = 6;
		}

		public override bool IsLoadingEnabled(Mod mod) => false;

		public override void SetDefaults()
		{
			NPC.width = 190;
			NPC.height = 114;
			NPC.damage = 0;
			NPC.defense = 28;
			NPC.lifeMax = 12000;
			NPC.aiStyle = -1;
			NPC.HitSound = SoundID.DD2_CrystalCartImpact;
			NPC.DeathSound = SoundID.DD2_ExplosiveTrapExplode;
			NPC.value = Item.buyPrice(0, 0, 15, 0);
			NPC.knockBackResist = 0f;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.boss = true;
		}

		public override bool CheckActive() => !ModContent.GetInstance<StarjinxEventWorld>().StarjinxActive;
		public override bool CanHitPlayer(Player target, ref int cooldownSlot) => NPC.defDamage != 0;

		public override void AI()
		{
			if (Background == null) //init stuff
			{
				Background = new ArchonBG(NPC.Center, NPC);
				BackgroundItemManager.AddItem(Background);

				SpiritMod.Metaballs.EnemyLayer.Sprites.Add(this);
			}

			NPC.TargetClosest(true);
			NPC.dontTakeDamage = !inFG;
			NPC.defDamage = NPC.damage = 0;

			if (transitionFG)
			{
				Transition();
				return;
			}

			if (inFG)
				UpdateForeground();
			else
				UpdateBackground();
		}

		#region Background Stuff
		private void Transition()
		{
			NPC.velocity.Y -= 0.06f;

			if (NPC.Center.Y < -200 && fgTime > MaxForegroundTransitionTime / 2)
				fgTime = MaxForegroundTransitionTime / 2;

			if (fgTime == MaxForegroundTransitionTime / 2)
			{
				inFG = !inFG;
				NPC.position.Y = -150;
			}

			if (fgTime < MaxForegroundTransitionTime / 2)
			{
				NPC.Center = Vector2.Lerp(NPC.Center, fgPos, 0.125f);
				NPC.velocity *= 0f;
			}

			if (fgTime == 0)
				transitionFG = false;
		}

		private void ToggleForeground()
		{
			transitionFG = true;
			fgTime = MaxForegroundTransitionTime;
			fgPos = NPC.Center;
		}

		private void UpdateBackground()
		{
		}
		#endregion

		private void UpdateForeground()
		{
			switch (stage)
			{
				case EnchantAttackStage:
					EnchantBehaviour();
					break;
				default:
					break;
			}

			Visuals();
		}

		private void Visuals()
		{
			if (enchantment == Enchantment.Void)
			{
				if (Main.rand.NextBool(2))
				{
					float scl = Main.rand.NextFloat(1f, 3.5f);
					EnchantParticle(HeadPosition + Main.rand.NextVector2Circular(20, 20), NPC.DirectionTo(HeadPosition) * (3.5f - scl) * 2, scl);
				}
			}
		}

		internal void VoidDuoBehaviour(List<int> portals)
		{
			if (NPC.velocity.Length() < 0.25f)
				NPC.rotation *= 0.95f;
		}

		private void EnchantBehaviour()
		{
			timers["ENCHANT"]++;

			if (attack == AttackType.None)
				SetupRandomAttack();
			else if (enchantment != Enchantment.None)
				timers["ATTACK"]++;

			NPC.velocity.Y = (float)Math.Sin(timers["ENCHANT"] * 0.1f) * 0.2f;

			switch (enchantment)
			{
				case Enchantment.Starlight:
					StarlightBehaviour();
					break;
				case Enchantment.Meteor:
					MeteorBehaviour();
					break;
				case Enchantment.Void:
					VoidBehaviour();
					break;
				default:
					break;
			}

			if (timers["ENCHANT"] == EnchantMaxTime && !waitingOnAttack)
			{
				stage = VulnerableStage;
				timers["ENCHANT"] = 0;
			}
		}

		private void VoidBehaviour()
		{
			switch (attack)
			{
				case AttackType.TeleportSlash:
					SlashAttack();
					break;
				case AttackType.Cast:
					VoidCastAttack();
					break;
				default:
					break;
			}
		}

		private void VoidCastAttack()
		{
			const float BeginCastThreshold = 0.05f;
			const float BeginMeteorThreshold = 0.1f;
			const float SingleCastThreshold = 0.075f;

			waitingOnAttack = true;
			NPC.rotation = MathHelper.Lerp(NPC.rotation, 0f, 0.2f);
			NPC.velocity *= 0.95f;
			NPC.Center = Vector2.Lerp(NPC.Center, Target.GetModPlayer<StarjinxPlayer>().StarjinxPosition, 0.05f);

			if (timers["ATTACK"] == (int)(attackTimeMax * BeginCastThreshold)) //Begin singularity anim
			{

			}
			else if (timers["ATTACK"] > (int)(attackTimeMax * BeginMeteorThreshold) && timers["ATTACK"] < attackTimeMax) //Spawn meteors
			{
				waitingOnAttack = true;
				if (timers["ATTACK"] % (int)(attackTimeMax * SingleCastThreshold) == 0)
				{
					Vector2 sjinx = Target.GetModPlayer<StarjinxPlayer>().StarjinxPosition;
					Vector2 realPos = sjinx + new Vector2(StarjinxMeteorite.EVENT_RADIUS, 0).RotatedByRandom(MathHelper.TwoPi);

					int p = Projectile.NewProjectile(NPC.GetSource_FromAI(), realPos, NPC.DirectionFrom(realPos) * 8, ModContent.ProjectileType<MeteorEnchantment_Meteor>(), 40, 1f, Main.myPlayer);
					Main.projectile[p].timeLeft = 200;
				}
			}
			else if (timers["ATTACK"] >= attackTimeMax)
			{
				timers["ATTACK"] = 0;
				attack = AttackType.None;
				waitingOnAttack = false;
			}
		}

		private void MeteorBehaviour()
		{
			switch (attack)
			{
				case AttackType.TeleportSlash:
					SlashAttack();
					break;
				case AttackType.Cast:
					CastAttack();
					break;
				case AttackType.MeteorDash:
					MeteorDashAttack();
					break;
				default:
					break;
			}
		}

		private void MeteorDashAttack()
		{
			const float AnticipationThreshold = 0.15f;
			const float MeteorRainThreshold = 0.95f;

			if (timers["ATTACK"] < (int)(attackTimeMax * AnticipationThreshold)) //Anticipation & move to position
			{
				if (meteorDashOffset == Vector2.Zero)
					meteorDashOffset = new Vector2(0, -Main.rand.Next(300, 400)).RotatedByRandom(MathHelper.PiOver2);

				NPC.velocity = Vector2.Zero;
				NPC.Center = Vector2.Lerp(NPC.Center, Target.Center + meteorDashOffset, 0.15f);
			}
			else if (timers["ATTACK"] == (int)(attackTimeMax * AnticipationThreshold)) //Dash initialization
			{
				waitingOnAttack = true;
				cachedAttackPos = NPC.Center - ((NPC.Center - Target.Center) * 2.5f);
				meteorDashBegin = NPC.Center;
			}
			else if (timers["ATTACK"] >= attackTimeMax * AnticipationThreshold && timers["ATTACK"] < attackTimeMax * MeteorRainThreshold - 2) //Actual dash
			{
				meteorDashFactor = MathHelper.Lerp(meteorDashFactor, 1f, 0.15f);
				NPC.velocity = NPC.DirectionTo(cachedAttackPos) * 30 * meteorDashFactor;

				if (NPC.DistanceSQ(cachedAttackPos) > 32 * 32)
					NPC.defDamage = 70;
				else
					timers["ATTACK"] = (int)(attackTimeMax * MeteorRainThreshold) - 2;
			}
			else if (timers["ATTACK"] >= (int)(attackTimeMax * MeteorRainThreshold) && timers["ATTACK"] < attackTimeMax)
			{
				if (timers["ATTACK"] == (int)(attackTimeMax * MeteorRainThreshold)) //Spawn meteors
				{
					for (int i = 0; i < 4; ++i)
					{
						Vector2 vel = NPC.DirectionFrom(meteorDashBegin).RotatedByRandom(0.1f) * Main.rand.NextFloat(6f, 17f);
						Vector2 pos = meteorDashBegin - (vel * 30) - new Vector2(Main.rand.Next(-300, 300), 0);
						Projectile.NewProjectile(pos, vel, ModContent.ProjectileType<MeteorEnchantment_Meteor>(), 20, 1f);
					}
				}

				NPC.velocity *= 0.8f; //Slow down
			}
			else if (timers["ATTACK"] >= attackTimeMax)
			{
				timers["ATTACK"] = 0;
				attack = AttackType.None;
				waitingOnAttack = false;

				meteorDashOffset = Vector2.Zero;
				meteorDashBegin = Vector2.Zero;
				meteorDashFactor = 0;
			}
		}

		#region Starlight Attacks
		private void StarlightBehaviour()
		{
			switch (attack)
			{
				case AttackType.TeleportSlash:
					SlashAttack();
					break;
				case AttackType.Cast:
					CastAttack();
					break;
				case AttackType.StarlightConstellation:
					StarlightConstellationAttack();
					break;
				case AttackType.StarlightShootingStar:
					StarlightShootingStarAttack();
					break;
				default:
					break;
			}
		}

		private void StarlightShootingStarAttack()
		{
			const float CastWaitThreshold = 0.4f;

			NPC.velocity *= 0.95f;

			if (timers["ATTACK"] == (int)(attackTimeMax * CastWaitThreshold)) //Shoot bg star
			{
				Projectile.NewProjectile(Target.Center, Vector2.Zero, ModContent.ProjectileType<BGStarProjectile>(), 20, 1f);
			}
			else if (timers["ATTACK"] >= attackTimeMax)
			{
				timers["ATTACK"] = 0;
				attack = AttackType.None;
				waitingOnAttack = false;
			}
		}

		private void StarlightConstellationAttack()
		{
			const float MaxSetupThreadsThreshold = 0.5f;
			const float SetupThreadsThreshold = 0.125f;

			waitingOnAttack = true;

			if (threads.Count > 0)
			{
				foreach (StarThread thread in threads)
					thread.Update();

				foreach (StarThread thread in threads.ToArray())
					if (thread.Counter > thread.Duration)
						threads.Remove(thread);
			}

			if (timers["ATTACK"] <= (int)(attackTimeMax * MaxSetupThreadsThreshold))
			{
				NPC.velocity *= 0.95f;

				if (timers["ATTACK"] % (int)(attackTimeMax * SetupThreadsThreshold) == 0) //Setup threads
					AddThread();
			}
			else if (timers["ATTACK"] > attackTimeMax * MaxSetupThreadsThreshold && timers["ATTACK"] < attackTimeMax)
			{
				int maxTimePerThread = (int)(attackTimeMax * (1 - MaxSetupThreadsThreshold) / threads.Count);
				float progress = (timers["ATTACK"] % maxTimePerThread) / (float)maxTimePerThread;

				NPC.Center = Vector2.Lerp(threads[currentThread].StartPoint, threads[currentThread].EndPoint, progress);

				if (progress + (1f / maxTimePerThread) >= 0.99f)
					currentThread++;
			}
			else if (timers["ATTACK"] >= attackTimeMax)
			{
				threads.Clear();
				currentThread = 0;
				timers["ATTACK"] = 0;
				attack = AttackType.None;
				waitingOnAttack = false;
				NPC.rotation = 0f;
			}
		}

		private void AddThread()
		{
			Vector2 startPos = NPC.Center;
			if (threads.Count > 0)
				startPos = threads[threads.Count - 1].EndPoint;

			Vector2 endPoint = GetThreadEndPoint(threads.Count == 0);
			threads.Add(new StarThread(startPos, endPoint));
		}

		private Vector2 GetThreadEndPoint(bool start)
		{
			if (start)
				return NPC.Center - (NPC.DirectionFrom(Target.Center) * (NPC.Distance(Target.Center) + 420));
			else
			{
				int id = NPC.FindFirstNPC(ModContent.NPCType<StarjinxMeteorite>());
				Vector2 centre = Target.Center;

				if (id >= 0)
					centre = Target.GetModPlayer<StarjinxPlayer>().StarjinxPosition;

				float Size() => StarjinxMeteorite.EVENT_RADIUS * Main.rand.NextFloat(0.7f, 0.85f);

				Vector2 nextPos = centre + Main.rand.NextVector2CircularEdge(Size(), Size());

				while (Vector2.DistanceSquared(threads[threads.Count - 1].EndPoint, nextPos) < 300 * 300)
					nextPos = centre + Main.rand.NextVector2CircularEdge(Size(), Size());
				return nextPos;
			}
		}
		#endregion

		private void CastAttack()
		{
			float AnimationWaitThreshold = 0.8f;

			waitingOnAttack = true;
			NPC.rotation = MathHelper.Lerp(NPC.rotation, 0f, 0.2f);
			NPC.velocity *= 0.95f;

			if (timers["ATTACK"] == (int)(attackTimeMax * AnimationWaitThreshold)) //Wait for animation to finish then shoot
			{
				if (enchantment == Enchantment.Starlight)
				{
					Vector2 baseVel = NPC.DirectionTo(Target.Center) * 38;

					int reps = Main.rand.Next(10, 14);
					for (int i = 0; i < reps; ++i)
					{
						//Projectiles have lower velocity the further they deviate from the base velocity, forming a v-like shape
						const float maxAngularOffset = MathHelper.PiOver4;
						float angularOffset = Main.rand.NextFloat(-1, 1) * maxAngularOffset;
						Vector2 velocity = baseVel.RotatedBy(angularOffset) * (1 - Math.Abs(angularOffset / maxAngularOffset)) * Main.rand.NextFloat(0.75f, 1.1f);

						Projectile.NewProjectile(NPC.Center, velocity, ModContent.ProjectileType<ArchonStarFragment>(), 20, 1f);

						if (!Main.dedServ)
							for (int j = 0; j < 2; j++)
								ParticleHandler.SpawnParticle(new StarParticle(NPC.Center, velocity.RotatedByRandom(0.2f) * Main.rand.NextFloat(0.5f), Color.White, Color.Cyan, Main.rand.NextFloat(0.1f, 0.2f), 25));
					}

					NPC.velocity -= baseVel / 4; //Recoil effect
				}
				else if (enchantment == Enchantment.Meteor)
				{
					const float Speed = 15f;

					Vector2 spawnPos = new Vector2(NPC.Center.X, NPC.Center.Y > Target.Center.Y ? Target.Center.Y : NPC.Center.Y) - new Vector2(Main.rand.Next(-150, 150), 400);
					Vector2 velocity = Vector2.Normalize(Target.Center - spawnPos) * Speed;
					Vector2 destination = spawnPos + (velocity * (Vector2.Distance(Target.Center, spawnPos) / Speed));

					Projectile proj = Main.projectile[Projectile.NewProjectile(spawnPos, velocity, ModContent.ProjectileType<MeteorCastProjectile>(), 20, 1f)];

					if (proj.ModProjectile != null && proj.ModProjectile is MeteorCastProjectile)
					{
						proj.ai[0] = destination.X;
						proj.ai[1] = destination.Y;
					}
				}
			}
			else if (timers["ATTACK"] >= attackTimeMax)
			{
				timers["ATTACK"] = 0;
				attack = AttackType.None;
				waitingOnAttack = false;
			}
		}

		private void SlashAttack()
		{
			const float AnticipationThreshold = 0.1f;
			const float BeginAttackThreshold = 0.3f;

			if (timers["ATTACK"] < (int)(attackTimeMax * AnticipationThreshold))
				NPC.rotation = MathHelper.Lerp(NPC.rotation, 0f, 0.2f);
			else if (timers["ATTACK"] == (int)(attackTimeMax * AnticipationThreshold)) //Teleport & start slash anticipation
			{
				waitingOnAttack = true;

				for (int i = 0; i < 8; ++i)
					EnchantParticle(NPC.Center, new Vector2(0, Main.rand.NextFloat(4f, 7f)).RotatedByRandom(MathHelper.TwoPi), 2f);

				NPC.Center = Target.Center + new Vector2(0, Main.rand.Next(300, 400)).RotatedByRandom(MathHelper.TwoPi);
				if (enchantment == Enchantment.Void)
					NPC.Center = Target.Center + new Vector2(0, Main.rand.Next(300, 400)).RotatedBy(Main.rand.NextBool() ? 0 : MathHelper.Pi).RotatedByRandom(MathHelper.PiOver4);

				for (int i = 0; i < 8; ++i)
					EnchantParticle(NPC.Center, new Vector2(0, Main.rand.NextFloat(8f, 11f)).RotatedByRandom(MathHelper.TwoPi), 3f);

				cachedAttackPos = NPC.Center - (NPC.DirectionFrom(Target.Center) * NPC.Distance(Target.Center) * 2);
			}
			else if (timers["ATTACK"] > attackTimeMax * AnticipationThreshold && timers["ATTACK"] < attackTimeMax * BeginAttackThreshold)
			{
				float mult = (attackTimeMax * BeginAttackThreshold) - timers["ATTACK"];
				NPC.velocity = -NPC.DirectionTo(cachedAttackPos) * 0.5f * mult;
				NPC.rotation = MathHelper.Lerp(NPC.rotation, NPC.AngleTo(cachedAttackPos), 0.2f);
			}
			else if (timers["ATTACK"] >= (int)(attackTimeMax * BeginAttackThreshold))
			{
				float factor = enchantment == Enchantment.Void ? 0.05f : 0.075f;
				NPC.Center = Vector2.Lerp(NPC.Center, cachedAttackPos, factor);

				if (NPC.DistanceSQ(cachedAttackPos) > 10 * 10)
					NPC.defDamage = 70;
				else
					timers["ATTACK"] = (int)attackTimeMax;
			}

			if (timers["ATTACK"] >= attackTimeMax)
			{
				timers["ATTACK"] = 0;
				NPC.velocity = Vector2.Zero;

				if (!starlightDoubleSlash)
				{
					attack = AttackType.None;
					waitingOnAttack = false;
				}
				else
					starlightDoubleSlash = false;
			}
		}

		public override bool CheckDead()
		{
			SpiritMod.Metaballs.EnemyLayer.Sprites.Remove(this);
			return true;
		}

		private void EnchantParticle(Vector2 center, Vector2 velocity, float scale = 1f)
		{
			if (Main.dedServ)
				return;

			if (enchantment == Enchantment.Starlight)
				ParticleHandler.SpawnParticle(new StarParticle(center, velocity, Color.White, Color.Cyan, Main.rand.NextFloat(0.1f, 0.2f) * scale, 25));

			if (enchantment == Enchantment.Void)
				Dust.NewDustPerfect(center, ModContent.DustType<Dusts.EnemyStargoopDust>(), velocity * (3.5f - scale), Scale: scale);
		}

		public override void FindFrame(int frameHeight)
		{
			if (NPC.frameCounter++ > 6)
			{
				NPC.frameCounter = 0;
				NPC.frame.Y += frameHeight;

				if (NPC.frame.Y >= frameHeight * Main.npcFrameCount[NPC.type])
					NPC.frame.Y = 0;
			}
		}

		#region Drawing
		public void AdditiveCall(SpriteBatch sB)
		{
			if (!inFG) //if not in foreground stop drawing
				return;

			DrawThreads(sB);
		}

		public void DrawGalaxyMappedSprite(SpriteBatch sB)
		{
			if (enchantment == Enchantment.Void)
				DrawSword(true);
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			if (!inFG) //if not in foreground stop drawing
				return false;

			GetRotation(out float realRot, out SpriteEffects effect);

			Color col = Lighting.GetColor((int)(NPC.Center.X / 16f), (int)(NPC.Center.Y / 16f));
			spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, NPC.Center - Main.screenPosition, NPC.frame, StarjinxGlobalNPC.GetColorBrightness(col), realRot, NPC.frame.Size() / 2f, 1f, effect, 0f);

			if (enchantment != Enchantment.Void)
				DrawSword();
			return false;
		}

		public void DrawSword(bool gloop = false)
		{
			GetRotation(out float realRot, out SpriteEffects effect);

			if (gloop)
				Main.spriteBatch.Draw(SwordExtra, (NPC.Center - Main.screenPosition) / 2f, NPC.frame, Color.White, realRot, NPC.frame.Size() / 2f, 1 / 2f, effect, 0f);
			else
			{
				Color col = Lighting.GetColor((int)(NPC.Center.X / 16f), (int)(NPC.Center.Y / 16f));
				Main.spriteBatch.Draw(SwordExtra, NPC.Center - Main.screenPosition, NPC.frame, StarjinxGlobalNPC.GetColorBrightness(col), realRot, NPC.frame.Size() / 2f, 1f, effect, 0f);
			}
		}

		private void GetRotation(out float realRot, out SpriteEffects effect)
		{
			realRot = NPC.rotation;
			effect = SpriteEffects.None;

			if (NPC.rotation < -MathHelper.PiOver2 || NPC.rotation > MathHelper.PiOver2)
			{
				realRot -= MathHelper.Pi;
				effect = SpriteEffects.FlipHorizontally;
			}
		}

		internal const float THREADGROWLERP = 30; //How many frames does it take for threads to fade in/out?

		private void DrawThreads(SpriteBatch spriteBatch)
		{
			if (threads.Count == 0 || attack != AttackType.StarlightConstellation)
				return;

			Texture2D tex = SpiritMod.Instance.GetTexture("Textures/Trails/Trail_4");
			Texture2D star = SpiritMod.Instance.GetTexture("NPCs/StarjinxEvent/Enemies/Starachnid/SpiderStar");

			Vector2 threadScale = new Vector2(1 / (float)tex.Width, 30 / (float)tex.Height); //Base scale of the thread based on the texture's size, stretched horizontally depending on thread length
			for (int i = 0; i < threads.Count; ++i)
			{
				StarThread thread = threads[i];

				float length = Math.Min(thread.Counter / THREADGROWLERP, (thread.Duration - thread.Counter) / THREADGROWLERP);
				length = Math.Min(length, 1);
				spriteBatch.Draw(tex, thread.StartPoint - Main.screenPosition, null, Color.SteelBlue * length, (thread.EndPoint - thread.StartPoint).ToRotation(), new Vector2(0f, tex.Height / 2),
					threadScale * new Vector2(thread.Length, 1), SpriteEffects.None, 0f);

				spriteBatch.Draw(star, thread.StartPoint - Main.screenPosition, null, Color.White, 0f, star.TextureCenter(), 1f, default, 0f);
			}
		}
		#endregion

		public enum AttackType : int
		{
			None = 0,
			Reset = 1,
			TeleportSlash = 2,
			Cast = 3,
			StarlightConstellation = 4,
			StarlightShootingStar = 5,
			MeteorDash = 6,
			VoidDash = 7,
		}

		public void SetupRandomAttack()
		{
			var choices = new WeightedRandom<AttackType>();
			choices.Add(AttackType.TeleportSlash, 1f); //These two are always options
			choices.Add(AttackType.Cast, 1f);

			if (enchantment == Enchantment.Starlight)
			{
				choices.Add(AttackType.StarlightConstellation, 0.75f);
				choices.Add(AttackType.StarlightShootingStar, 1f);
			}
			else if (enchantment == Enchantment.Meteor)
				choices.Add(AttackType.MeteorDash, 1.5f);

			attack = choices;

			if (attack == AttackType.TeleportSlash)
			{
				attackTimeMax = 60; //Default slash time

				if (enchantment == Enchantment.Starlight)
					starlightDoubleSlash = true;
				if (enchantment == Enchantment.Meteor)
					attackTimeMax = 200; //Meteor slash is slower
				if (enchantment == Enchantment.Void)
					attackTimeMax = 200; //Void slash is slower
			}
			else if (attack == AttackType.Cast)
			{
				attackTimeMax = 80; //Starlight cast time
				if (enchantment == Enchantment.Meteor)
					attackTimeMax = 100; //Meteor cast
				else if (enchantment == Enchantment.Void)
					attackTimeMax = 200; //Void cast
			}
			else if (attack == AttackType.StarlightConstellation)
				attackTimeMax = 200;
			else if (attack == AttackType.StarlightShootingStar)
				attackTimeMax = Main.rand.Next(80, 90);
			else if (attack == AttackType.MeteorDash)
				attackTimeMax = 200;
		}

		public enum Enchantment : int
		{
			None = 0,
			Starlight = 1,
			Meteor = 2,
			Void = 3,
			Count = 4
		}

		public void SetRandomEnchantment()
		{
			var ench = (Enchantment)Main.LocalPlayer.HeldItem.stack;

			if (Main.LocalPlayer.HeldItem.stack <= 0)
				ench = Enchantment.Starlight;
			else if (Main.LocalPlayer.HeldItem.stack >= 4)
				ench = Enchantment.Void;

			enchantment = Enchantment.Starlight;// (Enchantment)(Main.rand.Next((int)Enchantment.Count - 1) + 1);
		}
	}
}
