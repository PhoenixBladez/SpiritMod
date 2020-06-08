using SpiritMod.Buffs;
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
using SpiritMod.Items.Material;
using SpiritMod.Items.Weapon.Magic;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Tide.NPCs
{
    public class TideCaller : ModNPC
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Tide Caller");
            Main.npcFrameCount[npc.type] = 8;
        }

        public override void SetDefaults() {
            npc.width = 74;
            npc.height = 74;
            npc.damage = 26;
            npc.defense = 20;
            npc.lifeMax = 310;
            npc.HitSound = SoundID.NPCHit6;
            npc.DeathSound = SoundID.NPCDeath5;
            npc.value = 2329f;
            npc.knockBackResist = .45f;
            npc.aiStyle = 3;
            aiType = NPCID.AngryBones;

        }

        private int Counter;
        public override bool PreAI() {
            Counter++;
            if(Counter > 500) {
                int newNPC = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<Clamper>(), npc.whoAmI);
                Counter = 0;
            }
            return true;
        }

        public override void NPCLoot() {
            if(!NPC.downedMechBossAny) {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<PearlFragment>(), 1);
            }
            if(Main.rand.Next(25) == 0) {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<BlackTide>(), 1);
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo) {
            if(TideWorld.TheTide && TideWorld.InBeach)
                return 1f;

            return 0;
        }

        public override void HitEffect(int hitDirection, double damage) {
            if(npc.life <= 0) {
                if(TideWorld.TheTide) {
                    TideWorld.TidePoints2 += 2;
                }
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Callershell"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Callersquid"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Callerhead"), 1f);
            }
        }

        public override void OnHitPlayer(Player target, int damage, bool crit) {
            npc.lifeRegen += (int)Math.Sqrt(npc.lifeMax - npc.life) / 2 + 1;
            if(Main.rand.Next(12) == 1)
                target.AddBuff(ModContent.BuffType<Wobbly>(), 120);
        }

        public override void FindFrame(int frameHeight) {
            npc.frameCounter += 0.25f;
            npc.frameCounter %= Main.npcFrameCount[npc.type];
            int frame = (int)npc.frameCounter;
            npc.frame.Y = frame * frameHeight;
        }

        public override void AI() {
            npc.spriteDirection = npc.direction;
        }
    }
}

