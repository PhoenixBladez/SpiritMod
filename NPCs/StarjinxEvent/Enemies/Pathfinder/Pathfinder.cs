using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Linq;
using SpiritMod.Prim;
using SpiritMod.Particles;
using SpiritMod;
using System.IO;
using System.Collections.Generic;

namespace SpiritMod.NPCs.StarjinxEvent.Enemies.Pathfinder
{
	public class Pathfinder : SpiritNPC, IDrawAdditive
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pathfinder");
			Main.npcFrameCount[NPC.type] = 7;
		}

		public override bool IsLoadingEnabled(Mod mod) => false;

		public override void SetDefaults()
		{
			NPC.width = 45;
			NPC.height = 78;
			NPC.damage = 0;
			NPC.defense = 28;
			NPC.lifeMax = 450;
			NPC.aiStyle = -1;
			NPC.HitSound = SoundID.DD2_CrystalCartImpact;
			NPC.DeathSound = SoundID.DD2_ExplosiveTrapExplode;
			NPC.value = Item.buyPrice(0, 0, 15, 0);
			NPC.knockBackResist = 1f;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
		}

		private NPC Target = null;

		private bool LockedOn => Target != null && Target.active;
		private const int SPEED = 14;

		private bool selfDestruct = false;

		private ref float AiTimer => ref NPC.ai[0];
		private ref float BeamStrength => ref NPC.ai[1];
		private float flickerStrength = 0;

		private const int SCANTIME = 180; //How long it tries to look for a target before self destructing
		private const float FOLLOW_MINRANGE = 350; //How far away it tries to get to the enemy when buffing them
		private const float FOLLOW_MAXRANGE = 500; //The maximum distance it can maintain before it stops buffing the targetted enemy
		private const int SELFDESTRUCT_TIME = 120;

		public override void AI()
		{
			NPC.TargetClosest(false);

			Lighting.AddLight(NPC.Center, Color.HotPink.ToVector3() * flickerStrength);
			//Scan to find targets for first few seconds of not having one
			if (!LockedOn && !selfDestruct)
			{
				FindTarget();

				//Reset timer and start self destruct if scanning too long
				if (AiTimer == SCANTIME)
				{
					AiTimer = 0;
					selfDestruct = true;
					NPC.netUpdate = true;
				}
			}

			//If its target is still alive, beam to it
			else if (LockedOn)
				FollowTarget();

			//If its target is dead, start self-destructing
			else
				SelfDestruct();

			NPC.rotation = NPC.velocity.X * 0.03f;
			
			if(!selfDestruct)
				flickerStrength = MathHelper.Max(flickerStrength - 0.03f, 0);

			NPC.position.Y += (float)Math.Sin(Main.GameUpdateCount / 30f); //Constant hovering motion added on to other velocity
		}

		private void FindTarget()
		{
			AiTimer++;

			BeamStrength = 0;
			NPC.velocity *= 0.95f;

			NPC target = Main.npc.Where(n => n.active && !n.GetGlobalNPC<PathfinderGNPC>().Targetted && n.ModNPC is IStarjinxEnemy modEnemy).OrderBy(n => Vector2.Distance(n.Center, NPC.Center)).FirstOrDefault();
			if (target != default)
			{
				Target = target;
				target.GetGlobalNPC<PathfinderGNPC>().TargetTime = 2;
				NPC.netUpdate = true;
			}

			UpdateYFrame(8, 0, 6);
		}

		private void FollowTarget()
		{
			AiTimer = SCANTIME * 0.75f; //Set timer to portion of max scantime, as longer amount of time isn't needed when we already know every enemy has spawned
			Target.GetGlobalNPC<PathfinderGNPC>().TargetTime = 2;

			//Move to target if too far away
			Vector2 desiredPos = Target.Center + (NPC.DirectionFrom(Target.Center) * FOLLOW_MINRANGE);
			NPC.velocity = Vector2.Lerp(NPC.velocity, Vector2.Lerp(NPC.Center, desiredPos, 0.04f) - NPC.Center, 0.1f);
			float velHardCap = 30;
			if (NPC.velocity.Length() > velHardCap)
				NPC.velocity = Vector2.Normalize(NPC.velocity) * velHardCap;

			//Buff enemy if close enough
			if (NPC.Distance(Target.Center) <= FOLLOW_MAXRANGE)
			{
				BeamStrength = Math.Min(BeamStrength + 0.03f, 1);
				if (BeamStrength == 1)
					Target.GetGlobalNPC<PathfinderGNPC>().BuffTime = 2;
			}

			//If not, stop light and buff
			else
				BeamStrength = 0;

			if (NPC.soundDelay-- < 0)
			{
				SoundEngine.PlaySound(SoundID.Item8, NPC.Center);
				NPC.soundDelay = 60;
			}

			UpdateYFrame(15, 0, 6, delegate 
			{
				if (frame.Y == 5 && NPC.Distance(Target.Center) <= FOLLOW_MAXRANGE)
					flickerStrength = 1;
			});
		}

		private void SelfDestruct()
		{
			AiTimer++;

			BeamStrength = 0;
			flickerStrength = MathHelper.Lerp(flickerStrength, 1.5f, 0.015f);

			Target = null;

			NPC.velocity.X *= 0.95f;
			NPC.velocity.Y = -1;


			if(AiTimer > SELFDESTRUCT_TIME)
			{
				NPC.life = 0;
				SoundEngine.PlaySound(NPC.DeathSound, NPC.Center);
				NPC.HitEffect();
				NPC.NPCLoot();
				NPC.active = false;
			}
			UpdateYFrame(12, 0, 6);
		}

		public override Color? GetAlpha(Color drawColor) => Color.Lerp(drawColor, Color.White, 0.5f) * NPC.Opacity;

		public void AdditiveCall(SpriteBatch sB)
		{
			Vector2 drawPosition = NPC.Center + (Vector2.UnitY * NPC.height / 4);

			if (LockedOn && NPC.Distance(Target.Center) < FOLLOW_MAXRANGE)
			{
				Effect effect = ModContent.Request<Effect>("Effects/EmpowermentBeam", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
				effect.Parameters["uTexture"].SetValue(Mod.Assets.Request<Texture2D>("Textures/Trails/Trail_2", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
				effect.Parameters["progress"].SetValue(Main.GlobalTimeWrappedHourly / 3);
				effect.Parameters["uColor"].SetValue(new Color(245, 59, 164).ToVector4());
				effect.Parameters["uSecondaryColor"].SetValue(new Color(255, 138, 212).ToVector4());

				Vector2 dist = Target.Center - drawPosition;

				TrianglePrimitive tri = new TrianglePrimitive()
				{
					TipPosition = drawPosition - Main.screenPosition,
					Rotation = dist.ToRotation(),
					Height = 100 + dist.Length() * 1.5f,
					Color = Color.White * BeamStrength,
					Width = 80 + ((Target.width + Target.height) * 0.5f)
				};

				PrimitiveRenderer.DrawPrimitiveShape(tri, effect);
			}

			float blurLength = 250;
			float blurWidth = 25;

			Effect blurEffect = ModContent.Request<Effect>("Effects/BlurLine", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			SquarePrimitive blurLine = new SquarePrimitive()
			{
				Position = drawPosition - Main.screenPosition,
				Height = blurWidth * flickerStrength,
				Length = blurLength * flickerStrength,
				Rotation = NPC.rotation,
				Color = new Color(255, 138, 212) * flickerStrength
			};

			PrimitiveRenderer.DrawPrimitiveShape(blurLine, blurEffect);

			sB.Draw(ModContent.Request<Texture2D>(Texture + "_Glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value, NPC.Center - Main.screenPosition, NPC.frame, Color.White,
				NPC.rotation, NPC.frame.Size() / 2, NPC.scale, NPC.spriteDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
		}

		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) => GlowmaskUtils.DrawNPCGlowMask(spriteBatch, NPC, Mod.Assets.Request<Texture2D>("NPCs/StarjinxEvent/Enemies/Pathfinder/Pathfinder_Glow").Value, Color.White * 0.75f);

		public override void OnHitKill(int hitDirection, double damage)
		{
			if (!Main.dedServ)
			{
				for(int i = 0; i < 2; i++)
					ParticleHandler.SpawnParticle(new PulseCircle(NPC.Center, Color.HotPink * 0.2f, 120 + 30 * i, 15, PulseCircle.MovementType.OutwardsSquareRooted)
					{
						RingColor = Color.HotPink
					});

				for (int i = 0; i < 10; i++)
					ParticleHandler.SpawnParticle(new ImpactLine(NPC.Center, Main.rand.NextVector2Unit() * Main.rand.NextFloat(0.5f, 1.5f), Color.HotPink, new Vector2(0.5f, Main.rand.NextFloat(1f, 3f)), 15));

				for (int i = 0; i < 7; i++)
					ParticleHandler.SpawnParticle(new StarParticle(NPC.Center, Main.rand.NextVector2Unit() * Main.rand.NextFloat(6f), Color.LightPink, Color.HotPink, Main.rand.NextFloat(0.2f, 0.4f), 25));

				for (int i = 0; i < 7; i++)
					ParticleHandler.SpawnParticle(new PathfinderGores.PathfinderGore(NPC.Center, Main.rand.NextVector2Circular(14, 14), Main.rand.NextFloat(0.8f, 1.2f), 180));
			}
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(selfDestruct);

			if (Target == null)
				writer.Write(-1);
			else
				writer.Write(Target.whoAmI);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			selfDestruct = reader.ReadBoolean();

			int index = reader.ReadInt32();
			if (index == -1)
				Target = null;
			else
				Target = Main.npc[index];
		}
	}

	public class PathfinderGNPC : GlobalNPC
	{
		public override bool InstancePerEntity => true;

		//ints are used here instead of bools, as using bools that are reset to false in reseteffects or postai causes them to sometimes be set to false before the npc's ai or drawing is called, hence an extra tick is needed
		public int TargetTime = 0; //Whether or not the npc is used as a pathfinder's target, used to prevent overlapping buffs
		public int BuffTime = 0; //Whether or not the npc is currently receiving a buff from a pathfinder

		public bool Targetted => TargetTime > 0;
		public bool Buffed => BuffTime > 0;

		public override void ResetEffects(NPC npc)
		{
			TargetTime = Math.Max(TargetTime - 1, 0);
			BuffTime = Math.Max(BuffTime - 1, 0);
		}

		public static void DrawBuffedOutlines(SpriteBatch spriteBatch)
		{
			List<IStarjinxEnemy> starjinxEnemies = new List<IStarjinxEnemy>();
			foreach(NPC npc in Main.npc)
			{
				if (npc != null) //First, do a null check
					if (npc.active && npc.GetGlobalNPC<PathfinderGNPC>().Buffed && npc.ModNPC is IStarjinxEnemy starjinxEnemy) //If npc is active and a buffed starjinx enemy, add to list
						starjinxEnemies.Add(starjinxEnemy);
			}

			//If any buffed starjinx enemies exist, restart spritebatch and draw all outlines
			if(starjinxEnemies.Any())
			{
				spriteBatch.End(); spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, null, null, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

				foreach (IStarjinxEnemy sjinxEnemy in starjinxEnemies)
					sjinxEnemy.DrawPathfinderOutline(spriteBatch);

				spriteBatch.End(); spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
			}
		}
	}
}