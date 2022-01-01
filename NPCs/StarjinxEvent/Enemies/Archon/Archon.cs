using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Mechanics.BackgroundSystem;
using SpiritMod.NPCs.StarjinxEvent.Enemies.Starachnid;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.StarjinxEvent.Enemies.Archon
{
	public class Archon : SpiritNPC, IDrawAdditive
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

		private ref Player Target => ref Main.player[npc.target];

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
		private int realDamage = 0;
		private float attackTimeMax = 0;

		// Basic Slash
		private Vector2 cachedSlashPos = new Vector2();

		// Starlight Constellation
		private List<StarThread> threads = new List<StarThread>();
		private int currentThread = 0;
		private float currentThreadProgress = 0f;

		// Misc
		private readonly Dictionary<string, int> timers = new Dictionary<string, int>() { { "ENCHANT", 0 }, { "ATTACK", 0 } };

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Archon");
			Main.npcFrameCount[npc.type] = 1;
		}

		public override void SetDefaults()
		{
			npc.width = 136;
			npc.height = 128;
			npc.damage = 20;
			npc.defense = 28;
			npc.lifeMax = 1200;
			npc.aiStyle = -1;
			npc.HitSound = SoundID.DD2_CrystalCartImpact;
			npc.DeathSound = SoundID.DD2_ExplosiveTrapExplode;
			npc.value = Item.buyPrice(0, 0, 15, 0);
			npc.knockBackResist = 0f;
			npc.noGravity = true;
			npc.noTileCollide = true;
			npc.boss = true;
		}

		public override void AI()
		{
			if (Background == null) //init stuff
			{
				Background = new ArchonBG(npc.Center, npc);
				BackgroundItemManager.AddItem(Background);
			}
			//ArchonLiving = NPC.AnyNPCs(ModContent.NPCType<Archon>());

			npc.TargetClosest(true);
			npc.dontTakeDamage = !inFG;

			if (realDamage > 0)
				npc.damage = realDamage;

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
			npc.velocity.Y -= 0.06f;

			if (npc.Center.Y < -200 && fgTime > MaxForegroundTransitionTime / 2)
				fgTime = MaxForegroundTransitionTime / 2;

			if (fgTime == MaxForegroundTransitionTime / 2)
			{
				inFG = !inFG;
				npc.position.Y = -150;
			}

			if (fgTime < MaxForegroundTransitionTime / 2)
			{
				npc.Center = Vector2.Lerp(npc.Center, fgPos, 0.125f);
				npc.velocity *= 0f;
			}

			if (fgTime == 0)
				transitionFG = false;
		}

		private void ToggleForeground()
		{
			transitionFG = true;
			fgTime = MaxForegroundTransitionTime;
			fgPos = npc.Center;
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
		}

		private void EnchantBehaviour()
		{
			timers["ENCHANT"]++;

			switch (enchantment)
			{
				case Enchantment.Starlight:
					StarlightBehaviour();
					break;
				default:
					break;
			}

			npc.velocity.Y = (float)Math.Sin(timers["ENCHANT"] * 0.1f) * 0.2f;

			if (timers["ENCHANT"] == EnchantMaxTime && !waitingOnAttack)
			{
				stage = VulnerableStage;
				timers["ENCHANT"] = 0;
			}
		}

		private void StarlightBehaviour()
		{
			if (attack == AttackType.None)
				SetupRandomAttack();

			timers["ATTACK"]++;

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

			if (timers["ATTACK"] == (int)(attackTimeMax * CastWaitThreshold)) //Setup threads
			{
				//Projectile.NewProjectile();
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
			const float MaxSetupThreadsThreshold = 0.25f;
			const float SetupThreadsThreshold = 0.05f;

			waitingOnAttack = true;

			foreach (StarThread thread in threads)
				thread.Update();

			foreach (StarThread thread in threads.ToArray())
				if (thread.Counter > thread.Duration)
					threads.Remove(thread);

			if (timers["ATTACK"] % (int)(attackTimeMax * SetupThreadsThreshold) == 0 && timers["ATTACK"] <= (int)(attackTimeMax * MaxSetupThreadsThreshold)) //Setup threads
			{
				Vector2 startPos = npc.Center;
				if (threads.Count > 0)
					startPos = threads[threads.Count - 1].EndPoint;

				Vector2 endPoint = GetThreadEndPoint(threads.Count == 0);
				threads.Add(new StarThread(startPos, endPoint));
			}
			else if (timers["ATTACK"] > attackTimeMax * MaxSetupThreadsThreshold && timers["ATTACK"] <= attackTimeMax)
			{
				realDamage = 20;

				npc.Center = Vector2.Lerp(npc.Center, threads[currentThread].EndPoint, currentThreadProgress);
				currentThreadProgress += 1 / (attackTimeMax * MaxSetupThreadsThreshold / 3f);

				float finalRot = (threads[currentThread].EndPoint - threads[currentThread].StartPoint).ToRotation();
				if (currentThread == threads.Count - 1)
					finalRot = 0;
				npc.rotation = MathHelper.Lerp(npc.rotation, finalRot, 0.15f);
				
				if (currentThreadProgress > 1f)
				{
					currentThread++;
					currentThreadProgress = 0;

					if (currentThread >= threads.Count)
					{
						timers["ATTACK"] = (int)attackTimeMax;
						currentThread = 0;
					}
				}
			}
			else if (timers["ATTACK"] >= attackTimeMax)
			{
				threads.Clear();
				timers["ATTACK"] = 0;
				attack = AttackType.None;
				waitingOnAttack = false;
				npc.rotation = 0f;

				realDamage = 0;
			}
		}

		private Vector2 GetThreadEndPoint(bool start)
		{
			if (start)
				return npc.Center - (npc.DirectionFrom(Target.Center) * 600);
			else
			{
				Vector2 nextPos = threads[threads.Count - 1].EndPoint;
				Vector2 toPlayer = Vector2.Normalize(Target.Center - nextPos) * Main.rand.Next(300, 450);
				Vector2 offset = toPlayer.RotatedByRandom(MathHelper.PiOver4);
				return nextPos + offset;
			}
		}

		private void CastAttack()
		{
			float AnimationWaitThreshold = 0.8f;

			waitingOnAttack = true;
			npc.rotation = MathHelper.Lerp(npc.rotation, 0f, 0.2f);
			npc.velocity *= 0.95f;

			if (timers["ATTACK"] == (int)(attackTimeMax * AnimationWaitThreshold)) //Wait for animation to finish then shoot
			{
				if (enchantment == Enchantment.Starlight)
				{
					Vector2 baseVel = npc.DirectionTo(Target.Center) * 38;

					int reps = Main.rand.Next(5, 8);
					for (int i = 0; i < reps; ++i)
					{
						int p = Projectile.NewProjectile(npc.Center, baseVel.RotatedByRandom(0.8f) * Main.rand.NextFloat(0.75f, 1.1f), 
							ModContent.ProjectileType<Items.Sets.GunsMisc.HeavenFleet.HeavenfleetStar>(), 20, 1f);
						Main.projectile[p].hostile = true;
						Main.projectile[p].friendly = false;
					}
				}
			}
			else if (timers["ATTACK"] >= attackTimeMax)
			{
				timers["ATTACK"] = 0;
				attack = AttackType.None;
				waitingOnAttack = false;

				realDamage = 0;
			}
		}

		private void SlashAttack()
		{
			const float AnticipationThreshold = 0.1f;
			const float BeginAttackThreshold = 0.3f;

			if (timers["ATTACK"] == (int)(attackTimeMax * AnticipationThreshold)) //Teleport & start slash anticipation
			{
				waitingOnAttack = true;
				npc.Center = Target.Center + new Vector2(0, Main.rand.Next(300, 400)).RotatedByRandom(MathHelper.Pi);
				cachedSlashPos = npc.Center - (npc.DirectionFrom(Target.Center) * npc.Distance(Target.Center) * 2);
			}
			else if (timers["ATTACK"] > attackTimeMax * AnticipationThreshold && timers["ATTACK"] < attackTimeMax * BeginAttackThreshold)
			{
				float mult = (attackTimeMax * BeginAttackThreshold) - timers["ATTACK"];
				npc.velocity = -npc.DirectionTo(cachedSlashPos) * 0.5f * mult;
				npc.rotation = MathHelper.Lerp(npc.rotation, npc.AngleTo(cachedSlashPos), 0.2f);
			}
			else if (timers["ATTACK"] >= (int)(attackTimeMax * BeginAttackThreshold))
			{
				npc.Center = Vector2.Lerp(npc.Center, cachedSlashPos, 0.075f);

				if (npc.DistanceSQ(cachedSlashPos) > 10 * 10)
					realDamage = 20;
				else
					realDamage = 0;
			}

			if (timers["ATTACK"] >= attackTimeMax)
			{
				timers["ATTACK"] = 0;
				attack = AttackType.None;
				waitingOnAttack = false;

				npc.velocity = Vector2.Zero;
				realDamage = 0;
			}
		}

		public override bool CanHitPlayer(Player target, ref int cooldownSlot) => realDamage > 0;

		#region Drawing
		public void AdditiveCall(SpriteBatch sB)
		{
			if (!inFG) //if not in foreground stop drawing
				return;

			DrawThreads(sB);
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			if (!inFG) //if not in foreground stop drawing
				return false;

			float realRot = npc.rotation;
			SpriteEffects effect = SpriteEffects.None;

			if (npc.rotation < -MathHelper.PiOver2 || npc.rotation > MathHelper.PiOver2)
			{
				realRot -= MathHelper.Pi;
				effect = SpriteEffects.FlipHorizontally;
			}

			Color col = Lighting.GetColor((int)(npc.Center.X / 16f), (int)(npc.Center.Y / 16f));
			spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition, npc.frame, col, realRot, npc.frame.Size() / 2f, 1f, effect, 0f);
			return false;
		}

		internal const float THREADGROWLERP = 30; //How many frames does it take for threads to fade in/out?

		private void DrawThreads(SpriteBatch spriteBatch)
		{
			if (threads.Count == 0 || attack != AttackType.StarlightConstellation)
				return;

			Texture2D tex = SpiritMod.Instance.GetTexture("Textures/Trails/Trail_4");
			Vector2 threadScale = new Vector2(1 / (float)tex.Width, 30 / (float)tex.Height); //Base scale of the thread based on the texture's size, stretched horizontally depending on thread length
			foreach (StarThread thread in threads)
			{
				float length = Math.Min(thread.Counter / THREADGROWLERP, (thread.Duration - thread.Counter) / THREADGROWLERP);
				length = Math.Min(length, 1);
				spriteBatch.Draw(tex, thread.StartPoint - Main.screenPosition, null, Color.SteelBlue * length, (thread.EndPoint - thread.StartPoint).ToRotation(), new Vector2(0f, tex.Height / 2),
					threadScale * new Vector2(thread.Length, 1), SpriteEffects.None, 0f);
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
		}

		public void SetupRandomAttack()
		{
			var choices = new List<AttackType>() { AttackType.TeleportSlash, AttackType.Cast }; //These two are always options

			if (enchantment == Enchantment.Starlight)
			{
				choices.Add(AttackType.StarlightConstellation);
				choices.Add(AttackType.StarlightShootingStar);
			}

			attack = AttackType.StarlightShootingStar; //Main.rand.Next(choices);

			if (attack == AttackType.TeleportSlash)
			{
				attackTimeMax = 160; //Default slash time
				if (enchantment == Enchantment.Meteor)
					attackTimeMax = 200; //Meteor slash is slower
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
				attackTimeMax = 400;
			else if (attack == AttackType.StarlightShootingStar)
				attackTimeMax = Main.rand.Next(80, 90);
		}

		public enum Enchantment : int
		{
			None = 0,
			Starlight = 1,
			Meteor = 2,
			Void = 3,
			Count = 4
		}

		public void SetRandomEnchantment() => enchantment = Enchantment.Starlight;// (Enchantment)(Main.rand.Next((int)Enchantment.Count - 1) + 1);

		internal void ResetEnchantment()
		{
			enchantment = Enchantment.None;
		}
	}
}
