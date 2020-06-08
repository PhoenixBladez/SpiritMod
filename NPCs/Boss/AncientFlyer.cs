using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.BossBags;
using SpiritMod.Items.Material;
using SpiritMod.Projectiles.Boss;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss
{
    [AutoloadBossHead]
    public class AncientFlyer : ModNPC
    {
        public static int _type;

        int timer = 0;
        int moveSpeed = 0;
        int moveSpeedY = 0;
        float HomeY = 330f;
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Ancient Avian");
            Main.npcFrameCount[npc.type] = 6;
            NPCID.Sets.TrailCacheLength[npc.type] = 3;
            NPCID.Sets.TrailingMode[npc.type] = 0;
        }

        public override void SetDefaults() {
            npc.width = 148;
            npc.height = 120;
            npc.damage = 23;
            npc.defense = 14;
            npc.lifeMax = 3100;
            npc.knockBackResist = 0;
            npc.boss = true;
            npc.noGravity = true;
            music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/AncientAvian");
            npc.noTileCollide = true;
            npc.npcSlots = 5;
            bossBag = ModContent.ItemType<FlyerBag>();
            npc.HitSound = SoundID.NPCHit2;
            npc.DeathSound = SoundID.NPCDeath5;
            npc.scale = 1.1f;
        }
        bool displaycircle = false;
        private int Counter;
        float framenum = .2f;
        public override bool PreAI() {
            Counter++;
            int npcType = mod.NPCType("BoneHarpy1");
            bool harpySpawn = false;
            for(int num569 = 0; num569 < 200; num569++) {
                if((Main.npc[num569].active && Main.npc[num569].type == (npcType)))
                    harpySpawn = true;
            }

            if(!harpySpawn) {
                if(Counter > 1200) {
                    Vector2 direction = Vector2.One.RotatedByRandom(MathHelper.ToRadians(100));
                    int newNPC = NPC.NewNPC((int)npc.position.X, (int)npc.position.Y, mod.NPCType("BoneHarpy1"));
                    Main.npc[newNPC].velocity = direction * (Main.rand.Next(-2, 2));
                    Counter = 0;
                }
            }
            return true;
        }

        public override void AI() {
            npc.spriteDirection = -npc.direction;
            npc.rotation = npc.velocity.X * 0.07f;
            bool expertMode = Main.expertMode;
            Player player = Main.player[npc.target];
            MyPlayer modPlayer = player.GetSpiritPlayer();
            if(npc.Center.X >= player.Center.X && moveSpeed >= -120) // flies to players x position
                moveSpeed--;
            else if(npc.Center.X <= player.Center.X && moveSpeed <= 120)
                moveSpeed++;

            npc.velocity.X = moveSpeed * 0.10f;

            if(npc.Center.Y >= player.Center.Y - HomeY && moveSpeedY >= -30) //Flies to players Y position
            {
                moveSpeedY--;
                HomeY = 350f;
            } else if(npc.Center.Y <= player.Center.Y - HomeY && moveSpeedY <= 30)
                moveSpeedY++;

            npc.velocity.Y = moveSpeedY * 0.13f;

            timer++;
            if(timer == 200 || timer == 400 && npc.life >= (npc.lifeMax / 2)) //Fires desert feathers like a shotgun
            {
                Main.PlaySound(2, (int)npc.position.X, (int)npc.position.Y, 73);

                Main.PlaySound(3, (int)npc.position.X, (int)npc.position.Y, 2);
                Vector2 direction = Main.player[npc.target].Center - npc.Center;
                direction.Normalize();
                direction.X *= 11f;
                direction.Y *= 11f;

                int amountOfProjectiles = Main.rand.Next(8, 11);
                for(int i = 0; i < amountOfProjectiles; ++i) {
                    float A = (float)Main.rand.Next(-200, 200) * 0.01f;
                    float B = (float)Main.rand.Next(-200, 200) * 0.01f;
                    int damage = expertMode ? 15 : 17;
                    Projectile.NewProjectile(npc.Center.X, npc.Center.Y, direction.X + A, direction.Y + B, ModContent.ProjectileType<DesertFeather>(), damage, 1, Main.myPlayer, 0, 0);
                }
            } else if(timer == 300 || timer == 400 || timer == 500 || timer == 550) {
                if(Main.expertMode && npc.life >= (npc.lifeMax / 2)) {
                    Main.PlaySound(2, (int)npc.position.X, (int)npc.position.Y, 8);

                    Main.PlaySound(3, (int)npc.position.X, (int)npc.position.Y, 2);

                    Vector2 direction = Main.player[npc.target].Center - npc.Center;
                    direction.Normalize();
                    direction.X *= 11.5f;
                    direction.Y *= 11.5f;

                    int amountOfProjectiles = Main.rand.Next(5, 9);
                    for(int i = 0; i < amountOfProjectiles; ++i) {
                        float A = (float)Main.rand.Next(-300, 300) * 0.01f;
                        float B = (float)Main.rand.Next(-300, 300) * 0.01f;
                        int damage = expertMode ? 18 : 20;
                        Projectile.NewProjectile(npc.Center.X, npc.Center.Y, direction.X + A, direction.Y + B, ModContent.ProjectileType<ExplodingFeather>(), damage, 1, Main.myPlayer, 0, 0);
                    }
                }
            } else if(timer == 600 || timer == 650 || timer == 700 || timer == 800 || timer == 850 || timer == 880) // Fires bone waves
              {
                Main.PlaySound(2, (int)npc.position.X, (int)npc.position.Y, 8);

                Vector2 direction = Main.player[npc.target].Center - npc.Center;
                direction.Normalize();
                int damage = expertMode ? 15 : 19;
                Projectile.NewProjectile(npc.Center.X, npc.Center.Y, direction.X * 12f, direction.Y * 12f, ModContent.ProjectileType<BoneWave>(), damage, 1, Main.myPlayer, 0, 0);
            }
            if(timer == 500 || timer == 700) {
                HomeY = -35f;
            }
            if(timer == 900) {
                Main.PlaySound(SoundLoader.customSoundType, player.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/AvianScreech"));
            } else if(timer >= 900 && timer <= 1400) //Rains red comets
              {
                npc.defense = 30;
                if(expertMode) {
                    player.AddBuff(BuffID.WindPushed, 90);
                    modPlayer.windEffect = true;
                }
                framenum = .4f;
                {
                    npc.velocity = Vector2.Zero;
                }
                if(Main.rand.Next(9) == 0) {
                    Main.PlaySound(2, (int)npc.position.X, (int)npc.position.Y, 8);

                    int A = Main.rand.Next(-200, 200) * 6;
                    int B = Main.rand.Next(-200, 200) - 1000;
                    int damage = expertMode ? 18 : 22;
                    Projectile.NewProjectile(player.Center.X + A, player.Center.Y + B, 0f, 10f, ModContent.ProjectileType<RedComet>(), damage, 1, Main.myPlayer, 0, 0);
                }
                displaycircle = true;
            } else {
                npc.defense = 14;
                framenum = .2f;
                displaycircle = false;
                modPlayer.windEffect = false;
            }

            if(timer >= 1400) {
                timer = 0;
            }
            if(npc.life == 3000) {
                Main.PlaySound(15, (int)npc.position.X, (int)npc.position.Y, 0);
            }
            if(Main.expertMode && npc.life <= 3000) //Fires comets when low on health in expert
            {
                player.AddBuff(BuffID.WindPushed, 90);
                modPlayer.windEffect = true;
                if(Main.rand.Next(22) == 0) {
                    int A = Main.rand.Next(-2500, 2500) * 2;
                    int B = Main.rand.Next(-1000, 1000) - 700;
                    int damage = expertMode ? 15 : 17;
                    Projectile.NewProjectile(player.Center.X + A, player.Center.Y + B, 0f, 10f, ModContent.ProjectileType<RedComet>(), damage, 1, Main.myPlayer, 0, 0);
                }
            }

            if(!player.active || player.dead) //despawns when player is ded
            {
                npc.TargetClosest(false);
                npc.velocity.Y = -50;
                timer = 0;
            }
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale) {
            npc.lifeMax = (int)(npc.lifeMax * 0.8f * bossLifeScale);
        }
        public override bool PreNPCLoot() {
            MyWorld.downedAncientFlier = true;
            return true;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor) {
            var effects = npc.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            Vector2 vector2_3 = new Vector2((float)(Main.npcTexture[npc.type].Width / 2), (float)(Main.npcTexture[npc.type].Height / Main.npcFrameCount[npc.type] / 2));
            Vector2 drawOrigin = new Vector2(Main.npcTexture[npc.type].Width * 0.5f, (npc.height / Main.npcFrameCount[npc.type]) * 0.5f);
            if(npc.velocity != Vector2.Zero) {
                for(int k = 0; k < npc.oldPos.Length; k++) {
                    Vector2 drawPos = npc.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, npc.gfxOffY);
                    Color color = npc.GetAlpha(lightColor) * (float)(((float)(npc.oldPos.Length - k) / (float)npc.oldPos.Length) / 2);
                    spriteBatch.Draw(Main.npcTexture[npc.type], drawPos, new Microsoft.Xna.Framework.Rectangle?(npc.frame), color, npc.rotation, drawOrigin, npc.scale, effects, 0f);
                }
            }
            if(displaycircle) {
                Microsoft.Xna.Framework.Color color1 = Lighting.GetColor((int)((double)npc.position.X + (double)npc.width * 0.5) / 16, (int)(((double)npc.position.Y + (double)npc.height * 0.5) / 16.0));

                int r1 = (int)color1.R;
                drawOrigin.Y += 30f;
                drawOrigin.Y += 8f;
                --drawOrigin.X;
                Vector2 position1 = npc.Bottom - Main.screenPosition;
                Texture2D texture2D2 = Main.glowMaskTexture[239];
                float num11 = (float)((double)Main.GlobalTime % 1.0 / 1.0);
                float num12 = num11;
                if((double)num12 > 0.5)
                    num12 = 1f - num11;
                if((double)num12 < 0.0)
                    num12 = 0.0f;
                float num13 = (float)(((double)num11 + 0.5) % 1.0);
                float num14 = num13;
                if((double)num14 > 0.5)
                    num14 = 1f - num13;
                if((double)num14 < 0.0)
                    num14 = 0.0f;
                Microsoft.Xna.Framework.Rectangle r2 = texture2D2.Frame(1, 1, 0, 0);
                drawOrigin = r2.Size() / 2f;
                Vector2 position3 = position1 + new Vector2(0.0f, -40f);
                Microsoft.Xna.Framework.Color color3 = new Microsoft.Xna.Framework.Color(252, 3, 50) * 1.6f;
                Main.spriteBatch.Draw(texture2D2, position3, new Microsoft.Xna.Framework.Rectangle?(r2), color3, npc.rotation, drawOrigin, npc.scale * 0.75f, SpriteEffects.None ^ SpriteEffects.FlipHorizontally, 0.0f);
                float num15 = 1f + num11 * 0.75f;
                Main.spriteBatch.Draw(texture2D2, position3, new Microsoft.Xna.Framework.Rectangle?(r2), color3 * num12, npc.rotation, drawOrigin, npc.scale * 0.75f * num15, SpriteEffects.None ^ SpriteEffects.FlipHorizontally, 0.0f);
                float num16 = 1f + num13 * 0.75f;
                Main.spriteBatch.Draw(texture2D2, position3, new Microsoft.Xna.Framework.Rectangle?(r2), color3 * num14, npc.rotation, drawOrigin, npc.scale * 0.75f * num16, SpriteEffects.None ^ SpriteEffects.FlipHorizontally, 0.0f);
                Texture2D texture2D3 = Main.extraTexture[89];
                Microsoft.Xna.Framework.Rectangle r3 = texture2D3.Frame(1, 1, 0, 0);
                drawOrigin = r3.Size() / 2f;
                Vector2 scale = new Vector2(0.75f, 1f + num16) * 1.5f;
                float num17 = 1f + num13 * 0.75f;


            }
            return true;
        }
        public override void NPCLoot() {
            if(Main.expertMode) {
                npc.DropBossBags();
                return;
            }

            npc.DropItem(ModContent.ItemType<FossilFeather>(), 3, 6);

            string[] lootTable = { "SkeletalonStaff", "Talonginus", "SoaringScapula" };
            int loot = Main.rand.Next(lootTable.Length);
            npc.DropItem(mod.ItemType(lootTable[loot]));

            npc.DropItem(Items.Armor.Masks.FlierMask._type, 1f / 7);
            npc.DropItem(Items.Boss.Trophy2._type, 1f / 10);
        }

        public override void HitEffect(int hitDirection, double damage) {
            int d1 = 1;
            for(int k = 0; k < 30; k++) {
                Dust.NewDust(npc.position, npc.width, npc.height, d1, 2.5f * hitDirection, -2.5f, 0, Color.White, Main.rand.NextFloat(.2f, .8f));
                Dust.NewDust(npc.position, npc.width, npc.height, d1, 2.5f * hitDirection, -2.5f, 0, default(Color), .34f);
            }
            if(npc.life <= 0) {
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Gore1"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Gore2"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Gore3"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Gore4"), 1f);
            }
        }

        public override void FindFrame(int frameHeight) {
            npc.frameCounter += framenum;
            npc.frameCounter %= Main.npcFrameCount[npc.type];
            int frame = (int)npc.frameCounter;
            npc.frame.Y = frame * frameHeight;
        }
    }
}
