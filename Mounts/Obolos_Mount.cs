using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Buffs.Mount;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SpiritMod.Mounts
{
    public class Obolos_Mount : ModMount
    {
        public override void SetStaticDefaults()
        {
            MountData.spawnDust = 160;
            MountData.buff = ModContent.BuffType<Obolos_Buff>();
            MountData.heightBoost = 10;         
            MountData.fallDamage = 0f;
            MountData.runSpeed = 8f;
            MountData.dashSpeed = 8f;
            MountData.flightTimeMax = 320;
            MountData.fatigueMax = 320;
            MountData.jumpHeight = 10;
            MountData.acceleration = 0.4f;
            MountData.jumpSpeed = 10f;
            MountData.blockExtraJumps = true;
            MountData.totalFrames = 14;           
			MountData.usesHover = true;

            int[] array = new int[MountData.totalFrames];
            for (int l = 0; l < array.Length; l++)
                array[l] = 16;

			MountData.playerYOffsets = array;

            MountData.xOffset = -26;                    
            MountData.yOffset = -4;          
            MountData.bodyFrame = 0;          
            MountData.playerHeadOffset = 22;
            if (Main.netMode != NetmodeID.Server)
            {
                MountData.textureWidth = MountData.frontTexture.Width();
                MountData.textureHeight = MountData.frontTexture.Height();
            }
        }

        public override void UpdateEffects(Player player) //this is like mostly just decompiled vanilla flying mount code because using the default flying mount code did not work for custom animation style iirc
		{
            Lighting.AddLight(player.position, 0f, 0.5f, 1f); 
			player.gravity = 0;
			player.fallStart = (int)(player.position.Y / 16.0);
            float num1 = 0.5f;
            float acc = 0.4f;

            float yvelcap = -1f / 10000f;

			if (player.controlUp || player.controlJump)
            {
				yvelcap = -3f;
				player.velocity.Y -= acc * num1;
            }
            else if (player.controlDown)
            {
				player.velocity.Y += acc * num1;
				if (TileID.Sets.Platforms[Framing.GetTileSafely((int)(player.Center.X / 16), (int)((player.MountedCenter.Y + (player.height / 2)) / 16) + 1).TileType])
					player.position.Y += 1;

				yvelcap = 3f;
            }

            if (player.velocity.Y < yvelcap)
            {
                if (yvelcap - player.velocity.Y < acc)
					player.velocity.Y = yvelcap;
                else
					player.velocity.Y += acc * num1;
            }
            else if (player.velocity.Y > yvelcap)
            {
                if (player.velocity.Y - yvelcap < acc)
					player.velocity.Y = yvelcap;
                else
					player.velocity.Y -= acc * num1;
			}
        }

        public override bool UpdateFrame(Player mountedPlayer, int state, Vector2 velocity)
        {
            mountedPlayer.mount._frameCounter += 0.2f;

            if (Math.Abs(mountedPlayer.velocity.X) > 5)
                mountedPlayer.mount._frame = (int)(mountedPlayer.mount._frameCounter %= 8) + 6;
            else
                mountedPlayer.mount._frame = (int)(mountedPlayer.mount._frameCounter %= 6);
            return false;
        }

        public override bool Draw(List<Terraria.DataStructures.DrawData> playerDrawData, int drawType, Player drawPlayer, ref Texture2D texture, ref Texture2D glowTexture, ref Vector2 drawPosition, ref Rectangle frame, ref Color drawColor, ref Color glowColor, ref float rotation, ref SpriteEffects spriteEffects, ref Vector2 drawOrigin, ref float drawScale, float shadow)
        {
            glowTexture = Mod.Assets.Request<Texture2D>("Mounts/Obolos_Mount_Glow").Value;

            if (drawPlayer.velocity.X < 0 && drawPlayer.direction > 0)
            {
                spriteEffects = SpriteEffects.FlipHorizontally;
                drawOrigin.X -= 52;
            }
            if (drawPlayer.velocity.X > 0 && drawPlayer.direction < 0)
            {
                spriteEffects = 0;
                drawOrigin.X += 52;
            }
            return true;
        }
    }
}