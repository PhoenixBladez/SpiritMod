using SpiritMod.Items.Material;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;

namespace SpiritMod.NPCs.ScreechOwl
{
	public class ScreechOwl : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Screech Owl");
			Main.npcFrameCount[NPC.type] = 8;
		}

		public override void SetDefaults()
		{
			NPC.width = 28;
			NPC.height = 24;
			NPC.damage = 12;
			NPC.defense = 6;
			NPC.lifeMax = 60;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath4;
			NPC.value = 60f;
			NPC.knockBackResist = .62f;
			NPC.aiStyle = -1;
            NPC.noGravity = true;
			NPC.buffImmune[BuffID.Frostburn] = true;
			NPC.buffImmune[BuffID.Confused] = true;
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<Items.Banners.ScreechOwlBanner>();
		}

        int frame = 0;

        public override void AI()
        {
            NPC.spriteDirection = NPC.direction;
            Player player = Main.player[NPC.target];
            if (NPC.ai[1] == 0f)
            {
                NPC.ai[3]++;
                if (NPC.ai[3] >= 6)
                {
                    frame++;
                    NPC.ai[3] = 0;
                    NPC.netUpdate = true;
                }

                if (frame >= 6)
                    frame = 0;
            }
			else
                frame = 7;

            NPC.ai[2]++;
			if (NPC.ai[2] > 300)
            {
                NPC.ai[1] = 1f;
                NPC.netUpdate = true;
            }
			if (NPC.ai[2] == 300)
            {
                if (Vector2.Distance(NPC.Center, player.Center) < 480)
                {
                    SoundEngine.PlaySound(SoundLoader.customSoundType, NPC.position, Mod.GetSoundSlot(SoundType.Custom, "Sounds/ScreechOwlScreech"));
                }
                Vector2 direction = Main.player[NPC.target].Center - NPC.Center;
                direction.Normalize();
                direction.X *= 2.7f;
                direction.Y *= 2.7f;
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Projectile.NewProjectile(NPC.Center.X + (10 * NPC.spriteDirection), NPC.Center.Y - 12, direction.X, direction.Y, ModContent.ProjectileType<ScreechOwlNote>(), 9, 1, Main.myPlayer, 0, 0);
					for (int i = 0; i < 10; i++)
                    {
                        float angle = Main.rand.NextFloat(MathHelper.PiOver4, -MathHelper.Pi - MathHelper.PiOver4);
                        Vector2 spawnPlace = Vector2.Normalize(new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle))) * 20f;
                        int dustIndex = Dust.NewDust(new Vector2(NPC.Center.X + (10 * NPC.spriteDirection), NPC.Center.Y - 12), 2, 2, DustID.DungeonSpirit, 0f, 0f, 0, default, 1f);
                        Main.dust[dustIndex].noGravity = true;
                        Main.dust[dustIndex].velocity = Vector2.Normalize(spawnPlace.RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi))) * 1.6f;
                    }
                }
            }
			if (NPC.ai[2] > 330)
            {
                NPC.ai[2] = 0f;
                NPC.ai[1] = 0f;
                NPC.netUpdate = true;
            }
			if (NPC.ai[1] == 1f)
            {
                NPC.aiStyle = 0;
                NPC.velocity.X = 0;
                NPC.velocity.Y *= .5f;
                NPC.rotation = 0f;
                if (player.position.X > NPC.position.X)
                    NPC.spriteDirection = 1;
                else
                    NPC.spriteDirection = -1;
            }
			else
            {
                if (player.position.X > NPC.position.X)
                    NPC.spriteDirection = 1;
                else
                    NPC.spriteDirection = -1;
                NPC.aiStyle = -1;
                float num1 = 4f;
                float moveSpeed = 0.09f;
                NPC.TargetClosest(true);
                Vector2 vector2_1 = Main.player[NPC.target].Center - NPC.Center + new Vector2(0.0f, Main.rand.NextFloat(-250f, 0f));
                float num2 = vector2_1.Length();
                Vector2 desiredVelocity;
                if ((double)num2 < 20.0)
                    desiredVelocity = NPC.velocity;
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
                NPC.SimpleFlyMovement(desiredVelocity, moveSpeed);
                NPC.rotation = NPC.velocity.X * 0.055f;
                if (NPC.collideX)
                {
                    NPC.velocity.X = NPC.oldVelocity.X * -0.5f;
                    if (NPC.direction == -1 && NPC.velocity.X > 0f && NPC.velocity.X < 2f)
                        NPC.velocity.X = 2f;
                    if (NPC.direction == 1 && NPC.velocity.X < 0f && NPC.velocity.X > -2f)
                        NPC.velocity.X = -2f;
                }
                if (NPC.collideY)
                {
                    NPC.velocity.Y = NPC.oldVelocity.Y * -0.5f;
                    if (NPC.velocity.Y > 0f && NPC.velocity.Y < 1f)
                        NPC.velocity.Y = 1f;
                    if (NPC.velocity.Y < 0f && NPC.velocity.Y > -1f)
                        NPC.velocity.Y = -1f;
                }
            }
        }

		public override float SpawnChance(NPCSpawnInfo spawnInfo) => spawnInfo.SpawnTileY < Main.worldSurface && spawnInfo.Player.ZoneSnow && !Main.dayTime && !spawnInfo.PlayerSafe ? 0.05f : 0f;
		public override void FindFrame(int frameHeight) => NPC.frame.Y = frameHeight * frame;

		public override void HitEffect(int hitDirection, double damage)
        {
            if (NPC.life <= 0)
				for (int i = 1; i < 5; ++i)
					Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/ScreechOwl/ScreechOwl" + i).Type, 1f);
        }
	}
}
