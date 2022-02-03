using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Mechanics.BackgroundSystem;
using System;
using System.Collections.Generic;
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
		private List<int> doppelgangers = new List<int>();

		// Void black hole attack
		private int blackHoleWhoAmI = -1;
		private ref Projectile BlackHole => ref Main.projectile[blackHoleWhoAmI];

		// Starlight duo attack
		private List<int> massiveStarWhoAmIs = new List<int>();

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

		public int duoMaxTime = 0;

		private void DuoBehaviour()
		{
			timers["DUO"]++;

			switch (GetArchon.enchantment)
			{
				case Archon.Archon.Enchantment.Starlight:
					StarlightDuoAttack();
					break;
				default:
					BasicIdleMovement();
					break;
			}

			if (timers["DUO"] >= duoMaxTime)
			{
				timers["DUO"] = 0;

				GetArchon.ResetEnchantment();
				GetArchon.SetDuo(0);
				stage = EnchantStage;
			}
		}

		private void StarlightDuoAttack()
		{
			GetArchon.SetDuo(timers["DUO"]);

			duoMaxTime = StarlightDuoMaxTime;

			if (timers["DUO"] == (int)(StarlightDuoMaxTime * 0.05f))
			{
				int p = Projectile.NewProjectile(npc.Center, Vector2.Zero, ModContent.ProjectileType<Projectiles.SplitStar>(), 120, 1f);
				Main.projectile[p].timeLeft = 100000;
				Main.projectile[p].scale = 20f;

				massiveStarWhoAmIs.Add(p);
			}
			else if (timers["DUO"] % (int)(StarlightDuoMaxTime * 0.05f) == 0)
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

		private void BasicIdleMovement(float speedMult = 1f)
		{
			float magnitude = 10 - (npc.Distance(Target.Center) * 0.02f);
			Vector2 vel = -npc.DirectionTo(Target.Center) * magnitude * speedMult;

			if (magnitude < 0)
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
			if (npc.DistanceSQ(sjinxLoc) > (StarjinxMeteorite.EVENT_RADIUS * StarjinxMeteorite.EVENT_RADIUS) * 1.05f)
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