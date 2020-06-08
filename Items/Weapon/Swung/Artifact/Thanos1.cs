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
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Swung.Artifact
{
    public class Thanos1 : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Shard of Thanos");
            Tooltip.SetDefault("'As old as the dawn of Man'\nShoots out an afterimage of the Shard\nRight-click to summon a storm of rotating crystals around the player");

        }


        public override void SetDefaults() {
            item.damage = 22;
            item.melee = true;
            item.width = 42;
            item.height = 40;
            item.useTime = 22;
            item.useAnimation = 22;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.knockBack = 5;
            item.value = Item.sellPrice(0, 3, 0, 50);
            item.shoot = mod.ProjectileType("Thanos1Proj");
            item.rare = 2;
            item.shootSpeed = 9f;
            item.UseSound = SoundID.Item69;
            item.autoReuse = true;
        }

        public override void HoldItem(Player player) {
            if(player.GetSpiritPlayer().Resolve) {
                player.AddBuff(ModContent.BuffType<Resolve>(), 2);
            }
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips) {
            TooltipLine line = new TooltipLine(mod, "ItemName", "Artifact Weapon");
            line.overrideColor = new Color(100, 0, 230);
            tooltips.Add(line);
        }

        public override bool AltFunctionUse(Player player) {
            return true;
        }

        public override void MeleeEffects(Player player, Rectangle hitbox) {
            Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, ModContent.DustType<Dusts.Crystal>());
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack) {
            if(player.altFunctionUse == 2) {
                float kb = knockBack * .2f;
                Projectile.NewProjectile(player.Center.X - 100, player.Center.Y, speedX, speedY, ModContent.ProjectileType<Thanos1Crystal>(), damage, kb, player.whoAmI);
                Projectile.NewProjectile(player.Center.X + 100, player.Center.Y, speedX, speedY, ModContent.ProjectileType<Thanos1Crystal>(), damage, kb, player.whoAmI);
                Projectile.NewProjectile(player.Center.X + 115, player.Center.Y - 115, speedX, speedY, ModContent.ProjectileType<Thanos1Crystal>(), damage, kb, player.whoAmI);
                Projectile.NewProjectile(player.Center.X - 115, player.Center.Y + 115, speedX, speedY, ModContent.ProjectileType<Thanos1Crystal>(), damage, kb, player.whoAmI);
                Projectile.NewProjectile(player.Center.X, player.Center.Y + 110, speedX, speedY, ModContent.ProjectileType<Thanos1Crystal>(), damage, kb, player.whoAmI);
                Projectile.NewProjectile(player.Center.X, player.Center.Y - 110, speedX, speedY, ModContent.ProjectileType<Thanos1Crystal>(), damage, kb, player.whoAmI);
                return false;
            }
            return true;
        }
    }
}