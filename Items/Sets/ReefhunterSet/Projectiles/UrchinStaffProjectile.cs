using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.ReefhunterSet.Projectiles
{
	public class UrchinStaffProjectile : ModProjectile
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Urchin Staff");

		public override void SetDefaults()
		{
			Projectile.width = 50;
			Projectile.height = 54;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.aiStyle = -1;

			DrawHeldProjInFrontOfHeldItemAndArms = false;
		}

		public override bool? CanDamage()/* tModPorter Suggestion: Return null instead of false */ => false;

		public Vector2 TargetPosition { get; set; }
		private bool HasShotUrchin
		{
			get => Projectile.ai[0] == 1;
			set => Projectile.ai[0] = (value ? 1 : 0);
		}

		public override void AI()
		{
			Player p = Main.player[Projectile.owner];
			p.heldProj = Projectile.whoAmI;

			if (p.whoAmI != Main.myPlayer) return; //mp check (hopefully)

			Projectile.Center = p.MountedCenter;
			Projectile.timeLeft = p.itemAnimation;

			Projectile.rotation = ((1 - (p.itemAnimation / (float)p.itemAnimationMax)) * MathHelper.Pi) - MathHelper.PiOver2;
			if (p.direction == -1)
				Projectile.rotation = (p.itemAnimation / (float)p.itemAnimationMax) * MathHelper.Pi - MathHelper.Pi;

			TryShootUrchin(p);
		}

		/// <summary>
		/// Check if a projectile can be shot every tick
		/// If so, then fire it and prevent more projectiles from being launched
		/// </summary>
		/// <param name="player"></param>
		private void TryShootUrchin(Player player)
		{
			if (HasShotUrchin) //Stop early if already shot
				return;

			Vector2 pos = player.Center + new Vector2(27, -50).RotatedBy(Projectile.rotation);
			Vector2 vel = Utilities.ArcVelocityHelper.GetArcVel(pos, TargetPosition + player.MountedCenter, 0.2f, null, 150, 8, 25, 5f);

			if (CanShootUrchin(player, vel))
			{
				HasShotUrchin = true;

				Projectile proj = Projectile.NewProjectileDirect(pos, vel, ModContent.ProjectileType<UrchinBall>(), Projectile.damage, 2f, Projectile.owner);
				proj.rotation = Projectile.rotation;
				proj.Center = pos;
				if (Main.netMode != NetmodeID.SinglePlayer) //Sync projectile made only on one client
					NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, proj.whoAmI);

				SoundEngine.PlaySound(SoundID.Item1, Projectile.Center);
				Projectile.netUpdate = true;
			}
		}

		/// <summary>
		/// Returns true when the tangent to the projectile's current rotation is almost equal to the initial angle of the shot projectile
		/// Or when the initial angle is too high and the current player item animation is at a specified value
		/// </summary>
		/// <param name="player"></param>
		/// <param name="velocity"></param>
		/// <returns></returns>
		private bool CanShootUrchin(Player player, Vector2 velocity)
		{
			float shotAngle = velocity.ToRotation();
			float rotationTangent = (Projectile.rotation + MathHelper.PiOver4 * player.direction) - (player.direction < 0 ? MathHelper.PiOver2 : 0);
			float maxAngleDif = 0.104f;

			bool shotAngleTooHigh = Math.Abs(shotAngle + MathHelper.PiOver2) < 1.1f; //true if the shot angle is too high for the projectile rotation's tangent angle to ever be close to it
			const int tooHighShotTime = 25;

			return Math.Abs(MathHelper.WrapAngle(shotAngle - rotationTangent)) <= maxAngleDif //Check if the difference between the angle of the shot and the current tangent of the projectile's rotation is small enough
				|| (shotAngleTooHigh && player.itemAnimation == tooHighShotTime); //or if the difference will never be small enough and the item animation is equal to an arbitrary value(so that it's not instant)
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D t = TextureAssets.Projectile[Projectile.type].Value;

			Main.spriteBatch.Draw(t, Projectile.Center - Main.screenPosition, new Rectangle(0, 56 * (int)Projectile.ai[0], 50, 54), lightColor, Projectile.rotation, t.Size() * new Vector2(0, 0.5f), 1f, SpriteEffects.None, 1f);
			return false;
		}

		public override void SendExtraAI(BinaryWriter writer) => writer.WriteVector2(TargetPosition);

		public override void ReceiveExtraAI(BinaryReader reader) => TargetPosition = reader.ReadVector2();
	}
}
