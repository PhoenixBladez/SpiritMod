using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using SpiritMod.Buffs;
using SpiritMod.Buffs.DoT;

namespace SpiritMod.NPCs.BlizzardBandit
{
    public class BlizzardBandit : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blizzard Bandit");
            Main.npcFrameCount[npc.type] = 16;
        }

        int timer = 0;
        bool shooting = false;
        bool gettingballs = false;

        public override void SetDefaults()
        {
            npc.aiStyle = 3;
            npc.lifeMax = 55;
            npc.defense = 6;
            aiType = NPCID.SnowFlinx;
            npc.value = 65f;
            npc.knockBackResist = 0.7f;
            npc.width = 30;
			npc.buffImmune[BuffID.Frostburn] = true;
			npc.buffImmune[ModContent.BuffType<MageFreeze>()] = true;
			npc.buffImmune[ModContent.BuffType<CryoCrush>()] = true;
			npc.height = 54;
            npc.damage = 15;
            npc.lavaImmune = false;
            npc.noTileCollide = false;
            npc.alpha = 0;
            npc.dontTakeDamage = false;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = new Terraria.Audio.LegacySoundStyle(4, 1);
            banner = npc.type;
            bannerItem = ModContent.ItemType<Items.Banners.BlizzardBanditBanner>();
        }

        public override bool PreAI()
        {
            if (gettingballs)
            {
                npc.velocity.Y = 6;
                npc.velocity.X *= 0.08f;
            }

            if (timer == 240)
            {
                shooting = true;
                gettingballs = true;
                timer = 0;
            }

            if (!shooting)
                timer++;

            if (npc.velocity.X < 0f)
                npc.spriteDirection = 1;
            else if (npc.velocity.X > 0f)
                npc.spriteDirection = -1;
            return base.PreAI();
        }

        public override void AI()
        {
            if (shooting)
            {
                if (npc.velocity.X < 0f)
                    npc.spriteDirection = 1;
                else if (npc.velocity.X > 0f)
                    npc.spriteDirection = -1;
            }
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            for (int k = 0; k < 30; k++)
                Dust.NewDust(npc.position, npc.width, npc.height, DustID.Rope, 2.5f * hitDirection, -2.5f, 0, Color.White, Main.rand.NextFloat(.3f, 1.1f));

            if (npc.life <= 0)
				for (int i = 1; i < 5; ++i)
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/BlizzardBanditGore" + i), 1f);
        }

        int frame = 0;

        public override void FindFrame(int frameHeight)
        {
            Player player = Main.player[npc.target];
            npc.frameCounter++;

            if (!shooting)
            {
                if (npc.frameCounter >= 7)
                {
                    frame++;
                    npc.frameCounter = 0;
                }

                if (frame >= 6)
                    frame = 0;
            }
            else
            {
                if (Main.player[npc.target].Center.X < npc.Center.X)
                    npc.spriteDirection = 1;
                else
                    npc.spriteDirection = -1;

                if (npc.frameCounter >= 7)
                {
                    frame++;
                    npc.frameCounter = 0;
                }

                if (frame == 10)
                    gettingballs = false;

                if ((frame == 11 || frame == 14) && npc.frameCounter == 4)
                {
                    Main.PlaySound(SoundID.Item, (int)npc.position.X, (int)npc.position.Y, 19, 1f, 0f);
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Vector2 direction = Main.player[npc.target].Center - npc.Center;
                        direction.Normalize();
                        direction.X *= 8.5f;
                        direction.Y *= 8.5f;
                        float A = (float)Main.rand.Next(-50, 50) * 0.02f;
                        float B = (float)Main.rand.Next(-50, 50) * 0.02f;
                        int p = Projectile.NewProjectile(npc.Center.X + (npc.direction * 12), npc.Center.Y, direction.X + A, direction.Y + B, ProjectileID.SnowBallFriendly, npc.damage / 3, 1, Main.myPlayer, 0, 0);
                        Main.projectile[p].hostile = true;
                        Main.projectile[p].friendly = false;
                    }
                }

                if (frame >= 16)
                {
                    shooting = false;
                    frame = 0;
                }

                if (frame < 6)
                    frame = 6;
            }
            npc.frame.Y = frameHeight * frame;
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(timer);
			writer.Write(shooting);
			writer.Write(gettingballs);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			timer = reader.ReadInt32();
			shooting = reader.ReadBoolean();
			gettingballs = reader.ReadBoolean();
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) => spawnInfo.player.ZoneSnow && spawnInfo.player.ZoneOverworldHeight && Main.dayTime && !spawnInfo.playerSafe ? 0.0895f : 0f;

		public override void NPCLoot()
        {
            if (Main.rand.Next(20) == 0)
                npc.DropItem(ModContent.ItemType<Items.Armor.Masks.WinterHat>());
            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Snowball, Main.rand.Next(8, 14));
        }
    }
}