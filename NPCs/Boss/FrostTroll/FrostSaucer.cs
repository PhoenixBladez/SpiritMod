using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Accessory;
using SpiritMod.Items.DonatorItems.FrostTroll;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.FrostTroll
{
	[AutoloadBossHead]
	public class FrostSaucer : ModNPC
	{
		int timer = 0;
		int moveSpeed = 0;
		int moveSpeedY = 0;
		float HomeY = 150f;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Snow Monger");
		}

		public override void SetDefaults()
		{
			npc.width = 344;
			npc.height = 298;
			npc.damage = 0;
			npc.lifeMax = 4500;
			npc.knockBackResist = 0;

			npc.boss = true;
			npc.noGravity = true;
			npc.noTileCollide = true;

			npc.HitSound = SoundID.NPCHit7;
			npc.DeathSound = SoundID.NPCDeath5;
		}

		private int Counter;
		public override void AI()
        {
            float numYPos = -280f;
            npc.ai[0]++;
            Lighting.AddLight(new Vector2(npc.Center.X, npc.Center.Y), 0.075f, 0.231f, 0.255f);
            Player player = Main.player[npc.target];
            if (npc.ai[0] < 420 || npc.ai[0] > 490)
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
                float num1 = 4.7f;
                float moveSpeed = 0.13f;
                if (npc.ai[0] > 320 && npc.ai[0] < 400)
                {
                    num1 = 14f;
                    moveSpeed = .5f;
                }
				if (npc.ai[0] > 580 && npc.ai[0] < 625)
                {
                    numYPos = -0f;
                    num1 = 10f;
                    moveSpeed = .4f;
                    npc.damage = 45;
                }
				else
                {
                    npc.damage = 0;
                }
                npc.TargetClosest(true);
                Vector2 vector2_1 = Main.player[npc.target].Center - npc.Center + new Vector2(0.0f, numYPos);
                float num2 = vector2_1.Length();
                Vector2 desiredVelocity;
                if ((double)num2 < 20.0)
                    desiredVelocity = npc.velocity;
                else if ((double)num2 < 40.0)
                {
                    vector2_1.Normalize();
                    desiredVelocity = vector2_1 * (num1 * 0.45f);
                }
                else if ((double)num2 < 80.0)
                {
                    vector2_1.Normalize();
                    desiredVelocity = vector2_1 * (num1 * 0.75f);
                }
                else
                {
                    vector2_1.Normalize();
                    desiredVelocity = vector2_1 * num1;
                }
                npc.SimpleFlyMovement(desiredVelocity, moveSpeed);
            }
			if (npc.ai[0] == 420)
            {
                npc.velocity = Vector2.Zero;
                npc.netUpdate = true;
            }
			if (npc.ai[0] > 420 && npc.ai[0] < 445)
            {
                if (Main.netMode != NetmodeID.Server)
                {
                    Dust dust = Dust.NewDustDirect(new Vector2(npc.Center.X, npc.Center.Y + 120), npc.width, npc.height, 226);
                    dust.velocity *= -1f;
                    dust.scale *= .8f;
                    dust.noGravity = true;
                    Vector2 vector2_1 = new Vector2(Main.rand.Next(-80, 81), Main.rand.Next(-80, 81));
                    vector2_1.Normalize();
                    Vector2 vector2_2 = vector2_1 * (Main.rand.Next(50, 100) * 0.04f);
                    dust.velocity = vector2_2;
                    vector2_2.Normalize();
                    Vector2 vector2_3 = vector2_2 * 34f;
                    dust.position = new Vector2(npc.Center.X, npc.Center.Y + 120) - vector2_3;
                }
            }
			if (npc.ai[0] == 445 || npc.ai[0] == 455 || npc.ai[0] == 465)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    int p = Projectile.NewProjectile(npc.Center.X, npc.Center.Y + 60, 0, 26, ModContent.ProjectileType<SnowMongerBeam>(), 9, 1, Main.myPlayer, 0, 0);
                    
                }
            }
			if (npc.ai[0] >= 630)
            {
                npc.ai[0] = 0;
                npc.netUpdate = true;
            }
		}

		public override void NPCLoot()
		{
            if (Main.invasionType == 2)
            {
                Main.invasionSize -= 10;
                if (Main.invasionSize < 0)
                {
                    Main.invasionSize = 0;
                }
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Main.ReportInvasionProgress(Main.invasionSizeStart - Main.invasionSize, Main.invasionSizeStart, 2, 0);
                }
                if (Main.netMode == NetmodeID.Server)
                {
                    NetMessage.SendData(MessageID.InvasionProgressReport, -1, -1, null, Main.invasionProgress, (float)Main.invasionProgressMax, (float)Main.invasionProgressIcon, 0f, 0, 0, 0);
                }
            }
            int[] lootTable = {
				ModContent.ItemType<Bauble>(),
				ModContent.ItemType<BlizzardEdge>(),
				ModContent.ItemType<Chillrend>(),
				ModContent.ItemType<ShiverWind>()
			};
			int loot = Main.rand.Next(lootTable.Length);
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, lootTable[loot]);

			for (int i = 0; i < 15; ++i) {
				if (Main.rand.Next(8) == 0) {
					int newDust = Dust.NewDust(npc.position, npc.width, npc.height, 76, 0f, 0f, 100, default(Color), 2.5f);
					Main.dust[newDust].noGravity = true;
					Main.dust[newDust].velocity *= 5f;
				}
			}
		}
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame,
                             drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
            Vector2 vector2_3 = new Vector2((float)(Main.npcTexture[npc.type].Width / 2), (float)(Main.npcTexture[npc.type].Height / Main.npcFrameCount[npc.type] / 2));
            Vector2 drawOrigin = new Vector2(Main.npcTexture[npc.type].Width * 0.5f, (npc.height / Main.npcFrameCount[npc.type]) * 0.5f);

            return false;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            spriteBatch.Draw(
                mod.GetTexture("NPCs/Boss/FrostTroll/FrostSaucerGlass"),
                npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY),
                npc.frame,
                new Color(100, 100, 100, 100),
                npc.rotation,
                npc.frame.Size() / 2,
                npc.scale,
                effects,
                0
            );
            spriteBatch.Draw(
                mod.GetTexture("NPCs/Boss/FrostTroll/FrostSaucerPenguin"),
                npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY),
                npc.frame,
                new Color(100, 100, 100),
                npc.rotation,
                npc.frame.Size() / 2,
                npc.scale,
                effects,
                0
            );
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return Main.invasionType == 2 && !NPC.AnyNPCs(ModContent.NPCType<FrostSaucer>()) ? 0.018f : 0f;
		}

		public override void BossLoot(ref string name, ref int potionType)
		{
			potionType = ItemID.GreaterHealingPotion;
		}
	}
}
