using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;
using SpiritMod.Prim;
using System.Linq;

namespace SpiritMod.NPCs.StarjinxEvent.Comets
{
    public class SmallComet : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Small Starjinx Comet");
        }
        public override void SetDefaults()
        {
            npc.aiStyle = -1;
            npc.lifeMax = 10;
            npc.defense = 0;
            npc.value = 0f;
            aiType = 0;
            npc.knockBackResist = 0f;
            npc.width = 36;
            npc.height = 38;
            npc.damage = 0;
            npc.lavaImmune = false;
            npc.noTileCollide = false;
            npc.noGravity = true;
            npc.HitSound = SoundID.NPCHit7;
            npc.DeathSound = SoundID.Item89;
            npc.alpha = 255;
            for (int i = 0; i < BuffLoader.BuffCount; i++)
            {
                npc.buffImmune[i] = true;
            }
        }
        public override void ModifyHitByItem(Player player, Item item, ref int damage, ref float knockback, ref bool crit)
        {
            crit = false;
            if (player.HeldItem.pick > 0)
                damage = 5;
            else
                damage = 1;
        }
        public override void ModifyHitByProjectile(Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            Player player = Main.player[projectile.owner];
            crit = false;
            if (player.HeldItem.pick > 0)
                damage = 5;
            else
                damage = 1;
        }
        float sinCounter;
        float sinIncrement = 0;
        public override void AI()
        {
            if (npc.alpha > 0)
                npc.alpha -= 10;
            else
                npc.alpha = 0;

            npc.velocity.X = 0;
            npc.velocity.Y = (float)Math.Sin(sinCounter) * 0.75f;
            if (sinIncrement == 0)
            {
                sinIncrement = Main.rand.NextFloat(0.025f, 0.035f);
            }
            sinCounter += sinIncrement;
            Player player = Main.player[npc.target];
            npc.TargetClosest(true);
            NPC parent = Main.npc[(int)npc.ai[0]];

            if (!parent.active || parent.type != mod.NPCType("StarjinxMeteorite"))
                npc.active = false;

            if (Main.rand.Next(200) == 0)
                Gore.NewGorePerfect(npc.Center, (parent.Center - npc.Center) / 45, mod.GetGoreSlot("Gores/StarjinxGore"), 1);
        }
        public static void drawDustLine(Vector2 pos1, Vector2 pos2)
        {
            Vector2 range = pos1 - pos2;
            Vector2 cometRange = range;
            cometRange.Normalize();
            cometRange *= 40;
            range -= cometRange;
            for (int i = 0; i < 5; i++)
            {
                float alpha = Main.rand.NextFloat();
                int chosenDust = Main.rand.Next(2) == 0 ? 159 : 164;
                Dust dust = Main.dust[Dust.NewDust(pos2 + (range * alpha), 0, 0, chosenDust)];
                dust.noGravity = true;
                dust.noLight = false;
                dust.velocity = range * 0.014f;
                dust.scale = 1.8f - (alpha * 1.5f);
                dust.fadeIn += dust.scale / 3 * 2;
            }
        }

        public override void NPCLoot()
        {

        }

        public override void HitEffect(int hitDirection, double damage)
        {
            for (int k = 0; k < 7; k++)
            {
                Dust.NewDust(npc.position, npc.width, npc.height, 159, 2.5f * hitDirection, -2.5f, 0, default(Color), 0.6f);
                Dust.NewDust(npc.position, npc.width, npc.height, 164, 2.5f * hitDirection, -2.5f, 0, default(Color), 1.25f);
                Dust.NewDust(npc.position, npc.width, npc.height, 159, 2.5f * hitDirection, -2.5f, 0, default(Color), 0.85f);
            }
            if (npc.life <= 0)
            {
                Main.PlaySound(SoundID.Item14, npc.position);
            }
        }

        public float Timer => Main.GlobalTime + npc.ai[1];
        public float beamscale = 0.75f;

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Vector2 center = new Vector2((float)(Main.npcTexture[npc.type].Width / 2), (float)(Main.npcTexture[npc.type].Height / Main.npcFrameCount[npc.type] / 2));
            float num341 = 0f;
            float num340 = npc.height;
            float num108 = 4;
            float num107 = (float)Math.Cos((double)(Timer % 2.4f / 2.4f * 6.28318548f)) / 2f + 0.5f;
            float num106 = 0f;

			Texture2D beam = mod.GetTexture("Textures/Medusa_Ray");
			float rotation = npc.DirectionTo(Main.npc[(int)npc.ai[0]].Center).ToRotation();
            Color color = SpiritMod.StarjinxColor(Timer * 0.8f);
            float fluctuate = (float)Math.Sin(Timer * 1.2f) / 6 + 0.125f;
            color = Color.Lerp(color, Color.Transparent, fluctuate * 2);
            Rectangle rect = new Rectangle(0, 0, beam.Width, beam.Height);
            Vector2 scale = new Vector2((1 - fluctuate) * npc.Distance(Main.npc[(int)npc.ai[0]].Center) / beam.Width, 1) * 0.75f;
            Vector2 offset = new Vector2(100 * scale.X, 0).RotatedBy(rotation);
            Main.spriteBatch.Draw(beam, npc.Center - Main.screenPosition + offset / 2, new Rectangle?(rect), npc.GetAlpha(color), rotation, rect.Size() / 2, scale, SpriteEffects.None, 0);

            Texture2D texture2D6 = Main.npcTexture[npc.type];
            Vector2 vector15 = new Vector2((float)(Main.npcTexture[npc.type].Width / 2), (float)(Main.npcTexture[npc.type].Height / Main.npcFrameCount[npc.type] / 2));
            SpriteEffects spriteEffects3 = (npc.spriteDirection == 1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Vector2 vector33 = new Vector2(npc.Center.X, npc.Center.Y) - Main.screenPosition + new Vector2(0, npc.gfxOffY) - npc.velocity;
            Microsoft.Xna.Framework.Color color29 = new Microsoft.Xna.Framework.Color(127 - npc.alpha, 127 - npc.alpha, 127 - npc.alpha, 0).MultiplyRGBA(Microsoft.Xna.Framework.Color.White);
            Microsoft.Xna.Framework.Color color28 = color29;
            color28 = npc.GetAlpha(color28);
            color28 *= 1f - num107;

            Microsoft.Xna.Framework.Color color30 = color29;
            color30 = npc.GetAlpha(color28);
            color30 *= 1.18f - num107;

            Main.spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition, null, npc.GetAlpha(Color.White), 0f, center, 1, SpriteEffects.None, 0f);


            for (int num103 = 0; num103 < 6; num103++)
            {
                Vector2 vector29 = new Vector2(npc.Center.X, npc.Center.Y) + ((float)num103 / (float)num108 * 6.28318548f + npc.rotation + num106).ToRotationVector2() * (4f * num107 + 2f) - Main.screenPosition + new Vector2(0, npc.gfxOffY) - npc.velocity * (float)num103;
                Main.spriteBatch.Draw(ModContent.GetTexture("SpiritMod" + Texture.Remove(0, "Starjinx/".Length) + "Glow"), vector29, npc.frame, color28, npc.rotation, npc.frame.Size() / 2f, npc.scale, spriteEffects3, 0f);
            }


            return false;
        }
        public override bool CheckDead()
        {
            return false;
        }

        public override bool CheckActive()
        {
            NPC parent = Main.npc[(int)npc.ai[0]];
            if (parent.active && parent.type == ModContent.NPCType<StarjinxMeteorite>())
                return false;

            return true;
        }
    }
}