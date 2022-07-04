using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using SpiritMod.Buffs;
using SpiritMod.Buffs.DoT;
using Terraria.GameContent.ItemDropRules;

namespace SpiritMod.NPCs.BlizzardBandit
{
    public class BlizzardBandit : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blizzard Bandit");
            Main.npcFrameCount[NPC.type] = 16;
        }

        int timer = 0;
        bool shooting = false;
        bool gettingballs = false;

        public override void SetDefaults()
        {
            NPC.aiStyle = 3;
            NPC.lifeMax = 55;
            NPC.defense = 6;
            AIType = NPCID.SnowFlinx;
            NPC.value = 65f;
            NPC.knockBackResist = 0.7f;
            NPC.width = 30;
			NPC.buffImmune[BuffID.Frostburn] = true;
			NPC.buffImmune[ModContent.BuffType<MageFreeze>()] = true;
			NPC.buffImmune[ModContent.BuffType<CryoCrush>()] = true;
			NPC.height = 54;
            NPC.damage = 15;
            NPC.lavaImmune = false;
            NPC.noTileCollide = false;
            NPC.alpha = 0;
            NPC.dontTakeDamage = false;
            NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCHit1;// new Terraria.Audio.LegacySoundStyle(4, 1);
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<Items.Banners.BlizzardBanditBanner>();
        }

        public override bool PreAI()
        {
            if (gettingballs)
            {
                NPC.velocity.Y = 6;
                NPC.velocity.X *= 0.08f;
            }

            if (timer == 240)
            {
                shooting = true;
                gettingballs = true;
                timer = 0;
            }

            if (!shooting)
                timer++;

            if (NPC.velocity.X < 0f)
                NPC.spriteDirection = 1;
            else if (NPC.velocity.X > 0f)
                NPC.spriteDirection = -1;
            return base.PreAI();
        }

        public override void AI()
        {
            if (shooting)
            {
                if (NPC.velocity.X < 0f)
                    NPC.spriteDirection = 1;
                else if (NPC.velocity.X > 0f)
                    NPC.spriteDirection = -1;
            }
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            for (int k = 0; k < 30; k++)
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Rope, 2.5f * hitDirection, -2.5f, 0, Color.White, Main.rand.NextFloat(.3f, 1.1f));

            if (NPC.life <= 0)
				for (int i = 1; i < 5; ++i)
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("SpiritMod/Gores/BlizzardBanditGore" + i).Type, 1f);
        }

        int frame = 0;

        public override void FindFrame(int frameHeight)
        {
            Player player = Main.player[NPC.target];
            NPC.frameCounter++;

            if (!shooting)
            {
                if (NPC.frameCounter >= 7)
                {
                    frame++;
                    NPC.frameCounter = 0;
                }

                if (frame >= 6)
                    frame = 0;
            }
            else
            {
                if (Main.player[NPC.target].Center.X < NPC.Center.X)
                    NPC.spriteDirection = 1;
                else
                    NPC.spriteDirection = -1;

                if (NPC.frameCounter >= 7)
                {
                    frame++;
                    NPC.frameCounter = 0;
                }

                if (frame == 10)
                    gettingballs = false;

                if ((frame == 11 || frame == 14) && NPC.frameCounter == 4)
                {
                    SoundEngine.PlaySound(SoundID.Item19 with { Volume = 0.5f }, NPC.Center);
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Vector2 direction = Main.player[NPC.target].Center - NPC.Center;
                        direction.Normalize();
                        direction.X *= 8.5f;
                        direction.Y *= 8.5f;
                        float A = (float)Main.rand.Next(-50, 50) * 0.02f;
                        float B = (float)Main.rand.Next(-50, 50) * 0.02f;
                        int p = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X + (NPC.direction * 12), NPC.Center.Y, direction.X + A, direction.Y + B, ProjectileID.SnowBallFriendly, NPC.damage / 3, 1, Main.myPlayer, 0, 0);
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
            NPC.frame.Y = frameHeight * frame;
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

		public override float SpawnChance(NPCSpawnInfo spawnInfo) => spawnInfo.Player.ZoneSnow && spawnInfo.Player.ZoneOverworldHeight && Main.dayTime && !spawnInfo.PlayerSafe ? 0.0895f : 0f;

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ItemID.Snowball, 1, 8, 13));
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Armor.Masks.WinterHat>(), 20));
		}
	}
}