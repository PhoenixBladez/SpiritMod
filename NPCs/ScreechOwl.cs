using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;

namespace SpiritMod.NPCs
{
	public class ScreechOwl : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Screech Owl");
			Main.npcFrameCount[npc.type] = 8;
		}

		public override void SetDefaults()
		{
			npc.width = 28;
			npc.height = 24;
			npc.damage = 12;
			npc.defense = 6;
			npc.lifeMax = 60;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath4;
			npc.value = 60f;
			npc.knockBackResist = .62f;
			npc.aiStyle = -1;
            npc.noGravity = true;
        }
        int frame = 0;
        public override void AI()
        {
            npc.spriteDirection = npc.direction;
            Player player = Main.player[npc.target];
            if (npc.ai[1] == 0f)
            {
                npc.ai[3]++;
                if (npc.ai[3] >= 6)
                {
                    frame++;
                    npc.ai[3] = 0;
                    npc.netUpdate = true;
                }
                if (frame >= 6)
                {
                    frame = 0;
                }
            }
			else
            {
                frame = 7;
            }

            npc.ai[2]++;
			if (npc.ai[2] > 300)
            {
                npc.ai[1] = 1f;
                npc.netUpdate = true;
            }
			if (npc.ai[2] == 300)
            {
                if (Vector2.Distance(npc.Center, player.Center) < 480)
                {
                    Main.PlaySound(SoundLoader.customSoundType, npc.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/ScreechOwlScreech"));
                }
                Vector2 direction = Main.player[npc.target].Center - npc.Center;
                direction.Normalize();
                direction.X *= 2.7f;
                direction.Y *= 2.7f;
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    float A = (float)Main.rand.Next(-150, 150) * 0.03f;
                    float B = (float)Main.rand.Next(-150, 150) * 0.03f;
                    int p = Projectile.NewProjectile(npc.Center.X + (10 * npc.spriteDirection), npc.Center.Y - 12, direction.X, direction.Y, ModContent.ProjectileType<Projectiles.Hostile.ScreechOwlNote>(), 9, 1, Main.myPlayer, 0, 0);
					for (int i = 0; i < 10; i++)
                    {
                        float angle = Main.rand.NextFloat(MathHelper.PiOver4, -MathHelper.Pi - MathHelper.PiOver4);
                        Vector2 spawnPlace = Vector2.Normalize(new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle))) * 20f;
                        int dustIndex = Dust.NewDust(new Vector2(npc.Center.X + (10 * npc.spriteDirection), npc.Center.Y - 12), 2, 2, 180, 0f, 0f, 0, default(Color), 1f);
                        Main.dust[dustIndex].noGravity = true;
                        Main.dust[dustIndex].velocity = Vector2.Normalize(spawnPlace.RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi))) * 1.6f;
                    }
                }
            }
			if (npc.ai[2] > 330)
            {
                npc.ai[2] = 0f;
                npc.ai[1] = 0f;
                npc.netUpdate = true;
            }
			if (npc.ai[1] == 1f)
            {
                npc.aiStyle = 0;
                npc.velocity.X = 0;
                npc.velocity.Y *= .5f;
                npc.rotation = 0f;
                if (player.position.X > npc.position.X)
                {
                    npc.spriteDirection = 1;
                }
                else
                {
                    npc.spriteDirection = -1;
                }
            }
			else
            {
                if (player.position.X > npc.position.X)
                {
                    npc.spriteDirection = 1;
                }
                else
                {
                    npc.spriteDirection = -1;
                }
                npc.aiStyle = -1;
                float num1 = 4f;
                float moveSpeed = 0.09f;
                npc.TargetClosest(true);
                Vector2 vector2_1 = Main.player[npc.target].Center - npc.Center + new Vector2(0.0f, Main.rand.NextFloat(-250f, 0f));
                float num2 = vector2_1.Length();
                Vector2 desiredVelocity;
                if ((double)num2 < 20.0)
                    desiredVelocity = npc.velocity;
                else if ((double)num2 < 40.0)
                {
                    vector2_1.Normalize();
                    desiredVelocity = vector2_1 * (num1 * 0.35f);
                }
                else if ((double)num2 < 80.0)
                {
                    vector2_1.Normalize();
                    desiredVelocity = vector2_1 * (num1 * 0.65f);
                }
                else
                {
                    vector2_1.Normalize();
                    desiredVelocity = vector2_1 * num1;
                }
                npc.SimpleFlyMovement(desiredVelocity, moveSpeed);
                npc.rotation = npc.velocity.X * 0.055f;
                if (npc.collideX)
                {
                    npc.velocity.X = npc.oldVelocity.X * -0.5f;
                    if (npc.direction == -1 && npc.velocity.X > 0f && npc.velocity.X < 2f)
                    {
                        npc.velocity.X = 2f;
                    }
                    if (npc.direction == 1 && npc.velocity.X < 0f && npc.velocity.X > -2f)
                    {
                        npc.velocity.X = -2f;
                    }
                }
                if (npc.collideY)
                {
                    npc.velocity.Y = npc.oldVelocity.Y * -0.5f;
                    if (npc.velocity.Y > 0f && npc.velocity.Y < 1f)
                    {
                        npc.velocity.Y = 1f;
                    }
                    if (npc.velocity.Y < 0f && npc.velocity.Y > -1f)
                    {
                        npc.velocity.Y = -1f;
                    }
                }
            }
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return spawnInfo.spawnTileY < Main.worldSurface && spawnInfo.player.ZoneSnow && !Main.dayTime && !spawnInfo.playerSafe ? 0.025f : 0f;
        }
        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life <= 0)
            {
                if (Main.LocalPlayer.GetSpiritPlayer().emptyOwlScroll)
                {
                    MyWorld.numOwlsKilled++;
                }
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/ScreechOwl/ScreechOwl1"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/ScreechOwl/ScreechOwl2"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/ScreechOwl/ScreechOwl3"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/ScreechOwl/ScreechOwl4"), 1f);
            }
        }
        public override void FindFrame(int frameHeight)
        {
            npc.frame.Y = frameHeight * frame;
        }
    }
}
