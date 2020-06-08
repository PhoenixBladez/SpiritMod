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
using SpiritMod.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.BlueMoon
{
    public class MagicToadstool : ModNPC
    {
        int timer = 0;
        bool shrooms = false;

        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Magic Toadstool");
            Main.npcFrameCount[npc.type] = 17;
        }

        public override void SetDefaults() {
            npc.width = 36;
            npc.height = 38;
            npc.damage = 30;
            npc.defense = 10;
            npc.lifeMax = 600;
            npc.HitSound = SoundID.NPCHit45;
            npc.DeathSound = SoundID.NPCDeath47;
            npc.value = 1000f;
            npc.knockBackResist = 0f;
            npc.aiStyle = 3;
            aiType = 3;
        }

        public override void HitEffect(int hitDirection, double damage) {
            for(int k = 0; k < 5; k++) {
                Dust.NewDust(npc.position, npc.width, npc.height, 6, hitDirection, -1f, 0, default(Color), 1f);
            }
            if(npc.life <= 0) {
                npc.position.X = npc.position.X + (float)(npc.width / 2);
                npc.position.Y = npc.position.Y + (float)(npc.height / 2);
                npc.width = 36;
                npc.height = 38;
                npc.position.X = npc.position.X - (float)(npc.width / 2);
                npc.position.Y = npc.position.Y - (float)(npc.height / 2);
                for(int num621 = 0; num621 < 200; num621++) {
                    int num622 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 206, 0f, 0f, 100, default(Color), 2f);
                    Main.dust[num622].velocity *= 3f;
                    if(Main.rand.Next(2) == 0) {
                        Main.dust[num622].scale = 0.5f;
                        Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
                    }
                }
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo) {
            return MyWorld.BlueMoon ? 3f : 0f;
        }

        public override void FindFrame(int frameHeight) {
            if(timer % 300 >= 79) {
                npc.frameCounter += 0.40f;
                npc.frameCounter %= Main.npcFrameCount[npc.type] - 4;
                int frame = (int)npc.frameCounter;
                npc.frame.Y = frame * frameHeight;
            } else {
                npc.frame.Y = (int)((Main.npcFrameCount[npc.type] - 4) + ((timer % 300) / 20)) * frameHeight;
            }
        }

        public override void AI() {
            npc.TargetClosest(true);
            Player player = Main.player[npc.target];
            timer++;
            if(timer % 300 == 79 && shrooms == false) {
                bool expertMode = Main.expertMode;
                int damage = expertMode ? 15 : 20;

                int speed = 4;
                Main.PlaySound(2, (int)npc.position.X, (int)npc.position.Y, 43);

                Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, -1 * speed, ModContent.ProjectileType<ToadStool>(), damage, 1, Main.myPlayer, 0, 0);

                Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 1 * speed, 0, ModContent.ProjectileType<ToadStool>(), damage, 1, Main.myPlayer, 0, 0);

                Projectile.NewProjectile(npc.Center.X, npc.Center.Y, -1 * speed, 0, ModContent.ProjectileType<ToadStool>(), damage, 1, Main.myPlayer, 0, 0);

                Projectile.NewProjectile(npc.Center.X, npc.Center.Y, (float)(0.70710678118 * speed), (float)(-0.70710678118 * speed), ModContent.ProjectileType<ToadStool>(), damage, 1, Main.myPlayer, 0, 0);

                Projectile.NewProjectile(npc.Center.X, npc.Center.Y, (float)(-0.70710678118 * speed), (float)(-0.70710678118 * speed), ModContent.ProjectileType<ToadStool>(), damage, 1, Main.myPlayer, 0, 0);
                shrooms = true;
            }
            if(timer % 300 < 79) {
                if(player.position.X > npc.position.X) {
                    npc.spriteDirection = 1;
                    npc.netUpdate = true;
                } else {
                    npc.spriteDirection = 0;
                    npc.netUpdate = true;
                }

                npc.velocity.X = 0;

            } else {
                npc.spriteDirection = npc.direction;
                if(shrooms == true)
                    shrooms = false;
            }
        }

        public override void OnHitPlayer(Player target, int damage, bool crit) {
            if(Main.rand.Next(5) == 0)
                target.AddBuff(ModContent.BuffType<StarFlame>(), 200);
        }

        public override void NPCLoot() {
            if(Main.rand.Next(40) == 1)
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<GloomgusStaff>());
        }

    }
}

