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

namespace SpiritMod.Items.Weapon.Swung.Artifact
{
    public class DeathWind1 : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Death Wind");
            Tooltip.SetDefault("'The Herald of Death itself'\nSummons a returning scythe on swing\nEach scythe takes up one minion slot\nAttacks inflict 'Death Wreathe'\nRight-click to cause nearby enemies to take far more damage\n7 second cooldown");
        }


        public override void SetDefaults() {
            item.damage = 25;
            item.summon = true;
            item.width = 42;
            item.height = 40;
            item.useTime = 22;
            item.useAnimation = 22;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.knockBack = 5;
            item.value = Item.sellPrice(0, 3, 0, 50);
            item.shoot = mod.ProjectileType("DeathWind1Proj");
            item.rare = 2;
            item.shootSpeed = 11f;
            item.UseSound = SoundID.Item69;
            item.autoReuse = true;
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
            if(player.altFunctionUse == 2) {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 110);
                Main.dust[dust].noGravity = true;
            } else {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 110);
                Main.dust[dust].noGravity = true;
            }
        }

        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit) {
            if(Main.rand.Next(9) == 2)
                target.AddBuff(ModContent.BuffType<DeathWreathe>(), 180);
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack) {
            if(player.altFunctionUse == 2) {
                MyPlayer modPlayer = player.GetSpiritPlayer();
                modPlayer.shootDelay1 = 420;
                Projectile.NewProjectile(player.Center.X, player.Center.Y, 0, 0, ModContent.ProjectileType<SoulNet>(), 0, 0, player.whoAmI);
                return true;
            }
            return true;
        }

        public override bool CanUseItem(Player player) {
            if(player.altFunctionUse == 2) {
                MyPlayer modPlayer = player.GetSpiritPlayer();
                if(modPlayer.shootDelay1 == 0)
                    return true;
                return false;
            }

            return true;
        }
    }
}
