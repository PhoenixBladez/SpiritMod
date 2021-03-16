using Terraria.ModLoader;
using Terraria.ID;
using Terraria;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using System;
using System.Linq;

namespace SpiritMod.Items.Equipment.ScarabExpertDrop
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
            item.width = 20;
			item.height = 30;
            item.useTime = 20;
            item.useAnimation = 20;
            item.useStyle = ItemUseStyleID.HoldingUp;
            item.value = Item.buyPrice(gold: 1);
            item.rare = ItemRarityID.Blue;
			item.UseSound = SoundID.Item80;
            item.noMelee = true;
            item.mountType = mod.MountType("ScarabMount");
			item.expert = true;
        }
	}

	class PendantBuff : ModBuff
	{
		public override void SetDefaults()
		{
			Main.buffNoTimeDisplay[Type] = true;
			Main.buffNoSave[Type] = true;
			DisplayName.SetDefault("Pillbug friend");
			Description.SetDefault("Rock and Roll");
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.mount.SetMount(mod.MountType("ScarabMount"), player);
			player.buffTime[buffIndex] = 10;
		}
	}

	class ScarabMount : ModMountData
	{
		public override void SetDefaults()
		{
			mountData.buff = ModContent.BuffType<PendantBuff>();
			mountData.fallDamage = 0.5f;
			mountData.runSpeed = 9f;
			mountData.dashSpeed = 3f;
			mountData.flightTimeMax = 0;
			mountData.fatigueMax = 0;
			mountData.jumpHeight = 1;
			mountData.acceleration = 0.04f;
			mountData.jumpSpeed = 8f;
			mountData.blockExtraJumps = false;
			mountData.totalFrames = 1;
			mountData.constantJump = false;
			mountData.playerYOffsets = new int[] { 26 };
			mountData.bodyFrame = 3;
			mountData.heightBoost = 26;
			mountData.standingFrameCount = 1;
			mountData.spawnDust = mod.DustType("SandDust");
			if (Main.netMode != NetmodeID.Server) {
				mountData.textureWidth = mountData.backTexture.Width;
				mountData.textureHeight = mountData.backTexture.Height;
			}
		}
		public override void UpdateEffects(Player player)
		{
			ScarabMountPlayer modplayer = player.GetModPlayer<ScarabMountPlayer>();
			modplayer.scarabrotation += player.velocity.X / 25;
			modplayer.scarabtimer++;
			player.buffImmune[BuffID.WindPushed] = true;

			for(int i = (modplayer.scaraboldposition.Length - 1); i > 0; i--) {
				modplayer.scaraboldposition[i] = modplayer.scaraboldposition[i - 1];
			}
			modplayer.scaraboldposition[0] = player.Center;

			if(Math.Abs(player.velocity.X) > 3 && player.velocity.Y == 0) {
				if (modplayer.scarabtimer % 20 <= 1)
					Main.PlaySound(SoundID.Roar, player.Center, 1);

				for (int j = 0; j < Math.Abs(player.velocity.X) / 3; j++) {
					Dust.NewDustPerfect(player.Center + new Vector2(0, player.height / 2) + Main.rand.NextVector2Circular(6, 6), mountData.spawnDust,
						-player.velocity.RotatedBy(MathHelper.PiOver4 * Math.Sign(player.velocity.X)).RotatedByRandom(MathHelper.Pi / 6) * Main.rand.NextFloat(0.5f, 1),
						Scale: Main.rand.NextFloat(0.9f, 1.5f));
				}
			}

			if(Math.Abs(player.velocity.X) > 6) {
				modplayer.scarabtimer++;
				var enemies = Main.npc.Where(x => x.Hitbox.Intersects(player.Hitbox) && x.CanBeChasedBy(this) && x.immune[player.whoAmI] == 0);
				foreach(NPC npc in enemies) {
					npc.StrikeNPC((int)(25 * player.meleeDamage * Main.rand.NextFloat(0.9f, 1.2f)), 1, player.direction, Main.rand.NextBool(10));
					npc.immune[player.whoAmI] = 10;
				}
			}
		}

		public override void SetMount(Player player, ref bool skipDust)
		{
			ScarabMountPlayer modplayer = player.GetModPlayer<ScarabMountPlayer>();
			modplayer.scarabrotation = 0;
			modplayer.scarabtimer = 0;
			for (int i = 0; i < modplayer.scaraboldposition.Length; i++) {
				modplayer.scaraboldposition[i] = player.Center;
			}
		}

		public override bool Draw(List<DrawData> playerDrawData, int drawType, Player drawPlayer, ref Texture2D texture, ref Texture2D glowTexture, ref Vector2 drawPosition, ref Rectangle frame, ref Color drawColor, ref Color glowColor, ref float rotation, ref SpriteEffects spriteEffects, ref Vector2 drawOrigin, ref float drawScale, float shadow)
		{
			ScarabMountPlayer modplayer = drawPlayer.GetModPlayer<ScarabMountPlayer>();
			rotation = modplayer.scarabrotation;
			glowTexture = mod.GetTexture("Items/Equipment/ScarabExpertDrop/ScarabMount_Glow");
			Vector2 heightoffset = new Vector2(0, mountData.heightBoost - 16);
			drawPosition += heightoffset;
			if (Math.Abs(drawPlayer.velocity.X) > 6) {
				for (int i = 0; i < modplayer.scaraboldposition.Length; i++) {
					float opacity = (modplayer.scaraboldposition.Length - i) / (float)modplayer.scaraboldposition.Length;
					DrawData drawdata = new DrawData(texture, modplayer.scaraboldposition[i] - Main.screenPosition + heightoffset, null, drawColor * 0.2f * opacity, rotation, texture.Size() / 2, 1, SpriteEffects.None, 0);
					playerDrawData.Add(drawdata);

					DrawData drawdataglow = new DrawData(glowTexture, modplayer.scaraboldposition[i] - Main.screenPosition + heightoffset, null, Color.White * 0.2f * opacity, rotation, texture.Size() / 2, 1, SpriteEffects.None, 0);
					playerDrawData.Add(drawdataglow);
				}
			}
			return true;
		}
	}

	class ScarabMountPlayer : ModPlayer
	{
		public float scarabrotation;
		public int scarabtimer;
		public Vector2[] scaraboldposition = new Vector2[5];
		public override void Initialize()
		{
			scarabrotation = 0;
			scarabtimer = 0;
			scaraboldposition = new Vector2[5];
		}

		public override bool CanBeHitByNPC(NPC npc, ref int cooldownSlot)
		{
			if (player.mount.Type == mod.MountType("ScarabMount") && Math.Abs(player.velocity.X) > 6)
				return false;

			return base.CanBeHitByNPC(npc, ref cooldownSlot);
		}
	}
}