using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Buffs.Mount;
using SpiritMod.NPCs.Boss.FrostTroll;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Mounts.SnowMongerMount
{
	public class SnowmongerMount : ModMount
	{
		const int DashTimeMax = 20;

		int flightTime = 0;
		bool lowHover = false;
		int dashTime = 0;

		bool Dashing => dashTime > 0;

		public override void Load() => On.Terraria.Player.KeyDoubleTap += Player_KeyDoubleTap;

		private void Player_KeyDoubleTap(On.Terraria.Player.orig_KeyDoubleTap orig, Player self, int keyDir)
		{
			if ((keyDir == 2 || keyDir == 3) && self.mount.Active && self.mount.Type == ModContent.MountType<SnowmongerMount>() && dashTime <= -DashTimeMax * 3)
				Dash(self, keyDir);
			else
				orig(self, keyDir);
		}

		public override void SetStaticDefaults()
		{
			MountData.spawnDust = 160;
			MountData.buff = ModContent.BuffType<SnowmongerMountBuff>();
			MountData.heightBoost = 10;
			MountData.fallDamage = 0f;
			MountData.runSpeed = 5f;
			MountData.dashSpeed = 8f;
			MountData.flightTimeMax = 200;
			MountData.fatigueMax = 320;
			MountData.jumpHeight = 10;
			MountData.acceleration = 0.4f;
			MountData.jumpSpeed = 10f;
			MountData.blockExtraJumps = true;
			MountData.totalFrames = 14;
			MountData.usesHover = false;

			int[] array = new int[MountData.totalFrames];
			for (int l = 0; l < array.Length; l++)
				array[l] = 16;

			MountData.playerYOffsets = array;

			MountData.xOffset = 0;
			MountData.yOffset = 4;
			MountData.bodyFrame = 0;
			MountData.playerHeadOffset = 22;

			if (Main.netMode != NetmodeID.Server)
			{
				MountData.textureWidth = MountData.frontTexture.Width();
				MountData.textureHeight = MountData.frontTexture.Height();
			}
		}

		public override void UpdateEffects(Player player)
		{
			Lighting.AddLight(player.position, 0f, 0.5f, 1f);
			player.gravity = 0;
			player.fallStart = (int)(player.position.Y / 16.0);
			dashTime--;

			if (!Dashing)
			{
				if (!player.controlJump || flightTime <= 0)
					ControlFloatHeight(player);
				else
				{
					player.velocity.Y = MathHelper.Clamp(player.velocity.Y - 0.4f, -8, 4000);
					flightTime--;
				}
			}
			else
				UpdateDash(player);
		}

		private void UpdateDash(Player player)
		{
			player.velocity.X = player.direction * 15;

			if (dashTime < DashTimeMax * 0.2f)
				player.velocity.X *= 0.88f;

			if (dashTime % 5 == 0)
			{
				Vector2 vel = new Vector2(0, 12).RotatedByRandom(0.2f);
				int damage = (int)player.GetDamage(DamageClass.Summon).ApplyTo(40);
				int proj = Projectile.NewProjectile(player.GetSource_FromThis("Mount"), player.MountedCenter + new Vector2(0, 40), vel, ModContent.ProjectileType<SnowMongerBeam>(), damage, 0.5f, player.whoAmI);
				Projectile projectile = Main.projectile[proj];
				projectile.hostile = false;
				projectile.friendly = true;

				Terraria.Audio.SoundEngine.PlaySound(SoundID.Item91 with { Volume = 0.7f }, player.Center);
				player.velocity.Y -= 0.5f;
			}
		}

		private void Dash(Player player, int dir)
		{
			dashTime = DashTimeMax;
			player.direction = dir == 2 ? -1 : 1;
		}

		private void ControlFloatHeight(Player player)
		{
			const float MaxSpeed = 8;
			const float MoveSpeed = 0.4f;

			float baseHeight = 16 * 16;

			if (player.controlDown && player.releaseDown)
				lowHover = !lowHover;

			if (lowHover)
				baseHeight = 16;

			Vector2 orig = player.Center;
			int tileY = GetTileAt(orig, 0, out bool _, false) * 16;
			float gotoY = tileY - baseHeight;

			Vector2 goPos = new Vector2(player.MountedCenter.X, gotoY - 16);

			if (player.DistanceSQ(goPos) > 32 * 32)
				player.velocity.Y += MathHelper.Clamp(player.DirectionTo(goPos).Y * MoveSpeed, -MaxSpeed, MaxSpeed);
			else
			{
				player.velocity.Y *= 0.95f;
				flightTime = (int)MathHelper.Clamp(flightTime + 1, 0, MountData.flightTimeMax);
			}
		}

		private int GetTileAt(Vector2 searchPos, int xOffset, out bool liquid, bool up = false)
		{
			int tileDist = (int)(searchPos.Y / 16f);
			liquid = true;

			while (true)
			{
				tileDist += !up ? 1 : -1;

				if (tileDist < 20)
					return -1;

				Tile t = Framing.GetTileSafely((int)(searchPos.X / 16f) + xOffset, tileDist);
				if (t.HasTile && Main.tileSolid[t.TileType])
				{
					liquid = false;
					break;
				}
				else if (t.LiquidAmount > 155)
					break;
			}
			return tileDist;
		}

		public override bool UpdateFrame(Player mountedPlayer, int state, Vector2 velocity)
		{
			mountedPlayer.mount._frameCounter += 0.2f;
			if (!Dashing)
			{
				mountedPlayer.mount._frame = (int)(mountedPlayer.mount._frameCounter %= 8);

				if (mountedPlayer.mount._frame > 4)
					mountedPlayer.mount._frameCounter = 0;
			}
			else
			{
				mountedPlayer.mount._frame = (int)(mountedPlayer.mount._frameCounter %= 8) + 5;

				if (mountedPlayer.mount._frame > 8)
				{
					mountedPlayer.mount._frame = 8;
					mountedPlayer.mount._frameCounter -= 0.2f;
				}

				if (dashTime > 5 && dashTime < 10)
					mountedPlayer.mount._frame = 9;
				else if (dashTime <= 5)
					mountedPlayer.mount._frame = 10;
			}
			return false;
		}

		public override bool Draw(List<DrawData> playerDrawData, int drawType, Player drawPlayer, ref Texture2D texture, ref Texture2D glowTexture, ref Vector2 drawPosition, ref Rectangle frame, ref Color drawColor, ref Color glowColor, ref float rotation, ref SpriteEffects spriteEffects, ref Vector2 drawOrigin, ref float drawScale, float shadow)
		{
			int curFrame = drawPlayer.mount._frame;
			texture = ModContent.Request<Texture2D>("SpiritMod/Mounts/SnowMongerMount/SnowmongerMount").Value;
			Rectangle sourceRect = new Rectangle(0, curFrame * 56, 78, 56);
			SpriteEffects effect = drawPlayer.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			int frameOffX = drawPlayer.direction == 1 ? 6 : -6;

			if (curFrame > 5)
				sourceRect = new Rectangle(78, (curFrame - 5) * 56, 78, 56);

			DrawData data = new DrawData(texture, drawPosition - new Vector2(40 + frameOffX, 20), sourceRect, drawColor, 0f, drawOrigin, 1f, effect, 0);
			playerDrawData.Add(data);
			return false;
		}
	}
}
