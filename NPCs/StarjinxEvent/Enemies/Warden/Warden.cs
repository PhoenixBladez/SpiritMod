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

		private ref Player Target => ref Main.player[npc.target];

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
		private ref Projectile PongMeteor => ref Main.projectile[meteorWhoAmI];

		private Vector2 meteorPongPosition = Vector2.Zero;
		private Vector2 archonMeteorPongPosition = Vector2.Zero;
		private int pongTimer = 0; //So they don't hit several times in succession

		// Void duo attack
		private readonly List<int> voidPortals = new List<int>();

		internal int archonWhoAmI = -1;
		private ref NPC ArchonNPC => ref Main.npc[archonWhoAmI];
		private Archon.Archon GetArchon => ArchonNPC.modNPC as Archon.Archon;

		private Dictionary<string, float> timers = new Dictionary<string, float>() { { "ENCHANT", 0 }, { "ARCHATK", 0 }, { "ATTACK", 0 }, { "DUO", 0 } };

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Warden");
			Main.npcFrameCount[npc.type] = 1;
		}

		public override void SetDefaults()
		{
			npc.width = 136;
			npc.height = 128;
			npc.damage = 0;
			npc.defense = 28;
			npc.lifeMax = 12000;
			npc.aiStyle = -1;
			npc.HitSound = SoundID.DD2_CrystalCartImpact;
			npc.DeathSound = SoundID.DD2_ExplosiveTrapExplode;
			npc.value = Item.buyPrice(0, 0, 15, 0);
			npc.knockBackResist = 0f;
			npc.noGravity = true;
			npc.noTileCollide = true;
			npc.boss = true;
		}

		public override bool CheckActive() => !ModContent.GetInstance<StarjinxEventWorld>().StarjinxActive;

		public override void AI()
		{
			if (Background == null) //init stuff
			{
				Background = new WardenBG(npc.Center, npc);
				BackgroundItemManager.AddItem(Background);

				archonWhoAmI = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y + 300, ModContent.NPCType<Archon.Archon>());

				if (Main.netMode != NetmodeID.SinglePlayer)
					NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, archonWhoAmI);

				stage = EnchantStage;
			}

			npc.TargetClosest(true);
			npc.dontTakeDamage = !inFG;

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
			npc.alpha = 0;

			float dist = npc.DistanceSQ(Target.GetModPlayer<StarjinxPlayer>().StarjinxPosition);
			if (dist > StarjinxMeteorite.EVENT_RADIUS * StarjinxMeteorite.EVENT_RADIUS)
			{
				dist -= StarjinxMeteorite.EVENT_RADIUS * StarjinxMeteorite.EVENT_RADIUS;
				npc.alpha = dist > 50 ? 255 : (int)(dist / 50f) * 255;
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

			Vector2 vel = npc.DirectionTo(portal) * 7;
			Projectile.NewProjectile(npc.Center, vel, ModContent.ProjectileType<Projectiles.VoidProjectile>(), 20, 1f);
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

				int p = Projectile.NewProjectile(center + pos, Vector2.Zero, ModContent.ProjectileType<Projectiles.VoidPortal>(), 0, 0);
				Main.projectile[p].timeLeft = VoidDuoMaxTime - (int)timers["DUO"];

				voidPortals.Add(p);
			}

			var unmatchedPortals = new List<int>(voidPortals);

			Projectiles.VoidPortal GetPortal(int index) => (Main.projectile[index].modProjectile != null && Main.projectile[index].modProjectile is Projectiles.VoidPortal portal) ? portal : null;

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

			if (!PongMeteor.active || meteorWhoAmI != -1)
				return;

			duoMaxTime = MeteorDuoMaxTime;

			ArchonNPC.velocity *= 0.93f;
			npc.velocity *= 0.93f;

			//Gets the next pong position of a boss
			void CalculateBouncePosition(bool setArchon)
			{
				Vector2 center = setArchon ? npc.Center : ArchonNPC.Center;
				float dir = (center - Target.GetModPlayer<StarjinxPlayer>().StarjinxPosition).ToRotation();
				float newDir = Main.rand.NextFloat(MathHelper.TwoPi);

				while (Math.Abs((newDir + MathHelper.Pi) - (dir + MathHelper.Pi)) < MathHelper.Pi)
					newDir = Main.rand.NextFloat(MathHelper.TwoPi);

				Vector2 newPos = Target.GetModPlayer<StarjinxPlayer>().StarjinxPosition + new Vector2(0, StarjinxMeteorite.EVENT_RADIUS - 120).RotatedBy(newDir);

				if (setArchon)
					archonMeteorPongPosition = newPos;
				else
					meteorPongPosition = newPos;
			}

			if (timers["DUO"] == 1) //Initialize
			{
				float rot = Main.rand.NextFloat(-MathHelper.Pi, MathHelper.Pi);
				meteorPongPosition = Target.GetModPlayer<StarjinxPlayer>().StarjinxPosition + new Vector2(0, StarjinxMeteorite.EVENT_RADIUS - 120).RotatedBy(rot);
			}
			else if (timers["DUO"] < (int)(duoMaxTime * MeteorPongStartThreshold)) //Set first pong position
				npc.Center = Vector2.Lerp(npc.Center, meteorPongPosition, timers["DUO"] / (duoMaxTime * 0.05f));
			else if (timers["DUO"] == (int)(duoMaxTime * MeteorPongStartThreshold)) //Spawn pong meteor and set Archon pong position
			{
				CalculateBouncePosition(true);

				int type = ModContent.ProjectileType<Archon.Projectiles.MeteorEnchantment_Meteor>();
				int p = Projectile.NewProjectile(npc.Center, npc.DirectionTo(archonMeteorPongPosition) * MeteorPongSpeed, type, 20, 1f);
				Main.projectile[p].timeLeft = MeteorDuoMaxTime;
				Main.projectile[p].scale = 10f;

				meteorWhoAmI = p;
				pongTimer = 10;
			}
			else
			{
				if (timers["DUO"] >= duoMaxTime * 0.95f)
				{
					PongMeteor.velocity *= 0.95f;
				}
				else if (timers["DUO"] >= duoMaxTime)
					return;

				npc.Center = Vector2.Lerp(npc.Center, meteorPongPosition, 0.05f);
				ArchonNPC.Center = Vector2.Lerp(ArchonNPC.Center, archonMeteorPongPosition, 0.05f);

				pongTimer--;

				if (pongTimer >= 0)
					return;

				const int PongDistance = 140 * 140;

				if (npc.DistanceSQ(PongMeteor.Center) < PongDistance)
				{
					CalculateBouncePosition(true);
					PongMeteor.velocity = npc.DirectionTo(archonMeteorPongPosition) * MeteorPongSpeed;
					pongTimer = 10;
				}
				else if (ArchonNPC.DistanceSQ(PongMeteor.Center) < PongDistance)
				{
					CalculateBouncePosition(false);
					PongMeteor.velocity = ArchonNPC.DirectionTo(meteorPongPosition) * MeteorPongSpeed;
					pongTimer = 10;
				}
			}
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
				int p = Projectile.NewProjectile(npc.Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.SplitStar>(), 20, 1f);
				Main.projectile[p].timeLeft = StarlightDuoMaxTime;
				Main.projectile[p].scale = 20f;

				massiveStarWhoAmIs.Add(p);
			}
			else if (timers["DUO"] % (int)(duoMaxTime * 0.1f) == 0)
			{
				Projectile proj = Main.projectile[Main.rand.Next(massiveStarWhoAmIs)];

				if (proj.active && proj.modProjectile is Projectiles.SplitStar star)
				{
					int newProj = star.Split();

					if (newProj != -1)
						massiveStarWhoAmIs.Add(newProj);
				}
			}
		}

		private void SoloBehaviour()
		{
			BasicIdleMovement(2f);
		}

		private void ArchonAttackBehaviour()
		{
			timers["ARCHATK"]++;

			if (timers["ARCHATK"] >= 20 && ArchonNPC.active && ArchonNPC.life > 0 && !GetArchon.waitingOnAttack)
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
				int p = Projectile.NewProjectile(npc.Center, Vector2.Zero, ProjectileID.CultistBossLightningOrb, 60, 1f);
				Main.projectile[p].timeLeft = ArchonAttackMaxTime - 100;

				blackHoleWhoAmI = p;
			}
			else if (timers["ARCHATK"] > 50 && timers["ARCHATK"] < ArchonAttackMaxTime - 100 && blackHoleWhoAmI > -1) //Control void projectile & break if hit
			{
				if (npc.justHit || !BlackHole.active)
				{
					BlackHole.timeLeft = 50;

					blackHoleWhoAmI = -1;
					return;
				}

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
			npc.velocity *= 0.94f;

			if (timers["ARCHATK"] == 50) //Spawn NPCs
			{
				int npcCount = Main.rand.Next(3, 6);
				int[] types = new int[] { ModContent.NPCType<Pathfinder.Pathfinder>(), ModContent.NPCType<Starachnid.Starachnid>(), ModContent.NPCType<StarWeaver.StarWeaverNPC>() };

				for (int i = 0; i < npcCount; ++i)
				{
					int id = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y - 200, Main.rand.Next(types), Main.rand.Next(10) * 10);

					doppelgangers.Add(id);
				}
			}
			else if (timers["ARCHATK"] == ArchonAttackMaxTime - 100) //Blow up NPCs
			{
				foreach (int item in doppelgangers)
				{
					NPC npc = Main.npc[item];
					if (npc.active && npc.life > -1)
					{
						npc.active = false;

						int p = Projectile.NewProjectile(npc.Center, Vector2.Zero, ProjectileID.Dynamite, 120, 1f);
						Main.projectile[p].timeLeft = 3;
					}
				}
			}
		}

		private void BasicIdleMovement(float speedMult = 1f, bool run = true)
		{
			float magnitude = 10 - (npc.Distance(Target.Center) * 0.02f);
			Vector2 vel = -npc.DirectionTo(Target.Center) * magnitude * speedMult;

			if (magnitude < 0 || !run)
			{
				magnitude = (npc.Distance(Target.GetModPlayer<StarjinxPlayer>().StarjinxPosition) * 0.02f) - 3;
				if (magnitude < 0)
					magnitude = 0;

				vel = npc.DirectionTo(Target.GetModPlayer<StarjinxPlayer>().StarjinxPosition) * magnitude;
			}

			npc.velocity = vel;

			WrapLocation();
		}

		private void WrapLocation()
		{
			Vector2 sjinxLoc = Target.GetModPlayer<StarjinxPlayer>().StarjinxPosition;
			if (npc.DistanceSQ(sjinxLoc) > StarjinxMeteorite.EVENT_RADIUS * StarjinxMeteorite.EVENT_RADIUS * 1.05f)
			{
				Vector2 offset = npc.Center - sjinxLoc;
				npc.Center = sjinxLoc - (offset * 0.95f);
			}
		}

		private void EnchantBehaviour()
		{
			timers["ENCHANT"]++;

			npc.velocity *= 0.94f;

			if (timers["ENCHANT"] == EnchantMaxTime / 2)
			{
				GetArchon.SetRandomEnchantment();
				CombatText.NewText(npc.getRect(), Color.Gold, $"Enchant moment - we got {GetArchon.enchantment}");
			}
			else if (timers["ENCHANT"] >= EnchantMaxTime)
			{
				GetArchon.stage = Archon.Archon.EnchantAttackStage;
				stage = ArchonAttackStage;
				timers["ENCHANT"] = 0;
			}
		}

		#region Drawing
		public void AdditiveCall(SpriteBatch sB)
		{
			if (!inFG) //if not in foreground stop drawing
				return;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			if (!inFG) //if not in foreground stop drawing
				return false;
			return true;
		}

		//public override void PostDraw(SpriteBatch spriteBatch, Color drawColor) => GlowmaskUtils.DrawNPCGlowMask(spriteBatch, npc, mod.GetTexture("NPCs/StarjinxEvent/Enemies/Pathfinder/Pathfinder_Glow"), Color.White * 0.75f);

		public override void OnHitKill(int hitDirection, double damage)
		{

		}
		#endregion
	}
}