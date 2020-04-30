using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Dungeon
{
	public class BlazingRattler : ModNPC
	{
		bool attack = false;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blazing Rattler");
			Main.npcFrameCount[npc.type] = 4;
		}

		public override void SetDefaults()
		{
			npc.width = 58;
			npc.height = 46;
			npc.damage = 30;
			npc.defense = 16;
			npc.lifeMax = 300;
			npc.HitSound = SoundID.NPCHit2;
			npc.DeathSound = SoundID.NPCDeath2;
			npc.value = 1000f;
			npc.knockBackResist = .35f;
            npc.aiStyle = 3;
            aiType = 218;
        }
        int hitCounter;
        public override void HitEffect(int hitDirection, double damage)
		{
            npc.scale -= .02f;
            int d = 0;
            int d1 = 6;
            for (int k = 0; k < 30; k++)
            {
                Dust.NewDust(npc.position, npc.width, npc.height, d, 2.5f * hitDirection, -2.5f, 0, Color.White, 0.7f);
                Dust.NewDust(npc.position, npc.width, npc.height, d1, 2.5f * hitDirection, -2.5f, 0, default(Color), .34f);
            }
            hitCounter++;
            if (hitCounter >= 3)
            {
                hitCounter = 0;
                Vector2 dir = Main.player[npc.target].Center - npc.Center;
                dir.Normalize();
                dir.X *= 4f;
                dir.Y *= 4f;
                bool expertMode = Main.expertMode;
                for (int i = 0; i < 1; ++i)
                {
                    float A = (float)Main.rand.Next(-200, 200) * 0.01f;
                    float B = (float)Main.rand.Next(-200, 200) * 0.01f;
                    int damagenumber = expertMode ? 12 : 17;
                    int p = Projectile.NewProjectile(npc.Center.X, npc.Center.Y, dir.X + A, dir.Y + B, 15, damagenumber, 1, Main.myPlayer, 0, 0);
                    Main.projectile[p].friendly = false;
                    Main.projectile[p].hostile = true;
                }
            }
            if (npc.life <= 0)
            {
                Vector2 dir = Main.player[npc.target].Center;
                dir.Normalize();
                dir.X *= Main.rand.NextFloat(-12f, 12f);
                dir.Y *= Main.rand.NextFloat(-12f, 12f);
                bool expertMode = Main.expertMode;
                for (int i = 0; i < 3; ++i)
                {
                    float A = (float)Main.rand.Next(-200, 200) * 0.01f;
                    float B = (float)Main.rand.Next(-200, 200) * 0.01f;
                    int damagenumber = expertMode ? 12 : 17;
                    int p = Projectile.NewProjectile(npc.Center.X, npc.Center.Y, dir.X + A, dir.Y + B, 15, damagenumber, 1, Main.myPlayer, 0, 0);
                    Main.projectile[p].friendly = false;
                    Main.projectile[p].hostile = true;
                }
            }
        }
		int frame = 0;
		int timer = 0;
      //  int shootTimer = 0;
		public override void AI()
		{
			npc.spriteDirection = npc.direction;
            if (npc.scale <= .85f)
            {
                npc.scale = .85f;
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame,
                             drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
            return false;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            GlowmaskUtils.DrawNPCGlowMask(spriteBatch, npc, mod.GetTexture("NPCs/Dungeon/BlazingRattler_Glow"));
        }
        public override void FindFrame(int frameHeight)
		{
            npc.frameCounter += 0.15f;
            npc.frameCounter %= Main.npcFrameCount[npc.type];
            int frame = (int)npc.frameCounter;
            npc.frame.Y = frame * frameHeight;
        }
	}
}
