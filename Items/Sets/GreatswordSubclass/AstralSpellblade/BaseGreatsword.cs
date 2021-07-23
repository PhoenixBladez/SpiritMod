using Terraria;
using Microsoft.Xna.Framework;
using System;
using Terraria.ModLoader;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Particles;
using Terraria.ID;

namespace SpiritMod.Items.Sets.GreatswordSubclass.AstralSpellblade
{
    public abstract class BaseGreatsword : ModProjectile
    {
		public BaseGreatsword(float ChargeRate, float DrawBackTime = 0)
		{
			this.ChargeRate = ChargeRate;
			this.DrawBackTime = DrawBackTime;
		}

		public override void SetDefaults()
		{
			projectile.Size = new Vector2(10, 10);
			projectile.friendly = true;
			projectile.melee = true;
			projectile.ownerHitCheck = true;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			projectile.penetrate = -1;
			projectile.aiStyle = -1;

			SafeSetDefaults();
		}

		/// <summary>
		/// Called after setdefaults, allowing default properties to be overrided.
		/// </summary>
		internal virtual void SafeSetDefaults() { }

		public override bool CanDamage() => AiState != State_Charging;

		internal ref float AiState => ref projectile.ai[0];
		internal ref float Combo => ref projectile.ai[1];
		internal ref float AiTimer => ref projectile.localAI[0];
		internal ref float Charge => ref projectile.localAI[1];

		internal const float State_Charging = 0;
		internal const float State_Released = 1;

		internal float ChargeRate = 1 / 90f;
		internal float DrawBackTime = 20f;

		internal Player ProjOwner => Main.player[projectile.owner];
		public override void AI()
		{
			if (!ProjOwner.active || ProjOwner.dead || ProjOwner.frozen)
			{
				projectile.Kill();
				return;
			}

			Vector2 ownerMountedCenter = ProjOwner.RotatedRelativePoint(ProjOwner.MountedCenter, true);
			ProjOwner.heldProj = projectile.whoAmI;
			ProjOwner.itemTime = 2;
			ProjOwner.itemAnimation = 2;

			if(AiState == State_Charging)
			{
				++AiTimer;
				if (!ProjOwner.channel && AiTimer > DrawBackTime)
				{
					AiTimer = 0;
					Charge = (Charge >= 1) ? 1 : 0;
					AiState = State_Released;

					OnSwing(ownerMountedCenter);
				}
				else
				{
					if(AiTimer > DrawBackTime)
						Charge = Math.Min(Charge + ChargeRate, 1);

					Charging(ownerMountedCenter, AiTimer > DrawBackTime);
				}

				if(ProjOwner == Main.LocalPlayer)
				{
					bool AimCursor = true;
					Vector2 mouseDirection = ProjOwner.DirectionTo(Main.MouseWorld);
					projectile.Center = ChargeHoldout(ownerMountedCenter, mouseDirection, ref AimCursor);
					if (AimCursor)
					{
						projectile.velocity = ProjOwner.DirectionTo(Main.MouseWorld);
						ProjOwner.ChangeDir(projectile.velocity.X > 0 ? 1 : -1);
					}
					projectile.netUpdate = true;
				}
			}

			if(AiState == State_Released)
			{
				AiTimer++;
				Swinging(ownerMountedCenter);
			}

			projectile.spriteDirection = projectile.direction = ProjOwner.direction;
			projectile.rotation = projectile.AngleFrom(ownerMountedCenter) - (3 * MathHelper.PiOver4) - ((projectile.spriteDirection < 0) ? MathHelper.PiOver2 : MathHelper.Pi);
			ProjOwner.itemRotation = MathHelper.WrapAngle(ProjOwner.AngleTo(projectile.Center) - ((ProjOwner.direction < 0) ? MathHelper.Pi : 0));

			SafeAI(ownerMountedCenter);
		}

		/// <summary>
		/// Called every tick after default ai
		/// </summary>
		/// <param name="ownerPos"></param>
		public virtual void SafeAI(Vector2 ownerPos) { }

		/// <summary>
		/// Allows you to have special effects when the player is channelling.
		/// </summary>
		/// <param name="ownerPos"></param>
		/// <param name="SwingReady"></param>
		public virtual void Charging(Vector2 ownerPos, bool SwingReady) { }

		/// <summary>
		/// Determines the position of the projectile while being charged. Set AutoAimCursor to false to disallow automatically updating the projectile's velocity to be towards the cursor.
		/// Runs only on the client of the projectile's owner, netupdate is used to sync
		/// </summary>
		/// <param name="ownerPos"></param>
		/// <param name="ownerMouseDirection"></param>
		/// <param name="AutoAimCursor"></param>
		/// <returns></returns>
		public virtual Vector2 ChargeHoldout(Vector2 ownerPos, Vector2 ownerMouseDirection, ref bool AutoAimCursor) => Vector2.Zero;

		/// <summary>
		/// Allows for special effects the tick at which channelling is stopped and while the projectile is fully drawn back.
		/// </summary>
		/// <param name="ownerPos"></param>
		public virtual void OnSwing(Vector2 ownerPos) { }

		/// <summary>
		/// Used for the behavior of the projectile after the swing starts, and for killing the projectile
		/// </summary>
		/// <param name="ownerPos"></param>
		public virtual void Swinging(Vector2 ownerPos) { }

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(AiTimer);
			writer.Write(Charge);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			AiTimer = reader.ReadSingle();
			Charge = reader.ReadSingle();
		}
	}
}