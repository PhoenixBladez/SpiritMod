using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Linq;

namespace SpiritMod.NPCs.StarjinxEvent.Enemies.Pathfinder
{
	public class Pathfinder : ModNPC, IDrawAdditive
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pathfinder");
			Main.npcFrameCount[npc.type] = 7;
		}

		private NPC Target;

		private bool LockedOn => Target != null && Target.active;
		private int Speed => 14;

		public override void SetDefaults()
		{
			npc.width = 45;
			npc.height = 78;
			npc.damage = 0;
			npc.defense = 28;
			npc.lifeMax = 450;
			npc.HitSound = SoundID.NPCHit3;
			npc.DeathSound = SoundID.NPCDeath6;//SoundID.DD2_LightningBugDeath;
			npc.value = Item.buyPrice(0, 0, 15, 0);
			npc.knockBackResist = 1f;
			npc.noGravity = true;
			npc.noTileCollide = true;
		}

		public override void AI()
		{
			if (Target == null || !Target.active)
				FindTarget();
			else
				FollowTarget();

			npc.rotation = npc.velocity.X * 0.08f;
		}

		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
			npc.frameCounter += (LockedOn) ? 0.30f : 0.2f;
		}

		private void FindTarget()
		{
			npc.velocity.X *= 0.99f;
			npc.velocity.Y = MathHelper.Lerp(npc.velocity.Y, (float)Math.Sin(Main.GameUpdateCount / 40f) * 1.5f, 0.1f);

			if (Target != null)
				Target.GetGlobalNPC<PathfinderGNPC>().Targetted = false;

			NPC target = Main.npc.Where(n => n.active && !n.GetGlobalNPC<PathfinderGNPC>().Targetted && n.modNPC is IStarjinxEnemy modEnemy).OrderBy(n => Vector2.Distance(n.Center, npc.Center)).FirstOrDefault();
			if (target != default)
				Target = target;
		}

		private void FollowTarget()
		{
			Target.GetGlobalNPC<PathfinderGNPC>().Targetted = true;
			Lighting.AddLight(npc.Center, Color.HotPink.ToVector3());

			Vector2 direction = Target.Center - npc.Center;

			if (direction.Length() > 250)
			{
				direction.Normalize();
				npc.velocity = Vector2.Lerp(npc.velocity, direction * Speed, 0.018f);
			}

			if (npc.soundDelay-- < 0)
			{
				Main.PlaySound(SoundID.Item8, npc.Center);
				npc.soundDelay = 60;
			}
		}

		public void AdditiveCall(SpriteBatch sB)
		{
			if (LockedOn)
			{
				Texture2D beam = mod.GetTexture("Effects/Mining_Helmet");

				Vector2 drawPosition = npc.Center + (Vector2.UnitY * npc.height / 4);

				Color color = Color.Lerp(Color.HotPink, Color.White, 0.33f);
				float beamOpacity = 1;

				Vector2 dist = Target.Center - drawPosition;

				for (int i = -4; i <= 4; i++)
				{
					float rot = dist.ToRotation();
					Vector2 offset = (i == 0) ? Vector2.Zero : Vector2.UnitX.RotatedBy(rot + MathHelper.PiOver4 * i) * 4;
					float opacity = 1 - (Math.Abs(i / 3f));
					opacity *= 0.75f;
					sB.Draw(beam, drawPosition + offset - Main.screenPosition, null, color * beamOpacity * opacity, rot + MathHelper.PiOver2, new Vector2(beam.Width / 2f, beam.Height * 0.58f), new Vector2(1, dist.Length() / (beam.Height / 2f)), SpriteEffects.None, 0);
				}

				sB.Draw(ModContent.GetTexture(Texture + "_Glow"), npc.Center - Main.screenPosition, npc.frame, Color.White * 0.5f,
					npc.rotation, npc.frame.Size() / 2, npc.scale * 1.15f, npc.spriteDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
			}
		}

		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor) => GlowmaskUtils.DrawNPCGlowMask(spriteBatch, npc, mod.GetTexture("NPCs/StarjinxEvent/Enemies/Pathfinder/Pathfinder_Glow"), Color.White * 0.75f);

		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0 && Target != null && LockedOn)
				Target.GetGlobalNPC<PathfinderGNPC>().Targetted = false;

			if (npc.life <= 0)
				for (int k = 0; k < 4; k++)
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/StarjinxEvent/Pathfinder/Pathfinder_" + Main.rand.Next(3)), Main.rand.NextFloat(.6f, 1f));
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