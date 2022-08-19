using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.BossLoot.ScarabeusDrops.ScarabExpertDrop
{
	public class ScarabPendant : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Scarab Pendant");
			Tooltip.SetDefault("Summons a rideable giant pillbug that rolls through enemies at high speed\nRolling through an enemy protects against contact damage");
		}

		public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 30;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.value = Item.buyPrice(gold: 1);
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item80;
			Item.noMelee = true;
			Item.mountType = Mod.Find<ModMount>("ScarabMount").Type;
			Item.expert = true;
		}
	}

	internal class PendantBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			Main.buffNoTimeDisplay[Type] = true;
			Main.buffNoSave[Type] = true;
			DisplayName.SetDefault("Pillbug Friend");
			Description.SetDefault("'Rock and roll!'");
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.mount.SetMount(Mod.Find<ModMount>("ScarabMount").Type, player);
			player.buffTime[buffIndex] = 10;
		}
	}

	internal class ScarabMount : ModMount
	{
		public override void SetStaticDefaults()
		{
			MountData.buff = ModContent.BuffType<PendantBuff>();
			MountData.fallDamage = 0f;
			MountData.runSpeed = 9f;
			MountData.dashSpeed = 3f;
			MountData.flightTimeMax = 0;
			MountData.fatigueMax = 0;
			MountData.jumpHeight = 1;
			MountData.acceleration = 0.04f;
			MountData.jumpSpeed = 8f;
			MountData.blockExtraJumps = false;
			MountData.totalFrames = 1;
			MountData.constantJump = false;
			MountData.playerYOffsets = new int[] { 26 };
			MountData.playerHeadOffset = 30;
			MountData.bodyFrame = 3;
			MountData.heightBoost = 26;
			MountData.standingFrameCount = 1;
			MountData.spawnDust = Mod.Find<ModDust>("SandDust").Type;
			if (Main.netMode != NetmodeID.Server)
			{
				MountData.textureWidth = MountData.backTexture.Width();
				MountData.textureHeight = MountData.backTexture.Height();
			}
		}

		public override void UpdateEffects(Player player)
		{
			ScarabMountPlayer modplayer = player.GetModPlayer<ScarabMountPlayer>();
			modplayer.scarabRotation += player.velocity.X / 25;
			modplayer.scarabTimer++;
			player.buffImmune[BuffID.WindPushed] = true;

			for (int i = (modplayer.scarabOldPosition.Length - 1); i > 0; i--)
				modplayer.scarabOldPosition[i] = modplayer.scarabOldPosition[i - 1];

			modplayer.scarabOldPosition[0] = player.Center;

			if (Math.Abs(player.velocity.X) > 3 && player.velocity.Y == 0)
			{
				if (modplayer.scarabTimer % 20 == 0)
					SoundEngine.PlaySound(SoundID.Dig with { Volume = 0.6f, PitchVariance = 0.5f }, player.Center);

				for (int j = 0; j < Math.Abs(player.velocity.X) / 3; j++)
				{
					Dust.NewDustPerfect(player.Center + new Vector2(0, player.height / 2) + Main.rand.NextVector2Circular(6, 6), MountData.spawnDust,
						-player.velocity.RotatedBy(MathHelper.PiOver4 * Math.Sign(player.velocity.X)).RotatedByRandom(MathHelper.Pi / 6) * Main.rand.NextFloat(0.5f, 1),
						Scale: Main.rand.NextFloat(0.9f, 1.5f));
				}
			}

			if (Math.Abs(player.velocity.X) > 6)
			{
				modplayer.scarabTimer++;
				var enemies = Main.npc.SkipLast(1).Where(x => x.Hitbox.Intersects(player.Hitbox) && x.CanBeChasedBy(this) && x.immune[player.whoAmI] == 0);
				foreach (NPC npc in enemies)
				{
					npc.StrikeNPC((int)(player.GetDamage(DamageClass.Melee).ApplyTo(25 * Main.rand.NextFloat(0.9f, 1.2f))), 1, player.direction, Main.rand.NextBool(10));
					npc.immune[player.whoAmI] = 10;
				}
			}
		}

		public override void SetMount(Player player, ref bool skipDust)
		{
			ScarabMountPlayer modplayer = player.GetModPlayer<ScarabMountPlayer>();
			modplayer.scarabRotation = 0;
			modplayer.scarabTimer = 0;
			for (int i = 0; i < modplayer.scarabOldPosition.Length; i++)
			{
				modplayer.scarabOldPosition[i] = player.Center;
			}
		}

		public override bool Draw(List<DrawData> playerDrawData, int drawType, Player drawPlayer, ref Texture2D texture, ref Texture2D glowTexture, ref Vector2 drawPosition, ref Rectangle frame, ref Color drawColor, ref Color glowColor, ref float rotation, ref SpriteEffects spriteEffects, ref Vector2 drawOrigin, ref float drawScale, float shadow)
		{
			ScarabMountPlayer modplayer = drawPlayer.GetModPlayer<ScarabMountPlayer>();
			rotation = modplayer.scarabRotation;
			glowTexture = Mod.Assets.Request<Texture2D>("Items/BossLoot/ScarabeusDrops/ScarabExpertDrop/ScarabMount_Glow").Value;
			Vector2 heightoffset = new Vector2(0, MountData.heightBoost - 16);
			drawPosition += heightoffset;
			if (Math.Abs(drawPlayer.velocity.X) > 6)
			{
				for (int i = 0; i < modplayer.scarabOldPosition.Length; i++)
				{
					float opacity = (modplayer.scarabOldPosition.Length - i) / (float)modplayer.scarabOldPosition.Length;
					DrawData drawdata = new DrawData(texture, modplayer.scarabOldPosition[i] - Main.screenPosition + heightoffset, null, drawColor * 0.2f * opacity, rotation, texture.Size() / 2, 1, SpriteEffects.None, 0);
					playerDrawData.Add(drawdata);

					DrawData drawdataglow = new DrawData(glowTexture, modplayer.scarabOldPosition[i] - Main.screenPosition + heightoffset, null, Color.White * 0.2f * opacity, rotation, texture.Size() / 2, 1, SpriteEffects.None, 0);
					playerDrawData.Add(drawdataglow);
				}
			}
			return true;
		}
	}

	internal class ScarabMountPlayer : ModPlayer
	{
		public float scarabRotation;
		public int scarabTimer;
		public Vector2[] scarabOldPosition = new Vector2[5];
		public override void Initialize()
		{
			scarabRotation = 0;
			scarabTimer = 0;
			scarabOldPosition = new Vector2[5];
		}

		public override bool CanBeHitByNPC(NPC npc, ref int cooldownSlot)
		{
			if (Player.mount.Type == Mod.Find<ModMount>("ScarabMount").Type && Math.Abs(Player.velocity.X) > 6)
				return false;

			return base.CanBeHitByNPC(npc, ref cooldownSlot);
		}
	}
}