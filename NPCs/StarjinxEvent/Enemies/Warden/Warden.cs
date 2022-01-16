using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Mechanics.BackgroundSystem;
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

		public const int EnchantMaxTime = 200;
		public const int ArchonAttackMaxTime = 1200;

		private int stage = InitStage;

		private ref Player Target => ref Main.player[npc.target];

		internal bool inFG = true;
		private bool transitionFG = false;
		private int fgTime = 0;
		private Vector2 fgPos = new Vector2();
		private WardenBG Background = null;

		internal int archonWhoAmI = -1;
		private ref NPC ArchonNPC => ref Main.npc[archonWhoAmI];
		private Archon.Archon GetArchon() => ArchonNPC.modNPC as Archon.Archon;


		private Dictionary<string, float> timers = new Dictionary<string, float>() { { "ENCHANT", 0 }, { "ARCHATK", 0 } };

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
				default:
					break;
			}
		}

		private void ArchonAttackBehaviour()
		{
			timers["ARCHATK"]++;

			if (timers["ARCHATK"] == ArchonAttackMaxTime && !GetArchon().waitingOnAttack)
			{
				GetArchon().ResetEnchantment();
				stage = EnchantStage;
				timers["ARCHATK"] = 0;
			}

			npc.velocity *= 0;// -npc.DirectionTo(Target.Center);
		}

		private void EnchantBehaviour()
		{
			timers["ENCHANT"]++;

			npc.velocity *= 0.94f;

			if (timers["ENCHANT"] == EnchantMaxTime / 2)
			{
				GetArchon().SetRandomEnchantment();
				CombatText.NewText(npc.getRect(), Color.Gold, $"Enchant moment - we got {GetArchon().enchantment}");
			}
			else if (timers["ENCHANT"] >= EnchantMaxTime)
			{
				GetArchon().stage = Archon.Archon.EnchantAttackStage;
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