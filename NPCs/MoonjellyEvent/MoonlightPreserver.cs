using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Consumable;
using SpiritMod.Tiles.Block;
using System;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.MoonjellyEvent
{
	public class MoonlightPreserver : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Moonlight Preserver");
			Main.npcFrameCount[npc.type] = 7;
		}

		public override void SetDefaults()
		{
			npc.width = 28;
			npc.height = 80;
			npc.damage = 0;
			npc.defense = 0;
			npc.lifeMax = 5;
			npc.HitSound = SoundID.NPCHit3;
			npc.DeathSound = SoundID.NPCHit6;
			npc.value = 60f;
			Main.npcCatchable[npc.type] = true;
			npc.catchItem = (short)ModContent.ItemType<SoulOrbItem>();
			npc.knockBackResist = .45f;
			npc.aiStyle = 64;
            npc.scale = 1f;
			npc.noGravity = true;
			aiType = NPCID.Firefly;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0) {
				Gore.NewGore(npc.position, npc.velocity, 13);
				Gore.NewGore(npc.position, npc.velocity, 12);
				Gore.NewGore(npc.position, npc.velocity, 11);
			}
		}

		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += 0.15f;
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
		}
        bool chosen = false;
		public override void AI()
		{
            Lighting.AddLight((int)((npc.position.X + (float)(npc.width / 2)) / 16f), (int)((npc.position.Y + (float)(npc.height / 2)) / 16f), 0.05f, 0.05f, 0.4f);
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Microsoft.Xna.Framework.Color color9 = Lighting.GetColor((int)((double)npc.position.X + (double)npc.width * 0.5) / 16, (int)(((double)npc.position.Y + (double)npc.height * 0.5) / 16.0));

            int num356 = 20;
            float num355 = 1f;
            float num354 = 22f * npc.scale;
            float num353 = 18f * npc.scale;
            float num352 = (float)Main.itemTexture[num356].Width;
            float num351 = (float)Main.itemTexture[num356].Height;
            if (num352 > num354)
            {
                num355 *= num354 / num352;
                num352 *= num355;
                num351 *= num355;
            }
            if (num351 > num353)
            {
                num355 *= num353 / num351;
                num352 *= num355;
                num351 *= num355;
            }
            float num348 = -1f;
            float num347 = 1f;
            int num346 = npc.frame.Y / (Main.npcTexture[npc.type].Height / Main.npcFrameCount[npc.type]);
            num347 -= (float)Math.Sin(num346);
            num348 += (float)(Math.Sin(num346) * Main.rand.NextFloat(1.9f, 2.1f));
            float num349 = -1f + (float)(Math.Sin(num346) * -3f);
            float num343 = 0.2f;
            num343 -= 0.02f * (float)num346;
            Main.spriteBatch.Draw(Main.itemTexture[num356], new Vector2(npc.Center.X - Main.screenPosition.X + num348, npc.Center.Y - Main.screenPosition.Y + npc.gfxOffY + num347), null, color9, num343, new Vector2((float)(Main.itemTexture[num356].Width / 2), (float)(Main.itemTexture[num356].Height / 2)), num355, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(Main.itemTexture[num356], new Vector2(npc.Center.X + 5 - Main.screenPosition.X + num348, npc.Center.Y + 7 - Main.screenPosition.Y + npc.gfxOffY + num347), null, color9, num343, new Vector2((float)(Main.itemTexture[num356].Width / 2), (float)(Main.itemTexture[num356].Height / 2)), num355, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(Main.itemTexture[num356], new Vector2(npc.Center.X + 4 - Main.screenPosition.X + num349, npc.Center.Y - 14 - Main.screenPosition.Y + npc.gfxOffY + num347), null, color9, num343, new Vector2((float)(Main.itemTexture[num356].Width / 2), (float)(Main.itemTexture[num356].Height / 2)), num355, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(Main.itemTexture[num356], new Vector2(npc.Center.X - 8 - Main.screenPosition.X + num349, npc.Center.Y + 4 - Main.screenPosition.Y + npc.gfxOffY + num347), null, color9, num343, new Vector2((float)(Main.itemTexture[num356].Width / 2), (float)(Main.itemTexture[num356].Height / 2)), num355, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(Main.itemTexture[num356], new Vector2(npc.Center.X - 6 - Main.screenPosition.X + num348, npc.Center.Y - 9 - Main.screenPosition.Y + npc.gfxOffY + num347), null, color9, num343, new Vector2((float)(Main.itemTexture[num356].Width / 2), (float)(Main.itemTexture[num356].Height / 2)), num355, SpriteEffects.None, 0f);

            var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame,
                             drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
            return false;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            float num341 = 0f;
            float num340 = npc.height;
            float num108 = 4;
            float num107 = (float)Math.Cos((double)(Main.GlobalTime % 2.4f / 2.4f * 6.28318548f)) / 2f + 0.5f;
            float num106 = 0f;


            Texture2D texture2D6 = Main.npcTexture[npc.type];
            Vector2 vector15 = new Vector2((float)(Main.npcTexture[npc.type].Width / 2), (float)(Main.npcTexture[npc.type].Height / Main.npcFrameCount[npc.type] / 2));
            SpriteEffects spriteEffects3 = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            Vector2 vector33 = new Vector2(npc.Center.X, npc.Center.Y - 18) - Main.screenPosition + new Vector2(0, npc.gfxOffY) - npc.velocity;
            Microsoft.Xna.Framework.Color color29 = new Microsoft.Xna.Framework.Color(127 - npc.alpha, 127 - npc.alpha, 127 - npc.alpha, 0).MultiplyRGBA(Microsoft.Xna.Framework.Color.LightBlue);
            for (int num103 = 0; num103 < 4; num103++)
            {
                Microsoft.Xna.Framework.Color color28 = color29;
                color28 = npc.GetAlpha(color28);
                color28 *= 1f - num107;
                Vector2 vector29 = new Vector2(npc.Center.X, npc.Center.Y) + ((float)num103 / (float)num108 * 6.28318548f + npc.rotation + num106).ToRotationVector2() * (4f * num107 + 2f) - Main.screenPosition + new Vector2(0, npc.gfxOffY) - npc.velocity * (float)num103;
                Main.spriteBatch.Draw(mod.GetTexture("NPCs/MoonjellyEvent/MoonlightPreserver_Glow"), vector29, npc.frame, color28, npc.rotation, npc.frame.Size() / 2f, npc.scale, spriteEffects3, 0f);
            }
            GlowmaskUtils.DrawNPCGlowMask(spriteBatch, npc, mod.GetTexture("NPCs/MoonjellyEvent/MoonlightPreserver_Glow"));
        }
    }
}
