using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
namespace SpiritMod.NPCs
{
    [AutoloadBossHead]
    public class Snaptrapper : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Snaptrapper");
			Main.npcFrameCount[npc.type] = 11;
            NPCID.Sets.TrailCacheLength[npc.type] = 5;
            NPCID.Sets.TrailingMode[npc.type] = 0;
        }

		public override void SetDefaults()
		{
			npc.width = 62;
			npc.height = 74;
			npc.damage = 25;
			npc.defense = 12;
			npc.lifeMax = 340;
			npc.HitSound = SoundID.NPCHit32;
            npc.buffImmune[BuffID.Poisoned] = true;
            npc.buffImmune[ModContent.BuffType<Buffs.FesteringWounds>()] = true;
            npc.buffImmune[BuffID.Venom] = true;
            npc.DeathSound = SoundID.NPCDeath25;
			npc.value = 629f;
			npc.knockBackResist = 0f;
			npc.aiStyle = 3;
			aiType = NPCID.SnowFlinx;
			banner = npc.type;
			bannerItem = ModContent.ItemType<Items.Banners.AntlionAssassinBanner>();
		}
        public override bool PreNPCLoot()
        {
            Main.PlaySound(SoundLoader.customSoundType, npc.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/DownedMiniboss"));
            MyWorld.downedSnaptrapper = true;
            return true;
        }
        public override void NPCLoot()
        {
            string[] lootTable = { "SnapsporeStaff", "SporeClub" };
            int loot = Main.rand.Next(lootTable.Length);
            {
                if (Main.rand.NextBool(2))
                    npc.DropItem(mod.ItemType(lootTable[loot]));
            }
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
                for (int j = 0; j < 12; j++)
                {
                    int a = Gore.NewGore(new Vector2(npc.Center.X + Main.rand.Next(-10, 10), npc.Center.Y + Main.rand.Next(-10, 10)), npc.velocity, 911);
                    Main.gore[a].timeLeft = 20;
                    Main.gore[a].scale = Main.rand.NextFloat(.5f, 1f);
                }
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Snaptrapper/Snaptrapper1"), 1f);
                for (int k = 0; k < 6; k++)
                {
                    Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Snaptrapper/Snaptrapper2"), Main.rand.NextFloat(.6f, 1f));
                }
                for (int z = 0; z < 2; z++)
                {
                    Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Snaptrapper/Snaptrapper3"), Main.rand.NextFloat(.8f, 1f));
                }
            }
        }
        int frame;
		public override void FindFrame(int frameHeight)
		{
            npc.frame.Y = frameHeight * frame;
        }
        bool chargePhase;
        int frameTimer;
        int timer;
		public override void AI()
		{
            npc.spriteDirection = npc.direction;
            Player target = Main.player[npc.target];
            Vector2 direction = Main.player[npc.target].Center - npc.Center;
            direction.Normalize();
            direction.X *= 3.75f;
            direction.Y *= 8f;
            int distance = (int)Math.Sqrt((npc.Center.X - target.Center.X) * (npc.Center.X - target.Center.X) + (npc.Center.Y - target.Center.Y) * (npc.Center.Y - target.Center.Y));
            if (!chargePhase && distance < 62)
            {
                npc.velocity.X = 0;
                npc.noGravity = false;
                if (target.position.X < npc.position.X)
                {
                    npc.spriteDirection = -1;
                }
                else
                {
                    npc.spriteDirection = 1;
                }
                frameTimer++;
                if (frameTimer >= 8)
                {
                    frameTimer = 0;
                    frame++;
                }
                if (frame > 10 || frame < 7)
                {
                    frame = 7;
                }
            }
            else if (!chargePhase)
            {
                frameTimer++;
                if (frameTimer >= 6)
                {
                    frameTimer = 0;
                    frame++;
                }
                if (frame >= 7)
                {
                    frame = 0;
                }
            }
            npc.localAI[1]++;
            if (npc.life >= npc.lifeMax * .4f)
            {
                if (npc.localAI[1] >= 340 && npc.localAI[1] < 400)
                {
                    npc.velocity.X = 0f;
                    npc.noGravity = false;
                    if (target.position.X < npc.position.X)
                    {
                        npc.spriteDirection = -1;
                    }
                    else
                    {
                        npc.spriteDirection = 1;
                    }
                    if (Main.rand.Next(3) == 0)
                    {
                        Dust dust = Main.dust[Dust.NewDust(new Vector2(npc.position.X, npc.position.Y + 2f), npc.width, npc.height, ModContent.DustType<Dusts.PoisonGas>(), npc.velocity.X * 0.2f, npc.velocity.Y * 0.2f, 100, new Color(), 7f)];
                        dust.noGravity = true;
                        dust.velocity.X = dust.velocity.X * 0.3f;
                        dust.velocity.Y = (dust.velocity.Y * 0.2f) - 1;
                    }
                }
                if (npc.localAI[1] == 360)
                {
                    {
                        Main.PlaySound(6, (int)npc.position.X, (int)npc.position.Y, 0);
                    }
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        int amountOfProjectiles = Main.rand.Next(2, 4);
                        for (int i = 0; i < amountOfProjectiles; ++i)
                        {
                            float A = Main.rand.Next(-60, 60) * 0.05f;
                            float B = Main.rand.Next(-80, -10) * 0.05f;
                            int p = Projectile.NewProjectile(npc.Center.X, npc.Center.Y, direction.X + A, -direction.Y + B, ModContent.ProjectileType<Projectiles.Hostile.SnaptrapperSpore>(), 18, 1, Main.myPlayer, 0, 0);
                            Main.projectile[p].hostile = true;
                            Main.projectile[p].friendly = false;
                        }
                    }
                }
                if (npc.localAI[1] == 400)
                {
                    Main.PlaySound(4, (int)npc.position.X, (int)npc.position.Y, 5);
                }
                if (npc.localAI[1] >= 400 && npc.localAI[1] <= 530)
                {
                    npc.netUpdate = true;
                    Charging();
                    if (npc.collideY)
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            Dust dust = Main.dust[Dust.NewDust(new Vector2(npc.position.X, npc.position.Y + 2f), npc.width, npc.height, ModContent.DustType<Dusts.PoisonGas>(), npc.velocity.X * 0.2f, npc.velocity.Y * 0.2f, 100, new Color(), 5.3f)];
                            dust.noGravity = true;
                            dust.velocity.X = dust.velocity.X * 0.3f;
                            dust.velocity.Y = (dust.velocity.Y * 0.2f) - 1;
                        }
                        frame = 7;
                    }
					else
                    {
                        frame = 8;
                    }
                }
                else
                {
                    chargePhase = false;
                    npc.aiStyle = 3;
                    aiType = NPCID.SnowFlinx;
                }
                if (npc.localAI[1] >= 800)
                {
                    npc.localAI[1] = 0;
                }
            }
			else
            {
                if (npc.localAI[1] >= 240 && npc.localAI[1] < 300)
                {
                    npc.velocity.X = 0f;
                    npc.noGravity = false;
                    if (target.position.X < npc.position.X)
                    {
                        npc.spriteDirection = -1;
                    }
                    else
                    {
                        npc.spriteDirection = 1;
                    }
                    if (Main.rand.Next(2) == 0)
                    {
                        Dust dust = Main.dust[Dust.NewDust(new Vector2(npc.position.X, npc.position.Y + 2f), npc.width, npc.height, ModContent.DustType<Dusts.PoisonGas>(), npc.velocity.X * 0.2f, npc.velocity.Y * 0.2f, 100, new Color(), 7f)];
                        dust.noGravity = true;
                        dust.velocity.X = dust.velocity.X * 0.3f;
                        dust.velocity.Y = (dust.velocity.Y * 0.2f) - 1;

                    }
                }
                if (npc.localAI[1] == 260)
                {
                    {
                        Main.PlaySound(6, (int)npc.position.X, (int)npc.position.Y, 0);
                    }
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        int amountOfProjectiles = Main.rand.Next(3, 6);
                        for (int i = 0; i < amountOfProjectiles; ++i)
                        {
                            float A = Main.rand.Next(-60, 60) * 0.05f;
                            float B = Main.rand.Next(-80, -10) * 0.05f;
                            int p = Projectile.NewProjectile(npc.Center.X, npc.Center.Y, direction.X + A, -direction.Y + B, ModContent.ProjectileType<Projectiles.Hostile.SnaptrapperSpore>(), 18, 1, Main.myPlayer, 0, 0);
                            Main.projectile[p].hostile = true;
                            Main.projectile[p].friendly = false;
                        }
                    }
                }
				if (npc.localAI[1] == 300)
                {
                    Main.PlaySound(4, (int)npc.position.X, (int)npc.position.Y, 5);
                }
                if (npc.localAI[1] >= 300 && npc.localAI[1] <= 510)
                {
                    if (npc.collideY && Main.rand.Next(6) == 0)
                    {
                        int p = Projectile.NewProjectile(npc.Center.X, npc.Center.Y, Main.rand.Next(-2, 2), Main.rand.Next(-3, -1), ModContent.ProjectileType<Projectiles.Hostile.SnaptrapperGas>(), 12, 1, Main.myPlayer, 0, 0);
                        Main.projectile[p].hostile = true;
                        Main.projectile[p].friendly = false;
                    }
                    npc.netUpdate = true;
                    Charging();
                    ChargeFrames();
                    if (npc.collideY)
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            Dust dust = Main.dust[Dust.NewDust(new Vector2(npc.position.X, npc.position.Y + 2f), npc.width, npc.height, ModContent.DustType<Dusts.PoisonGas>(), npc.velocity.X * 0.2f, npc.velocity.Y * 0.2f, 100, new Color(), 5.4f)];
                            dust.noGravity = true;
                            dust.velocity.X = dust.velocity.X * 0.3f;
                            dust.velocity.Y = (dust.velocity.Y * 0.2f) - 1;
                        }
                        frame = 7;
                    }
                }
                else
                {
                    chargePhase = false;
                    npc.aiStyle = 3;
                    aiType = NPCID.SnowFlinx;
                }
                if (npc.localAI[1] >= 540)
                {
                    npc.localAI[1] = 0;
                }
            }
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
			if (MyWorld.downedSnaptrapper)
            {
                return spawnInfo.player.ZoneJungle && NPC.downedBoss2 && !NPC.AnyNPCs(ModContent.NPCType<Snaptrapper>()) && spawnInfo.player.ZoneOverworldHeight ? 0.008125f : 0f;
            }
            return spawnInfo.player.ZoneJungle && NPC.downedBoss2 && !NPC.AnyNPCs(ModContent.NPCType<Snaptrapper>()) && spawnInfo.player.ZoneOverworldHeight ? 0.036f : 0f;
        }
        public void ChargeFrames()
        {
            frameTimer++;
            if (frameTimer >= 8)
            {
                frameTimer = 0;
                frame++;
            }
            if (frame > 10 || frame < 8)
            {
                frame = 8;
            }
        }
		public void Charging()
        {
            npc.aiStyle = -1;
            chargePhase = true;
            if (npc.ai[2] > 1f)
            {
                npc.ai[2] -= 1f;
            }
            if (npc.ai[2] == 0f)
            {
                npc.ai[0] = -100f;
                npc.ai[2] = 1f;
                npc.TargetClosest(true);
                npc.spriteDirection = npc.direction;
            }

            if (npc.wet)
            {
                if (npc.collideX)
                {
                    npc.direction *= -npc.direction;
                    npc.spriteDirection = npc.direction;
                }
                if (npc.collideY)
                {
                    npc.TargetClosest(true);
                    if (npc.oldVelocity.Y < 0f)
                    {
                        npc.velocity.Y = 5f;
                    }
                    else
                    {
                        npc.velocity.Y = npc.velocity.Y - 2f;
                    }
                    npc.spriteDirection = npc.direction;
                }
                if (npc.velocity.Y > 4f)
                {
                    npc.velocity.Y = npc.velocity.Y * 0.95f;
                }
                npc.velocity.Y = npc.velocity.Y - 0.3f;
                if (npc.velocity.Y < -4f)
                {
                    npc.velocity.Y = -4f;
                }
            }
            if (npc.velocity.Y == 0f)
            {
                if (npc.ai[3] == npc.position.X)
                {
                    npc.direction *= -1;
                    npc.ai[2] = 300f;
                }
                npc.ai[3] = 0f;
                npc.velocity.X = npc.velocity.X * 0.9f;
                if ((double)npc.velocity.X > -0.1 && (double)npc.velocity.X < 0.1)
                {
                    npc.velocity.X = 0f;
                }
                {
                    npc.ai[0] += 5f;
                }
                Vector2 vector416 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
                float num2721 = Main.player[npc.target].position.X + (float)Main.player[npc.target].width * 0.5f - vector416.X;
                float num2720 = Main.player[npc.target].position.Y + (float)Main.player[npc.target].height * 0.5f - vector416.Y;
                float num2719 = (float)Math.Sqrt((double)(num2721 * num2721 + num2720 * num2720));
                float num2718 = 400f / num2719;
                num2718 = ((npc.type != 177) ? (num2718 * 10f) : (num2718 * 5f));
                if (num2718 > 30f)
                {
                    num2718 = 30f;
                }
                npc.ai[0] += (float)(int)num2718;
                if (npc.ai[0] >= 0f)
                {
                    npc.netUpdate = true;
                    if (npc.ai[2] == 1f)
                    {
                        npc.TargetClosest(true);
                    }
                    float velXnum = ((npc.life < npc.lifeMax * .3f) ? (4.75f) : (2.525f));
                    float velYnum = ((npc.life < npc.lifeMax * .3f) ? (10.75f) : (8.5f));
                    {
                        npc.velocity.Y = -velYnum;
                        npc.velocity.X = npc.velocity.X + (float)(velXnum * npc.direction);
                        if (num2719 < 350f && num2719 > 200f)
                        {
                            npc.velocity.X = npc.velocity.X + (float)npc.direction;
                        }
                        npc.ai[0] = -120f;
                        npc.ai[1] += 1f;
                    }
                }
                else if (npc.ai[0] >= -30f)
                {
                    npc.aiAction = 1;
                }
                npc.spriteDirection = npc.direction;
            }
            else if (npc.target < 255)
            {
                if (npc.direction != 1 || !(npc.velocity.X < 3f))
                {
                    if (npc.direction != -1)
                    {
                        return;
                    }
                    if (!(npc.velocity.X > -3f))
                    {
                        return;
                    }
                }
                if (npc.direction == -1 && (double)npc.velocity.X < 0.1)
                {
                    npc.velocity.X = npc.velocity.X + 0.2f * (float)npc.direction;
                }
                if (npc.direction == 1 && (double)npc.velocity.X > -0.1)
                {
                    npc.velocity.X = npc.velocity.X + 0.2f * (float)npc.direction;
                }
                npc.velocity.X = npc.velocity.X * 0.93f;
            }
        }
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.velocity.X = 11f * -target.direction;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            Vector2 drawOrigin = new Vector2(Main.npcTexture[npc.type].Width * 0.5f, (npc.height / Main.npcFrameCount[npc.type]) * 0.5f);
            if (chargePhase)
            {
                for (int k = 0; k < npc.oldPos.Length; k++)
                {
                    Vector2 drawPos = npc.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, npc.gfxOffY);
                    Color color = npc.GetAlpha(drawColor) * (float)(((float)(npc.oldPos.Length - k) / (float)npc.oldPos.Length) / 2);
                    spriteBatch.Draw(Main.npcTexture[npc.type], drawPos, new Microsoft.Xna.Framework.Rectangle?(npc.frame), color, npc.rotation, drawOrigin, npc.scale, effects, 0f);
                }
            }
            return true;
        }
	}
}
