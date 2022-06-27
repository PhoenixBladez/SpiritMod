using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
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
using SpiritMod.Prim;
using Terraria.DataStructures;

namespace SpiritMod.Items.Sets.OlympiumSet.BetrayersChains
{
	public class BetrayersChains : ModItem
	{
		int combo;

		public override void SetStaticDefaults() => DisplayName.SetDefault("Blades of Chaos");

		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 16;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.useTime = Item.useAnimation = 28;
			Item.shootSpeed = 1f;
			Item.knockBack = 4f;
			Item.UseSound = SoundID.Item116;
			Item.shoot = ModContent.ProjectileType<BetrayersChainsProj>();
			Item.value = Item.sellPrice(gold: 2);
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.channel = true;
			Item.autoReuse = true;
			Item.DamageType = DamageClass.Melee;
			Item.damage = 45;
			Item.rare = ItemRarityID.LightRed;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			combo++;

			float distanceMult = Main.rand.NextFloat(0.8f, 1.2f);
			float curvatureMult = 0.7f;

			bool slam = combo % 3 == 2;

			Vector2 direction = velocity.RotatedBy(Main.rand.NextFloat(-0.2f, 0.2f));
			Projectile proj = Projectile.NewProjectileDirect(source, position, direction, type, damage, knockback, player.whoAmI);
			if (proj.ModProjectile is BetrayersChainsProj modProj)
			{
				modProj.SwingTime = (int)(Item.useTime * UseTimeMultiplier(player) * (slam ? 1.8f : 1));
				modProj.SwingDistance = player.Distance(Main.MouseWorld) * distanceMult;
				modProj.Curvature = 0.33f * curvatureMult;
				modProj.Flip = combo % 2 == 1;
				modProj.Slam = slam;
				modProj.PreSlam = combo % 3 == 1;
			}

			if (slam)
			{
				Projectile proj2 = Projectile.NewProjectileDirect(source, position, direction, type, damage, knockback, player.whoAmI);
				if (proj2.ModProjectile is BetrayersChainsProj modProj2)
				{
					modProj2.SwingTime = (int)(Item.useTime * UseTimeMultiplier(player) * 1.8f);
					modProj2.SwingDistance = player.Distance(Main.MouseWorld) * distanceMult;
					modProj2.Curvature = 0.33f * curvatureMult;
					modProj2.Flip = combo % 2 == 0;
					modProj2.Slam = slam;
				}
			}

			if (Main.netMode != NetmodeID.SinglePlayer)
				NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, proj.whoAmI);
			return false;
		}

		public override float UseTimeMultiplier(Player player) => player.GetAttackSpeed(DamageClass.Melee); //Scale with melee speed buffs, like whips
	}

	public class BetrayersChainsProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blades of Chaos");

			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			Projectile.friendly = true;
			Projectile.Size = new Vector2(85, 85);
			Projectile.tileCollide = false;
			Projectile.ownerHitCheck = true;
			Projectile.ignoreWater = true;
			Projectile.penetrate = -1;
			Projectile.usesLocalNPCImmunity = true;
		}

		private Player Owner => Main.player[Projectile.owner];

		public int SwingTime;
		public float SwingDistance;
		public float Curvature;

		public ref float Timer => ref Projectile.ai[0];
		public ref float AiState => ref Projectile.ai[1];

		private Vector2 returnPosOffset; //The position of the projectile when it starts returning to the player from being hooked
		private Vector2 npcHookOffset = Vector2.Zero; //Used to determine the offset from the hooked npc's center
		private float npcHookRotation; //Stores the projectile's rotation when hitting an npc
		private NPC hookNPC; //The npc the projectile is hooked into

		public const float THROW_RANGE = 250; //Peak distance from player when thrown out, in pixels
		public const float HOOK_MAXRANGE = 700; //Maximum distance between owner and hooked enemies before it automatically rips out
		public const int HOOK_HITTIME = 30; //Time between damage ticks while hooked in
		public const int RETURN_TIME = 6; //Time it takes for the projectile to return to the owner after being ripped out

		private int _flashTime;

		public bool Flip = false;
		public bool Slam = false;
		public bool PreSlam = false;

		private List<Vector2> oldBase = new List<Vector2>();

		public Vector2 CurrentBase = Vector2.Zero;

		private int slamTimer = 0;

		public FireChainPrimTrail trail;
		Projectile phantomProj;

		public override void AI()
		{
			if (Projectile.timeLeft > 2) //Initialize chain control points on first tick, in case of projectile hooking in on first tick
			{
				_chainMidA = Projectile.Center;
				_chainMidB = Projectile.Center;
				CurrentBase = Owner.Center;

				if (Slam)
				{
					trail = new FireChainPrimTrail(Projectile);
					SpiritMod.primitives.CreateTrail(trail);

					//not using itrail interface for reasons that make sense but i dont feel like explaining

					phantomProj = new Projectile();
					phantomProj.Size = Projectile.Size;
					phantomProj.active = true;
					phantomProj.Center = CurrentBase;
					SpiritMod.TrailManager?.CreateTrail(phantomProj, new GradientTrail(new Color(252, 73, 3) * 0.6f, new Color(255, 160, 40) * 0.3f), new RoundCap(), new DefaultTrailPosition(), 40f, 400f, default);
				}
			}
			if (Slam)
			{
				phantomProj.Center = CurrentBase;
				trail?.AddPoints();
			}
			Lighting.AddLight(CurrentBase, Color.Orange.ToVector3());
			Projectile.timeLeft = 2;

			if (Slam)
				Owner.itemTime = Owner.itemAnimation = 40;
			else if (PreSlam)
				Owner.itemTime = Owner.itemAnimation = 5;

			ThrowOutAI();

			if (!Slam)
				Owner.itemRotation = MathHelper.WrapAngle(Owner.AngleTo(Projectile.Center) - (Owner.direction < 0 ? MathHelper.Pi : 0));
			else
				Owner.itemRotation = MathHelper.WrapAngle(Owner.AngleTo(Main.MouseWorld) - (Owner.direction < 0 ? MathHelper.Pi : 0));
			_flashTime = Math.Max(_flashTime - 1, 0);
		}

		private Vector2 GetSwingPosition(float progress)
		{
			//Starts at owner center, goes to peak range, then returns to owner center
			float distance = MathHelper.Clamp(SwingDistance, THROW_RANGE * 0.1f, THROW_RANGE) * MathHelper.Lerp((float)Math.Sin(progress * MathHelper.Pi), 1, 0.04f);
			distance = Math.Max(distance, 5); //Dont be too close to player

			float angleMaxDeviation = MathHelper.Pi / 1.2f;
			float angleOffset = Owner.direction * (Flip ? -1 : 1) * MathHelper.Lerp(-angleMaxDeviation, angleMaxDeviation, progress); //Moves clockwise if player is facing right, counterclockwise if facing left
			return Projectile.velocity.RotatedBy(angleOffset) * distance;
		}

		private void ThrowOutAI()
		{
			Projectile.rotation = Projectile.AngleFrom(Owner.Center);
			Vector2 position = Owner.MountedCenter;
			float progress = ++Timer / SwingTime; //How far the projectile is through its swing
			if (Slam)
			{
				slamTimer++;
				progress = EaseFunction.EaseCubicInOut.Ease(progress);
				if (progress > 0.15f && progress < 0.85f)
					Dust.NewDustPerfect(Projectile.Center + Projectile.velocity + Main.rand.NextVector2Circular(15, 15), 6, Main.rand.NextVector2Circular(2, 2), 0, default, 1.15f).noGravity = true;
			}

			if (slamTimer == 5)
				SoundEngine.PlaySound(SoundID.NPCDeath7, Projectile.Center);

			Projectile.Center = position + GetSwingPosition(progress);
			Projectile.direction = Projectile.spriteDirection = -Owner.direction * (Flip ? -1 : 1);

			if (Timer >= SwingTime + 1)
				Projectile.Kill();
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

			//Draw from bottom center of texture
			Vector2 origin = new Vector2(projTexture.Width / 2, projTexture.Height);
			SpriteEffects flip = (Projectile.spriteDirection < 0) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

			lightColor = Lighting.GetColor((int)(Projectile.Center.X / 16f), (int)(Projectile.Center.Y / 16f));

			Main.spriteBatch.Draw(projTexture, projBottom - Main.screenPosition, null, lightColor, newRotation, origin, Projectile.scale, flip, 0);

			CurrentBase = projBottom + (newRotation - 1.57f).ToRotationVector2() * (projTexture.Height / 2);

			oldBase.Add(projBottom - Main.screenPosition);

			if (oldBase.Count > 8)
				oldBase.RemoveAt(0);

			if (!Slam)
				return false;

			Texture2D whiteTexture = ModContent.Request<Texture2D>(Texture + "_White", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
			if (slamTimer < 20 && slamTimer > 5)
			{
				float progress = (slamTimer - 5) / 15f;
				float transparency = (float)Math.Pow(1 - progress, 2);
				float scale = 1 + progress;
				Main.spriteBatch.Draw(whiteTexture, projBottom - Main.screenPosition, null, Color.White * transparency, newRotation, origin, Projectile.scale * scale, flip, 0);
			}
			return false;
		}

		//Control points for drawing chain bezier, update slowly when hooked in
		private Vector2 _chainMidA;
		private Vector2 _chainMidB;
		private void DrawChainCurve(SpriteBatch spriteBatch, Vector2 projBottom, out Vector2[] chainPositions)
		{
			Texture2D chainTex = ModContent.Request<Texture2D>(Texture + "_Chain", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

			float progress = Timer / SwingTime;

			if (Slam)
				progress = EaseFunction.EaseCubicInOut.Ease(progress);

			float angleMaxDeviation = MathHelper.Pi * 0.85f;
			float angleOffset = Owner.direction * (Flip ? -1 : 1) * MathHelper.Lerp(angleMaxDeviation, -angleMaxDeviation / 4, progress);

			_chainMidA = Owner.MountedCenter + GetSwingPosition(progress).RotatedBy(angleOffset) * Curvature;
			_chainMidB = Owner.MountedCenter + GetSwingPosition(progress).RotatedBy(angleOffset / 2) * Curvature * 2.5f;

			BezierCurve curve = new BezierCurve(new Vector2[] { Owner.MountedCenter, _chainMidA, _chainMidB, projBottom });

			int numPoints = 20; //Should make dynamic based on curve length, but I'm not sure how to smoothly do that while using a bezier curve
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

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			BezierCurve curve = new BezierCurve(new Vector2[] { Owner.MountedCenter, _chainMidA, _chainMidB, Projectile.Center });

			int numPoints = 32;
			Vector2[] chainPositions = curve.GetPoints(numPoints).ToArray();
			float collisionPoint = 0;
			for (int i = 1; i < numPoints; i++)
			{
				Vector2 position = chainPositions[i];
				Vector2 previousPosition = chainPositions[i - 1];
				if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), position, previousPosition, 6, ref collisionPoint))
					return true;
			}
			return base.Colliding(projHitbox, targetHitbox);
		}

		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			if (Slam)
			{
				crit = true;
				target.AddBuff(BuffID.OnFire, 180);
			}
			if (Collision.CheckAABBvAABBCollision(target.position, target.Size, Projectile.position, Projectile.Size))
			{
				damage = (int)(damage * 1.3f);
				for (int i = 0; i < 8; i++)
				{
					Vector2 vel = Main.rand.NextFloat(6.28f).ToRotationVector2();
					vel *= Main.rand.NextFloat(2, 5);
					ImpactLine line = new ImpactLine(target.Center - (vel * 10), vel, Color.Lerp(Color.Orange, Color.Red, Main.rand.NextFloat()), new Vector2(0.25f, Main.rand.NextFloat(0.35f, 0.55f) * 1.5f), 70);
					line.TimeActive = 30;
					ParticleHandler.SpawnParticle(line);

				}
			}
		}

		public override void Kill(int timeLeft)
		{
			if (phantomProj != null)
				phantomProj.active = false;
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