using SpiritMod.Projectiles.Hostile;
using Terraria;
using System;
using Terraria.Audio;
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
            Main.npcFrameCount[NPC.type] = 11;
        }

        public override void SetDefaults()
        {
            NPC.width = 34;
            NPC.height = 44;
            NPC.damage = 10;
            NPC.defense = 18;
            NPC.lifeMax = 121;
			NPC.buffImmune[BuffID.Poisoned] = true;
			NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = 220f;
            NPC.knockBackResist = .05f;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<Items.Banners.HemaphoraBanner>();
        }
        int frame = 0;
        public override void AI()
        {
            NPC.velocity.Y += .25f;
            NPC.ai[3]++;
            if (NPC.ai[3] >= 6)
            {
                frame++;
                NPC.ai[3] = 0;
                NPC.netUpdate = true;
            }
            if (NPC.ai[0] < 138)
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
            Player target = Main.player[NPC.target];
            int distance = (int)Math.Sqrt((NPC.Center.X - target.Center.X) * (NPC.Center.X - target.Center.X) + (NPC.Center.Y - target.Center.Y) * (NPC.Center.Y - target.Center.Y));
            if (target.position.X > NPC.position.X)
            {
                NPC.spriteDirection = -1;
            }
            else
            {
                NPC.spriteDirection = 1;
            }
            if (distance < 680)
            {
                NPC.ai[0]++;
                if (NPC.ai[0] == 172)
                {
                    SoundEngine.PlaySound(SoundID.Item, NPC.Center, 95);
                    int type = ModContent.ProjectileType<HemophoraProj>();
                    int p = Terraria.Projectile.NewProjectile(NPC.Center.X, NPC.Center.Y, -(NPC.position.X - target.position.X) / distance * 4, -(NPC.position.Y - target.position.Y) / distance * 4, type, (int)((NPC.damage * .5)), 0);
                    for (int k = 0; k < 20; k++)
                    {
                        int d = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, -(NPC.position.X - target.position.X) / distance * 2, -(NPC.position.Y - target.position.Y) / distance * 4, 0, default, Main.rand.NextFloat(.65f, .85f));
                        Main.dust[d].fadeIn = 1f;
                        Main.dust[d].velocity *= .88f;
                        Main.dust[d].noGravity = true;
                    }
                }
				if (NPC.ai[0] >= 180)
                {
                    NPC.ai[0] = 0;
                    NPC.netUpdate = true;
                }
            }
			else
            {
                NPC.ai[0] = 0;
            }
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (SpawnHelper.SupressSpawns(spawnInfo, SpawnFlags.None))
                return 0;

            return (spawnInfo.SpawnTileY > Main.rockLayer && spawnInfo.Player.ZoneJungle) ? 0.0368f : 0f;
        }
        public override void HitEffect(int hitDirection, double damage)
        {
            for (int i = 0; i < 6; i++)
            {
                int a = Gore.NewGore(new Vector2(NPC.Center.X + Main.rand.Next(-10, 10), NPC.Center.Y + Main.rand.Next(-10, 10)), NPC.velocity, 911);
                Main.gore[a].timeLeft = 20;
                Main.gore[a].scale = Main.rand.NextFloat(.5f, 1f);
            }
			if (NPC.life <= 0)
            {
                for (int i = 0; i < 6; i++)
                {
                    int a = Gore.NewGore(new Vector2(NPC.Center.X + Main.rand.Next(-10, 10), NPC.Center.Y + Main.rand.Next(-10, 10)), NPC.velocity/4, 386);
                    Main.gore[a].timeLeft = 20;
                    Main.gore[a].scale = Main.rand.NextFloat(.5f, 1f);
                }
            }
            for (int k = 0; k < 20; k++)
            {
                int d = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hitDirection * 2.5f, -1f, 0, default, Main.rand.NextFloat(.45f, .95f));
                Main.dust[d].fadeIn = 1f;
                Main.dust[d].noGravity = true;
            }
        }

        public override void OnKill()
        {
            if (Main.rand.NextBool(2))
            {
                Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ItemID.Vine);
            }
            if (Main.rand.NextBool(3))
            {
                Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ItemID.JungleSpores, Main.rand.Next(2,5));
            }
            Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ModContent.ItemType<Items.Sets.ThrownMisc.FlaskofGore.FlaskOfGore>(), Main.rand.Next(21, 43));

        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frame.Y = frameHeight * frame;
        }
    }
}