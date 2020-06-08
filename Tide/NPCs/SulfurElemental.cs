using Microsoft.Xna.Framework;
using SpiritMod.Tiles.Furniture.Reach;
using SpiritMod.NPCs.Critters;
using SpiritMod.Mounts;
using SpiritMod.NPCs.Boss.SpiritCore;
using SpiritMod.Boss.SpiritCore;
using SpiritMod.Buffs.Candy;
using SpiritMod.Buffs.Potion;
using SpiritMod.Projectiles.Pet;
using SpiritMod.Buffs.Pet;
using SpiritMod.Projectiles.Arrow.Artifact;
using SpiritMod.Projectiles.Bullet.Crimbine;
using SpiritMod.Projectiles.Bullet;
using SpiritMod.Projectiles.Magic.Artifact;
using SpiritMod.Projectiles.Summon.Artifact;
using SpiritMod.Projectiles.Summon.LaserGate;
using SpiritMod.Projectiles.Flail;
using SpiritMod.Projectiles.Arrow;
using SpiritMod.Projectiles.Magic;
using SpiritMod.Projectiles.Sword.Artifact;
using SpiritMod.Projectiles.Summon.Dragon;
using SpiritMod.Projectiles.Sword;
using SpiritMod.Projectiles.Thrown.Artifact;
using SpiritMod.Items.Boss;
using SpiritMod.Items.Armor.Masks;
using SpiritMod.Projectiles.Returning;
using SpiritMod.Projectiles.Held;
using SpiritMod.Projectiles.Thrown;
using SpiritMod.Items.Equipment;
using SpiritMod.Projectiles.DonatorItems;
using SpiritMod.Buffs.Mount;
using SpiritMod.Items.Weapon.Yoyo;
using SpiritMod.Projectiles.Yoyo;
using SpiritMod.Items.Weapon.Spear;
using SpiritMod.Items.Weapon.Swung;
using SpiritMod.NPCs.Boss;
using SpiritMod.Items.Material;
using SpiritMod.Items.Pets;
using SpiritMod.Items.Weapon.Summon;
using SpiritMod.Projectiles.Boss;
using SpiritMod.Items.BossBags;
using SpiritMod.Items.Consumable.Fish;
using SpiritMod.Buffs.Summon;
using SpiritMod.Projectiles.Summon;
using SpiritMod.NPCs.Spirit;
using SpiritMod.Items.Consumable;
using SpiritMod.Tiles.Block;
using SpiritMod.Items.Placeable.Furniture;
using SpiritMod.Items.Consumable.Quest;
using SpiritMod.Items.Consumable.Potion;
using SpiritMod.Items.Placeable.IceSculpture;
using SpiritMod.Items.Weapon.Bow;
using SpiritMod.Items.Weapon.Gun;
using SpiritMod.Buffs;
using SpiritMod.Items;
using SpiritMod.Items.Weapon;
using SpiritMod.Items.Weapon.Returning;
using SpiritMod.Items.Weapon.Thrown;
using SpiritMod.Items.Material;
using SpiritMod.Items.Weapon.Magic;
using SpiritMod.Items.Accessory;

using SpiritMod.Items.Accessory.Leather;
using SpiritMod.Items.Ammo;
using SpiritMod.Items.Armor;
using SpiritMod.Dusts;
using SpiritMod.Buffs;
using SpiritMod.Buffs.Artifact;
using SpiritMod.NPCs;
using SpiritMod.NPCs.Asteroid;
using SpiritMod.Projectiles;
using SpiritMod.Projectiles.Hostile;
using SpiritMod.Tiles;
using SpiritMod.Tiles.Ambient;
using SpiritMod.Tiles.Ambient.IceSculpture;
using SpiritMod.Tiles.Ambient.ReachGrass;
using SpiritMod.Tiles.Ambient.ReachMicros;
using SpiritMod.Items.Accessory;
using SpiritMod.Items.Material;
using SpiritMod.Items.Weapon.Magic;
using SpiritMod.Items.Weapon.Spear;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Tide.NPCs
{
    [AutoloadBossHead]
    public class SulfurElemental : ModNPC
    {
        int timer = 0;
        int moveSpeed = 0;
        int moveSpeedY = 0;
        float HomeY = 150f;
        private bool circling;
        int startdist = 0;
        Vector2 target = Vector2.Zero;

        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Lavavent Elemental");
            Main.npcFrameCount[npc.type] = 13;
        }

        public override void SetDefaults() {
            npc.width = 120;
            npc.height = 120;
            npc.damage = 80;
            npc.lifeMax = 7000;
            npc.knockBackResist = 0;
            npc.noGravity = true;
            npc.noTileCollide = true;

            npc.HitSound = SoundID.NPCHit7;
            npc.DeathSound = SoundID.NPCDeath5;
        }

        public override void FindFrame(int frameHeight) {
            npc.frameCounter += 0.15f;
            npc.frameCounter %= Main.npcFrameCount[npc.type];
            int frame = (int)npc.frameCounter;
            npc.frame.Y = frame * frameHeight;
        }

        private int Counter;
        public override bool PreAI() {
            {
                npc.spriteDirection = npc.direction;
            }
            {
                npc.TargetClosest(true);
                Player player = Main.player[npc.target];
                Vector2 delta = player.position - npc.position;
                Lighting.AddLight((int)((npc.position.X + (float)(npc.width / 2)) / 16f), (int)((npc.position.Y + (float)(npc.height / 2)) / 16f), 2.4f, 1.07f, .66f);
                //64 pixel radius

                if(Math.Sqrt((delta.X * delta.X) + (delta.Y * delta.Y)) <= 250f && circling == false) //circle starting
                {
                    npc.ai[1] = 0; //reset rotation
                    npc.aiStyle = -1;
                    circling = true; //launch circle action into effect
                }
                if(Math.Sqrt((delta.X * delta.X) + (delta.Y * delta.Y)) > 250f && circling == false) //normal AI
                {
                    npc.aiStyle = 22;
                    aiType = NPCID.Wraith;
                    Counter++;
                    if(Counter > 50) {
                        Vector2 direction = Main.player[npc.target].Center - npc.Center;
                        direction.Normalize();
                        direction.X *= 5f;
                        direction.Y *= 5f;

                        int amountOfProjectiles = Main.rand.Next(1, 2);
                        for(int i = 0; i < amountOfProjectiles; ++i) {
                            float A = (float)Main.rand.Next(-150, 150) * 0.03f;
                            float B = (float)Main.rand.Next(-150, 150) * 0.03f;
                            Projectile.NewProjectile(npc.Center.X, npc.Center.Y, direction.X, direction.Y, ProjectileID.Fireball, 28, 1, Main.myPlayer, 0, 0);
                            Counter = 0;
                        }
                    }
                    timer++;
                    if(timer == 200) {
                        for(int i = 0; i < 50; ++i) //Create dust before teleport
                        {
                            int dust = Dust.NewDust(npc.position, npc.width, npc.height, 6);
                            Main.dust[dust].scale = 1.5f;
                        }
                        Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 2f, 2f, ProjectileID.InfernoHostileBolt, 30, 1, Main.myPlayer, 0, 0); //Make projectilllllelelelelele
                        npc.position.X = player.position.X + 300f;
                        npc.position.Y = player.position.Y - 300f;
                        Vector2 direction = Main.player[npc.target].Center - npc.Center;
                        direction.Normalize();
                        npc.velocity.Y = direction.Y * 12f;
                        npc.velocity.X = direction.X * 12f;

                        for(int i = 0; i < 50; ++i) //Create dust after teleport
                        {
                            int dust = Dust.NewDust(npc.position, npc.width, npc.height, 6);
                            Main.dust[dust].scale = 1.5f;
                        }
                    }

                    if(timer == 400) {
                        for(int i = 0; i < 50; ++i) //Create dust before teleport
                        {
                            int dust = Dust.NewDust(npc.position, npc.width, npc.height, 6);
                            Main.dust[dust].scale = 1.5f;
                        }
                        Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 2f, 2f, ProjectileID.InfernoHostileBolt, 30, 1, Main.myPlayer, 0, 0); //Make projectilllllelelelelele
                        npc.position.X = player.position.X - 300f;
                        npc.position.Y = player.position.Y - 300f;
                        Vector2 direction = Main.player[npc.target].Center - npc.Center;
                        direction.Normalize();
                        npc.velocity.Y = direction.Y * 12f;
                        npc.velocity.X = direction.X * 12f;

                        for(int i = 0; i < 50; ++i) //Create dust after teleport
                        {
                            int dust = Dust.NewDust(npc.position, npc.width, npc.height, 6);
                            Main.dust[dust].scale = 1.5f;
                        }
                    }
                    if(timer == 600) {
                        for(int i = 0; i < 50; ++i) //Create dust before teleport
                        {
                            int dust = Dust.NewDust(npc.position, npc.width, npc.height, 6);
                            Main.dust[dust].scale = 1.5f;
                        }
                        Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 2f, 2f, ProjectileID.InfernoHostileBolt, 30, 1, Main.myPlayer, 0, 0); //Make projectilllllelelelelele
                        npc.position.X = player.position.X - 300f;
                        npc.position.Y = player.position.Y + 300f;
                        Vector2 direction = Main.player[npc.target].Center - npc.Center;
                        direction.Normalize();
                        npc.velocity.Y = direction.Y * 12f;
                        npc.velocity.X = direction.X * 12f;

                        for(int i = 0; i < 50; ++i) //Create dust after teleport
                        {
                            int dust = Dust.NewDust(npc.position, npc.width, npc.height, 6);
                            Main.dust[dust].scale = 1.5f;
                        }
                    }

                    if(timer >= 600) {
                        timer = 0;
                    }
                    return true;
                }
                if(Math.Sqrt((delta.X * delta.X) + (delta.Y * delta.Y)) > 500f && circling == true) //stop circling
                {
                    circling = false;
                    npc.velocity = Vector2.Zero;
                }
                if(circling == true) {
                    double deg = (double)npc.ai[1]; //The degrees, you can multiply npc.ai[1] to make it orbit faster, may be choppy depending on the value
                    double rad = deg * (Math.PI / 180); //Convert degrees to radians
                    double dist = 250; //Distance away from the player
                    Counter++;
                    if(Counter > 100) {
                        Vector2 direction = Main.player[npc.target].Center - npc.Center;
                        direction.Normalize();
                        direction.X *= 5f;
                        direction.Y *= 5f;

                        int amountOfProjectiles = Main.rand.Next(1, 2);
                        for(int i = 0; i < amountOfProjectiles; ++i) {
                            float A = (float)Main.rand.Next(-150, 150) * 0.03f;
                            float B = (float)Main.rand.Next(-150, 150) * 0.03f;
                            Projectile.NewProjectile(npc.Center.X, npc.Center.Y, direction.X, direction.Y, ProjectileID.InfernoHostileBolt, 40, 1, Main.myPlayer, 0, 0);
                            Counter = 0;
                        }
                    }

                    /*Position the npc based on where the player is, the Sin/Cos of the angle times the /
                    /distance for the desired distance away from the player minus the npc's width   /
                    /and height divided by two so the center of the npc is at the right place.     */
                    target.X = player.Center.X - (int)(Math.Cos(rad) * dist) - npc.width / 2;
                    target.Y = player.Center.Y - (int)(Math.Sin(rad) * dist) - npc.height / 2;

                    //Increase the counter/angle in degrees by 1 point, you can change the rate here too, but the orbit may look choppy depending on the value
                    npc.ai[1] += 1f;
                    Vector2 Vel = target - npc.Center;
                    Vel.Normalize();
                    Vel *= 4f;
                    npc.velocity = Vel;
                    timer++;
                    if(timer == 200) {
                        for(int i = 0; i < 50; ++i) //Create dust before teleport
                        {
                            int dust = Dust.NewDust(npc.position, npc.width, npc.height, 6);
                            Main.dust[dust].scale = 1.5f;
                        }
                        Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 2f, 2f, ProjectileID.InfernoHostileBolt, 30, 1, Main.myPlayer, 0, 0); //Make projectilllllelelelelele
                        npc.position.X = player.position.X + 300f;
                        npc.position.Y = player.position.Y - 300f;
                        Vector2 direction = Main.player[npc.target].Center - npc.Center;
                        direction.Normalize();
                        npc.velocity.Y = direction.Y * 12f;
                        npc.velocity.X = direction.X * 12f;

                        for(int i = 0; i < 50; ++i) //Create dust after teleport
                        {
                            int dust = Dust.NewDust(npc.position, npc.width, npc.height, 6);
                            Main.dust[dust].scale = 1.5f;
                        }
                    }

                    if(timer == 400) {
                        for(int i = 0; i < 50; ++i) //Create dust before teleport
                        {
                            int dust = Dust.NewDust(npc.position, npc.width, npc.height, 6);
                            Main.dust[dust].scale = 1.5f;
                        }
                        Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 2f, 2f, ProjectileID.InfernoHostileBolt, 30, 1, Main.myPlayer, 0, 0); //Make projectilllllelelelelele
                        npc.position.X = player.position.X - 300f;
                        npc.position.Y = player.position.Y - 300f;
                        Vector2 direction = Main.player[npc.target].Center - npc.Center;
                        direction.Normalize();
                        npc.velocity.Y = direction.Y * 12f;
                        npc.velocity.X = direction.X * 12f;

                        for(int i = 0; i < 50; ++i) //Create dust after teleport
                        {
                            int dust = Dust.NewDust(npc.position, npc.width, npc.height, 6);
                            Main.dust[dust].scale = 1.5f;
                        }
                    }
                    if(timer == 600) {
                        for(int i = 0; i < 50; ++i) //Create dust before teleport
                        {
                            int dust = Dust.NewDust(npc.position, npc.width, npc.height, 6);
                            Main.dust[dust].scale = 1.5f;
                        }
                        Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 2f, 2f, ProjectileID.InfernoHostileBolt, 30, 1, Main.myPlayer, 0, 0); //Make projectilllllelelelelele
                        npc.position.X = player.position.X - 300f;
                        npc.position.Y = player.position.Y + 300f;
                        Vector2 direction = Main.player[npc.target].Center - npc.Center;
                        direction.Normalize();
                        npc.velocity.Y = direction.Y * 12f;
                        npc.velocity.X = direction.X * 12f;

                        for(int i = 0; i < 50; ++i) //Create dust after teleport
                        {
                            int dust = Dust.NewDust(npc.position, npc.width, npc.height, 6);
                            Main.dust[dust].scale = 1.5f;
                        }
                    }

                    if(timer >= 600) {
                        timer = 0;
                    }
                    return true;
                }
            }
            return true;
        }

        public override void NPCLoot() {
            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<DepthShard>(), 1);

            int loot = 0;
            switch(Main.rand.Next(3)) {
                case 0:
                    loot = ModContent.ItemType<FierySoul>();
                    break;
                case 1:
                    loot = ModContent.ItemType<LavaStaff>();
                    break;
                case 2:
                    loot = ModContent.ItemType<LavaSpear>();
                    break;
            }
            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, loot);
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo) {
            if(TideWorld.TheTide && TideWorld.InBeach && NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3 && !NPC.AnyNPCs(ModContent.NPCType<SulfurElemental>()))
                return 0.3f;

            return 0;
        }

        public override void HitEffect(int hitDirection, double damage) {
            for(int k = 0; k < 5; k++) {
                Dust.NewDust(npc.position, npc.width, npc.height, 6, hitDirection, -1f, 0, default(Color), 1f);
            }
            if(npc.life <= 0) {
                if(TideWorld.TheTide) {
                    TideWorld.TidePoints2 += 4;
                }
                npc.position.X = npc.position.X + (float)(npc.width / 2);
                npc.position.Y = npc.position.Y + (float)(npc.height / 2);
                npc.width = 50;
                npc.height = 50;
                npc.position.X = npc.position.X - (float)(npc.width / 2);
                npc.position.Y = npc.position.Y - (float)(npc.height / 2);
                for(int num621 = 0; num621 < 20; num621++) {
                    int num622 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 6, 0f, 0f, 100, default(Color), 2f);
                    Main.dust[num622].velocity *= 3f;
                    if(Main.rand.Next(2) == 0) {
                        Main.dust[num622].scale = 0.5f;
                        Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
                    }
                }
                for(int num623 = 0; num623 < 40; num623++) {
                    int num624 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 6, 0f, 0f, 100, default(Color), 3f);
                    Main.dust[num624].noGravity = true;
                    Main.dust[num624].velocity *= 5f;
                    num624 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 6, 0f, 0f, 100, default(Color), 2f);
                    Main.dust[num624].velocity *= 2f;
                }
            }
        }

        public override void AI() {
            int dust = Dust.NewDust(npc.position, npc.width, npc.height, 6);
            npc.spriteDirection = npc.direction;
        }

        public override void BossLoot(ref string name, ref int potionType) {
            potionType = ItemID.GreaterHealingPotion;
        }
    }
}
