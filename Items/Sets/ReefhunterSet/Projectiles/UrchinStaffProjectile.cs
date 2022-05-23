using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.ReefhunterSet.Projectiles
{
	public class UrchinStaffProjectile : ModProjectile
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Urchin Staff");

		public override void SetDefaults()
		{
			projectile.width = 50;
			projectile.height = 54;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			projectile.magic = true;
			projectile.aiStyle = -1;

			drawHeldProjInFrontOfHeldItemAndArms = false;
		}

		public override bool CanDamage() => false;

		public Vector2 TargetPosition { get; set; }
		private bool HasShotUrchin
		{
			get => projectile.ai[0] == 1;
			set => projectile.ai[0] = (value ? 1 : 0);
		}

		public override void AI()
		{
			Player p = Main.player[projectile.owner];
			p.heldProj = projectile.whoAmI;

			if (p.whoAmI != Main.myPlayer) return; //mp check (hopefully)

			projectile.Center = p.Center;
			projectile.timeLeft = p.itemAnimation;

			projectile.rotation = ((1 - (p.itemAnimation / (float)p.itemAnimationMax)) * MathHelper.Pi) - MathHelper.PiOver2;
			if (p.direction == -1)
				projectile.rotation = (p.itemAnimation / (float)p.itemAnimationMax) * MathHelper.Pi - MathHelper.Pi;

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

			Vector2 pos = player.Center + new Vector2(27, -50).RotatedBy(projectile.rotation);
			Vector2 vel = Utilities.ArcVelocityHelper.GetArcVel(pos, TargetPosition, 0.2f, null, 150, 8, 25, 5f);

			if (CanShootUrchin(player, vel))
			{
				HasShotUrchin = true;

				Projectile proj = Projectile.NewProjectileDirect(pos, vel, ModContent.ProjectileType<UrchinBall>(), projectile.damage, 2f, projectile.owner);
				proj.rotation = projectile.rotation;
				proj.Center = pos;
				if (Main.netMode != NetmodeID.SinglePlayer) //Sync projectile made only on one client
					NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, proj.whoAmI);

				Main.PlaySound(SoundID.Item1, projectile.Center);
				projectile.netUpdate = true;
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
			float rotationTangent = (projectile.rotation + MathHelper.PiOver4 * player.direction) - (player.direction < 0 ? MathHelper.PiOver2 : 0);
			float maxAngleDif = 0.104f;

			bool shotAngleTooHigh = Math.Abs(shotAngle + MathHelper.PiOver2) < 1f; //true if the shot angle is too high for the projectile rotation's tangent angle to ever be close to it
			const int tooHighShotTime = 25;

			return Math.Abs(MathHelper.WrapAngle(shotAngle - rotationTangent)) <= maxAngleDif //Check if the difference between the angle of the shot and the current tangent of the projectile's rotation is small enough
				|| (shotAngleTooHigh && player.itemAnimation == tooHighShotTime); //or if the difference will never be small enough and the item animation is equal to an arbitrary value(so that it's not instant)
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D t = Main.projectileTexture[projectile.type];

			spriteBatch.Draw(t, projectile.Center - Main.screenPosition, new Rectangle(0, 56 * (int)projectile.ai[0], 50, 54), lightColor, projectile.rotation, t.Size() * new Vector2(0, 0.5f), 1f, SpriteEffects.None, 1f);
			return false;
		}

		public override void SendExtraAI(BinaryWriter writer) => writer.WriteVector2(TargetPosition);

		public override void ReceiveExtraAI(BinaryReader reader) => TargetPosition = reader.ReadVector2();
	}
}
