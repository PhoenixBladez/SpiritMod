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

namespace SpiritMod.Items.Weapon.Swung
{
    public class ReachBossSword : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Bloodthorn Blade");
            Tooltip.SetDefault("Increases in damage and speed as health wanes\nOccasionally shoots out a bloody wave\nFires waves more frequently when below 1/2 health\nMelee critical hits poison enemies and inflict 'Withering Leaf'");
        }


        public override void SetDefaults() {
            item.damage = 22;
            item.melee = true;
            item.width = 64;
            item.height = 62;
            item.useTime = 32;
            item.useAnimation = 32;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.knockBack = 6;
            item.value = Terraria.Item.sellPrice(0, 2, 0, 0);
            item.shoot = ModContent.ProjectileType<BloodWave>();
            item.rare = 2;
            item.shootSpeed = 8f;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
        }
        public override bool CanUseItem(Player player) {
            if(player.statLife <= player.statLifeMax2 / 2) {
                item.damage = 27;
                item.useTime = 22;
                item.useAnimation = 22;
            } else if(player.statLife <= player.statLifeMax2 / 3) {
                item.damage = 26;
                item.useTime = 27;
                item.useAnimation = 27;
            } else if(player.statLife <= player.statLifeMax2 / 4) {
                item.damage = 24;
                item.useTime = 29;
                item.useAnimation = 29;
            } else {
                item.damage = 22;
                item.useTime = 32;
                item.useAnimation = 32;
            }
            return base.CanUseItem(player);
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack) {
            if(Main.rand.Next(4) == 1 && player.statLife >= player.statLifeMax2 / 2) {
                Main.PlaySound(2, (int)player.position.X, (int)player.position.Y, 20);
                int proj = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, 0, player.whoAmI);
                return false;
            } else if(Main.rand.Next(2) == 1) {
                Main.PlaySound(2, (int)player.position.X, (int)player.position.Y, 20);
                int proj = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, 0, player.whoAmI);
                return false;

            }
            return false;
        }
        public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit) {
            if(crit) {
                target.AddBuff(BuffID.Poisoned, 240);
                target.AddBuff(ModContent.BuffType<WitheringLeaf>(), 120, true);
            }
        }
    }
}