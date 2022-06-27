using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.BaseProj
{
	/// <summary>
	/// Handles logic for exploding upon death, not multi-hitting an npc, becoming hostile to players, and doing the correct amount of damage to players.
	/// </summary>
	public abstract class BaseHeldProj : ModProjectile
	{
		public Player Owner => Main.player[Projectile.owner];

		public override void AI()
		{
			Projectile.Center = Owner.MountedCenter; //attatch the projectile to the player's mounted center
			if (AutoAimCursor() && Main.LocalPlayer == Owner) //only run if the owner is the current client, as to avoid the projectile moving in the direction of the cursors of other players
			{
				Projectile.velocity = Vector2.Normalize(Vector2.Lerp(Projectile.velocity, Projectile.DirectionTo(Main.MouseWorld), CursorLerpSpeed())); //adjust projectile velocity, which is used to store its direction, towards the owner's cursor
				Projectile.netUpdate = true;

				int ownerDir = Owner.direction;
				Owner.ChangeDir(Projectile.velocity.X > 0 ? 1 : -1); //change the owner's direction based on the projectile's velocity
				if(ownerDir != Owner.direction && Main.netMode != NetmodeID.SinglePlayer) //if in multiplayer and the owner's last direction is not equal to the owner's current, sync the owner
					NetMessage.SendData(MessageID.SyncPlayer, -1, -1, null, Owner.whoAmI);
			}

			Projectile.Center += HoldoutOffset(); //add in the holdout offset after calculating the direction
			Projectile.direction = Projectile.spriteDirection = Owner.direction;
			Projectile.rotation = MathHelper.WrapAngle(Projectile.velocity.ToRotation() + (Projectile.direction < 0 ? MathHelper.Pi : 0));
			Owner.itemRotation = Projectile.rotation;
			Owner.itemAnimation = 2;
			Owner.itemTime = 2;
			Owner.heldProj = Projectile.whoAmI;
			Projectile.timeLeft = 2;
			AbstractAI();

			Projectile.Center = Owner.MountedCenter + HoldoutOffset();
		}

		/// <summary>
		/// Use for behavior of the projectile, due to baseheldprojectile's implementation of AI()
		/// </summary>
		public virtual void AbstractAI() { }

		/// <summary>
		/// Whether or not the projectile will automatically adjust its rotation towards the player's cursor.
		/// Defaults to true.
		/// </summary>
		/// <returns></returns>
		public virtual bool AutoAimCursor() => true;

		/// <summary>
		/// Added to the position of the projectile after being attatched to the player. Use to adjust how the player holds the projectile.
		/// </summary>
		/// <returns></returns>
		public virtual Vector2 HoldoutOffset() => Vector2.Zero;

		/// <summary>
		/// Run when you want the projectile to be killed if the player isn't channelling. Returns true if the projectile is killed, allowing the use of this bool to prevent further code from running.
		/// </summary>
		/// <returns></returns>
		public bool ChannelKillCheck()
		{
			if (!Owner.channel)
			{
				Projectile.Kill();
				return true;
			}
			return false;
		}

		/// <summary>
		/// The rate at which the angle of the projectile interpolates towards the direction of the cursor. Returns 1 by default, meaning the projectile spends no time interpolating.
		/// </summary>
		/// <returns></returns>
		public virtual float CursorLerpSpeed() => 1f;
	}
}