using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Mechanics.BackgroundSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.StarjinxEvent.Enemies.Warden
{
	[AutoloadBossHead]
	public class Warden : SpiritNPC, IDrawAdditive
	{
		public const int MaxForegroundTransitionTime = 1200;

		public const int InitStage = 0;
		public const int EnchantStage = 1;
		public const int ArchonAttackStage = 2;
		public const int ArchonDeadStage = 3;
		public const int DuoAttackStage = 4;

		public const int EnchantMaxTime = 200;
		public const int ArchonAttackMaxTime = 1200;

		private int stage = InitStage;

		private ref Player Target => ref Main.player[NPC.target];

		// Foreground/Background stuff
		internal bool inFG = true;
		private bool transitionFG = false;
		private int fgTime = 0;
		private Vector2 fgPos = new Vector2();
		private WardenBG Background = null;

		// Starlight doppelganger attack
		private readonly List<int> doppelgangers = new List<int>();

		// Void black hole attack
		private int blackHoleWhoAmI = -1;
		private ref Projectile BlackHole => ref Main.projectile[blackHoleWhoAmI];

		// Starlight duo attack
		private readonly List<int> massiveStarWhoAmIs = new List<int>();

		// Meteorite duo attack
		private int meteorWhoAmI = -1;
		private Projectile PongMeteor
		{
			get
			{
				if (meteorWhoAmI != -1)
					return Main.projectile[meteorWhoAmI];
				return new Projectile();
			}
		}

		private Vector2 meteorPongPosition = Vector2.Zero;
		private Vector2 archonMeteorPongPosition = Vector2.Zero;
		private int pongTimer = 0; //So they don't hit several times in succession

		// Void duo attack
		private readonly List<int> voidPortals = new List<int>();

		internal int archonWhoAmI = -1;
		private ref NPC ArchonNPC => ref Main.npc[archonWhoAmI];
		private Archon.Archon GetArchon => ArchonNPC.ModNPC as Archon.Archon;

		private readonly Dictionary<string, float> timers = new Dictionary<string, float>() { { "ENCHANT", 0 }, { "ARCHATK", 0 }, { "ATTACK", 0 }, { "DUO", 0 } };

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Warden");
			Main.npcFrameCount[NPC.type] = 1;
		}

		public override bool IsLoadingEnabled(Mod mod) => false;

		public override void SetDefaults()
		{
			NPC.width = 136;
			NPC.height = 128;
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

		public override void AI()
		{
			if (Background == null) //init stuff
			{
				Background = new WardenBG(NPC.Center, NPC);
				BackgroundItemManager.AddItem(Background);

				archonWhoAmI = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y + 300, ModContent.NPCType<Archon.Archon>());

				if (Main.netMode != NetmodeID.SinglePlayer)
					NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, archonWhoAmI);

				stage = EnchantStage;
			}

			NPC.TargetClosest(true);
			NPC.dontTakeDamage = !inFG;

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
				case EnchantStage:
					EnchantBehaviour();
					break;
				case ArchonAttackStage:
					ArchonAttackBehaviour();
					break;
				case ArchonDeadStage:
					SoloBehaviour();
					break;
				case DuoAttackStage:
					DuoBehaviour();
					break;
				default:
					break;
			}

			FadeAtEdges();
		}

		private void FadeAtEdges()
		{
			NPC.alpha = 0;

			float dist = NPC.DistanceSQ(Target.GetModPlayer<StarjinxPlayer>().StarjinxPosition);
			if (dist > StarjinxMeteorite.EVENT_RADIUS * StarjinxMeteorite.EVENT_RADIUS)
			{
				dist -= StarjinxMeteorite.EVENT_RADIUS * StarjinxMeteorite.EVENT_RADIUS;
				NPC.alpha = dist > 50 ? 255 : (int)(dist / 50f) * 255;
			}
		}

		public const int StarlightDuoMaxTime = 750;
		public const int MeteorDuoMaxTime = 1000;
		public const int VoidDuoMaxTime = 900;

		public int duoMaxTime = 0;

		private void DuoBehaviour()
		{
			timers["DUO"]++;
			GetArchon.SetDuo(timers["DUO"]);

			switch (GetArchon.enchantment)
			{
				case Archon.Archon.Enchantment.Starlight:
					StarlightDuoAttack();
					break;
				case Archon.Archon.Enchantment.Meteor:
					MeteorDuoAttack();
					break;
				case Archon.Archon.Enchantment.Void:
					VoidDuoAttack();
					break;
				default:
					BasicIdleMovement();
					break;
			}

			if (timers["DUO"] >= duoMaxTime)
			{
				timers["DUO"] = 0;

				ClearAttackInfo();
				GetArchon.enchantment = Archon.Archon.Enchantment.None;
				GetArchon.SetDuo(0);
				stage = EnchantStage;
			}
		}

		private void ClearAttackInfo()
		{
			//Portal
			voidPortals.Clear();

			//Pong Meteor
			if (meteorWhoAmI != -1)
				PongMeteor.Kill();
			meteorWhoAmI = -1;
		}

		private void VoidDuoAttack()
		{
			const float SpawnPortalsThreshold = 0.05f;

			duoMaxTime = VoidDuoMaxTime;

			if (timers["DUO"] == (int)(duoMaxTime * SpawnPortalsThreshold)) //Initialize
				SpawnPortals();
			else if (timers["DUO"] > (int)(duoMaxTime * SpawnPortalsThreshold))
			{
				BasicIdleMovement(1f, false);

				if ((timers["DUO"] + 1) % (int)(duoMaxTime * SpawnPortalsThreshold) == 0)
					FireVoidProjectile();

				GetArchon.VoidDuoBehaviour(voidPortals);
			}
		}

		private void FireVoidProjectile()
		{
			Vector2 portal = Main.projectile[GetVoidPortalHitLine()].Center;

			Vector2 vel = NPC.DirectionTo(portal) * 7;
			Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, vel, ModContent.ProjectileType<Projectiles.VoidProjectile>(), 20, 1f);
		}

		private int GetVoidPortalHitLine()
		{
			List<int> portals = new List<int>(voidPortals);
			while (true)
			{
				int cPort = Main.rand.Next(portals);
				return cPort;
			}
			//return -1;
		}

		private void SpawnPortals()
		{
			const int MinDist = 500;
			const int PortalDist = 300 * 300;

			Vector2 center = Target.GetModPlayer<StarjinxPlayer>().StarjinxPosition;

			for (int i = 0; i < 6; ++i)
			{
				Vector2 pos = Main.rand.NextVector2Circular(StarjinxMeteorite.EVENT_RADIUS * 0.9f, StarjinxMeteorite.EVENT_RADIUS * 0.9f);

				while ((voidPortals.Count > 0 && voidPortals.Any(x => Vector2.DistanceSquared(Main.projectile[x].Center, center + pos) < PortalDist)) || pos.Length() < MinDist)
					pos = Main.rand.NextVector2Circular(StarjinxMeteorite.EVENT_RADIUS * 0.9f, StarjinxMeteorite.EVENT_RADIUS * 0.9f);

				int p = Projectile.NewProjectile(NPC.GetSource_FromAI(), center + pos, Vector2.Zero, ModContent.ProjectileType<Projectiles.VoidPortal>(), 0, 0);
				Main.projectile[p].timeLeft = VoidDuoMaxTime - (int)timers["DUO"];

				voidPortals.Add(p);
			}

			var unmatchedPortals = new List<int>(voidPortals);

			Projectiles.VoidPortal GetPortal(int index) => (Main.projectile[index].ModProjectile != null && Main.projectile[index].ModProjectile is Projectiles.VoidPortal portal) ? portal : null;

			//"Connects" every portal
			while (unmatchedPortals.Count > 0)
			{
				int start = Main.rand.Next(unmatchedPortals);
				int end;

				do
				{
					end = Main.rand.Next(unmatchedPortals);
				} while (end == start);

				unmatchedPortals.Remove(start);
				unmatchedPortals.Remove(end);

				GetPortal(start).connectedWhoAmI = end;
				GetPortal(end).connectedWhoAmI = start;
			}
		}

		private void MeteorDuoAttack()
		{
			const float MeteorPongSpeed = 12f;
			const float MeteorPongStartThreshold = 0.05f;

			duoMaxTime = MeteorDuoMaxTime;

			ArchonNPC.velocity *= 0.93f;
			ArchonNPC.rotation *= 0.93f;
			NPC.velocity *= 0.93f;

			if (timers["DUO"] == 1) //Initialize
			{
				float rot = Main.rand.NextFloat(-MathHelper.Pi, MathHelper.Pi);
				meteorPongPosition = Target.GetModPlayer<StarjinxPlayer>().StarjinxPosition + new Vector2(0, StarjinxMeteorite.EVENT_RADIUS - 120).RotatedBy(rot);
			}
			else if (timers["DUO"] < (int)(duoMaxTime * MeteorPongStartThreshold)) //Set first pong position
				NPC.Center = Vector2.Lerp(NPC.Center, meteorPongPosition, timers["DUO"] / (duoMaxTime * 0.05f));
			else if (timers["DUO"] == (int)(duoMaxTime * MeteorPongStartThreshold)) //Spawn pong meteor and set Archon pong position
			{
				CalculateBouncePosition(true);

				int type = ModContent.ProjectileType<Archon.Projectiles.MeteorCastProjectile>();
				int p = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.DirectionTo(archonMeteorPongPosition) * MeteorPongSpeed, type, 20, 1f, Main.myPlayer);
				Main.projectile[p].timeLeft = MeteorDuoMaxTime;
				Main.projectile[p].scale = 10f;

				meteorWhoAmI = p;
				pongTimer = 10;
			}
			else if (timers["DUO"] > duoMaxTime * MeteorPongStartThreshold)
			{
				if (!PongMeteor.active || meteorWhoAmI == -1)
					return;

				if (timers["DUO"] >= duoMaxTime * 0.95f)
					PongMeteor.velocity *= 0.95f;
				else if (timers["DUO"] >= duoMaxTime)
					return;

				NPC.Center = Vector2.Lerp(NPC.Center, meteorPongPosition, 0.05f);
				ArchonNPC.Center = Vector2.Lerp(ArchonNPC.Center, archonMeteorPongPosition, 0.05f);

				pongTimer--;

				if (pongTimer >= 0)
					return;

				const int PongDistance = 140 * 140;

				if (NPC.DistanceSQ(PongMeteor.Center) < PongDistance)
				{
					CalculateBouncePosition(true);
					PongMeteor.velocity = NPC.DirectionTo(archonMeteorPongPosition) * MeteorPongSpeed;
					pongTimer = 10;
				}

				if (ArchonNPC.DistanceSQ(PongMeteor.Center) < PongDistance)
				{
					CalculateBouncePosition(false);
					PongMeteor.velocity = ArchonNPC.DirectionTo(meteorPongPosition) * MeteorPongSpeed;
					pongTimer = 10;
				}
			}
		}

		/// <summary>Gets the next pong position of a boss.</summary>
		void CalculateBouncePosition(bool setArchon)
		{
			Vector2 center = setArchon ? NPC.Center : ArchonNPC.Center;
			float dir = (center - Target.GetModPlayer<StarjinxPlayer>().StarjinxPosition).ToRotation();
			float newDir = dir + (dir < 0 ? MathHelper.Pi : -MathHelper.Pi);
			newDir += Main.rand.NextFloat(-MathHelper.PiOver4 / 2f, MathHelper.PiOver4 / 2f);

			Vector2 newPos = Target.GetModPlayer<StarjinxPlayer>().StarjinxPosition + new Vector2(StarjinxMeteorite.EVENT_RADIUS - 200, 0).RotatedBy(newDir);

			if (setArchon)
				archonMeteorPongPosition = newPos;
			else
				meteorPongPosition = newPos;
		}

		private void StarlightDuoAttack()
		{
			duoMaxTime = StarlightDuoMaxTime;

			if (timers["DUO"] >= duoMaxTime * 0.99f) //Stop attacking if I'm done
			{
				massiveStarWhoAmIs.Clear();
				return;
			}

			if (timers["DUO"] == (int)(duoMaxTime * 0.05f))
			{
				int p = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.SplitStar>(), 20, 1f);
				Main.projectile[p].timeLeft = StarlightDuoMaxTime;
				Main.projectile[p].scale = 20f;

				massiveStarWhoAmIs.Add(p);
			}
			else if (timers["DUO"] % (int)(duoMaxTime * 0.1f) == 0)
			{
				Projectile proj = Main.projectile[Main.rand.Next(massiveStarWhoAmIs)];

				if (proj.active && proj.ModProjectile is Projectiles.SplitStar star)
				{
					int newProj = star.Split();

					if (newProj != -1)
						massiveStarWhoAmIs.Add(newProj);
				}
			}
		}

		private void SoloBehaviour() => BasicIdleMovement(2f);

		private void ArchonAttackBehaviour()
		{
			timers["ARCHATK"]++;

			if (timers["ARCHATK"] >= ArchonAttackMaxTime && ArchonNPC.active && ArchonNPC.life > 0 && !GetArchon.waitingOnAttack)
			{
				stage = DuoAttackStage;
				timers["ARCHATK"] = 0;

				GetArchon.stage = -1;
			}
			else if (!ArchonNPC.active || ArchonNPC.life <= 0)
			{
				stage = ArchonDeadStage;
				return;
			}

			switch (GetArchon.enchantment)
			{
				case Archon.Archon.Enchantment.Starlight:
					StarlightAttack();
					break;
				case Archon.Archon.Enchantment.Void:
					VoidAttack();
					break;
				default: 
					BasicIdleMovement();
					break;
			}
		}

		private void VoidAttack()
		{
			BasicIdleMovement(blackHoleWhoAmI > -1 ? 0.8f : 0.2f);

			if (timers["ARCHATK"] == 50) //Spawn projectile
			{
				int p = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.VoidCastProjectile>(), 60, 1f, Main.myPlayer);
				Main.projectile[p].timeLeft = ArchonAttackMaxTime - 50;

				blackHoleWhoAmI = p;
			}
			else if (timers["ARCHATK"] > 50 && timers["ARCHATK"] < ArchonAttackMaxTime - 100 && blackHoleWhoAmI > -1) //Control void projectile & break if hit
			{
				if (!BlackHole.active)
					return;

				BlackHole.velocity = BlackHole.DirectionTo(Target.Center) * 4;

				for (int i = 0; i < Main.maxProjectiles; ++i)
				{
					Projectile proj = Main.projectile[i];

					if (i != blackHoleWhoAmI && proj.active && !proj.hostile && proj.friendly && proj.DistanceSQ(BlackHole.Center) < 40 * 40) //Detect all friendly projectiles
						proj.Kill();
				}
			}
		}

		private void StarlightAttack()
		{
			NPC.velocity *= 0.94f;

			if (timers["ARCHATK"] == 50) //Spawn NPCs
			{
				int npcCount = Main.rand.Next(3, 6);
				int[] types = new int[] { ModContent.NPCType<Pathfinder.Pathfinder>(), ModContent.NPCType<Starachnid.Starachnid>(), ModContent.NPCType<StarWeaver.StarWeaverNPC>() };

				for (int i = 0; i < npcCount; ++i)
				{
					int id = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y - 200, Main.rand.Next(types), Main.rand.Next(10) * 10);

					Main.npc[id].hide = true;
					doppelgangers.Add(id);
				}
			}
			else if (timers["ARCHATK"] > 50 && timers["ARCHATK"] < ArchonAttackMaxTime - 50) //Chase the center of doppelgangers
			{
				Vector2 doppelLoc = Vector2.Zero;

				foreach (int item in doppelgangers)
				{
					NPC npc = Main.npc[item];
					if (npc.active && npc.life > -1)
						doppelLoc += npc.Center;
				}

				if (doppelLoc != Vector2.Zero)
					NPC.Center = Vector2.Lerp(NPC.Center, doppelLoc / doppelgangers.Count, 0.05f);
				else
					NPC.Center = Vector2.Lerp(NPC.Center, Target.GetModPlayer<StarjinxPlayer>().StarjinxPosition, 0.05f);
			}
			else if (timers["ARCHATK"] == ArchonAttackMaxTime - 50) //Blow up NPCs
			{
				foreach (int item in doppelgangers)
				{
					NPC npc = Main.npc[item];
					if (npc.active && npc.life > -1)
					{
						npc.active = false;

						int p = Projectile.NewProjectile(NPC.GetSource_FromAI(), npc.Center, Vector2.Zero, ProjectileID.Dynamite, 120, 1f);
						Main.projectile[p].timeLeft = 3;
					}
				}

				doppelgangers.Clear();
			}
		}

		private void BasicIdleMovement(float speedMult = 1f, bool run = true)
		{
			float magnitude = 10 - (NPC.Distance(Target.Center) * 0.02f);
			Vector2 vel = -NPC.DirectionTo(Target.Center) * magnitude * speedMult;

			if (magnitude < 0 || !run)
			{
				magnitude = (NPC.Distance(Target.GetModPlayer<StarjinxPlayer>().StarjinxPosition) * 0.02f) - 3;
				if (magnitude < 0)
					magnitude = 0;

				vel = NPC.DirectionTo(Target.GetModPlayer<StarjinxPlayer>().StarjinxPosition) * magnitude;
			}

			NPC.velocity = vel;

			WrapLocation();
		}

		private void WrapLocation()
		{
			Vector2 sjinxLoc = Target.GetModPlayer<StarjinxPlayer>().StarjinxPosition;
			if (NPC.DistanceSQ(sjinxLoc) > StarjinxMeteorite.EVENT_RADIUS * StarjinxMeteorite.EVENT_RADIUS * 1.05f)
			{
				Vector2 offset = NPC.Center - sjinxLoc;
				NPC.Center = sjinxLoc - (offset * 0.95f);
			}
		}

		private void EnchantBehaviour()
		{
			timers["ENCHANT"]++;

			NPC.velocity *= 0.94f;

			if (timers["ENCHANT"] == EnchantMaxTime / 2)
			{
				GetArchon.SetRandomEnchantment();
				CombatText.NewText(NPC.getRect(), Color.Gold, $"Enchant moment - we got {GetArchon.enchantment}");
			}
			else if (timers["ENCHANT"] >= EnchantMaxTime)
			{
				GetArchon.stage = Archon.Archon.EnchantAttackStage;
				stage = ArchonAttackStage;
				timers["ENCHANT"] = 0;
			}
		}

		public override void OnHitByItem(Player player, Item item, int damage, float knockback, bool crit)
		{
			if (GetArchon.enchantment == Archon.Archon.Enchantment.Void && item.IsMelee() && blackHoleWhoAmI != -1)
			{
				BlackHole.Kill();
				blackHoleWhoAmI = -1;
			}
		}

		#region Drawing
		public override Color? GetAlpha(Color drawColor) => StarjinxGlobalNPC.GetColorBrightness(drawColor);

		public void AdditiveCall(SpriteBatch sB, Vector2 screenPos)
		{
			if (!inFG) //if not in foreground stop drawing
				return;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			if (!inFG) //if not in foreground stop drawing
				return false;
			return true;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			if (GetArchon.enchantment == Archon.Archon.Enchantment.Starlight && stage == ArchonAttackStage)
				DrawDoppelgangers();
		}

		private void DrawDoppelgangers()
		{
			Main.spriteBatch.End();

			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, SpiritMod.stardustOverlayEffect, Main.GameViewMatrix.TransformationMatrix);

			foreach (int item in doppelgangers)
			{
				NPC drawNPC = Main.npc[item];

				if (!drawNPC.active)
					continue;

				Vector2 oldPos = drawNPC.position;
				drawNPC.position += new Vector2(Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 2f));

				float oldRotation = drawNPC.rotation;
				drawNPC.rotation += Main.rand.NextFloat(-0.10f, 0.10f);

				Main.instance.DrawNPC(item, false);

				drawNPC.position = oldPos;
				drawNPC.rotation = oldRotation;
			}

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
		}

		//public override void PostDraw(SpriteBatch spriteBatch, Color drawColor) => GlowmaskUtils.DrawNPCGlowMask(spriteBatch, npc, ModContent.Request<Texture2D>("NPCs/StarjinxEvent/Enemies/Pathfinder/Pathfinder_Glow"), Color.White * 0.75f);

		public override void OnHitKill(int hitDirection, double damage)
		{

		}
		#endregion
	}
}