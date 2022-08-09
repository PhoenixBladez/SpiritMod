using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Tiles.Ambient;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.StarplateDrops.StarplatePet
{
	public class StarplatePetProjectile : ModProjectile
	{
		const int INIT = 0;
		const int HEAD = 1;
		const int BODY_VAR1 = 2;
		const int BODY_VAR2 = 3;
		const int TAIL = 4;

		private Player Owner => Main.player[Projectile.owner];
		private ref float State => ref Projectile.ai[0];

		/// <summary>This is the whoAmI of the first body segment for the head, and the segment ahead of the current for everything else.</summary>
		private ref float AttachedWhoAmI => ref Projectile.ai[1];
		private Projectile AttachedProjectile => Main.projectile[(int)AttachedWhoAmI];

		private bool flipDir = true;
		private float spinner = 0;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Starplate Miniature");
			Main.projFrames[Projectile.type] = 2;
			Main.projPet[Projectile.type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.Truffle);
			Projectile.aiStyle = 0;
			Projectile.width = 24;
			Projectile.height = 22;
			Projectile.light = 0;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;

			AIType = 0;
		}

		public override void AI()
		{
			var modPlayer = Main.player[Projectile.owner].GetModPlayer<GlobalClasses.Players.PetPlayer>();

			if (Main.player[Projectile.owner].dead)
				modPlayer.starplatePet = false;

			if (modPlayer.starplatePet)
				Projectile.timeLeft = 2;

			if (State < BODY_VAR1)
			{
				if (State == INIT)
					InitializeWorm();
				else if (State == HEAD)
					HeadMovement();
			}
			else
				BodyMovement();

			Projectile.spriteDirection = -1;
		}

		private void InitializeWorm()
		{
			int Length = Main.rand.Next(10, 14);

			int lastWhoAmI = 0;
			for (int i = 0; i < Length; ++i)
			{
				int proj = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.position, Vector2.Zero, Projectile.type, 0, 0, Projectile.owner);

				Projectile newProj = Main.projectile[proj];
				StarplatePetProjectile petProj = newProj.ModProjectile as StarplatePetProjectile;

				newProj.frame = Main.rand.Next(16);
				newProj.frameCounter = Main.rand.Next(6);

				if (i == 0)
				{
					AttachedWhoAmI = proj;
					petProj.AttachedWhoAmI = Projectile.whoAmI;
				}
				else
					petProj.AttachedWhoAmI = lastWhoAmI;

				petProj.flipDir = Main.rand.NextBool(2);
				petProj.State = i % 2 == 0 ? BODY_VAR1 : BODY_VAR2;
				if (i == Length - 1)
					petProj.State = TAIL;

				lastWhoAmI = proj;
			}

			State = HEAD;
		}

		private void HeadMovement()
		{
			float maxSpeed = 12;

			spinner += 0.02f * Owner.direction;

			Vector2 targetPosition = Owner.Center;
			CheckForOre(ref targetPosition);
			float projDist = Projectile.DistanceSQ(targetPosition);
			float ownerDist = Projectile.DistanceSQ(Owner.Center);

			if (projDist <= 200 * 200 && ownerDist < 900 * 900)
			{
				targetPosition += new Vector2(120, 0).RotatedBy(spinner % MathHelper.TwoPi);
				maxSpeed = Projectile.Distance(targetPosition) - 10;
				if (maxSpeed > 12)
					maxSpeed = 12;
			}
			else if (ownerDist > 900 * 900)
				targetPosition = Owner.Center;

			Projectile.velocity += Projectile.DirectionTo(targetPosition - Projectile.velocity) * 0.3f;

			if (Projectile.velocity.LengthSquared() > maxSpeed * maxSpeed)
				Projectile.velocity = Vector2.Normalize(Projectile.velocity) * maxSpeed;

			Projectile.rotation = Projectile.velocity.ToRotation();
		}

		private bool CheckForOre(ref Vector2 targetPosition)
		{
			for (int i = -20; i < 20; ++i)
			{
				for (int j = -20; j < 20; ++j)
				{
					Point projPos = Projectile.Center.ToTileCoordinates();
					Point pos = new Point(projPos.X + i, projPos.Y + j);

					if (!WorldGen.InWorld(pos.X, pos.Y))
						continue;

					Tile tile = Main.tile[pos.X, pos.Y];

					if (tile.HasTile && tile.TileType == ModContent.TileType<StarBeacon>())
					{
						targetPosition = pos.ToWorldCoordinates();
						return true;
					}
				}
			}
			return false;
		}

		private void BodyMovement()
		{
			float BodyLength = State == TAIL ? 16 : 18;

			if (AttachedProjectile.DistanceSQ(Projectile.Center) > BodyLength * BodyLength)
				Projectile.velocity = Projectile.DirectionTo(AttachedProjectile.Center) * (AttachedProjectile.Distance(Projectile.Center) - BodyLength);
			else
				Projectile.velocity = Vector2.Zero;

			Projectile.rotation = Projectile.AngleTo(AttachedProjectile.Center);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			const int FRAME_WIDTH = 24;
			const int FRAME_HEIGHT = 24;

			Texture2D tex = ModContent.Request<Texture2D>(Texture).Value;
			SpriteEffects effect = Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			int offset = 36 * Projectile.frame;
			Rectangle rect = new Rectangle(0, 6 + offset, FRAME_WIDTH, FRAME_HEIGHT);

			if (State >= BODY_VAR1 && State != TAIL)
				rect = new Rectangle(State == BODY_VAR1 ? 26 : 48, 6 + offset, FRAME_WIDTH, FRAME_HEIGHT);
			else if (State == TAIL)
				rect = new Rectangle(70, 6 + offset, FRAME_WIDTH - 8, FRAME_HEIGHT);

			Projectile.frameCounter++;
			if (Projectile.frameCounter > 6)
			{
				Projectile.frameCounter = 0;

				if (flipDir)
				{
					if (++Projectile.frame > 15)
						Projectile.frame = 0;
				}
				else
				{
					if (--Projectile.frame < 0)
						Projectile.frame = 15;
				}
			}

			Main.EntitySpriteDraw(tex, Projectile.position - Main.screenPosition, rect, lightColor, Projectile.rotation, rect.Size() / 2f, 1f, effect, 0);
			return false;
		}
	}
}
