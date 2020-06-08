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
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic.Artifact
{
    public class Solus4 : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Solus");
            Tooltip.SetDefault("'You are the Artifact Keeper of Asra Nox'\nShoots out three homing Bolts with varied effects\nPhoenix Bolts explode upon hitting foes and inflict 'Blaze'\nEnemies inflicted with 'Blaze' may randomly combust\nFrost Bolts may freeze enemies in place and explode into chilling wisps\nShadow Bolts pierce multiple enemies and inflict 'Shadow Burn,' which hinders enemy damage and deals large amounts of damage\nAttacks may release multiple different revolving embers with varied effects");
        }


        public override void SetDefaults() {
            item.damage = 87;
            item.magic = true;
            item.mana = 13;
            item.width = 58;
            item.height = 66;
            item.useTime = 18;
            item.useAnimation = 18;
            item.useStyle = ItemUseStyleID.HoldingOut;
            Item.staff[item.type] = true;
            item.noMelee = true;
            item.knockBack = 4;
            item.value = Item.sellPrice(0, 7, 0, 50);
            item.rare = 10;
            item.crit = 6;
            item.UseSound = SoundID.Item74;
            item.autoReuse = true;
            item.shoot = ModContent.ProjectileType<PhoenixBolt>();
            item.shootSpeed = 1f;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips) {
            TooltipLine line = new TooltipLine(mod, "ItemName", "Artifact Weapon");
            line.overrideColor = new Color(100, 0, 230);
            tooltips.Add(line);
            foreach(TooltipLine line2 in tooltips) {
                if(line2.mod == "Terraria" && line2.Name == "ItemName") {
                    line2.overrideColor = new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB);
                    break;
                }
            }
        }

        public override void HoldItem(Player player) {
            if(player.GetSpiritPlayer().HolyGrail) {
                player.AddBuff(ModContent.BuffType<Righteous>(), 2);
                item.crit = 10;
                item.mana = 10;
            } else {
                item.crit = 6;
                item.mana = 13;
            }
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack) {
            int choice = Main.rand.Next(6);
            if(choice == 0)
                Projectile.NewProjectile(position.X, position.Y, speedX, speedY, ModContent.ProjectileType<Ember1>(), damage, 1, player.whoAmI);
            else if(choice == 1)
                Projectile.NewProjectile(position.X, position.Y, speedX, speedY, ModContent.ProjectileType<Ember2>(), damage, 1, player.whoAmI);
            else if(choice == 2)
                Projectile.NewProjectile(position.X, position.Y, speedX, speedY, ModContent.ProjectileType<Ember3>(), damage, 1, player.whoAmI);

            Projectile.NewProjectile(position.X, position.Y, speedX * (Main.rand.Next(500, 900) / 100), speedY * (Main.rand.Next(500, 900) / 100), ModContent.ProjectileType<PhoenixBolt1>(), damage, knockBack, player.whoAmI);
            Projectile.NewProjectile(position.X, position.Y, speedX * (Main.rand.Next(500, 900) / 100), speedY * (Main.rand.Next(500, 900) / 100), ModContent.ProjectileType<DarkBolt1>(), damage, knockBack, player.whoAmI);
            Projectile.NewProjectile(position.X, position.Y, speedX * (Main.rand.Next(500, 900) / 100), speedY * (Main.rand.Next(500, 900) / 100), ModContent.ProjectileType<FreezeBolt1>(), damage, knockBack, player.whoAmI);
            return false;
        }
    }
}
