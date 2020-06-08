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
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Weapon.Swung
{
    public class ShadowflameSword : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Shadowflame Sword");
            Tooltip.SetDefault("Causes explosions of Shadowflames to appear when hitting enemies\nShoots out Shadow Embers that damage nearby foes");
            SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Weapon/Swung/ShadowflameSword_Glow");
        }


        public override void SetDefaults() {
            item.width = item.height = 42;
            item.rare = 6;
            item.damage = 44;
            item.knockBack = 6;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.useTime = item.useAnimation = 20;
            item.melee = true;
            item.shoot = mod.ProjectileType("ShadowPulse1");
            item.shootSpeed = 2;
            item.value = Terraria.Item.sellPrice(0, 5, 0, 0);
            item.autoReuse = true;
            item.UseSound = SoundID.Item33;
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI) {
            Texture2D texture;
            texture = Main.itemTexture[item.type];
            spriteBatch.Draw
            (
                ModContent.GetTexture("SpiritMod/Items/Weapon/Swung/ShadowflameSword_Glow"),
                new Vector2
                (
                    item.position.X - Main.screenPosition.X + item.width * 0.5f,
                    item.position.Y - Main.screenPosition.Y + item.height - texture.Height * 0.5f + 2f
                ),
                new Rectangle(0, 0, texture.Width, texture.Height),
                Color.White,
                rotation,
                texture.Size() * 0.5f,
                scale,
                SpriteEffects.None,
                0f
            );
        }

        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit) {
            if(Main.rand.Next(5) == 0) {
                int dist = 80;
                Vector2 targetExplosionPos = target.Center;
                for(int i = 0; i < 200; ++i) {
                    if(Main.npc[i].active && (Main.npc[i].Center - targetExplosionPos).Length() < dist) {
                        Main.npc[i].HitEffect(0, damage);
                    }
                }
                for(int i = 0; i < 15; ++i) {
                    int newDust = Dust.NewDust(new Vector2(targetExplosionPos.X - (dist / 2), targetExplosionPos.Y - (dist / 2)), dist, dist, DustID.Shadowflame, 0f, 0f, 100, default(Color), 2.5f);
                    Main.dust[newDust].noGravity = true;
                    Main.dust[newDust].velocity *= 5f;
                    newDust = Dust.NewDust(new Vector2(targetExplosionPos.X - (dist / 2), targetExplosionPos.Y - (dist / 2)), dist, dist, DustID.Shadowflame, 0f, 0f, 100, default(Color), 1.5f);
                    Main.dust[newDust].velocity *= 3f;
                }
            }
            if(Main.rand.Next(4) == 0) {
                target.AddBuff(BuffID.ShadowFlame, 300, true);
            }
        }
    }
}