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

namespace SpiritMod.Items.Glyphs
{
    public class DazeGlyph : GlyphBase, IGlowing
    {
        public static int _type;
        public static Microsoft.Xna.Framework.Graphics.Texture2D[] _textures;

        Microsoft.Xna.Framework.Graphics.Texture2D IGlowing.Glowmask(out float bias) {
            bias = GLOW_BIAS;
            return _textures[1];
        }

        public override GlyphType Glyph => GlyphType.Daze;
        public override Microsoft.Xna.Framework.Graphics.Texture2D Overlay => _textures[2];
        public override Color Color => new Color { PackedValue = 0xce5aba };
        public override string Effect => "Dazed Dance";
        public override string Addendum =>
            "All attacks inflict confusion\n" +
            "Confused enemies take extra damage\n" +
            "Getting hurt may confuse you";

        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Daze Glyph");
            Tooltip.SetDefault(
                "All attacks inflict confusion\n" +
                "Confused enemies take extra damage\n" +
                "Getting hurt may confuse the player");
        }


        public override void SetDefaults() {
            item.width = 28;
            item.height = 28;
            item.value = Item.sellPrice(0, 2, 0, 0);
            item.rare = 5;

            item.maxStack = 999;
        }


        public static void Daze(NPC target, ref int damage) {
            if(target.FindBuffIndex(BuffID.Confused) > -1) {
                Main.NewText("Daze");
                damage += 30;
            }

            if(Main.rand.Next(9) == 1)
                target.AddBuff(BuffID.Confused, 240);
        }

        public static void Daze(Player target, ref int damage) {
            if(target.FindBuffIndex(BuffID.Confused) > -1)
                damage += 30;

            if(Main.rand.Next(9) == 1)
                target.AddBuff(BuffID.Confused, 240, false);
        }
    }
}