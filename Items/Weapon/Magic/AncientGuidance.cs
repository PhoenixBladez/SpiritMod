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

namespace SpiritMod.Items.Weapon.Magic
{
    public class AncientGuidance : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Ancient Guidance");
            Tooltip.SetDefault("'Harness the elements of the cosmos'\nShoots one of four projectiles with varied effects");

        }



        public override void SetDefaults() {
            item.damage = 155;
            Item.staff[item.type] = true;
            item.noMelee = true;
            item.magic = true;
            item.width = 28;
            item.height = 28;
            item.useTime = 24;
            item.mana = 27;
            item.useAnimation = 24;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.knockBack = 7;
            item.value = Terraria.Item.sellPrice(0, 12, 0, 0);
            item.value = Terraria.Item.buyPrice(0, 85, 0, 0);
            item.rare = 10;
            item.crit = 3;
            item.UseSound = SoundID.Item123;
            item.autoReuse = true;
            item.shootSpeed = 6;
            item.shoot = mod.ProjectileType("Starshock1");
        }
        public static Vector2[] randomSpread(float speedX, float speedY, int angle, int num) {
            var posArray = new Vector2[num];
            float spread = (float)(angle * 0.0274532925);
            float baseSpeed = (float)System.Math.Sqrt(speedX * speedX + speedY * speedY);
            double baseAngle = System.Math.Atan2(speedX, speedY);
            double randomAngle;
            for(int i = 0; i < num; ++i) {
                randomAngle = baseAngle + (Main.rand.NextFloat() - 0.4f) * spread;
                posArray[i] = new Vector2(baseSpeed * (float)System.Math.Sin(randomAngle), baseSpeed * (float)System.Math.Cos(randomAngle));
            }
            return (Vector2[])posArray;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack) {
            if(Main.rand.Next(7) == 0) {
                int pl = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, ModContent.ProjectileType<CultistIce>(), damage, knockBack, player.whoAmI, 0f, 0f);
                Main.projectile[pl].friendly = true;
                Main.projectile[pl].hostile = false;
                return false;
            }
            if(Main.rand.Next(5) == 0) {
                for(int i = 0; i < 3; i++) {
                    Main.PlaySound(SoundLoader.customSoundType, player.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/Thunder"));
                    Vector2 vector82 = -Main.player[Main.myPlayer].Center + Main.MouseWorld;
                    float ai = Main.rand.Next(100);
                    Vector2 vector83 = Vector2.Normalize(vector82) * item.shootSpeed;
                    Projectile.NewProjectile(player.Center.X, player.Center.Y, vector83.X, vector83.Y, 580, damage, .5f, player.whoAmI, vector82.ToRotation(), ai);
                    return false;
                }
            }
            if(Main.rand.Next(3) == 0) {
                for(int i = 0; i < 3; i++) {
                    float spread = 30f * 0.0174f;//45 degrees converted to radians
                    float baseSpeed = (float)Math.Sqrt(speedX * speedX + speedY * speedY);
                    double baseAngle = Math.Atan2(speedX, speedY);
                    double randomAngle = baseAngle + (Main.rand.NextFloat() - 0.5f) * spread;
                    speedX = baseSpeed * (float)Math.Sin(randomAngle);
                    speedY = baseSpeed * (float)Math.Cos(randomAngle);
                    Projectile.NewProjectile(position.X, position.Y, speedX, speedY, ModContent.ProjectileType<CultistFire>(), item.damage, knockBack, item.owner, 0, 0);
                }
            } else {
                for(int i = 0; i < 5; i++) {
                    float spread = 30f * 0.0174f;//45 degrees converted to radians
                    float baseSpeed = (float)Math.Sqrt(speedX * speedX + speedY * speedY);
                    double baseAngle = Math.Atan2(speedX, speedY);
                    double randomAngle = baseAngle + (Main.rand.NextFloat() - 0.5f) * spread;
                    speedX = baseSpeed * (float)Math.Sin(randomAngle);
                    speedY = baseSpeed * (float)Math.Cos(randomAngle);
                    Projectile.NewProjectile(position.X, position.Y, speedX, speedY, ModContent.ProjectileType<CultistLight>(), item.damage, knockBack, item.owner, 0, 0);
                }
            }
            return false;
        }
        public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(531, 1);
            recipe.AddIngredient(3456, 4);
            recipe.AddIngredient(3457, 4);
            recipe.AddIngredient(3458, 4);
            recipe.AddIngredient(3459, 4);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
