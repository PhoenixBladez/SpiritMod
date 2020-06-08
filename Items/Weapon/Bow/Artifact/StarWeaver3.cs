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

namespace SpiritMod.Items.Weapon.Bow.Artifact
{
    public class StarWeaver3 : ModItem
    {
        int charger;
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Star Weaver");
            Tooltip.SetDefault("Converts arrows into two Astral Bolts\nAstral Bolts may split into five damaging shards of energy\nCritical hits with Astral Bolts cause homing Astral Arrows to rain from the sky\nRight click to shoot out an explosive Burning Core\nHold right-click to increase the power of Burning Cores, resetting at three");
        }

        public override void SetDefaults() {
            item.damage = 48;
            item.noMelee = true;
            item.ranged = true;
            item.width = 36;
            item.height = 74;
            item.useTime = 22;
            item.useAnimation = 22;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.shoot = 1;
            item.useAmmo = AmmoID.Arrow;
            item.knockBack = 1.75f;
            item.value = Terraria.Item.sellPrice(0, 7, 0, 50);
            item.rare = 7;
            item.crit = 4;
            item.UseSound = SoundID.Item77;
            item.autoReuse = true;
            item.useTurn = false;
            item.shootSpeed = 10f;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips) {
            TooltipLine line = new TooltipLine(mod, "ItemName", "Artifact Weapon");
            line.overrideColor = new Color(100, 0, 230);
            tooltips.Add(line);
        }
        public override bool AltFunctionUse(Player player) {
            return true;
        }
        public override Vector2? HoldoutOffset() {
            return new Vector2(-10, 0);
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack) {
            if(player.altFunctionUse == 2) {

                charger++;
                if(charger >= 1) {
                    {
                        Projectile.NewProjectile(position.X, position.Y, speedX, speedY, mod.ProjectileType("Stars1"), 70, 4, player.whoAmI, 0f, 0f);
                    }
                }
                if(charger >= 2) {
                    {
                        Projectile.NewProjectile(position.X, position.Y, speedX, speedY, mod.ProjectileType("Stars2"), 80, 5, player.whoAmI, 0f, 0f);
                    }
                }
                if(charger >= 3) {
                    {
                        Projectile.NewProjectile(position.X, position.Y - 10, speedX, speedY, mod.ProjectileType("Stars3"), 120, 6, player.whoAmI, 0f, 0f);
                    }
                    charger = 0;
                }
                return false;
            } else {
                charger = 0;
                for(int I = 0; I < 2; I++) {
                    Projectile.NewProjectile(position.X, position.Y, speedX + ((float)Main.rand.Next(-102, 102) / 100), speedY + ((float)Main.rand.Next(-102, 102) / 100), mod.ProjectileType("StarPin1"), damage, knockBack, player.whoAmI, 0f, 0f);
                };
            }
            return false;
        }
        public override bool CanUseItem(Player player) {
            if(player.altFunctionUse == 2) {
                item.useTime = 40;
                item.useAnimation = 40;
                return true;
            } else {
                item.useTime = 24;
                item.useAnimation = 24;
                return true;
            }
        }
    }
}