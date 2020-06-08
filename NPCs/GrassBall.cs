using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs
{
    public class GrassBall : ModNPC
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Old Gods' Wrath");
        }

        public override void SetDefaults() {
            npc.width = 8;
            npc.height = 8;
            npc.alpha = 255;

            npc.damage = 18;
            npc.defense = 0;
            npc.lifeMax = 1;
            npc.knockBackResist = 0;

            npc.friendly = false;
            npc.noGravity = true;
            npc.noTileCollide = true;

            npc.HitSound = SoundID.NPCHit3;
            npc.DeathSound = SoundID.NPCDeath3;
        }
        public float counter = -1440;
        public override bool PreAI() {
            if(npc.target == 255) {
                npc.TargetClosest(true);
                float num1 = 6f;
                Vector2 vector2 = new Vector2(npc.position.X + (npc.width * 0.5f), npc.position.Y + (npc.height * 0.5f));
                float num2 = Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2) - vector2.X;
                float num3 = Main.player[npc.target].position.Y + (float)(Main.player[npc.target].height / 2) - vector2.Y;
                float num4 = (float)Math.Sqrt(num2 * num2 + num3 * num3);
                float num5 = num1 / num4;
                npc.velocity.X = num2 * num5;
                npc.velocity.Y = num3 * num5;
            }

            if(npc.timeLeft > 100)
                npc.timeLeft = 100;
            counter++;
            if(counter >= 1440) {
                counter = -1440;
            }
            for(int i = 0; i < 10; i++) {
                float x = npc.Center.X - npc.velocity.X / 10f * (float)i;
                float y = npc.Center.Y - npc.velocity.Y / 10f * (float)i;

                int num = Dust.NewDust(npc.Center + new Vector2(0, (float)Math.Cos(counter / 8.2f) * 9.2f).RotatedBy(npc.rotation), 6, 6, 228, 0f, 0f, 0, default(Color), 1f);
                Main.dust[num].velocity *= .1f;
                Main.dust[num].scale *= .9f;
                Main.dust[num].noGravity = true;

            }
            for(int f = 0; f < 10; f++) {
                float x = npc.Center.X - npc.velocity.X / 10f * (float)f;
                float y = npc.Center.Y - npc.velocity.Y / 10f * (float)f;

                int num = Dust.NewDust(npc.Center - new Vector2(0, (float)Math.Cos(counter / 8.2f) * 9.2f).RotatedBy(npc.rotation), 6, 6, 228, 0f, 0f, 0, default(Color), 1f);
                Main.dust[num].velocity *= .1f;
                Main.dust[num].scale *= .9f;
                Main.dust[num].noGravity = true;

            }
            for(int j = 0; j < 6; j++) {

                int num2 = Dust.NewDust(npc.Center, 6, 6, 244, 0f, 0f, 0, default(Color), 1f);
                Main.dust[num2].velocity *= 0f;
                Main.dust[num2].scale *= .6f;
                Main.dust[num2].noGravity = true;
            }
            return false;
        }

        public override void OnHitPlayer(Player target, int damage, bool crit) {
            target.AddBuff(BuffID.Poisoned, 180);
        }
    }
}
