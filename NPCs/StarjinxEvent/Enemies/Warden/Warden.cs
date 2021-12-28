using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Mechanics.BackgroundSystem;
using Terraria;
using Terraria.ID;

namespace SpiritMod.NPCs.StarjinxEvent.Enemies.Warden
{
	public class Warden : SpiritNPC, IDrawAdditive
	{
		public const int MaxForegroundTransitionTime = 1200;

		private ref Player Target => ref Main.player[npc.target];
		private WardenBG Background = null;

		internal bool inFG = true;

		private int fgTime = 0;
		private Vector2 fgPos = new Vector2();

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
			}
			//ArchonLiving = NPC.AnyNPCs(ModContent.NPCType<Archon>());

			npc.TargetClosest(true);
			npc.dontTakeDamage = !inFG;

			if (fgTime-- > 0)
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
		}

		private void ToggleForeground()
		{
			fgTime = MaxForegroundTransitionTime;
			fgPos = npc.Center;
		}

		private void UpdateBackground()
		{
		}
		#endregion

		private void UpdateForeground()
		{
		}

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
	}
}