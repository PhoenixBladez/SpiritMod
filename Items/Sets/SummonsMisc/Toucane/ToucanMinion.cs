using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Buffs.Summon;
using SpiritMod.Projectiles.BaseProj;
using SpiritMod.Utilities;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.SummonsMisc.Toucane
{
	[AutoloadMinionBuff("Toucans", "Despite all of my rage")]
	public class ToucanMinion : BaseMinion
	{
		public ToucanMinion() : base(700, 1200, new Vector2(40, 40)) { }

		public override void AbstractSetStaticDefaults()
		{
			DisplayName.SetDefault("Toucan");
			Main.projFrames[Projectile.type] = 6;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
		}

		public override void AbstractSetDefaults() => Projectile.localNPCHitCooldown = 20;

		public override bool DoAutoFrameUpdate(ref int framespersecond, ref int startframe, ref int endframe)
		{
			if(AiState == STATE_GLIDING)
			{
				Projectile.frame = 3;
				return false;
			}

			if(AiState == STATE_RESTING)
			{
				Projectile.frame = 5;
				return false;
			}

			if(_featherShotFrameTime > 0)
			{
				Projectile.frame = 4;
				return false;
			}
			endframe = 3; //only animate through first 3 frames
			framespersecond = (int)MathHelper.Lerp(6, 14, Math.Min(Projectile.velocity.Length() / 6, 1));
			return true;
		}

		public override bool MinionContactDamage() => AiState == STATE_GLIDING;

		private ref float AiState => ref Projectile.ai[0];
		private ref float AiTimer => ref Projectile.ai[1];

		private const float STATE_HOVERTORESTSPOT = 0;
		private const float STATE_RESTING = 1;
		private const float STATE_HOVERTOTARGET = 2;
		private const float STATE_FEATHERSHOOT = 3;
		private const float STATE_GLIDING = 4;

		private int _featherShotFrameTime;

		public override bool PreAI()
		{
			Projectile.rotation = Projectile.velocity.X * 0.05f;

			return true;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if(AiState == STATE_GLIDING)
			{
				SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
				Collision.HitTiles(Projectile.Center, Projectile.velocity, Projectile.width, Projectile.height);
				Projectile.Bounce(oldVelocity, 0.5f);
			}
			return false;
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			fallThrough = (AiState != STATE_RESTING);
			return true;
		}

		public override void IdleMovement(Player player)
		{
			AiTimer++;
			_featherShotFrameTime = 0;
			if (AiState != STATE_HOVERTORESTSPOT && AiState != STATE_RESTING)
			{
				AiTimer = 0;
				AiState = STATE_HOVERTORESTSPOT;
			}

			Projectile.direction = Projectile.spriteDirection = (Projectile.Center.X < player.MountedCenter.X) ? -1 : 1;
			Vector2 targetCenter = player.MountedCenter - new Vector2(50 * (IndexOfType + 1) * player.direction, 0);
			//take the base desired homing position, and try to find the lowest solid tile for the minion to rest at from it
			Point tilepos = targetCenter.ToTileCoordinates();
			int tilesfrombase = 0;
			int maxtilesfrombase = 15;
			bool canstartrest = true; //dont start resting if rest position is too far from the base desired position

			int startX = tilepos.X + ((Projectile.direction > 0) ? -1 : 0);
			while (ProjectileExtensions.CheckSolidTilesAndPlatforms(new Rectangle(startX, tilepos.Y, 1, 1))) //move up until not inside a tile
			{
				tilepos.Y--;
				if(++tilesfrombase >= maxtilesfrombase)
				{
					canstartrest = false;
					break;
				}
			}
			while(!ProjectileExtensions.CheckSolidTilesAndPlatforms(new Rectangle(startX, tilepos.Y + 1, 1, 1))) //move down until just above a tile
			{
				tilepos.Y++;
				if (++tilesfrombase >= maxtilesfrombase)
				{
					canstartrest = false;
					break;
				}
			}

			if (AiState == STATE_RESTING || AiState == STATE_HOVERTORESTSPOT && canstartrest) //if not too far, or too far but already resting, set the desired position to the lowest non solid tile
				targetCenter = tilepos.ToWorldCoordinates();

			switch (AiState)
			{
				case STATE_HOVERTORESTSPOT:
					Projectile.tileCollide = false;
					Projectile.AccelFlyingMovement(targetCenter, 0.15f, 0.1f, 15);
					//check if close enough to and above the rest spot, and not inside a tile, if so, start resting
					if (Projectile.Distance(targetCenter) < 10 && Projectile.Center.Y < targetCenter.Y && canstartrest && !ProjectileExtensions.CheckSolidTilesAndPlatforms(new Rectangle(startX, tilepos.Y, 1, 1)) && AiTimer > 30)
					{
						AiTimer = 0;
						AiState = STATE_RESTING;
					}
					break;
				case STATE_RESTING:
					Projectile.tileCollide = true;
					Projectile.velocity.X *= 0.7f;
					Projectile.velocity.Y += 0.4f;
					//if too far from the new resting position, start flying to it
					if (Projectile.Distance(targetCenter) > 150)
					{
						AiState = STATE_HOVERTORESTSPOT;
						AiTimer = 0;
						Projectile.velocity.Y = -5;
					}
					break;
			}

			if (Projectile.Distance(targetCenter) > 1800)
			{
				Projectile.Center = targetCenter;
				Projectile.netUpdate = true;
			}
		}

		public override void TargettingBehavior(Player player, NPC target)
		{
			Projectile.tileCollide = true;
			int FeatherMinRange = 200;
			int FeatherMaxRange = 600;
			int FeatherShootTime = 30;
			int FeatherShots = 3;
			float GlideStartVelocity = 8;
			float GlideMaxVelocity = 12;
			int GlideTime = 45;

			switch (AiState)
			{
				case STATE_HOVERTORESTSPOT: //start flying to the target if in an idle ai state
				case STATE_RESTING:
					AiState = STATE_HOVERTOTARGET;
					AiTimer = 0;
					break;

				case STATE_HOVERTOTARGET:
					Projectile.direction = Projectile.spriteDirection = (Projectile.Center.X < target.Center.X) ? -1 : 1;
					if (Projectile.Distance(target.Center) <= FeatherMinRange) //switch to shooting feathers if close enough to a target
					{
						AiState = STATE_FEATHERSHOOT;
						AiTimer = 0;
						Projectile.netUpdate = true;
					}

					Projectile.AccelFlyingMovement(target.Center, 0.25f, 0.1f, 12);
					break;
				case STATE_FEATHERSHOOT:
					Projectile.direction = Projectile.spriteDirection = (Projectile.Center.X < target.Center.X) ? -1 : 1;
					if (Projectile.Distance(target.Center) >= FeatherMaxRange && AiTimer <= (FeatherShootTime * (FeatherShots + 0.5f))) //fly back to target if too far, and if before the anticipation before the glide
					{
						AiState = STATE_HOVERTOTARGET;
						AiTimer = 0;
						Projectile.netUpdate = true;
						break;
					}
					
					if(AiTimer >= (FeatherShootTime * (FeatherShots + 1))) //start gliding after enough shots
					{
						Projectile.velocity = Projectile.DirectionTo(target.Center).RotatedByRandom(MathHelper.PiOver4) * GlideStartVelocity;
						AiState = STATE_GLIDING;
						AiTimer = 0;
						Projectile.netUpdate = true;
						break;
					}

					if(AiTimer % FeatherShootTime == 0) //shoot feather after given amount of time, with some recoil on the minion
					{
						if (Main.netMode != NetmodeID.Server)
							SoundEngine.PlaySound(SoundID.DD2_WyvernDiveDown.WithPitchVariance(0.3f).WithVolume(0.5f), Projectile.Center);

						Projectile.NewProjectileDirect(Projectile.Center, Projectile.DirectionTo(target.Center) * 8, ModContent.ProjectileType<ToucanFeather>(), (int)(Projectile.damage * 0.8), Projectile.knockBack, Projectile.owner);
						for (int j = 0; j < 6; j++)
						{
							Dust dust = Dust.NewDustPerfect(Projectile.Center, 90, Projectile.DirectionTo(target.Center).RotatedByRandom(MathHelper.Pi / 3) * Main.rand.NextFloat(1f, 2f), 100, default, Main.rand.NextFloat(0.15f, 0.3f));
							dust.fadeIn = 0.75f;
							dust.noGravity = true;
						}
						Projectile.velocity = -Projectile.DirectionTo(target.Center).RotatedByRandom(MathHelper.PiOver4) * 6;
						_featherShotFrameTime = FeatherShootTime / 2;
						Projectile.netUpdate = true;
					}

					if(AiTimer > (FeatherShootTime * (FeatherShots + 0.5f))) //move backwards before the glide in anticipation
						Projectile.velocity = Vector2.Lerp(Projectile.velocity, Projectile.DirectionFrom(target.Center) * 6, 0.2f);
					else //otherwise slowly move towards the target to make up for the recoil
						Projectile.velocity = Vector2.Lerp(Projectile.velocity, Projectile.DirectionTo(target.Center), 0.1f);

					break;
				case STATE_GLIDING:
					if(AiTimer >= GlideTime) //switch state after enough time has passed
					{
						AiState = (Projectile.Distance(target.Center) < FeatherMinRange) ? STATE_FEATHERSHOOT : STATE_HOVERTOTARGET; //fly to target if too far, otherwise continue firing feathers
						AiTimer = 0;
						Projectile.netUpdate = true;
						break;
					}

					if (Projectile.velocity.Length() < GlideMaxVelocity) //accelerate until reaching capped velocity
						Projectile.velocity *= 1.033f;

					//loop back around after passing the target
					Projectile.velocity = Projectile.velocity.Length() * Vector2.Normalize(Vector2.Lerp(Projectile.velocity, Projectile.DirectionTo(target.Center) * Projectile.velocity.Length(), 0.08f));

					//adjust direction and rotation to look like its flying in the direction it's moving in
					Projectile.direction = Projectile.spriteDirection = (Projectile.velocity.X > 0) ? -1 : 1;
					Projectile.rotation = Projectile.velocity.ToRotation() - ((Projectile.direction > 0) ? MathHelper.Pi : 0);
					CanRetarget = false; //retargetting to closest npc not wanted here

					break;
			}

			_featherShotFrameTime = Math.Max(_featherShotFrameTime - 1, 0);
			AiTimer++;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			if(AiState == STATE_GLIDING)
				Projectile.QuickDrawTrail(spriteBatch);

			Projectile.QuickDraw(spriteBatch);
			return false;
		}
	}
}