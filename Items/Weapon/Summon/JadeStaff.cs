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

namespace SpiritMod.Items.Weapon.Summon
{
    public class JadeStaff : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Staff of the Jade Dragon");
            Tooltip.SetDefault("Summons two revolving ethereal dragons\nEach dragon takes up 1/2 a minion slot");
        }

        public override void SetDefaults() {
            item.damage = 14;
            item.summon = true;
            item.mana = 60;
            item.width = 44;
            item.height = 48;
            item.useTime = 80;
            item.useAnimation = 80;
            item.useStyle = ItemUseStyleID.HoldingOut;
            Item.staff[item.type] = true;
            item.noMelee = true;
            item.knockBack = 1.25f;
            item.value = 20000;
            item.rare = 3;
            item.UseSound = new Terraria.Audio.LegacySoundStyle(3, 56);
            item.autoReuse = false;
            item.shoot = ModContent.ProjectileType<DragonHeadOne>();
            item.shootSpeed = 3f;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack) {
            // Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 124));
            int dragonLength = 8;
            int offset = 0;
            if(speedX > 0) {
                offset = -32;
            } else {
                offset = 32;
            }

            int latestprojectile = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, ModContent.ProjectileType<DragonHeadOne>(), damage, knockBack, player.whoAmI, speedX, speedY); //bottom
            for(int i = 0; i < dragonLength; ++i) {
                latestprojectile = Projectile.NewProjectile(position.X + (i * offset), position.Y, 0, 0, ModContent.ProjectileType<DragonBodyOne>(), damage, 0, player.whoAmI, 0, latestprojectile);
            }
            latestprojectile = Projectile.NewProjectile(position.X + (dragonLength * offset), position.Y, 0, 0, ModContent.ProjectileType<DragonTailOne>(), damage, 0, player.whoAmI, 0, latestprojectile);

            latestprojectile = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, ModContent.ProjectileType<DragonHeadTwo>(), damage, knockBack, player.whoAmI, speedX, speedY); //bottom
            for(int j = 0; j < dragonLength; ++j) {
                latestprojectile = Projectile.NewProjectile(position.X + (j * offset), position.Y, 0, 0, ModContent.ProjectileType<DragonBodyTwo>(), damage, 0, player.whoAmI, 0, latestprojectile);
            }
            latestprojectile = Projectile.NewProjectile(position.X + (dragonLength * offset), position.Y, 0, 0, ModContent.ProjectileType<DragonTailTwo>(), damage, 0, player.whoAmI, 0, latestprojectile);
            //Main.projectile[(int)latestprojectile].realLife = projectile.whoAmI;
            return true;
        }
    }
}