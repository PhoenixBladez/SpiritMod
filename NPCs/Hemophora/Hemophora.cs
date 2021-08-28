using SpiritMod.Projectiles.Hostile;
using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Hemophora
{
    public class Hemophora : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hemophora");
            Main.npcFrameCount[npc.type] = 11;
        }

        public override void SetDefaults()
        {
            npc.width = 34;
            npc.height = 44;
            npc.damage = 10;
            npc.defense = 18;
            npc.lifeMax = 121;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.value = 220f;
            npc.knockBackResist = .05f;
            banner = npc.type;
            bannerItem = ModContent.ItemType<Items.Banners.HemaphoraBanner>();
        }
        int frame = 0;
        public override void AI()
        {
            npc.velocity.Y += .25f;
            npc.ai[3]++;
            if (npc.ai[3] >= 6)
            {
                frame++;
                npc.ai[3] = 0;
                npc.netUpdate = true;
            }
            if (npc.ai[0] < 138)
            {
                if (frame > 3)
                {
                    frame = 0;
                }
            }
			else
            {
                if (frame > 10 || frame <= 3)
                {
                    frame = 4;
                }
            }
            Player target = Main.player[npc.target];
            int distance = (int)Math.Sqrt((npc.Center.X - target.Center.X) * (npc.Center.X - target.Center.X) + (npc.Center.Y - target.Center.Y) * (npc.Center.Y - target.Center.Y));
            if (target.position.X > npc.position.X)
            {
                npc.spriteDirection = -1;
            }
            else
            {
                npc.spriteDirection = 1;
            }
            if (distance < 680)
            {
                npc.ai[0]++;
                if (npc.ai[0] == 172)
                {
                    Main.PlaySound(SoundID.Item, npc.Center, 95);
                    int type = ModContent.ProjectileType<HemophoraProj>();
                    int p = Terraria.Projectile.NewProjectile(npc.Center.X, npc.Center.Y, -(npc.position.X - target.position.X) / distance * 4, -(npc.position.Y - target.position.Y) / distance * 4, type, (int)((npc.damage * .5)), 0);
                    for (int k = 0; k < 20; k++)
                    {
                        int d = Dust.NewDust(npc.position, npc.width, npc.height, DustID.Blood, -(npc.position.X - target.position.X) / distance * 2, -(npc.position.Y - target.position.Y) / distance * 4, 0, default(Color), Main.rand.NextFloat(.65f, .85f));
                        Main.dust[d].fadeIn = 1f;
                        Main.dust[d].velocity *= .88f;
                        Main.dust[d].noGravity = true;
                    }
                }
				if (npc.ai[0] >= 180)
                {
                    npc.ai[0] = 0;
                    npc.netUpdate = true;
                }
            }
			else
            {
                npc.ai[0] = 0;
            }
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (SpawnHelper.SupressSpawns(spawnInfo, SpawnFlags.None))
                return 0;

            return (spawnInfo.spawnTileY > Main.rockLayer && spawnInfo.player.ZoneJungle) ? 0.0368f : 0f;
        }
        public override void HitEffect(int hitDirection, double damage)
        {
            for (int i = 0; i < 6; i++)
            {
                int a = Gore.NewGore(new Vector2(npc.Center.X + Main.rand.Next(-10, 10), npc.Center.Y + Main.rand.Next(-10, 10)), npc.velocity, 911);
                Main.gore[a].timeLeft = 20;
                Main.gore[a].scale = Main.rand.NextFloat(.5f, 1f);
            }
			if (npc.life <= 0)
            {
                for (int i = 0; i < 6; i++)
                {
                    int a = Gore.NewGore(new Vector2(npc.Center.X + Main.rand.Next(-10, 10), npc.Center.Y + Main.rand.Next(-10, 10)), npc.velocity/4, 386);
                    Main.gore[a].timeLeft = 20;
                    Main.gore[a].scale = Main.rand.NextFloat(.5f, 1f);
                }
            }
            for (int k = 0; k < 20; k++)
            {
                int d = Dust.NewDust(npc.position, npc.width, npc.height, DustID.Blood, hitDirection * 2.5f, -1f, 0, default(Color), Main.rand.NextFloat(.45f, .95f));
                Main.dust[d].fadeIn = 1f;
                Main.dust[d].noGravity = true;
            }
        }

        public override void NPCLoot()
        {
            if (Main.rand.NextBool(2))
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Vine);
            }
            if (Main.rand.NextBool(3))
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.JungleSpores, Main.rand.Next(2,5));
            }
            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.Sets.ThrownMisc.FlaskofGore.FlaskOfGore>(), Main.rand.Next(21, 43));

        }

        public override void FindFrame(int frameHeight)
        {
            npc.frame.Y = frameHeight * frame;
        }
    }
}