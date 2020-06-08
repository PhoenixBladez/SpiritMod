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
using SpiritMod.NPCs;
using System;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Glyph
{
    public class SanguineBleed : ModBuff
    {
        public override void SetDefaults() {
            DisplayName.SetDefault("Crimson Bleed");
            Description.SetDefault("You are rapidly losing blood.");
            Main.buffNoSave[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.debuff[Type] = true;
        }

        public override bool ReApply(NPC npc, int time, int buffIndex) {
            if(time < 357) {
                return false;
            }

            npc.buffTime[buffIndex] = 357;

            return true;
        }

        public override void Update(NPC npc, ref int buffIndex) {
            GNPC modNPC = npc.GetGlobalNPC<GNPC>();

            double chance = Math.Max(0.02, npc.width * npc.height * 0.00003);
            if(!modNPC.sanguinePrev) {
                for(int i = 0; i < 4; i++) {
                    Vector2 offset = Main.rand.NextVec2CircularEven(npc.width >> 1, npc.height >> 1);
                    Dust.NewDustPerfect(npc.Center + offset, ModContent.DustType<Dusts.Blood>()).customData = npc;
                }
            }

            if(Main.rand.NextDouble() < chance && npc.buffTime[buffIndex] > 60) {
                Vector2 offset = Main.rand.NextVec2CircularEven(npc.width >> 1, npc.height >> 1);
                Dust.NewDustPerfect(npc.Center + offset, ModContent.DustType<Dusts.Blood>()).customData = npc;
            }

            modNPC.sanguineBleed = true;
        }
    }
}
