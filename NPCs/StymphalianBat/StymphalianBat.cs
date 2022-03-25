using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.IO;
using SpiritMod.Buffs;
using SpiritMod.Mechanics.BoonSystem;

namespace SpiritMod.NPCs.StymphalianBat
{
    public class StymphalianBat : ModNPC, IBoonable
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Stymphalian Bat");
            Main.npcFrameCount[npc.type] = 7;
            NPCID.Sets.TrailCacheLength[npc.type] = 2;
            NPCID.Sets.TrailingMode[npc.type] = 0;
        }

        public override void SetDefaults()
        {
            npc.width = 50;
            npc.height = 40;
            npc.damage = 50;
            npc.defense = 21;
            npc.lifeMax = 155;
            npc.knockBackResist = .23f;
            npc.noGravity = true;
            npc.value = 560f;
            npc.noTileCollide = false;
            npc.HitSound = SoundID.NPCHit4;
            npc.DeathSound = SoundID.NPCDeath4;
			npc.buffImmune[BuffID.Poisoned] = true;
			npc.buffImmune[BuffID.Venom] = true;
			npc.buffImmune[ModContent.BuffType<FesteringWounds>()] = true;
			npc.buffImmune[ModContent.BuffType<BCorrupt>()] = true;
			npc.buffImmune[ModContent.BuffType<BloodInfusion>()] = true;
			banner = npc.type;
			bannerItem = ModContent.ItemType<Items.Banners.StymphalianBatBanner>();
		}

        int frame;

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(npc.localAI[0]);
			writer.Write(npc.localAI[1]);
			writer.Write(npc.localAI[2]);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			npc.localAI[0] = reader.ReadSingle();
			npc.localAI[1] = reader.ReadSingle();
			npc.localAI[2] = reader.ReadSingle();
		}
		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if (Main.rand.Next(3) == 0)
			{
				target.AddBuff(BuffID.Bleeding, 3600);
			}
		}
		public override void AI()
        {
			if (npc.ai[3] == 0)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    int npc1 = NPC.NewNPC((int)npc.Center.X + Main.rand.Next(-10, 10), (int)npc.Center.Y+ Main.rand.Next(-10, 10), ModContent.NPCType<StymphalianBat>(), npc.whoAmI, 40f, 0f, 0f, 1f);
                    npc.ai[3] = 1f;
                    npc.netUpdate = true;
                    Main.npc[npc1].netUpdate = true;
                }
            }

            npc.spriteDirection = npc.direction;
			Player target = Main.player[npc.target];

            if (npc.ai[1] != 1f)
                npc.rotation = npc.velocity.X * .1f;
            else
            {
                if (npc.direction == 1)
                    npc.rotation = (float)Math.Sqrt((npc.velocity.X * npc.velocity.X) + (npc.velocity.Y * npc.velocity.Y)) * .1f;             
                else
                    npc.rotation = (float)Math.Sqrt((npc.velocity.X * npc.velocity.X) + (npc.velocity.Y * npc.velocity.Y)) * .1f - 1.57f;             
            }
            
            npc.ai[0]++;
            if (!target.dead && npc.ai[1] < 1f)
            {
                if (npc.collideX)
                {
                    npc.velocity.X = npc.oldVelocity.X * -0.5f;
                    if (npc.direction == -1 && npc.velocity.X > 0f && npc.velocity.X < 2f)
                        npc.velocity.X = 2f;
                    if (npc.direction == 1 && npc.velocity.X < 0f && npc.velocity.X > -2f)
                        npc.velocity.X = -2f;
                }
                if (npc.collideY)
                {
                    npc.velocity.Y = npc.oldVelocity.Y * -0.5f;
                    if (npc.velocity.Y > 0f && npc.velocity.Y < 1f)
                        npc.velocity.Y = 1f;
                    if (npc.velocity.Y < 0f && npc.velocity.Y > -1f)
                        npc.velocity.Y = -1f;
                }

                npc.TargetClosest(true);

                if (npc.direction == -1 && npc.velocity.X > -7f)
                {
                    npc.velocity.X = npc.velocity.X - 0.26f;
                    if (npc.velocity.X > 7f)
                        npc.velocity.X = npc.velocity.X - 0.26f;
                    else if (npc.velocity.X > 0f)
                        npc.velocity.X = npc.velocity.X - 0.05f;
                    if (npc.velocity.X < -7f)
                        npc.velocity.X = -7f;
                }
                else if (npc.direction == 1 && npc.velocity.X < 7f)
                {
                    npc.velocity.X = npc.velocity.X + 0.26f;
                    if (npc.velocity.X < -7f)
                        npc.velocity.X = npc.velocity.X + 0.26f;
                    else if (npc.velocity.X < 0f)
                        npc.velocity.X = npc.velocity.X + 0.05f;
                    if (npc.velocity.X > 7f)
                        npc.velocity.X = 7f;
                }

                float num3225 = Math.Abs(npc.Center.X - target.Center.X);
                float num3224 = target.position.Y - (npc.height / 2f);

                if (num3225 > 50f)
                    num3224 -= 150f;
                if (npc.position.Y < num3224)
                {
                    npc.velocity.Y = npc.velocity.Y + 0.05f;
                    if (npc.velocity.Y < 0f)
                        npc.velocity.Y = npc.velocity.Y + 0.01f;
                }
                else
                {
                    npc.velocity.Y = npc.velocity.Y - 0.05f;
                    if (npc.velocity.Y > 0f)
                        npc.velocity.Y = npc.velocity.Y - 0.01f;
                }
                if (npc.velocity.Y < -4f)
                    npc.velocity.Y = -4f;
                if (npc.velocity.Y > 4f)
                    npc.velocity.Y = 3f;
            }
            if ((npc.collideX || npc.collideY) && npc.ai[1] == 1f)
            {
                npc.velocity = Vector2.Zero;
                npc.noGravity = false;
                frame = 6;
                npc.netUpdate = true;

 				if (Main.netMode != NetmodeID.Server)
				{
					npc.rotation += Main.rand.NextFloat(-0.06f,0.06f);
                    drawOffsetY = 15;
				}
            }

            Vector2 direction = target.Center - npc.Center;

            if (npc.ai[0] == 190)
            {
                npc.ai[1] = 1f;
                npc.netUpdate = true;
            }
            npc.localAI[0]++;
            if (npc.localAI[0] >= 6)
            {
                frame++;
                npc.localAI[0] = 0;
                npc.netUpdate = true;
            }
            if (frame > 5)
                frame = 0;
            if (npc.ai[1] == 1f)
            {
                frame = 6;
				if (npc.ai[2] == 0)
                {
                    direction.Normalize();
                    Main.PlaySound(SoundID.DD2_WyvernDiveDown, npc.Center);
                    direction.X *= Main.rand.Next(16, 22);
                    direction.Y *= Main.rand.Next(10, 15);
                    npc.velocity.X = direction.X;
                    npc.velocity.Y = direction.Y;
                    npc.ai[2]++;
                }
            }
	        if (npc.ai[0] > 265)
            {
                npc.ai[0] = 0f;
                npc.ai[1] = 0f;
                npc.ai[2] = 0f;
                npc.netUpdate = true;
                npc.noGravity = true;
                drawOffsetY = 0;
            }
        }

		public override float SpawnChance(NPCSpawnInfo spawnInfo) {

			int x = spawnInfo.spawnTileX;
			int y = spawnInfo.spawnTileY;
			int tile = (int)Main.tile[x, y].type;
			return (tile == 367) && spawnInfo.player.GetSpiritPlayer().ZoneMarble && spawnInfo.spawnTileY > Main.rockLayer && Main.hardMode ? 0.435f : 0f;
        } 

		public override void HitEffect(int hitDirection, double damage)
        {
            for (int k = 0; k < 10; k++)
            {
                Dust.NewDust(npc.position, npc.width, npc.height, DustID.Blood, hitDirection * 2.5f, -1f, 0, default, Main.rand.NextFloat(.45f, 1.15f));
                Dust.NewDust(npc.position, npc.width, npc.height, DustID.Wraith, 2.5f * hitDirection, -2.5f, 0, default, 0.27f);
            }
            if (npc.life <= 0)
            {
				for (int i = 0; i < 3; ++i)
					Gore.NewGore(npc.position, npc.velocity, 99);
				for (int i = 1; i < 4; ++i)
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/StymphalianBat/StymphalianBat" + i), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/StymphalianBat/StymphalianBat1"), 1f);
            }
        }
		public override void NPCLoot()
		{
			if (Main.rand.NextBool(100))
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.AdhesiveBandage);
			if (Main.rand.NextBool(85))
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.Accessory.GoldenApple>());
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if (npc.ai[2] == 1f && !npc.collideX && !npc.collideY)
            {
                Vector2 drawOrigin = new Vector2(Main.npcTexture[npc.type].Width * 0.5f, (npc.height * 0.5f));
                for (int k = 0; k < npc.oldPos.Length; k++)
                {
                    var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
                    Vector2 drawPos = npc.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, npc.gfxOffY);
                    Color color = npc.GetAlpha(lightColor) * (float)(((npc.oldPos.Length - k) / (float)npc.oldPos.Length) / 2f);
                    spriteBatch.Draw(Main.npcTexture[npc.type], drawPos, new Microsoft.Xna.Framework.Rectangle?(npc.frame), color, npc.rotation, drawOrigin, npc.scale, effects, 0f);
                }
            }
            return true;
        }

		public override void FindFrame(int frameHeight) => npc.frame.Y = frameHeight * frame;
	}
}