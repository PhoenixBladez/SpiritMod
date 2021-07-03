using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Particles;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Equipment.AuroraSaddle
{
	public class AuroraSaddle : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Aurora Saddle");
			Tooltip.SetDefault("Summons a vibrant steed mount");
		}

		public override void SetDefaults()
		{
			item.width = 46;
			item.height = 34;
			item.useTime = 20;
			item.useAnimation = 20;
			item.useStyle = ItemUseStyleID.HoldingUp;
			item.value = Item.buyPrice(gold: 10);
			item.rare = ItemRarityID.Pink;
			item.UseSound = SoundID.Item79;
			item.noMelee = true;
			item.noUseGraphic = true;
			item.mountType = ModContent.MountType<AuroraStagMount>();
		}
	}

	internal class SaddleBuff : ModBuff
	{
		public override void SetDefaults()
		{
			Main.buffNoTimeDisplay[Type] = true;
			Main.buffNoSave[Type] = true;
			DisplayName.SetDefault("Aurora Stag Mount");
			Description.SetDefault(""); //todo: put something here
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.mount.SetMount(ModContent.MountType<AuroraStagMount>(), player);
			player.buffTime[buffIndex] = 10;
		}
	}

	public class AuroraStagMount : ModMountData 
	{
		public override void SetDefaults()
		{
			mountData.buff = ModContent.BuffType<SaddleBuff>();
			mountData.fallDamage = 0f;
			mountData.runSpeed = 9f;
			mountData.dashSpeed = 6f;
			mountData.flightTimeMax = 0;
			mountData.fatigueMax = 0;
			mountData.jumpHeight = 14;
			mountData.acceleration = 0.04f;
			mountData.jumpSpeed = 12f;
			mountData.blockExtraJumps = false;
			mountData.totalFrames = 1;
			mountData.constantJump = false;
			mountData.playerYOffsets = new int[] { 42 };
			mountData.playerHeadOffset = 56;
			mountData.bodyFrame = 3;
			mountData.heightBoost = 42;
			mountData.standingFrameCount = 1;
			if (Main.netMode != NetmodeID.Server)
			{
				mountData.textureWidth = mountData.backTexture.Width/3;
				mountData.textureHeight = mountData.backTexture.Height/10;
			}
		}

		public override void SetMount(Player player, ref bool skipDust)
		{
			for (int i = 0; i < 25; i++)
				MakeStar(Main.rand.NextFloat(0.2f, 0.6f), player.Center);

			skipDust = true;

			AuroraPlayer modplayer = player.GetModPlayer<AuroraPlayer>();

			for (int i = (modplayer.auroraoldposition.Length - 1); i >= 0; i--)
			{
				modplayer.auroraoldposition[i] = player.Center;
				modplayer.auroraoldrotation[i] = player.fullRotation;
			}
		}

		public static Color AuroraColor => Color.Lerp(new Color(85, 255, 229), new Color(28, 155, 255), Main.rand.NextFloat());
		public static void MakeStar(float scale, Vector2 center)
		{
			Color color = AuroraColor;
			color.A = (byte)Main.rand.Next(100, 150);
			StarParticle particle = new StarParticle(
				center + Main.rand.NextVector2Circular(30, 30),
				Main.rand.NextVector2Unit() * Main.rand.NextFloat(1.5f, 3f),
				color,
				scale,
				Main.rand.Next(60, 120));

			ParticleHandler.SpawnParticle(particle);
		}

		public override void Dismount(Player player, ref bool skipDust) => SetMount(player, ref skipDust);

		public override void UpdateEffects(Player player)
		{
			AuroraPlayer modplayer = player.GetModPlayer<AuroraPlayer>();

			for (int i = (modplayer.auroraoldposition.Length - 1); i > 0; i--)
			{
				modplayer.auroraoldposition[i] = modplayer.auroraoldposition[i - 1];
				modplayer.auroraoldrotation[i] = modplayer.auroraoldrotation[i - 1];
			}
			modplayer.auroraoldposition[0] = player.Center;
			modplayer.auroraoldrotation[0] = player.fullRotation;

			float velocity = Math.Abs(player.velocity.X);

			mountData.jumpHeight = (int)MathHelper.Clamp(velocity * 2, 3, 14);

			if (Main.rand.NextBool(20) && !Main.dedServ)
				MakeStar(Main.rand.NextFloat(0.1f, 0.2f), player.Center);

			if ((player.velocity.Y != 0 || player.oldVelocity.Y != 0))
			{
				int direction = (velocity == 0) ? 0 :
					(player.direction == Math.Sign(player.velocity.X)) ? 1 : -1;
				player.fullRotation = player.velocity.Y * 0.05f * player.direction * direction * mountData.jumpHeight / 14f;
				player.fullRotationOrigin = (player.Hitbox.Size() + new Vector2(0, 42)) / 2;
			}

			else
				player.fullRotation = 0;
		}

		public override bool Draw(List<DrawData> playerDrawData, int drawType, Player drawPlayer, ref Texture2D texture, ref Texture2D glowTexture, ref Vector2 drawPosition, ref Rectangle frame, ref Color drawColor, ref Color glowColor, ref float rotation, ref SpriteEffects spriteEffects, ref Vector2 drawOrigin, ref float drawScale, float shadow)
		{
			float velocity = Math.Abs(drawPlayer.velocity.X);
			AuroraPlayer modplayer = drawPlayer.GetModPlayer<AuroraPlayer>();

			//ripped from stag npc drawcode
			int frameHeight = texture.Height / 10;
			int frameWidth = texture.Width / 3;
			int frameX = 0;
			int frameY = 0;
			float drawYOffset = -18;
			bool trail = false;

			if (velocity > 6 || drawPlayer.velocity.Y != 0 || drawPlayer.oldVelocity.Y != 0)
			{
				frameX = frameWidth * 2 - 20;
				frameY = (frameHeight + 6) * (int)(Main.GameUpdateCount / 6 % 6) - 2;
				frameHeight += 6;
				frameWidth += 32;

				if (frameY > 6 * frameHeight)
					frameY = 0;

				if (drawPlayer.velocity.Y != 0 || drawPlayer.oldVelocity.Y != 0)
					frameY = (4 * frameHeight) - 2;

				drawYOffset = -24;
				trail = true;
			}
			else if (velocity > 0)
			{
				frameX = frameWidth - 12;
				frameY = (frameHeight + 1) * (int)((Main.GameUpdateCount / 8) % 10);

				drawYOffset = -20;
			}

			Rectangle sourceRectangle = new Rectangle(frameX, frameY, frameWidth - 12, frameHeight);

			void AddDrawDataWithMountShader(DrawData data)
			{
				if (!drawPlayer.miscDyes[3].active || drawPlayer.miscDyes[3] == null)
				{
					playerDrawData.Add(data);
					return;
				}

				data.shader = GameShaders.Armor.GetShaderIdFromItemId(drawPlayer.miscDyes[3].type);
				playerDrawData.Add(data);
			}

			if (trail)
			{
				for (int i = 0; i < modplayer.auroraoldposition.Length; i++)
				{
					float opacity = (float)Math.Pow(0.5f * ((modplayer.auroraoldposition.Length - i) / (float)modplayer.auroraoldposition.Length), 3);
					Vector2 DrawPos = modplayer.auroraoldposition[i] - Main.screenPosition + new Vector2(0, drawYOffset);
					AddDrawDataWithMountShader(new DrawData(texture, DrawPos, sourceRectangle, drawColor * opacity, rotation, sourceRectangle.Size() / 2, drawScale, 1 - spriteEffects, 0));

					AddDrawDataWithMountShader(new DrawData(mod.GetTexture("Items/Equipment/AuroraSaddle/AuroraStagMount_Glow"), DrawPos, sourceRectangle, Color.White * opacity, rotation, sourceRectangle.Size() / 2, drawScale, 1 - spriteEffects, 0));

				}
			}
			AddDrawDataWithMountShader(new DrawData(texture, drawPosition + new Vector2(0, drawYOffset), sourceRectangle, drawColor, rotation, sourceRectangle.Size() / 2, drawScale, 1 - spriteEffects, 0));
			
			for (int i = 0; i < 6; i++)
			{
				float glowtimer = (float)(Math.Sin(Main.GlobalTime * 3) / 2 + 0.5f);
				Color glowcolor = Color.White * glowtimer;
				Vector2 pulsedrawpos = drawPosition + new Vector2(0, drawYOffset) + new Vector2(5, 0).RotatedBy(i * MathHelper.TwoPi / 6) * (1.25f - glowtimer);
				AddDrawDataWithMountShader(new DrawData(mod.GetTexture("Items/Equipment/AuroraSaddle/AuroraStagMount_Glow"), pulsedrawpos, sourceRectangle, glowcolor * 0.5f, rotation, sourceRectangle.Size() / 2, drawScale, 1 - spriteEffects, 0));
			}
			AddDrawDataWithMountShader(new DrawData(mod.GetTexture("Items/Equipment/AuroraSaddle/AuroraStagMount_Glow"), drawPosition + new Vector2(0, drawYOffset), sourceRectangle, Color.White, rotation, sourceRectangle.Size() / 2, drawScale, 1 - spriteEffects, 0));
			return false;
		}
	}

	public class AuroraPlayer : ModPlayer
	{
		private static readonly int length = 10;
		public Vector2[] auroraoldposition = new Vector2[length];
		public float[] auroraoldrotation = new float[length];
		public override void Initialize()
		{
			auroraoldposition = new Vector2[10]; 
			auroraoldrotation = new float[10];
		}
	}
}
