using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Asteroid
{
	public class GloopGloop : ModNPC
	{
		int counter = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gloop");
			Main.npcFrameCount[npc.type] = 3;
		}

		public override void SetDefaults()
		{
			npc.width = 32;
			npc.height = 48;
			npc.damage = 18;
			npc.defense = 10;
			npc.lifeMax = 170;
			npc.noGravity = true;
			npc.value = 800f;
			npc.noTileCollide = true;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath22;
		}
        public override void AI()
        {
			npc.rotation = npc.velocity.ToRotation() + 1.57f;
            counter++;
			npc.velocity *= 0.995f;
            float num395 = Main.mouseTextColor / 200f - 0.35f;
            num395 *= 0.2f;
            npc.scale = num395 + 0.95f;
            if (counter > 65)
			{
				Vector2 direction = Main.player[npc.target].Center - npc.Center;
				direction.Normalize();
				direction *= 10;
				npc.velocity = direction;
                for (int i = 0; i < 10; i++)
                {
                    int num = Dust.NewDust(npc.position, npc.width, npc.height, 167, 0f, -2f, 0, default(Color), 2f);
                    Main.dust[num].noGravity = true;
                    Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
                    Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
                    Main.dust[num].scale *= .25f;
                    if (Main.dust[num].position != npc.Center)
                        Main.dust[num].velocity = npc.DirectionTo(Main.dust[num].position) * 4f;
                }
                counter = 0;
			}
        }
        public override void HitEffect(int hitDirection, double damage)
        {
            for (int k = 0; k < 11; k++)
            {
                Dust.NewDust(npc.position, npc.width, npc.height, 167, hitDirection, -1f, 0, default(Color), .61f);
            }
            if (npc.life <= 0)
            {
                for (int k = 0; k < 15; k++)
                {
                    Dust.NewDust(npc.position, npc.width, npc.height, 167, hitDirection, -1f, 0, default(Color), .81f);
                }
            }
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
