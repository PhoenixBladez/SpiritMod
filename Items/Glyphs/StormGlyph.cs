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

namespace SpiritMod.Items.Glyphs
{
    public class StormGlyph : GlyphBase, IGlowing
    {
        public static int _type;
        public static Microsoft.Xna.Framework.Graphics.Texture2D[] _textures;

        Microsoft.Xna.Framework.Graphics.Texture2D IGlowing.Glowmask(out float bias) {
            bias = GLOW_BIAS;
            return _textures[1];
        }

        public override GlyphType Glyph => GlyphType.Storm;
        public override Microsoft.Xna.Framework.Graphics.Texture2D Overlay => _textures[2];
        public override Color Color => new Color { PackedValue = 0xc0b26d };
        public override string Effect => "Slicing Storms";
        public override string Addendum =>
            "Every third attack will cause a Slicing Gust\n" +
            "This Gust will knock enemies in the air and inflict Wind Burst\n" +
            "Enemies afflicted with Wind Burst receive amplified knockback";

        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Storm Glyph");
            Tooltip.SetDefault(
                "+3 Defense\n" +
                "Every third attack will cause a Slicing Gust\n" +
                "This Gust will knock enemies in the air and inflict Wind Burst\n" +
                "Enemies afflicted with Wind Burst receive amplified knockback");
        }


        public override void SetDefaults() {
            item.width = 28;
            item.height = 28;
            item.value = Item.sellPrice(0, 2, 0, 0);
            item.rare = 2;

            item.maxStack = 999;
        }


        public static void WindBurst(MyPlayer player, Item item) {
            if(player.player.whoAmI != Main.myPlayer)
                return;

            if(player.stormStacks < 2) {
                player.stormStacks++;
                return;
            }
            player.stormStacks = 0;

            int damage = (int)(item.damage * player.player.GetDamageBoost());
            Vector2 position = player.player.MountedCenter;
            Vector2 velocity = Main.MouseWorld - position;
            float scale = 1 / velocity.Length();
            if(float.IsNaN(scale))
                velocity = new Vector2(player.player.direction, 0);
            else
                velocity *= scale;
            velocity *= 8f;
            Projectile.NewProjectile(position, velocity, Projectiles.SlicingGust._type, damage, 8f, Main.myPlayer);
        }
    }
}