using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.IO;
using SpiritMod.Buffs;
using SpiritMod.Mechanics.BoonSystem;
using SpiritMod.Buffs.DoT;

namespace SpiritMod.NPCs.StymphalianBat
{
    public class StymphalianBat : ModNPC, IBoonable
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Stymphalian Bat");
            Main.npcFrameCount[NPC.type] = 7;
            NPCID.Sets.TrailCacheLength[NPC.type] = 2;
            NPCID.Sets.TrailingMode[NPC.type] = 0;
        }

        public override void SetDefaults()
        {
            NPC.width = 50;
            NPC.height = 40;
            NPC.damage = 50;
            NPC.defense = 21;
            NPC.lifeMax = 155;
            NPC.knockBackResist = .23f;
            NPC.noGravity = true;
            NPC.value = 560f;
            NPC.noTileCollide = false;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath4;
			NPC.buffImmune[BuffID.Poisoned] = true;
			NPC.buffImmune[BuffID.Venom] = true;
			NPC.buffImmune[ModContent.BuffType<FesteringWounds>()] = true;
			NPC.buffImmune[ModContent.BuffType<BloodCorrupt>()] = true;
			NPC.buffImmune[ModContent.BuffType<BloodInfusion>()] = true;
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<Items.Banners.StymphalianBatBanner>();
		}

        int frame;

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(NPC.localAI[0]);
			writer.Write(NPC.localAI[1]);
			writer.Write(NPC.localAI[2]);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			NPC.localAI[0] = reader.ReadSingle();
			NPC.localAI[1] = reader.ReadSingle();
			NPC.localAI[2] = reader.ReadSingle();
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
			if (NPC.ai[3] == 0)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    int npc1 = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X + Main.rand.Next(-10, 10), (int)NPC.Center.Y+ Main.rand.Next(-10, 10), ModContent.NPCType<StymphalianBat>(), NPC.whoAmI, 40f, 0f, 0f, 1f);
                    NPC.ai[3] = 1f;
                    NPC.netUpdate = true;
                    Main.npc[npc1].netUpdate = true;
                }
            }

            NPC.spriteDirection = NPC.direction;
			Player target = Main.player[NPC.target];

            if (NPC.ai[1] != 1f)
                NPC.rotation = NPC.velocity.X * .1f;
            else
            {
                if (NPC.direction == 1)
                    NPC.rotation = (float)Math.Sqrt((NPC.velocity.X * NPC.velocity.X) + (NPC.velocity.Y * NPC.velocity.Y)) * .1f;             
                else
                    NPC.rotation = (float)Math.Sqrt((NPC.velocity.X * NPC.velocity.X) + (NPC.velocity.Y * NPC.velocity.Y)) * .1f - 1.57f;             
            }
            
            NPC.ai[0]++;
            if (!target.dead && NPC.ai[1] < 1f)
            {
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

                NPC.TargetClosest(true);

                if (NPC.direction == -1 && NPC.velocity.X > -7f)
                {
                    NPC.velocity.X = NPC.velocity.X - 0.26f;
                    if (NPC.velocity.X > 7f)
                        NPC.velocity.X = NPC.velocity.X - 0.26f;
                    else if (NPC.velocity.X > 0f)
                        NPC.velocity.X = NPC.velocity.X - 0.05f;
                    if (NPC.velocity.X < -7f)
                        NPC.velocity.X = -7f;
                }
                else if (NPC.direction == 1 && NPC.velocity.X < 7f)
                {
                    NPC.velocity.X = NPC.velocity.X + 0.26f;
                    if (NPC.velocity.X < -7f)
                        NPC.velocity.X = NPC.velocity.X + 0.26f;
                    else if (NPC.velocity.X < 0f)
                        NPC.velocity.X = NPC.velocity.X + 0.05f;
                    if (NPC.velocity.X > 7f)
                        NPC.velocity.X = 7f;
                }

                float num3225 = Math.Abs(NPC.Center.X - target.Center.X);
                float num3224 = target.position.Y - (NPC.height / 2f);

                if (num3225 > 50f)
                    num3224 -= 150f;
                if (NPC.position.Y < num3224)
                {
                    NPC.velocity.Y = NPC.velocity.Y + 0.05f;
                    if (NPC.velocity.Y < 0f)
                        NPC.velocity.Y = NPC.velocity.Y + 0.01f;
                }
                else
                {
                    NPC.velocity.Y = NPC.velocity.Y - 0.05f;
                    if (NPC.velocity.Y > 0f)
                        NPC.velocity.Y = NPC.velocity.Y - 0.01f;
                }
                if (NPC.velocity.Y < -4f)
                    NPC.velocity.Y = -4f;
                if (NPC.velocity.Y > 4f)
                    NPC.velocity.Y = 3f;
            }
            if ((NPC.collideX || NPC.collideY) && NPC.ai[1] == 1f)
            {
                NPC.velocity = Vector2.Zero;
                NPC.noGravity = false;
                frame = 6;
                NPC.netUpdate = true;

 				if (Main.netMode != NetmodeID.Server)
				{
					NPC.rotation += Main.rand.NextFloat(-0.06f,0.06f);
                    DrawOffsetY = 15;
				}
            }

            Vector2 direction = target.Center - NPC.Center;

            if (NPC.ai[0] == 190)
            {
                NPC.ai[1] = 1f;
                NPC.netUpdate = true;
            }
            NPC.localAI[0]++;
            if (NPC.localAI[0] >= 6)
            {
                frame++;
                NPC.localAI[0] = 0;
                NPC.netUpdate = true;
            }
            if (frame > 5)
                frame = 0;
            if (NPC.ai[1] == 1f)
            {
                frame = 6;
				if (NPC.ai[2] == 0)
                {
                    direction.Normalize();
                    SoundEngine.PlaySound(SoundID.DD2_WyvernDiveDown, NPC.Center);
                    direction.X *= Main.rand.Next(16, 22);
                    direction.Y *= Main.rand.Next(10, 15);
                    NPC.velocity.X = direction.X;
                    NPC.velocity.Y = direction.Y;
                    NPC.ai[2]++;
                }
            }
	        if (NPC.ai[0] > 265)
            {
                NPC.ai[0] = 0f;
                NPC.ai[1] = 0f;
                NPC.ai[2] = 0f;
                NPC.netUpdate = true;
                NPC.noGravity = true;
                DrawOffsetY = 0;
            }
        }

		public override float SpawnChance(NPCSpawnInfo spawnInfo) {

			int x = spawnInfo.SpawnTileX;
			int y = spawnInfo.SpawnTileY;
			int tile = (int)Main.tile[x, y].TileType;
			return (tile == 367) && spawnInfo.Player.GetSpiritPlayer().ZoneMarble && spawnInfo.SpawnTileY > Main.rockLayer && Main.hardMode ? 0.435f : 0f;
        } 

		public override void HitEffect(int hitDirection, double damage)
        {
            for (int k = 0; k < 10; k++)
            {
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, hitDirection * 2.5f, -1f, 0, default, Main.rand.NextFloat(.45f, 1.15f));
                Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Wraith, 2.5f * hitDirection, -2.5f, 0, default, 0.27f);
            }
            if (NPC.life <= 0)
            {
				for (int i = 0; i < 3; ++i)
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, 99);
				for (int i = 1; i < 4; ++i)
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("SpiritMod/Gores/StymphalianBat/StymphalianBat" + i).Type, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("SpiritMod/Gores/StymphalianBat/StymphalianBat1").Type, 1f);
            }
        }

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.AddCommon(ItemID.AdhesiveBandage, 100);
			npcLoot.AddCommon<Items.Accessory.GoldenApple>(85);
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (NPC.ai[2] == 1f && !NPC.collideX && !NPC.collideY)
            {
                Vector2 drawOrigin = new Vector2(TextureAssets.Npc[NPC.type].Value.Width * 0.5f, (NPC.height * 0.5f));
                for (int k = 0; k < NPC.oldPos.Length; k++)
                {
                    var effects = NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
                    Vector2 drawPos = NPC.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, NPC.gfxOffY);
                    Color color = NPC.GetAlpha(drawColor) * (float)(((NPC.oldPos.Length - k) / (float)NPC.oldPos.Length) / 2f);
                    spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, drawPos, new Microsoft.Xna.Framework.Rectangle?(NPC.frame), color, NPC.rotation, drawOrigin, NPC.scale, effects, 0f);
                }
            }
            return true;
        }

		public override void FindFrame(int frameHeight) => NPC.frame.Y = frameHeight * frame;
	}
}