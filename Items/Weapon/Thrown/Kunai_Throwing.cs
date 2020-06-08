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

namespace SpiritMod.Items.Weapon.Thrown
{
    public class Kunai_Throwing : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Kunai");
        }


        float downX;
        float downY;
        float upX;
        float upY;
        public override void SetDefaults() {
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.width = 9;
            item.height = 15;
            item.noUseGraphic = true;
            item.UseSound = SoundID.Item1;
            item.ranged = true;
            item.channel = true;
            item.noMelee = true;
            item.consumable = true;
            item.maxStack = 999;
            item.shoot = mod.ProjectileType("Kunai_Throwing");
            item.useAnimation = 25;
            item.useTime = 25;
            item.shootSpeed = 7.5f;
            item.damage = 12;
            item.knockBack = 1.5f;
            item.value = Terraria.Item.sellPrice(0, 0, 1, 0);
            item.crit = 8;
            item.rare = 1;
            item.autoReuse = true;
            item.maxStack = 999;
            item.consumable = true;
        }

        public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Silk, 1);
            recipe.AddIngredient(ItemID.IronBar, 2);
            recipe.AddTile(16);
            recipe.SetResult(this, 50);
            recipe.AddRecipe();
            recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Silk, 1);
            recipe.AddIngredient(ItemID.LeadBar, 2);
            recipe.AddTile(16);
            recipe.SetResult(this, 50);
            recipe.AddRecipe();
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack) {
            float SdirX = (float)(Main.MouseWorld.X - player.position.X);
            float SdirY = (float)(Main.MouseWorld.Y - player.position.Y);
            float angleup = (float)Math.Atan(SdirX / SdirY) + 25;
            float angledown = (float)Math.Atan(SdirX / SdirY) - 25;
            if(SdirY < 0) {
                downX = (float)(0 - (Math.Sin(angledown) * 8.5));
                downY = (float)(0 - (Math.Cos(angledown) * 8.5));
                upX = (float)(0 - (Math.Sin(angleup) * 8.5));
                upY = (float)(0 - (Math.Cos(angleup) * 8.5));
            }

            if(SdirY > 0) {
                downX = (float)(Math.Sin(angledown) * 8.5);
                downY = (float)(Math.Cos(angledown) * 8.5);
                upX = (float)(Math.Sin(angleup) * 8.5);
                upY = (float)(Math.Cos(angleup) * 8.5);
            }
            Terraria.Projectile.NewProjectile(position.X, position.Y, speedX, speedY, mod.ProjectileType("Kunai_Throwing"), damage, knockBack, player.whoAmI, 0f, 0f);
            Terraria.Projectile.NewProjectile(position.X, position.Y, downX, downY, mod.ProjectileType("Kunai_Throwing"), damage, knockBack, player.whoAmI, 0f, 0f);
            Terraria.Projectile.NewProjectile(position.X, position.Y, upX, upY, mod.ProjectileType("Kunai_Throwing"), damage, knockBack, player.whoAmI, 0f, 0f);
            return false;
        }
    }
}
