using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using SpiritMod.NPCs.StarjinxEvent.Enemies.Starachnid;

namespace SpiritMod.NPCs.StarjinxEvent.Enemies.Pathfinder
{
	public class Pathfinder : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pathfinder");
			Main.npcFrameCount[npc.type] = 7;
		}

		private NPC Target;

		private bool LockedOn
		{
			get
			{
				if (Target != null)
					return Target.active && Vector2.Distance(Target.Center, npc.Center) <= 800;
				return false;
			}
		}

		private int Speed => 14;

		public override void SetDefaults()
		{
			npc.width = 45;
			npc.height = 78;
			npc.damage = 0;
			npc.defense = 28;
			npc.lifeMax = 450;
			npc.HitSound = SoundID.NPCHit3;
			npc.DeathSound = SoundID.DD2_LightningBugDeath;
			npc.value = 600f;
			npc.knockBackResist = 0;
			npc.noGravity = true;
			npc.noTileCollide = true;
		}

		public override void AI()
		{
			if (Target == null)
				FindTarget();
			else if (!Target.active || Vector2.Distance(Target.Center, npc.Center) > 800)
			{
				FindTarget();
			}
			else
				FollowTarget();
		}

		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
			npc.frameCounter += 0.30f;
		}

		private void FindTarget()
		{
			npc.velocity *= 0.99f;
			if (Target != null)
				Target.GetGlobalNPC<PathfinderGNPC>().Targetted = false;

			NPC target = Main.npc.Where(n => n.active && Vector2.Distance(n.Center, npc.Center) < 600 && !n.GetGlobalNPC<PathfinderGNPC>().Targetted && n.modNPC is IStarjinxEnemy modEnemy).OrderBy(n => Vector2.Distance(n.Center, npc.Center)).FirstOrDefault();
			if (target != default)
			{
				Target = target;
			}
		}

		private void FollowTarget()
		{
			Target.GetGlobalNPC<PathfinderGNPC>().Targetted = true;
			Vector2 direction = Target.Center - npc.Center;
			if (direction.Length() > 250)
			{
				direction.Normalize();
				npc.velocity = Vector2.Lerp(npc.velocity, direction * Speed, 0.018f);
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			if (LockedOn)
			{
				Texture2D beam = mod.GetTexture("Effects/Mining_Helmet");

				Vector2 drawPosition = npc.Center;

				Color color = Color.Lerp(SpiritMod.StarjinxColor(Main.GameUpdateCount / 12f), Color.White, 0.5f);
				color.A = 0;
				float beamOpacity = 1;

				Vector2 dist = Target.Center - drawPosition;

				for (int i = -1; i <= 1; i++)
				{
					float rot = dist.ToRotation();
					Vector2 offset = (i == 0) ? Vector2.Zero : Vector2.UnitX.RotatedBy(rot + MathHelper.PiOver4 * i) * 4;
					float opacity = (i == 0) ? 1f : 0.5f;
					spriteBatch.Draw(beam, drawPosition + offset - Main.screenPosition, null, color * beamOpacity * opacity, rot + MathHelper.PiOver2, new Vector2(beam.Width / 2f, beam.Height * 0.58f), new Vector2(1, dist.Length() / (beam.Height / 2f)), SpriteEffects.None, 0);
				}
			}
			return true;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			GlowmaskUtils.DrawNPCGlowMask(spriteBatch, npc, mod.GetTexture("NPCs/StarjinxEvent/Enemies/Pathfinder/Pathfinder_Glow"));
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				if (Target != null)
				{
					if (LockedOn)
						Target.GetGlobalNPC<PathfinderGNPC>().Targetted = false;
				}
			}
		}

	}
	public class PathfinderGNPC : GlobalNPC 
	{
		public override bool InstancePerEntity => true;

		public bool Targetted = false;

		public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Color drawColor)
		{
			if (Targetted && npc.modNPC is IStarjinxEnemy starjinxEnemy)
				starjinxEnemy.DrawPathfinderOutline(spriteBatch);
			return base.PreDraw(npc, spriteBatch, drawColor);
		}
	}
}