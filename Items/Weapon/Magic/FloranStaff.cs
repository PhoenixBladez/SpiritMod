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
    public class FloranStaff : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Floran Staff");
            Tooltip.SetDefault("Calls three guarding energies that surround the player before dissipating\nVines occasionally ensnare the foes, reducing their movement speed");
        }


        public override void SetDefaults() {
            item.width = 48;
            item.height = 50;
            item.value = Item.buyPrice(0, 2, 0, 0);
            item.rare = 2;
            item.damage = 17;
            item.useStyle = ItemUseStyleID.HoldingOut;
            Item.staff[item.type] = true;
            item.useTime = 45;
            item.useAnimation = 45;
            item.mana = 12;
            item.knockBack = 3;
            item.magic = true;
            item.UseSound = SoundID.Item20;
            item.noMelee = true;
            item.shoot = ModContent.ProjectileType<FloranOrb>();
            item.shootSpeed = 10f;
        }

        public override void AddRecipes() {
            ModRecipe modRecipe = new ModRecipe(mod);
            modRecipe.AddIngredient(null, "FloranBar", 15);
            modRecipe.AddTile(TileID.Anvils);
            modRecipe.SetResult(this);
            modRecipe.AddRecipe();
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack) {
            //Remove all previous Floran projectiles - creates "reset" behavior
            for(int i = 0; i < Main.projectile.Length; i++) {
                Projectile p = Main.projectile[i];
                if(p.active && p.type == item.shoot && p.owner == player.whoAmI) {
                    p.active = false;
                }
            }

            //get degrees from direction vector
            int dir = (int)(new Vector2(speedX, speedY).ToRotation() / (Math.PI / 180));
            int dir2 = dir + 120;
            int dir3 = dir - 120;

            //spawn the new projectiles
            Projectile.NewProjectile(position.X, position.Y, speedX, speedY, item.shoot, damage, knockBack, player.whoAmI, 0, dir);
            Projectile.NewProjectile(position.X, position.Y, speedX, speedY, item.shoot, damage, knockBack, player.whoAmI, 0, dir2);
            Projectile.NewProjectile(position.X, position.Y, speedX, speedY, item.shoot, damage, knockBack, player.whoAmI, 0, dir3);
            return false;
        }
    }
}
