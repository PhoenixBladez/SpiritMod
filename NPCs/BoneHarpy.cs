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
using SpiritMod.Items.Consumable;
using SpiritMod.Items.Material;
using SpiritMod.Projectiles.Boss;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs
{
    public class BoneHarpy : ModNPC
    {
        int moveSpeed = 0;
        int moveSpeedY = 0;
        float HomeY = 150f;

        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Ancient Apostle");
            Main.npcFrameCount[npc.type] = 4;
        }

        public override void SetDefaults() {
            npc.width = 32;
            npc.height = 34;
            npc.damage = 18;
            npc.defense = 10;
            npc.lifeMax = 170;
            npc.noGravity = true;
            npc.value = 800f;
            npc.noTileCollide = false;
            npc.HitSound = SoundID.NPCHit2;
            npc.DeathSound = SoundID.NPCDeath6;
        }
        int counter;
        public override void AI() {
            counter++;
            npc.spriteDirection = npc.direction;
            Player player = Main.player[npc.target];
            if(npc.Center.X >= player.Center.X && moveSpeed >= -60) // flies to players x position
            {
                moveSpeed--;
            }

            if(npc.Center.X <= player.Center.X && moveSpeed <= 60) {
                moveSpeed++;
            }

            npc.velocity.X = moveSpeed * 0.08f;

            if(npc.Center.Y >= player.Center.Y - HomeY && moveSpeedY >= -50) //Flies to players Y position
            {
                moveSpeedY--;
                HomeY = 150f;
            }

            if(npc.Center.Y <= player.Center.Y - HomeY && moveSpeedY <= 50) {
                moveSpeedY++;
            }

            npc.velocity.Y = moveSpeedY * 0.2f;
            if(Main.rand.Next(220) == 8) {
                HomeY = -25f;
            }
            if(counter >= 240) //Fires desert feathers like a shotgun
            {
                counter = 0;
                Main.PlaySound(2, (int)npc.position.X, (int)npc.position.Y, 73);
                Vector2 direction = Main.player[npc.target].Center - npc.Center;
                direction.Normalize();
                direction.X *= 11f;
                direction.Y *= 11f;

                int amountOfProjectiles = 3;
                for(int i = 0; i < amountOfProjectiles; ++i) {
                    float A = (float)Main.rand.Next(-150, 150) * 0.01f;
                    float B = (float)Main.rand.Next(-150, 150) * 0.01f;
                    int p = Projectile.NewProjectile(npc.Center.X, npc.Center.Y, direction.X + A, direction.Y + B, ModContent.ProjectileType<DesertFeather>(), 11, 1, Main.myPlayer, 0, 0);
                    Main.projectile[p].scale = .6f;
                }
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo) {
            return spawnInfo.sky && NPC.downedQueenBee && !Main.LocalPlayer.GetSpiritPlayer().ZoneAsteroid ? 0.16f : 0f;
        }

        public override void NPCLoot() {
            if(Main.rand.Next(6) == 0) {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<JewelCrown>());
            }
            if(Main.rand.Next(2) == 0) {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Talon>());
            }
        }

        public override void HitEffect(int hitDirection, double damage) {
            int d1 = 1;
            for(int k = 0; k < 30; k++) {
                Dust.NewDust(npc.position, npc.width, npc.height, d1, 2.5f * hitDirection, -2.5f, 0, Color.White, Main.rand.NextFloat(.2f, .8f));
                Dust.NewDust(npc.position, npc.width, npc.height, d1, 2.5f * hitDirection, -2.5f, 0, default(Color), .34f);
            }
            if(npc.life <= 0) {
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Apostle2"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Apostle3"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Apostle4"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Apostle1"), 1f);
            }
        }

        public override void FindFrame(int frameHeight) {
            npc.frameCounter += 0.25f;
            npc.frameCounter %= Main.npcFrameCount[npc.type];
            int frame = (int)npc.frameCounter;
            npc.frame.Y = frame * frameHeight;
        }
    }
}
