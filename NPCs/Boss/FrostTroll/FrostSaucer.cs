using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Accessory;
using SpiritMod.Items.DonatorItems.FrostTroll;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
        	NPCID.Sets.TrailCacheLength[npc.type] = 5;
			NPCID.Sets.TrailingMode[npc.type] = 0;
		}

		public override void SetDefaults()
		{
			npc.width = 344;
			npc.height = 298;
			npc.damage = 45;
			npc.lifeMax = 3800;
			npc.knockBackResist = 0;

			npc.boss = true;
			npc.noGravity = true;
			npc.noTileCollide = true;

			npc.HitSound = SoundID.NPCHit4;
			npc.DeathSound = SoundID.NPCDeath5;
		}

		private int Counter;
        bool trailBehind;
        bool canHitPlayer;
        public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
                Main.PlaySound(2, npc.Center, 14);
                for (int num625 = 0; num625 < 10; num625++)
                {
                    float scaleFactor10 = 0.2f;
                    if (num625 == 1) {
                        scaleFactor10 = 0.5f;
                    }
                    if (num625 == 2) {
                        scaleFactor10 = 1f;
                    }
                    int num626 = Gore.NewGore(new Vector2(npc.Center.X + Main.rand.Next(-100, 100), npc.Center.Y + Main.rand.Next(-100, 100)), default(Vector2), Main.rand.Next(61, 64), 1f);
                    Main.gore[num626].velocity *= scaleFactor10;
                    Gore expr_13AB6_cp_0 = Main.gore[num626];
                    expr_13AB6_cp_0.velocity.X = expr_13AB6_cp_0.velocity.X + 1f;
                    Gore expr_13AD6_cp_0 = Main.gore[num626];
                    expr_13AD6_cp_0.velocity.Y = expr_13AD6_cp_0.velocity.Y + 1f;
                    num626 = Gore.NewGore(new Vector2(npc.Center.X + Main.rand.Next(-100, 100), npc.Center.Y + Main.rand.Next(-100, 100)), default(Vector2), Main.rand.Next(61, 64), 1f);
                    Main.gore[num626].velocity *= scaleFactor10;
                    Gore expr_13B79_cp_0 = Main.gore[num626];
                    expr_13B79_cp_0.velocity.X = expr_13B79_cp_0.velocity.X - 1f;
                    Gore expr_13B99_cp_0 = Main.gore[num626];
                    expr_13B99_cp_0.velocity.Y = expr_13B99_cp_0.velocity.Y + 1f;
                }
                for (int j = 0; j < 17; j++)
                {
                    Dust.NewDust(npc.position, npc.width, npc.height, 226, 2.5f * hitDirection, -2.5f, 0, default(Color), 0.75f);
                }               
			}
			for (int k = 0; k < 7; k++)
			{
				Dust.NewDust(npc.position, npc.width, npc.height, 226, 2.5f * hitDirection, -2.5f, 0, default(Color), 0.5f);
			}
		}
		public override void AI()
        {
    		bool expertMode = Main.expertMode;
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
				if (npc.ai[0] > 580 && npc.ai[0] < 638)
                {
                    trailBehind = true;
                    canHitPlayer = true;
                    numYPos = 0f;
                    num1 = 10.5f;
                    moveSpeed = .4f;
                }
				else
                {
                    canHitPlayer = false;
                    trailBehind = false;
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
            if (npc.ai[0] == 60 || npc.ai[0] == 150 || npc.ai[0] == 220 || npc.ai[0] == 280)
            {
                for (int i = 0; i < 12; i++)
                {
                    Dust dust = Dust.NewDustDirect(npc.Center + new Vector2(-80, 70), npc.width, npc.height, 68);
                    dust.velocity *= -1f;
                    dust.scale *= .8f;
                    dust.noGravity = true;
                    Vector2 vector2_1 = new Vector2(Main.rand.Next(-80, 81), Main.rand.Next(-80, 81));
                    vector2_1.Normalize();
                    Vector2 vector2_2 = vector2_1 * (Main.rand.Next(50, 100) * 0.04f);
                    dust.velocity = vector2_2;
                    vector2_2.Normalize();
                    Vector2 vector2_3 = vector2_2 * 34f;
                    dust.position = npc.Center + new Vector2(-80, 70) - vector2_3;

                    Dust dust1 = Dust.NewDustDirect(npc.Center + new Vector2(80, 70), npc.width, npc.height, 68);
                    dust1.velocity *= -1f;
                    dust1.scale *= .8f;
                    dust1.noGravity = true;
                    dust1.velocity = vector2_2;
                    dust1.position = npc.Center + new Vector2(80, 70) - vector2_3; 
                }               

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {                  
                    Vector2 direction = Main.player[npc.target].Center - (npc.Center + new Vector2(80, 70));
                    direction.Normalize();
                    direction.X *= 7.5f;
                    direction.Y *= 7.5f;
                    int somedamage = expertMode ? 25 : 38;
                    int p = Projectile.NewProjectile(npc.Center.X + 80, npc.Center.Y + 70, direction.X, direction.Y, ProjectileID.IceBolt, somedamage, 1, Main.myPlayer, 0, 0);
                    Main.projectile[p].hostile = true;
                    Main.projectile[p].friendly = false;
                    Main.projectile[p].timeLeft = 280;

                    Vector2 direction1 = Main.player[npc.target].Center - (npc.Center + new Vector2(-80, 70));  
                    direction1.Normalize();
                    direction1.X *= 7.5f;
                    direction1.Y *= 7.5f;                  
                    int p1 = Projectile.NewProjectile(npc.Center.X - 80, npc.Center.Y + 70, direction1.X, direction1.Y, ProjectileID.IceBolt, somedamage, 1, Main.myPlayer, 0, 0);
                    Main.projectile[p1].hostile = true;
                    Main.projectile[p1].friendly = false;  
                    Main.projectile[p1].timeLeft = 280;  
                } 
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
                Main.PlaySound(2, npc.Center, 91);
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    int p = Projectile.NewProjectile(npc.Center.X, npc.Center.Y + 70, 0, 26, ModContent.ProjectileType<SnowMongerBeam>(), 45, 1, Main.myPlayer, 0, 0);                  
                }
            }
            if (npc.ai[0] == 570)
            {
                Main.PlaySound(15, npc.Center, 0);
            }
			if (npc.ai[0] >= 650)
            {
                npc.ai[0] = 0;
                npc.netUpdate = true;
            }
		}
        public override bool CanHitPlayer(Player target, ref int cooldownSlot) => canHitPlayer;
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(trailBehind);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			trailBehind = reader.ReadBoolean();
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
                    Main.ReportInvasionProgress(Main.invasionSizeStart - Main.invasionSize, Main.invasionSizeStart, 1, 0);
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

			if (trailBehind) {
				for (int k = 0; k < npc.oldPos.Length; k++) {
					Vector2 drawPos = npc.oldPos[k] - Main.screenPosition + new Vector2(npc.width/2, npc.height/2) + new Vector2(-10 * npc.spriteDirection, npc.gfxOffY - 16).RotatedBy(npc.rotation);
					Color color = npc.GetAlpha(drawColor) * (float)(((float)(npc.oldPos.Length - k) / (float)npc.oldPos.Length) / 2);
					spriteBatch.Draw(Main.npcTexture[npc.type], drawPos, new Microsoft.Xna.Framework.Rectangle?(npc.frame), color, npc.rotation, drawOrigin, npc.scale, effects, 0f);
				}
			}
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
