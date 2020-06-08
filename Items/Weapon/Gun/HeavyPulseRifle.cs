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
using Terraria.ModLoader;
namespace SpiritMod.Items.Weapon.Gun
{
    public class HeavyPulseRifle : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Heavy Pulse Rifle");
            Tooltip.SetDefault("'Try not to get yourself killed out there'\nShoots out very quick Pulse Bullets\nRight click to shoot out three Helix Rockets\nHelix Rockets have a 4 second cooldown\nEvery 30 hits with the Pulse Rifle activates your tactical visor, causing bullets shot out to deal more damage and home onto foes");

        }


        private Vector2 newVect;
        int charger;
        public override void SetDefaults() {
            item.damage = 44;
            item.ranged = true;
            item.width = 54;
            item.height = 30;
            item.useTime = 9;
            item.useAnimation = 9;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.noMelee = true;
            item.knockBack = 5f;
            item.useTurn = false;
            item.value = Terraria.Item.sellPrice(0, 3, 50, 0);
            item.rare = 8;
            item.autoReuse = true;
            item.shoot = ModContent.ProjectileType<PulseBullet>();
            item.shootSpeed = 16f;
            item.useAmmo = AmmoID.Bullet;
        }
        public override bool AltFunctionUse(Player player) {
            return true;
        }
        public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack) {
            charger++;
            if(player.altFunctionUse == 2) {
                Main.PlaySound(SoundLoader.customSoundType, player.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/Soldier1"));

                Projectile.NewProjectile(position.X, position.Y - 14, speedX * 2, speedY * 2, ModContent.ProjectileType<HelixRocket>(), item.damage * 2 / 4 * 5 - 4, 11, player.whoAmI, 0f, 0f);
                Projectile.NewProjectile(position.X + 10, position.Y, speedX * 2, speedY * 2, ModContent.ProjectileType<HelixRocket>(), item.damage * 2 / 4 * 5 - 4, 11, player.whoAmI, 0f, 0f);
                Projectile.NewProjectile(position.X - 10, position.Y, speedX * 2, speedY * 2, ModContent.ProjectileType<HelixRocket>(), item.damage * 2 / 4 * 5 - 4, 11, player.whoAmI, 0f, 0f);


                MyPlayer modPlayer = player.GetSpiritPlayer();
                modPlayer.shootDelay2 = 240;
                charger = 0;
                return false;
            } else
                Main.PlaySound(SoundLoader.customSoundType, player.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/Soldier2"));
            {
                if(charger == 30) {
                    {
                        Main.PlaySound(SoundLoader.customSoundType, player.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/SoldierUlt"));
                        Projectile.NewProjectile(position.X, position.Y, speedX + ((float)Main.rand.Next(-230, 230) / 100), speedY + ((float)Main.rand.Next(-230, 230) / 100), ModContent.ProjectileType<VisorBullet>(), item.damage / 4 * 5, knockBack, player.whoAmI, 0f, 0f);
                        return false;
                    }
                }
                if(charger == 31) {
                    {
                        Projectile.NewProjectile(position.X, position.Y, speedX + ((float)Main.rand.Next(-230, 230) / 100), speedY + ((float)Main.rand.Next(-230, 230) / 100), ModContent.ProjectileType<VisorBullet>(), item.damage / 4 * 5, knockBack, player.whoAmI, 0f, 0f);
                        return false;
                    }
                }
                if(charger == 32) {
                    {
                        Projectile.NewProjectile(position.X, position.Y, speedX + ((float)Main.rand.Next(-230, 230) / 100), speedY + ((float)Main.rand.Next(-230, 230) / 100), ModContent.ProjectileType<VisorBullet>(), item.damage / 4 * 5, knockBack, player.whoAmI, 0f, 0f);
                        return false;
                    }
                }
                if(charger == 33) {
                    {
                        Projectile.NewProjectile(position.X, position.Y, speedX + ((float)Main.rand.Next(-230, 230) / 100), speedY + ((float)Main.rand.Next(-230, 230) / 100), ModContent.ProjectileType<VisorBullet>(), item.damage / 4 * 5, knockBack, player.whoAmI, 0f, 0f);
                        return false;
                    }
                }
                if(charger == 34) {
                    {
                        Projectile.NewProjectile(position.X, position.Y, speedX + ((float)Main.rand.Next(-230, 230) / 100), speedY + ((float)Main.rand.Next(-230, 230) / 100), ModContent.ProjectileType<VisorBullet>(), item.damage / 4 * 5, knockBack, player.whoAmI, 0f, 0f);
                        return false;
                    }
                }
                if(charger == 35) {
                    {
                        Projectile.NewProjectile(position.X, position.Y, speedX + ((float)Main.rand.Next(-230, 230) / 100), speedY + ((float)Main.rand.Next(-230, 230) / 100), ModContent.ProjectileType<VisorBullet>(), item.damage / 4 * 5, knockBack, player.whoAmI, 0f, 0f);
                        return false;
                    }
                }
                if(charger == 36) {
                    {
                        Projectile.NewProjectile(position.X, position.Y, speedX + ((float)Main.rand.Next(-230, 230) / 100), speedY + ((float)Main.rand.Next(-230, 230) / 100), ModContent.ProjectileType<VisorBullet>(), item.damage / 4 * 5, knockBack, player.whoAmI, 0f, 0f);
                        return false;
                    }
                }
                if(charger == 37) {
                    {
                        Projectile.NewProjectile(position.X, position.Y, speedX + ((float)Main.rand.Next(-230, 230) / 100), speedY + ((float)Main.rand.Next(-230, 230) / 100), ModContent.ProjectileType<VisorBullet>(), item.damage / 4 * 5, knockBack, player.whoAmI, 0f, 0f);
                        return false;
                    }
                }
                if(charger == 38) {
                    {
                        Projectile.NewProjectile(position.X, position.Y, speedX + ((float)Main.rand.Next(-230, 230) / 100), speedY + ((float)Main.rand.Next(-230, 230) / 100), ModContent.ProjectileType<VisorBullet>(), item.damage / 4 * 5, knockBack, player.whoAmI, 0f, 0f);
                        return false;
                    }
                }
                if(charger == 39) {
                    {
                        Projectile.NewProjectile(position.X, position.Y, speedX + ((float)Main.rand.Next(-230, 230) / 100), speedY + ((float)Main.rand.Next(-230, 230) / 100), ModContent.ProjectileType<VisorBullet>(), item.damage / 4 * 5, knockBack, player.whoAmI, 0f, 0f);
                        return false;
                    }
                }
                if(charger == 40) {
                    {
                        Projectile.NewProjectile(position.X, position.Y, speedX + ((float)Main.rand.Next(-230, 230) / 100), speedY + ((float)Main.rand.Next(-230, 230) / 100), ModContent.ProjectileType<VisorBullet>(), item.damage / 4 * 5, knockBack, player.whoAmI, 0f, 0f);
                        return false;
                    }
                }
                if(charger == 41) {
                    {
                        Projectile.NewProjectile(position.X, position.Y, speedX + ((float)Main.rand.Next(-230, 230) / 100), speedY + ((float)Main.rand.Next(-230, 230) / 100), ModContent.ProjectileType<VisorBullet>(), item.damage / 4 * 5, knockBack, player.whoAmI, 0f, 0f);
                        return false;
                    }
                }
                if(charger == 42) {
                    {
                        Projectile.NewProjectile(position.X, position.Y, speedX + ((float)Main.rand.Next(-230, 230) / 100), speedY + ((float)Main.rand.Next(-230, 230) / 100), ModContent.ProjectileType<VisorBullet>(), item.damage / 4 * 5, knockBack, player.whoAmI, 0f, 0f);
                        return false;
                    }
                }
                if(charger >= 42) {
                    charger = 0;
                }
                type = ModContent.ProjectileType<PulseBullet>();
                float spread = 7 * 0.0174f;//45 degrees converted to radians
                float baseSpeed = (float)Math.Sqrt(speedX * speedX + speedY * speedY);
                double baseAngle = Math.Atan2(speedX, speedY);
                double randomAngle = baseAngle + (Main.rand.NextFloat() - 0.5f) * spread;
                speedX = baseSpeed * (float)Math.Sin(randomAngle);
                speedY = baseSpeed * (float)Math.Cos(randomAngle);
                return true;
            }
        }
        public override bool CanUseItem(Player player) {
            if(player.altFunctionUse == 2) {

                MyPlayer modPlayer = player.GetSpiritPlayer();
                if(modPlayer.shootDelay2 == 0)
                    return true;
                return false;
            } else {
                return true;
            }
        }
        public override Vector2? HoldoutOffset() {
            return new Vector2(-7, 0);
        }
        public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Uzi, 1);
            recipe.AddIngredient(null, "SunShard", 2);
            recipe.AddIngredient(null, "TechDrive", 5);
            recipe.AddIngredient(null, "SteamParts", 6);
            recipe.AddIngredient(null, "SpiritBar", 10);
            recipe.AddIngredient(ItemID.ChlorophyteBar, 12);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}