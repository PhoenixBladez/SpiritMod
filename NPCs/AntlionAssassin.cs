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
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs
{
    public class AntlionAssassin : ModNPC
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Antlion Assassin");
            Main.npcFrameCount[npc.type] = 15;
        }

        public override void SetDefaults() {
            npc.width = 24;
            npc.height = 44;
            npc.damage = 21;
            npc.defense = 8;
            npc.lifeMax = 74;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath6;
            npc.value = 329f;
            npc.knockBackResist = .65f;
            npc.aiStyle = 3;
            aiType = NPCID.AngryBones;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo) {
            if(SpawnHelper.SupressSpawns(spawnInfo, SpawnFlags.Daytime, SpawnZones.Desert | SpawnZones.Overworld))
                return 0;

            if(Main.tileSand[spawnInfo.spawnTileType])
                return SpawnCondition.OverworldDayDesert.Chance * 0.04f;
            return 0;
        }
        int timer;
        public override void HitEffect(int hitDirection, double damage) {
            for(int k = 0; k < 11; k++) {
                Dust.NewDust(npc.position, npc.width, npc.height, 85, hitDirection, -1f, 1, default(Color), .61f);
            }
            if(npc.life <= 0) {
                for(int k = 0; k < 11; k++) {
                    Dust.NewDust(npc.position, npc.width, npc.height, 85, hitDirection, -1f, 1, default(Color), .61f);
                }
                int ing = Gore.NewGore(npc.position, npc.velocity, 825);
                Main.gore[ing].timeLeft = 30;
                int ing1 = Gore.NewGore(npc.position, npc.velocity, 826);
                Main.gore[ing1].timeLeft = 30;
                int ing2 = Gore.NewGore(npc.position, npc.velocity, 827);
                Main.gore[ing2].timeLeft = 30;
            }
        }

        public override void FindFrame(int frameHeight) {
            npc.frameCounter += 0.25f;
            npc.frameCounter %= Main.npcFrameCount[npc.type];
            int frame = (int)npc.frameCounter;
            npc.frame.Y = frame * frameHeight;
        }

        public override void AI() {
            npc.spriteDirection = npc.direction;
            npc.alpha++;
            timer++;
            if(timer >= 500) {
                for(int k = 0; k < 11; k++) {
                    Dust.NewDust(npc.position, npc.width, npc.height, 85, npc.direction, -1f, 1, default(Color), .61f);
                }
                Main.PlaySound(4, (int)npc.position.X, (int)npc.position.Y, 6);
                int ing = Gore.NewGore(npc.position, npc.velocity, 825);
                Main.gore[ing].timeLeft = 130;
                int ing1 = Gore.NewGore(npc.position, npc.velocity, 826);
                Main.gore[ing1].timeLeft = 130;
                int ing2 = Gore.NewGore(npc.position, npc.velocity, 827);
                Main.gore[ing2].timeLeft = 130;
                npc.alpha = 0;
                timer = 0;
            }
        }

        public override void OnHitByItem(Player player, Item item, int damage, float knockback, bool crit) {
            for(int k = 0; k < 11; k++) {
                Dust.NewDust(npc.position, npc.width, npc.height, 85, npc.direction, -1f, 1, default(Color), .61f);
            }
            Main.PlaySound(4, (int)npc.position.X, (int)npc.position.Y, 6);
            int ing = Gore.NewGore(npc.position, npc.velocity, 825);
            Main.gore[ing].timeLeft = 130;
            int ing1 = Gore.NewGore(npc.position, npc.velocity, 826);
            Main.gore[ing1].timeLeft = 130;
            int ing2 = Gore.NewGore(npc.position, npc.velocity, 827);
            Main.gore[ing2].timeLeft = 130;
            npc.alpha = 0;
            timer = 0;
            npc.alpha = 0;
        }
        public override void OnHitByProjectile(Projectile projectile, int damage, float knockback, bool crit) {
            for(int k = 0; k < 11; k++) {
                Dust.NewDust(npc.position, npc.width, npc.height, 85, npc.direction, -1f, 1, default(Color), .61f);
            }
            Main.PlaySound(4, (int)npc.position.X, (int)npc.position.Y, 6);
            int ing = Gore.NewGore(npc.position, npc.velocity, 825);
            Main.gore[ing].timeLeft = 130;
            int ing1 = Gore.NewGore(npc.position, npc.velocity, 826);
            Main.gore[ing1].timeLeft = 130;
            int ing2 = Gore.NewGore(npc.position, npc.velocity, 827);
            Main.gore[ing2].timeLeft = 130;
            npc.alpha = 0;
            timer = 0;
            npc.alpha = 0;
        }
    }
}
