using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Mechanics.BackgroundSystem;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;

namespace SpiritMod.NPCs.StarjinxEvent.Enemies.Archon
{
	public class Archon : SpiritNPC, IDrawAdditive
	{
		// Consts

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

		// Attacks
		private AttackType attack = AttackType.None;

		private Vector2 cachedSlashPos = new Vector2();
		private float slashTimeMax = 0;

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
			npc.damage = 0;
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

			if (timers["ENCHANT"] == EnchantMaxTime)
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
				default:
					break;
			}
		}

		private void SlashAttack()
		{
			const float AnticipationThreshold = 0.1f;
			const float BeginAttackThreshold = 0.3f;

			if (timers["ATTACK"] == (int)(slashTimeMax * AnticipationThreshold)) //Start slash anticipation
			{
				npc.Center = Target.Center + new Vector2(0, Main.rand.Next(300, 400)).RotatedByRandom(MathHelper.Pi);
				cachedSlashPos = npc.Center - (npc.DirectionFrom(Target.Center) * npc.Distance(Target.Center) * 2);
			}
			else if (timers["ATTACK"] > slashTimeMax * AnticipationThreshold && timers["ATTACK"] < slashTimeMax * BeginAttackThreshold)
			{
				float mult = (slashTimeMax * BeginAttackThreshold) - timers["ATTACK"];
				npc.velocity = -npc.DirectionTo(cachedSlashPos) * 0.5f * mult;
				npc.rotation = MathHelper.Lerp(npc.rotation, npc.AngleTo(cachedSlashPos), 0.2f);
			}
			else if (timers["ATTACK"] >= (int)(slashTimeMax * BeginAttackThreshold))
			{
				npc.Center = Vector2.Lerp(npc.Center, cachedSlashPos, 0.1f);
				npc.damage = 20;
			}

			if (timers["ATTACK"] >= slashTimeMax)
			{
				timers["ATTACK"] = 0;
				attack = AttackType.None;

				npc.velocity = Vector2.Zero;
				npc.damage = 0;
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
		#endregion

		public enum AttackType : int
		{
			None = 0,
			TeleportSlash = 1,
		}

		public void SetupRandomAttack()
		{
			attack = AttackType.TeleportSlash;// (Enchantment)(Main.rand.Next((int)Enchantment.Count - 1) + 1);

			if (attack == AttackType.TeleportSlash)
			{
				slashTimeMax = 160;
				if (enchantment == Enchantment.Meteor)
					slashTimeMax = 200; //Meteor slash is slower
			}
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
