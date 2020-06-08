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

namespace SpiritMod.Items.Weapon.Swung
{
    public class RunicSword : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Runic Sword");
            Tooltip.SetDefault("Shoots out exploding, homing runes");
        }


        float downX;
        float downY;
        float upX;
        float upY;
        public override void SetDefaults() {
            item.damage = 52;
            item.melee = true;
            item.width = 42;
            item.height = 50;
            item.useTime = 24;
            item.useAnimation = 24;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.knockBack = 4;
            item.rare = 5;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.value = Item.sellPrice(0, 1, 0, 0);
            item.useTurn = true;
            item.shoot = ModContent.ProjectileType<Projectiles.Magic.Rune>();
            item.shootSpeed = 4f;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack) {
            float SdirX = (float)(Main.MouseWorld.X - player.position.X);
            float SdirY = (float)(Main.MouseWorld.Y - player.position.Y);
            float angleup = (float)Math.Atan(SdirX / SdirY) + 25;
            float angledown = (float)Math.Atan(SdirX / SdirY) - 25;
            if(SdirY < 0) {
                downX = (float)(0 - (Math.Sin(angledown) * 7.5));
                downY = (float)(0 - (Math.Cos(angledown) * 7.5));
                upX = (float)(0 - (Math.Sin(angleup) * 7.5));
                upY = (float)(0 - (Math.Cos(angleup) * 7.5));
            }

            if(SdirY > 0) {
                downX = (float)(Math.Sin(angledown) * 7.5);
                downY = (float)(Math.Cos(angledown) * 7.5);
                upX = (float)(Math.Sin(angleup) * 7.5);
                upY = (float)(Math.Cos(angleup) * 7.5);
            }
            int newProj = Terraria.Projectile.NewProjectile(position.X, position.Y, downX, downY, type, damage, knockBack, player.whoAmI, 0f, 0f);
            int newProj2 = Terraria.Projectile.NewProjectile(position.X, position.Y, upX, upY, type, damage, knockBack, player.whoAmI, 0f, 0f);
            Main.projectile[newProj].magic = false;
            Main.projectile[newProj].melee = true;
            Main.projectile[newProj2].magic = false;
            Main.projectile[newProj2].melee = true;
            return false;
        }
        public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "Rune", 10);
            recipe.AddIngredient(null, "SoulShred", 4);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
