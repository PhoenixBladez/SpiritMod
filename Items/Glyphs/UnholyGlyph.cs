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
using System;
using Terraria;
using Terraria.ID;

namespace SpiritMod.Items.Glyphs
{
    public class UnholyGlyph : GlyphBase, IGlowing
    {
        public static int _type;
        public static Microsoft.Xna.Framework.Graphics.Texture2D[] _textures;

        Microsoft.Xna.Framework.Graphics.Texture2D IGlowing.Glowmask(out float bias) {
            bias = GLOW_BIAS;
            return _textures[1];
        }

        public override GlyphType Glyph => GlyphType.Unholy;
        public override Microsoft.Xna.Framework.Graphics.Texture2D Overlay => _textures[2];
        public override Color Color => new Color { PackedValue = 0x08dd5d };
        public override string Effect => "Pestilence";
        public override string Addendum =>
            "+6 Armor Penetration\n" +
            "Critical strikes can inflict Wandering Plague\n" +
            "Afflicted will slowly lose life and release toxic clouds";

        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Unholy Glyph");
            Tooltip.SetDefault(
                "+6 Armor Penetration\n" +
                "Critical strikes can inflict Wandering Plague\n" +
                "Afflicted will slowly lose life and release toxic clouds");
        }


        public override void SetDefaults() {
            item.width = 28;
            item.height = 28;
            item.value = Item.sellPrice(0, 2, 0, 0);
            item.rare = 2;

            item.maxStack = 999;
        }


        public static void PlagueEffects(NPC target, int owner, ref int damage, bool crit) {
            damage += target.checkArmorPenetration(6);
            if(!crit || !target.CanLeech())
                return;
            if(Main.rand.NextDouble() < 0.5) {
                target.AddBuff(SpiritMod.instance.BuffType("WanderingPlague"), 360);
                target.GetGlobalNPC<NPCs.GNPC>().unholySource = owner;
            }
        }

        public static void ReleasePoisonClouds(NPC target, int time) {
            if(Main.netMode == 1)
                return;
            if(time % 80 != 0)
                return;
            int owner = target.GetGlobalNPC<NPCs.GNPC>().unholySource;
            if(!Main.player[owner].active)
                return;
            int max = time != 0 ? 1 : Main.hardMode ? 3 : 1;
            for(int i = 0; i < max; i++) {
                Vector2 vel = Vector2.UnitY.RotatedByRandom(Math.PI * 2);
                vel *= Main.rand.Next(8, 40) * .125f;
                int projectile = Projectile.NewProjectile(target.Center, vel, Projectiles.PoisonCloud._type, Main.hardMode ? 35 : 20, 0, owner, target.whoAmI);
                if(Main.netMode == 2) {
                    Main.projectile[projectile].ai[0] = target.whoAmI;
                    NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, projectile);
                }
            }
        }
    }
}