using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.VerletChains;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.ReefhunterSet.Projectiles
{
	public class SkullSentryEye : ModProjectile
	{
		public const int MAX_DISTANCE = 600;
		public const int SHOOT_TIME = 60;

		public ref float Target => ref Projectile.ai[0];
		public ref float Timer => ref Projectile.ai[1];

		internal Vector2 anchor = Vector2.Zero;
		internal Vector2 pupilPos = Vector2.Zero;
		internal Projectile parent = null;
		internal Chain chain = null;

		public override void SetStaticDefaults() => DisplayName.SetDefault("Maneater");

		public override void SetDefaults()
		{
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.aiStyle = -1;
			Projectile.hide = true;
			Projectile.tileCollide = true; 
			Projectile.timeLeft = Projectile.SentryLifeTime;
		}

		public override bool? CanDamage()/* tModPorter Suggestion: Return null instead of false */ => false;

		private bool ParentActive() => parent.active && parent != null && parent.type == ModContent.ProjectileType<SkullSentrySentry>() && parent.owner == Projectile.owner;

		public override void AI()
		{
			if (!ParentActive()) //parent active check
				Projectile.Kill();
			else
				Projectile.timeLeft = Projectile.SentryLifeTime;

			Projectile.scale = parent.scale;
			Timer++;

			float distMod = Utilities.EaseFunction.EaseQuadOut.Ease(Math.Min(Timer / 480, 1)); //Ease worm distance from skull out over a few seconds
			Vector2 targetCenter = anchor + parent.Center + Vector2.Normalize(anchor) * 25 * distMod;

			float temp = Target;
			FindTarget();
			if (temp != Target) //If target changes, sync
				Projectile.netUpdate = true;

			if (Target == -1 || InvalidTarget((int)Target)) //Circular motion while idle
			{
				Vector2 displacement = Vector2.Normalize(anchor.RotatedBy(MathHelper.TwoPi * Timer / 240)) * 5 * distMod;
				targetCenter += displacement;
				if(Projectile.Center != targetCenter) //Prevent NaN (why is this necessary)
					Projectile.velocity = Vector2.Lerp(Projectile.velocity, Projectile.DirectionTo(targetCenter), 0.02f * distMod);

				if(Projectile.velocity.LengthSquared() > 0)
					pupilPos = Vector2.Lerp(pupilPos, Vector2.Normalize(Projectile.velocity) * 2, 0.03f); //Look in direction of movement
			}

			else
			{
				NPC target = Main.npc[(int)Target];
				Vector2 vel = Projectile.DirectionTo(target.Center + target.velocity) * 3f;
				Vector2 movementDelta = Vector2.Normalize(vel) * distMod * 2.5f; //Base factor for how shot projectile velocity affects this projectile's movement
				const float ANTICIPATION_TIME = SHOOT_TIME * 0.25f;

				if (Timer % SHOOT_TIME >= SHOOT_TIME - ANTICIPATION_TIME) //Right before shooting projectile, move backwards quickly in anticipation
				{
					Vector2 newPos = Projectile.position - (4 * movementDelta / (ANTICIPATION_TIME));
					if (!Collision.SolidCollision(newPos, Projectile.width, Projectile.height))
						Projectile.position = newPos;
				}

				if (Timer % SHOOT_TIME == 0) //Move with shot projectile 
				{
					Projectile shot = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center, vel, ModContent.ProjectileType<SkullSentryMucus>(), Projectile.damage, 10.2f, Projectile.owner);
					if(shot.ModProjectile is SkullSentryMucus mucus)
						mucus.MakeDust(Main.rand.Next(7, 10), 2, 1, 140, 5);

					Projectile.velocity += movementDelta;
					Projectile.netUpdate = true;
				}

				targetCenter += Projectile.DirectionTo(target.Center) * 10 * distMod; //Move slightly towards target
				Projectile.velocity = Vector2.Lerp(Projectile.velocity, Projectile.DirectionTo(targetCenter), 0.05f * distMod);
				pupilPos = Vector2.Lerp(pupilPos, Projectile.DirectionTo(target.Center) * 3, 0.05f); //Look at target
			}

			if (chain == null)
				chain = new Chain(10 * Projectile.scale, 5, Projectile.Center, new ChainPhysics(0.9f, 0.5f, 0.3f));

			else
				chain.Update(Projectile.Center, anchor + parent.Center);
		}

		private bool InvalidTarget(int target)
		{
			NPC npc = Main.npc[target];
			return !npc.CanBeChasedBy(this) || npc.Distance(parent.Center) > MAX_DISTANCE || !Collision.CanHitLine(Projectile.Center, 0, 0, npc.Center, 0, 0);
		}

		/// <summary>
		/// Scans nearby npcs and the current targetted npc, attempting to attack different npcs than other eye
		/// </summary>
		private void FindTarget()
		{
			int tempTarget = -1;
			List<int> lowPriority = new List<int>();
			Player owner = Main.player[Projectile.owner];
			if (owner.HasMinionAttackTargetNPC)
				if(!InvalidTarget(owner.MinionAttackTargetNPC))
					CheckOtherEyeTargets(ref lowPriority, ref tempTarget, owner.MinionAttackTargetNPC);

			if(tempTarget == -1) //If no minion attack target or another eye is targetting it, iterate through main.npc to see if there are other targets
			{
				float dist = MAX_DISTANCE;
				for (int i = 0; i < Main.maxNPCs; ++i)
				{
					NPC npc = Main.npc[i];

					if (!InvalidTarget(i) && npc.Distance(parent.Center) < dist)
					{
						CheckOtherEyeTargets(ref lowPriority, ref tempTarget, i);
						if (tempTarget == i)
							dist = npc.Distance(parent.Center);
					}
				}
			}

			//If still no target, use the first element on the low priority list
			if(lowPriority.Count > 0 && tempTarget == -1)
				tempTarget = lowPriority[0];

			Target = tempTarget;
		}

		/// <summary>
		/// Checks all other eyes of the parent skull- if one of them already is targetting the same npc, add it to a list of low priority targets
		/// </summary>
		/// <param name="lowPriority"></param>
		/// <param name="tempTarget"></param>
		/// <param name="npc"></param>
		private void CheckOtherEyeTargets(ref List<int> lowPriority, ref int tempTarget, int npc)
		{
			if(parent.ModProjectile is SkullSentrySentry skull)
			{
				int temp = tempTarget;
				foreach(Projectile proj in skull.GetEyeList())
				{
					if (proj == Projectile)
						continue;

					if(proj.ModProjectile is SkullSentryEye eye)
					{
						if (eye.Target == npc)
						{
							lowPriority.Add(npc);
							temp = tempTarget;
							break;
						}

						else
							temp = npc;
					}
				}

				tempTarget = temp;
			}
		}

		public void DrawChain(SpriteBatch spriteBatch)
		{ 
			if (chain != null)
				chain.Draw(spriteBatch, ModContent.Request<Texture2D>(Texture + "_Segment", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value, scale : Projectile.scale* 0.75f);
		}

		public void Draw(SpriteBatch spriteBatch, Color lightColor)
		{
			Projectile.QuickDraw(spriteBatch);

			Texture2D pupil = ModContent.Request<Texture2D>(Texture + "_Pupil", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			Vector2 origin = TextureAssets.Projectile[Projectile.type].Value.Size() / 2f;
			Vector2 pos = Projectile.Center - Main.screenPosition + origin - new Vector2(2) + pupilPos;

			spriteBatch.Draw(pupil, pos, null, Projectile.GetAlpha(lightColor), 0f, origin, 1f, SpriteEffects.None, 0f);
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.WriteVector2(anchor);
			writer.WriteVector2(pupilPos);

			if (parent == null)
				writer.Write(-1);
			else
				writer.Write(parent.whoAmI);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			anchor = reader.ReadVector2();
			pupilPos = reader.ReadVector2();

			int parentwhoAmI = reader.ReadInt32();
			if (parentwhoAmI == -1)
				parent = null;
			else
				parent = Main.projectile[parentwhoAmI];
		}

		public override bool PreKill(int timeLeft) => !ParentActive();

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.Bounce(oldVelocity, 1f);
			return false;
		}
	}
}
