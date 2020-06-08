using Microsoft.Xna.Framework;
using SpiritMod.Tiles.Furniture.Reach;
using SpiritMod.NPCs.Critters;
using SpiritMod.Mounts;
using SpiritMod.NPCs.Boss.SpiritCore;
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
using SpiritMod.Projectiles.Summon.Zipline;

namespace SpiritMod.Items.Equipment
{
    public class ZiplineGun : ModItem
    {
        bool leftactive = false;
        Vector2 direction9 = Vector2.Zero;
        int distance = 500;
        bool rightactive = false;
        int right = 0;
        int left = 0;
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Rail-gun");
            Tooltip.SetDefault("Left and right click to shoot tethers that latch to tiles\nThese tethers are connected by a rail\nHold UP to slide down the rail");
        }

        public override void SetDefaults() {
            item.ranged = true;
            item.width = 44;
            item.height = 48;
            item.useTime = 55;
            item.useAnimation = 55;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.noMelee = true;
            item.knockBack = 5;
            item.value = 20000;
            item.rare = 2;
            item.UseSound = SoundID.Item20;
            item.autoReuse = false;
            item.shoot = ModContent.ProjectileType<RightHopper>();
            item.shootSpeed = 13f;
        }
        public override bool AltFunctionUse(Player player) {
            return true;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack) {
            Main.PlaySound(SoundLoader.customSoundType, player.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/MaliwanShot1"));
            if((rightactive && Main.projectile[right].active == false) || (leftactive && Main.projectile[left].active == false)) //if the gates despawn, reset
            {
                Main.projectile[right].active = false;
                Main.projectile[left].active = false;
                rightactive = false;
                leftactive = false;
                right = 0;
                left = 0;
            }
            /*  if (player.statMana <= 12)
              {
                  return false;
              }*/
            if(player.altFunctionUse == 2) {
                if(rightactive) {
                    Main.projectile[right].active = false;
                }
                right = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, ModContent.ProjectileType<RightZipline>(), item.damage, 1, Main.myPlayer);
                rightactive = true;
                if(leftactive) {
                    Main.projectile[right].ai[1] = left;
                    Main.projectile[left].ai[1] = right;
                }
            } else {
                if(leftactive) {
                    Main.projectile[left].active = false;
                }
                left = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, ModContent.ProjectileType<LeftZipline>(), item.damage, 1, Main.myPlayer);
                leftactive = true;
                if(rightactive) {
                    Main.projectile[left].ai[1] = right;
                    Main.projectile[right].ai[1] = left;
                }
            }
            return false;
        }
        public override ModItem Clone(Item item) {
            ZiplineGun staff = (ZiplineGun)base.Clone(item);
            staff.left = this.left;
            staff.right = this.right;
            staff.distance = this.distance;
            staff.rightactive = this.rightactive;
            staff.leftactive = this.leftactive;
            return staff;
        }
    }
}