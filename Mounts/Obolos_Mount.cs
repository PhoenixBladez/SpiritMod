using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
            MountData.buff = Mod.Find<ModBuff>("Obolos_Buff").Type;
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
            {
                array[l] = 16;
            }
			MountData.playerYOffsets = array;
            MountData.xOffset = -26;                    
            MountData.yOffset = -4;          
            MountData.bodyFrame = 0;          
            MountData.playerHeadOffset = 22;
            if (Main.netMode != NetmodeID.Server)
            {
                MountData.textureWidth = MountData.frontTexture.Width;
                MountData.textureHeight = MountData.frontTexture.Height;
            }
        }

        //float num6;
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

			/*if (Math.Abs(player.velocity.X) > 5) {
				bool flip = player.velocity.X < 0 && player.direction > 0;
				for (int i = 0; i < 3; i++) {
					int offset = (flip) ? -1 : 1;
					Dust dust = Dust.NewDustDirect(new Vector2(player.position.X - (46 * offset), player.position.Y + 40) - player.velocity * (i / 3), 
						player.width, 8, 20, 0.0f, 0.0f, 100, new Color(), 1.4f);
					dust.noGravity = true;
					dust.velocity *= 0.1f;
					dust.velocity += player.velocity * 0.1f;
					dust.scale = 1.1f;
					dust.fadeIn = 1.3f;
				}
			} */

			//player.velocity.Y = num6;
        }

        public override bool UpdateFrame(Player mountedPlayer, int state, Vector2 velocity)
        {
            mountedPlayer.mount._frameCounter += 0.2f;
            if (Math.Abs(mountedPlayer.velocity.X) > 5)
            {
                mountedPlayer.mount._frame = (int)(mountedPlayer.mount._frameCounter %= 8) + 6;
            }
            else
            {
                mountedPlayer.mount._frame = (int)(mountedPlayer.mount._frameCounter %= 6);
            }
            return false;
        }

        public override bool Draw(List<Terraria.DataStructures.DrawData> playerDrawData, int drawType, Player drawPlayer, ref Texture2D texture, ref Texture2D glowTexture, ref Vector2 drawPosition, ref Rectangle frame, ref Color drawColor, ref Color glowColor, ref float rotation, ref SpriteEffects spriteEffects, ref Vector2 drawOrigin, ref float drawScale, float shadow)
        {
            glowTexture = Mod.GetTexture("Mounts/Obolos_Mount_Glow");

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