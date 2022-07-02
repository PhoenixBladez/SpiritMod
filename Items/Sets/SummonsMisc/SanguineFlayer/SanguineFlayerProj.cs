using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.IO;
using SpiritMod.Utilities;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Particles;
using System.Collections.Generic;
using SpiritMod.Mechanics.Trails;
using SpiritMod.Mechanics.Trails.CustomTrails;

namespace SpiritMod.Items.Sets.SummonsMisc.SanguineFlayer
{
	public class SanguineFlayerProj : ModProjectile, IDrawAdditive, IManualTrailProjectile
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Sanguine Flayer");

		public override void SetDefaults()
		{
			Projectile.friendly = true;
			Projectile.Size = new Vector2(55, 55);
			Projectile.tileCollide = false;
			Projectile.ownerHitCheck = true;
			Projectile.ignoreWater = true;
			Projectile.hide = true;
			Projectile.penetrate = -1;
			Projectile.usesLocalNPCImmunity = true;
		}

		public void DoTrailCreation(TrailManager tM)
		{
			float intensity = MathHelper.Lerp(0.85f, 1f, Math.Min(EaseFunction.EaseQuadIn.Ease(Intensity / HIGH_IMPACT_THRESHOLD), 1));
			tM.CreateCustomTrail(new VNoiseMotionTrail(Projectile, new Color(123, 19, 19), 50 * intensity, intensity));
		}

		private Player Owner => Main.player[Projectile.owner];

		public int SwingTime;
		public float SwingDistance;

		public ref float Timer => ref Projectile.ai[0];
		public ref float AiState => ref Projectile.ai[1];

		public const int STATE_THROWOUT = 0; //When initially thrown out by the player, acts like a whip
		public const int STATE_HOOKED = 1; //When hooked into an enemy
		public const int STATE_HOOKRETURN = 2; //When returning from being hooked into an enemy

		private Vector2 returnPosOffset; //The position of the projectile when it starts returning to the player from being hooked
		private Vector2 npcHookOffset = Vector2.Zero; //Used to determine the offset from the hooked npc's center
		private float npcHookRotation; //Stores the projectile's rotation when hitting an npc
		private NPC hookNPC; //The npc the projectile is hooked into

		public const float THROW_RANGE = 320; //Peak distance from player when thrown out, in pixels
		public const float HOOK_MAXRANGE = 700; //Maximum distance between owner and hooked enemies before it automatically rips out
		public const int HOOK_HITTIME = 30; //Time between damage ticks while hooked in
		public const int RETURN_TIME = 6; //Time it takes for the projectile to return to the owner after being ripped out

		private float _chainGravStrength = 0; //Amount at which gravity is applied to the chain

		private bool HighImpact => Intensity > HIGH_IMPACT_THRESHOLD;
		private static readonly float HIGH_IMPACT_THRESHOLD = 2.5f;

		private bool _hasFlashed;
		private int _flashTime;
		private static readonly int FLASH_TIME_MAX = 40;

		public override bool? CanDamage()/* tModPorter Suggestion: Return null instead of false */ => AiState == STATE_THROWOUT; //Only actually hit enemies on initial throw

		public override void AI()
		{
			if(Projectile.timeLeft > 2) //Initialize chain control points on first tick, in case of projectile hooking in on first tick
			{
				_chainMidA = Projectile.Center;
				_chainMidB = Projectile.Center;
			}
			Owner.itemAnimation = 2;
			Owner.itemTime = 2;
			Projectile.timeLeft = 2;

			switch (AiState)
			{
				case STATE_THROWOUT:
					ThrowOutAI();
					break;
				case STATE_HOOKED:
					HookedAI();
					break;
				case STATE_HOOKRETURN:
					HookReturnAI();
					break;
			}

			Owner.itemRotation = MathHelper.WrapAngle(Owner.AngleTo(Projectile.Center) - (Owner.direction < 0 ? MathHelper.Pi : 0));
			_flashTime = Math.Max(_flashTime - 1, 0);
		}

		private Vector2 GetSwingPosition(float progress)
		{
			//Starts at owner center, goes to peak range, then returns to owner center
			float distance = MathHelper.Clamp(SwingDistance, THROW_RANGE * 0.33f, THROW_RANGE) * MathHelper.Lerp((float)Math.Sin(progress * MathHelper.Pi), 1, 0.2f);
			distance = Math.Max(distance, Projectile.height); //Dont be too close to player

			float angleMaxDeviation = MathHelper.Pi / 2;
			float angleOffset = Owner.direction * MathHelper.Lerp(-angleMaxDeviation, angleMaxDeviation, progress); //Moves clockwise if player is facing right, counterclockwise if facing left
			return Projectile.velocity.RotatedBy(angleOffset) * distance;
		}

		private void ThrowOutAI()
		{
			Projectile.rotation = Projectile.AngleFrom(Owner.Center) + MathHelper.PiOver2;
			Vector2 position = Owner.MountedCenter;
			float progress = ++Timer / SwingTime; //How far the projectile is through its swing
			progress = EaseFunction.EaseCubicInOut.Ease(progress);

			Projectile.Center = position + GetSwingPosition(progress);
			Projectile.direction = Projectile.spriteDirection = -Owner.direction;

			if (Timer >= SwingTime - 2)
				Projectile.Kill();
		}

		private void HookedAI()
		{
			bool canStrike = hookNPC.active && !hookNPC.dontTakeDamage;

			//Return to player if player stops holding attack button, way too much distance is between the player and hooked npc, or if the hooked npc dies/becomes invulnerable
			if(!Owner.channel || Owner.DistanceSQ(hookNPC.Center) > HOOK_MAXRANGE * HOOK_MAXRANGE * 4 || !canStrike)
			{
				AiState = STATE_HOOKRETURN;
				returnPosOffset = Projectile.Center - Owner.MountedCenter;
				Timer = 0;

				if(canStrike) //If returning through stopping channelling or moving too far, do rip effects
					RipEnemy();

				Projectile.netUpdate = true;
				TrailManager.ManualTrailSpawn(Projectile);
				return;
			}


			Projectile.Center = hookNPC.Center + npcHookOffset;
			Projectile.rotation = npcHookRotation;
			_chainGravStrength = MathHelper.Lerp(_chainGravStrength, 1, 0.05f);
			if (++Timer % HOOK_HITTIME == 0) //Strike hooked enemy periodically, lowering projectile damage
			{
				hookNPC.StrikeNPC(RandomizeDamage(Projectile.damage), Projectile.knockBack, Math.Sign(Owner.DirectionTo(Projectile.Center).X));
				Projectile.damage = Math.Max((int)(Projectile.damage * 0.85f), 1);
				if (!Main.dedServ)
				{
					Vector2 projTip = Projectile.Center - Vector2.UnitY.RotatedBy(Projectile.rotation) * TextureAssets.Projectile[Projectile.type].Value.Height / 2;
					Vector2 direction = Projectile.direction * Vector2.UnitY.RotatedBy(Projectile.rotation);

					for (int i = 0; i < 6; i++)
						Dust.NewDustPerfect(projTip, DustID.Blood, -direction.RotatedByRandom(MathHelper.Pi / 7) * Main.rand.NextFloat(2f, 3f), 0, default, Main.rand.NextFloat(0.7f, 0.9f));

					for (int i = 0; i < 8; i++)
						Dust.NewDustPerfect(projTip, DustID.Blood, direction.RotatedByRandom(MathHelper.Pi / 14) * Main.rand.NextFloat(4f, 5f), 0, default, Main.rand.NextFloat(0.7f, 0.9f));
				}
			}

			//Switch player direction while hooked
			int ownerDir = Owner.direction;
			Owner.ChangeDir(Owner.DirectionTo(Projectile.Center).X > 0 ? 1 : -1);
			if (ownerDir != Owner.direction && Main.netMode != NetmodeID.SinglePlayer) //if in multiplayer and the owner's last direction is not equal to the owner's current, sync the owner
				NetMessage.SendData(MessageID.SyncPlayer, -1, -1, null, Owner.whoAmI);

			hookNPC.GetGlobalNPC<NPCs.GNPC>().summonTag += 7;
			hookNPC.GetGlobalNPC<SanguineFlayerGNPC>().IsHooked = true;

			if(HighImpact && !_hasFlashed)
			{
				_hasFlashed = true;
				_flashTime = FLASH_TIME_MAX;
			}
		}

		public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI) => behindNPCs.Add(index);

		private int RandomizeDamage(float damage) => (int)(damage * Main.rand.NextFloat(0.8f, 1.2f));

		private float Intensity
		{
			get
			{
				if (hookNPC == default(NPC))
					return 0;

				int hookDamage = hookNPC.GetGlobalNPC<SanguineFlayerGNPC>().HookDamage;
				if (hookDamage == 0)
					return 0;

				return (float)Math.Log10(hookNPC.GetGlobalNPC<SanguineFlayerGNPC>().HookDamage); //Damage modifier based on logarithmic function
			}
		}

		private void RipEnemy()
		{
			float baseDamage = Owner.HeldItem.damage * Math.Max(Intensity, 0.5f);
			if (HighImpact) //Make up for crit by lowering damage, while still being an overall damage boost after reaching the threshold
				baseDamage *= 0.75f;
			Vector2 direction = Projectile.DirectionTo(Owner.Center);

			hookNPC.StrikeNPC(RandomizeDamage(baseDamage), Projectile.knockBack, Math.Sign(Owner.DirectionTo(Projectile.Center).X), HighImpact);
			if (!hookNPC.boss) //Pull ripped enemy to player, depending on kB resist
				hookNPC.velocity += direction * hookNPC.knockBackResist * 10 * (HighImpact ? 1.5f : 1);

			//Owner.GetSpiritPlayer().Shake = HighImpact ? 18 : 6;

			if (!Main.dedServ)
			{
				for (int i = 0; i < 18; i++)
					Dust.NewDustPerfect(Projectile.Center, DustID.Blood, direction.RotatedByRandom(MathHelper.PiOver4) * Main.rand.Next(2, 10), 0, default, Main.rand.NextFloat(1.2f, 2.2f)).noGravity = true;

				if (HighImpact)
					ParticleHandler.SpawnParticle(new SanguineFlayerRip(Projectile.Center, 1.25f, (-direction).ToRotation()));
			}
		}
		
		private void HookReturnAI()
		{
			Projectile.rotation = Projectile.AngleFrom(Owner.Center) + MathHelper.PiOver2;
			Projectile.direction = Projectile.spriteDirection = -Owner.direction; 

			float progress = ++Timer / (RETURN_TIME * (1 + Projectile.extraUpdates));
			Projectile.Center = Owner.MountedCenter + returnPosOffset * (1 - progress);

			Rectangle returnHitbox = Projectile.Hitbox;
			if (returnHitbox.Contains(Owner.MountedCenter.ToPoint())) //Kill whenever the projectile's hitbox intersets the owner's center
				Projectile.Kill();
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if(AiState == STATE_THROWOUT && Owner.channel) //Hook into npc on hit
			{
				hookNPC = target;
				npcHookRotation = Projectile.rotation;
				npcHookOffset = (Projectile.Center - target.Center) * 0.4f; //Decrease offset from what it normally would be due to big hitbox
				AiState = STATE_HOOKED;
				Projectile.netUpdate = true;
				Owner.MinionAttackTargetNPC = target.whoAmI;
				Timer = 0;
			}

			if (!Main.dedServ)
			{
				Vector2 projTip = target.Center + npcHookOffset - Vector2.UnitY.RotatedBy(Projectile.rotation) * TextureAssets.Projectile[Projectile.type].Value.Height / 2;
				Vector2 direction = Projectile.direction * Vector2.UnitX.RotatedBy(Projectile.rotation);

				//Slower in opposite direction of movement
				for(int i = 0; i < 8; i++)
					Dust.NewDustPerfect(projTip, DustID.Blood, -direction.RotatedByRandom(MathHelper.Pi / 5) * Main.rand.NextFloat(2, 4), 0, default, Main.rand.NextFloat(0.8f, 1.1f));

				//Faster in direction of movement
				for (int i = 0; i < 10; i++)
					Dust.NewDustPerfect(projTip, DustID.Blood, direction.RotatedByRandom(MathHelper.Pi / 10) * Main.rand.NextFloat(3, 6), 0, default, Main.rand.NextFloat(0.8f, 1.1f));
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			if (Projectile.timeLeft > 2)
				return false;

			Texture2D projTexture = TextureAssets.Projectile[Projectile.type].Value;

			//End control point for the chain
			Vector2 projBottom = Projectile.Center + new Vector2(0, projTexture.Height / 2).RotatedBy(Projectile.rotation) * 0.75f;
			DrawChainCurve(Main.spriteBatch, projBottom, out Vector2[] chainPositions);

			//Adjust rotation to face from the last point in the bezier curve
			float newRotation = (projBottom - chainPositions[chainPositions.Length - 2]).ToRotation() + MathHelper.PiOver2;
			if (AiState == STATE_HOOKED)
				newRotation = npcHookRotation; //Use static rotation when hooked in instead

			//Draw from bottom center of texture
			Vector2 origin = new Vector2(projTexture.Width / 2, projTexture.Height);
			SpriteEffects flip = (Projectile.spriteDirection < 0) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

			lightColor = Lighting.GetColor((int)(Projectile.Center.X / 16f), (int)(Projectile.Center.Y / 16f));
			Main.spriteBatch.Draw(projTexture, projBottom - Main.screenPosition, null, lightColor, newRotation, origin, Projectile.scale, flip, 0);

			return false;
		}

		public void AdditiveCall(SpriteBatch spriteBatch)
		{
			if (AiState != STATE_HOOKED) //Skip if not hooked in
				return;

			Texture2D glow = ModContent.Request<Texture2D>(Texture + "_glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			Texture2D projTexture = TextureAssets.Projectile[Projectile.type].Value;
			Vector2 origin = new Vector2(projTexture.Width / 2, projTexture.Height);
			Vector2 originGlow = new Vector2(glow.Width / 2, glow.Height);
			Vector2 projBottom = Projectile.Center + new Vector2(0, projTexture.Height / 2).RotatedBy(Projectile.rotation) * 0.75f;
			SpriteEffects flip = (Projectile.spriteDirection < 0) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

			float Opacity = Math.Min(EaseFunction.EaseQuinticIn.Ease(Intensity / HIGH_IMPACT_THRESHOLD), 1);

			//Glowmask is drawn with an offset, due to using a larger texture
			Vector2 drawPosition = projBottom + (Vector2.UnitY.RotatedBy(Projectile.rotation) * 2) - Main.screenPosition;

			if (HighImpact) //pulse glow effect at high impact
			{
				int numToDraw = 6;
				float timer = (float)(Math.Sin(Main.GlobalTimeWrappedHourly * 2) / 2) + 0.5f;
				for (int i = 0; i < numToDraw; i++)
				{
					Vector2 offset = Vector2.UnitX.RotatedBy(npcHookRotation + (MathHelper.TwoPi * i / numToDraw)) * timer * 5;
					spriteBatch.Draw(glow, drawPosition + offset, null, Color.White * (1 - timer) * Opacity, npcHookRotation, originGlow, Projectile.scale, flip, 0);
				}
			}
			spriteBatch.Draw(glow, drawPosition, null, Color.White * Opacity, npcHookRotation, originGlow, Projectile.scale, flip, 0);

			float flashOpacity = _flashTime / (float)FLASH_TIME_MAX;
			for(int i = 0; i < 5; i++) //draw multiple times for more intense flash
				spriteBatch.Draw(projTexture, projBottom - Main.screenPosition, null, new Color(255, 0, 51) * flashOpacity, npcHookRotation, origin, Projectile.scale, flip, 0);
		}

		//Control points for drawing chain bezier, update slowly when hooked in
		private Vector2 _chainMidA;
		private Vector2 _chainMidB;
		private void DrawChainCurve(SpriteBatch spriteBatch, Vector2 projBottom, out Vector2[] chainPositions)
		{
			Texture2D chainTex = ModContent.Request<Texture2D>(Texture + "_chain", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

			switch (AiState)
			{
				//Curve in direction of swing movement when swinging
				case STATE_THROWOUT:
					float progress = Timer / SwingTime;
					progress = EaseFunction.EaseCubicInOut.Ease(progress);
					float angleMaxDeviation = MathHelper.Pi * 0.5f;
					float angleOffset = Owner.direction * MathHelper.Lerp(angleMaxDeviation, -angleMaxDeviation/4, progress);

					_chainMidA = Owner.MountedCenter + GetSwingPosition(progress).RotatedBy(angleOffset) * 0.33f;
					_chainMidB = Owner.MountedCenter + GetSwingPosition(progress).RotatedBy(angleOffset / 2) * 0.66f;
					break;

				//Cubic bezier when hooked into enemy, with some fluctuation for gravity like effect
				case STATE_HOOKED:
					Vector2 directionUnit = -Vector2.UnitX.RotatedBy(Projectile.rotation - MathHelper.PiOver2); //Direction opposite to direction hook is facing
					Vector2 playerDirUnit = -directionUnit.RotatedBy(-(Projectile.rotation - Owner.AngleTo(Projectile.Center) - MathHelper.PiOver2));
					float dist = Projectile.Distance(Owner.MountedCenter) * 0.33f;

					Vector2 gravityOffset = Vector2.UnitY * (dist / 6) * ((float)(Math.Sin(Main.GlobalTimeWrappedHourly) / 2) + 0.5f); //Make curve move up/down slowly to look less static
					Vector2 controlPlayer = Owner.MountedCenter + playerDirUnit * dist;
					Vector2 controlProj = projBottom + directionUnit * dist;

					//Add the fluctuating gravity offset to the lower of the control points, based on their vertical distance
					float gravityScale = Math.Min(Math.Abs(controlPlayer.Y - controlProj.Y) * 100, 1) * _chainGravStrength;
					if (controlPlayer.Y > controlProj.Y)
						controlPlayer += gravityOffset * gravityScale;
					else
						controlProj += gravityOffset * gravityScale;

					float lerpRate = (Main.gamePaused ? 0f : .025f);
					_chainMidA = Vector2.Lerp(_chainMidA, controlPlayer, lerpRate);
					_chainMidB = Vector2.Lerp(_chainMidB, controlProj, lerpRate);
					break;

				//Straight line when returning
				default:
					_chainMidA = Vector2.Lerp(Owner.MountedCenter, projBottom, 0.25f);
					_chainMidB = Vector2.Lerp(Owner.MountedCenter, projBottom, 0.75f);
					break;
			}
			BezierCurve curve = new BezierCurve(new Vector2[] { Owner.MountedCenter, _chainMidA, _chainMidB, projBottom });

			int numPoints = 32; //Should make dynamic based on curve length, but I'm not sure how to smoothly do that while using a bezier curve
			chainPositions = curve.GetPoints(numPoints).ToArray();

			//Draw each chain segment, skipping the very first one, as it draws partially behind the player
			for (int i = 1; i < numPoints; i++)
			{
				Vector2 position = chainPositions[i];

				float rotation = (chainPositions[i] - chainPositions[i - 1]).ToRotation() - MathHelper.PiOver2; //Calculate rotation based on direction from last point
				float yScale = Vector2.Distance(chainPositions[i], chainPositions[i - 1]) / chainTex.Height; //Calculate how much to squash/stretch for smooth chain based on distance between points

				Vector2 scale = new Vector2(1, yScale); // Stretch/Squash chain segment
				Color chainLightColor = Lighting.GetColor((int)position.X / 16, (int)position.Y / 16); //Lighting of the position of the chain segment
				Vector2 origin = new Vector2(chainTex.Width / 2, chainTex.Height); //Draw from center bottom of texture
				spriteBatch.Draw(chainTex, position - Main.screenPosition, null, chainLightColor, rotation, origin, scale, SpriteEffects.None, 0);
			}
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(SwingTime);
			writer.Write(SwingDistance);
			writer.WriteVector2(returnPosOffset);
			writer.WriteVector2(npcHookOffset);
			writer.Write(npcHookRotation);

			if (hookNPC == default(NPC)) //Write a -1 instead if the npc isnt set
				writer.Write(-1);
			else
				writer.Write(hookNPC.whoAmI);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			SwingTime = reader.ReadInt32();
			SwingDistance = reader.ReadSingle();
			returnPosOffset = reader.ReadVector2();
			npcHookOffset = reader.ReadVector2();
			npcHookRotation = reader.ReadSingle();

			int whoAmI = reader.ReadInt32(); //Read the whoami value sent
			if (whoAmI == -1) //If its a -1, sync that the npc hasn't been set yet
				hookNPC = default;
			else
				hookNPC = Main.npc[whoAmI];
		}
	}
}