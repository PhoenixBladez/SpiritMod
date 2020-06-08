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


namespace SpiritMod.Items.Weapon.Gun
{
    public class CthulhuGun : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("R'lyehian Sidearm");
            Tooltip.SetDefault("Converts bullets into Mystic Bullets\nThe second shot fired will cause Mystic Bullets to explode\nThe third shot fired increases the potency of the bullet and its explosion\nResets every three shots");
        }


        int charger;
        private int memes;
        public override void SetDefaults() {
            item.damage = 29;
            item.ranged = true;
            item.width = 34;
            item.height = 28;
            item.useTime = 21;
            item.useAnimation = 21;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.noMelee = true;
            item.knockBack = 3;
            item.useTurn = false;
            item.value = Terraria.Item.sellPrice(0, 3, 38, 0);
            item.rare = 5;
            item.UseSound = SoundID.Item95;
            item.autoReuse = true;
            item.shoot = 10;
            item.shootSpeed = 8f;
            item.useAmmo = AmmoID.Bullet;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack) {
            charger++;
            if(charger == 1) {
                for(int I = 0; I < 1; I++) {
                    Projectile.NewProjectile(position.X, position.Y, speedX, speedY, mod.ProjectileType("PurpleBullet1"), damage, knockBack, player.whoAmI, 0f, 0f);
                    return false;
                }
            }
            if(charger == 2) {
                for(int I = 0; I < 1; I++) {
                    Projectile.NewProjectile(position.X, position.Y, speedX, speedY, mod.ProjectileType("PurpleBullet2"), damage, knockBack, player.whoAmI, 0f, 0f);
                    return false;
                }
            }
            if(charger == 3) {
                for(int I = 0; I < 1; I++) {
                    Projectile.NewProjectile(position.X, position.Y, speedX, speedY, mod.ProjectileType("PurpleBullet3"), damage / 5 * 6, 7, player.whoAmI, 0f, 0f);
                    charger = 0;
                }
            }
            return false;
        }
        public override Vector2? HoldoutOffset() {
            return new Vector2(-10, 0);
        }
    }
}
