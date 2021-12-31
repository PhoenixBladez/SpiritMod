using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;

namespace SpiritMod.NPCs.Hookbat
{
    public class Hookbat : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hookbat");
            Main.npcFrameCount[npc.type] = 5;
            NPCID.Sets.TrailCacheLength[npc.type] = 2;
            NPCID.Sets.TrailingMode[npc.type] = 0;
        }

        public override void SetDefaults()
        {
            npc.width = 38;
            npc.height = 38;
            npc.damage = 10;
			npc.rarity = 2;
            npc.defense = 4;
            npc.lifeMax = 42;
            npc.knockBackResist = .53f;
            npc.noGravity = true;
            npc.value = 60f;
            npc.noTileCollide = false;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath4;

        }

        int frame;

        public override void AI()
        {
            npc.spriteDirection = npc.direction;
			Player target = Main.player[npc.target];

            npc.ai[0]++;
            if (!target.dead && npc.ai[1] < 2f)
            {
                if (npc.collideX)
                {
                    npc.velocity.X = npc.oldVelocity.X * -0.5f;
                    if (npc.direction == -1 && npc.velocity.X > 0f && npc.velocity.X < 2f)
                        npc.velocity.X = 2f;
                    if (npc.direction == 1 && npc.velocity.X < 0f && npc.velocity.X > -2f)
                        npc.velocity.X = -2f;
                }
                if (npc.collideY)
                {
                    npc.velocity.Y = npc.oldVelocity.Y * -0.5f;
                    if (npc.velocity.Y > 0f && npc.velocity.Y < 1f)
                        npc.velocity.Y = 1f;
                    if (npc.velocity.Y < 0f && npc.velocity.Y > -1f)
                        npc.velocity.Y = -1f;
                }

                npc.TargetClosest(true);

                if (npc.direction == -1 && npc.velocity.X > -5f)
                {
                    npc.velocity.X = npc.velocity.X - 0.21f;
                    if (npc.velocity.X > 5f)
                        npc.velocity.X = npc.velocity.X - 0.21f;
                    else if (npc.velocity.X > 0f)
                        npc.velocity.X = npc.velocity.X - 0.05f;

                    if (npc.velocity.X < -5f)
                        npc.velocity.X = -5f;
                }
                else if (npc.direction == 1 && npc.velocity.X < 5f)
                {
                    npc.velocity.X = npc.velocity.X + 0.21f;
                    if (npc.velocity.X < -5f)
                        npc.velocity.X = npc.velocity.X + 0.21f;
                    else if (npc.velocity.X < 0f)
                        npc.velocity.X = npc.velocity.X + 0.05f;

                    if (npc.velocity.X > 5f)
                        npc.velocity.X = 5f;
                }

                float num3225 = Math.Abs(npc.Center.X - target.Center.X);
                float num3224 = target.position.Y - (npc.height / 2f);

                if (num3225 > 50f)
                    num3224 -= 150f;

                if (npc.position.Y < num3224)
                {
                    npc.velocity.Y = npc.velocity.Y + 0.05f;
                    if (npc.velocity.Y < 0f)
                        npc.velocity.Y = npc.velocity.Y + 0.01f;
                }
                else
                {
                    npc.velocity.Y = npc.velocity.Y - 0.05f;
                    if (npc.velocity.Y > 0f)
                        npc.velocity.Y = npc.velocity.Y - 0.01f;
                }

                if (npc.velocity.Y < -4f)
                    npc.velocity.Y = -4f;
                if (npc.velocity.Y > 4f)
                    npc.velocity.Y = 3f;
            }

            Vector2 direction = Main.player[npc.target].Center - npc.Center;

            if (npc.ai[0] == 190)
            {
                npc.ai[1] = 1f;
                npc.netUpdate = true;
            }

            npc.ai[3]++;

            if (npc.ai[3] >= 6)
            {
                frame++;
                npc.ai[3] = 0;
                npc.netUpdate = true;
            }
            if (frame > 3)
                frame = 0;
            if (npc.ai[1] == 1f)
            {
                frame = 4;
				int distance = (int)Math.Sqrt((npc.Center.X - target.Center.X) * (npc.Center.X - target.Center.X) + (npc.Center.Y - target.Center.Y) * (npc.Center.Y - target.Center.Y));
				if (distance < 400)
				{
					if (npc.ai[2] == 0)
					{
						direction.Normalize();
						Main.PlaySound(SoundID.DD2_WyvernDiveDown, npc.Center);
						direction.X *= Main.rand.Next(14, 17);
						direction.Y *= Main.rand.Next(19, 27);
						npc.velocity.X = direction.X;
						npc.velocity.Y = direction.Y;
						npc.ai[2]++;
					}
					else
						npc.velocity.Y -= .0625f;
				}
				if (npc.ai[0] > 235)
                {
                    npc.ai[0] = 0f;
                    npc.ai[1] = 0f;
                    npc.ai[2] = 0f;
                    npc.netUpdate = true;
                }
            }
        }

		public override float SpawnChance(NPCSpawnInfo spawnInfo) => spawnInfo.spawnTileY < Main.rockLayer && (!Main.dayTime) && spawnInfo.player.ZoneOverworldHeight && !NPC.AnyNPCs(ModContent.NPCType<Hookbat>()) ? 0.003f : 0f;

		public override void HitEffect(int hitDirection, double damage)
        {
            for (int k = 0; k < 10; k++)
            {
                Dust.NewDust(npc.position, npc.width, npc.height, DustID.Blood, hitDirection * 2.5f, -1f, 0, default, Main.rand.NextFloat(.45f, 1.15f));
            }
            if (npc.life <= 0)
            {
				for (int i = 1; i < 4; ++i)
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Hookbat/Hookbat" + i), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Hookbat/Hookbat1"), 1f);
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if (npc.ai[1] == 1f)
            {
                Vector2 drawOrigin = new Vector2(Main.npcTexture[npc.type].Width * 0.5f, (npc.height * 0.5f));
                for (int k = 0; k < npc.oldPos.Length; k++)
                {
                    var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
                    Vector2 drawPos = npc.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, npc.gfxOffY);
                    Color color = npc.GetAlpha(lightColor) * (float)(((float)(npc.oldPos.Length - k) / (float)npc.oldPos.Length) / 2);
                    spriteBatch.Draw(Main.npcTexture[npc.type], drawPos, new Microsoft.Xna.Framework.Rectangle?(npc.frame), color, npc.rotation, drawOrigin, npc.scale, effects, 0f);
                }
            }
            return true;
        }

		public override void FindFrame(int frameHeight) => npc.frame.Y = frameHeight * frame;
	}
}