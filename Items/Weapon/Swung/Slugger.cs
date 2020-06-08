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

namespace SpiritMod.Items.Weapon.Swung
{
    public class Slugger : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("The Slugger");
            Tooltip.SetDefault("Right click to throw the hammer as a returning boomerang");
        }


        public override void SetDefaults() {
            item.damage = 33;
            item.melee = true;
            item.width = 36;
            item.height = 44;
            item.useTime = 47;
            item.useAnimation = 47;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.knockBack = 11;
            item.value = Terraria.Item.sellPrice(0, 2, 0, 0);
            item.shoot = mod.ProjectileType("Slugger1");
            item.rare = 3;
            item.shootSpeed = 12f;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
        }
        public override bool AltFunctionUse(Player player) {
            return true;
        }

        public override bool CanUseItem(Player player) {
            for(int i = 0; i < 1000; ++i) {
                if(Main.projectile[i].active && Main.projectile[i].owner == Main.myPlayer && Main.projectile[i].type == item.shoot) {
                    return false;
                }
            }
            if(player.altFunctionUse == 2) {
                item.noUseGraphic = true;
                item.shoot = mod.ProjectileType("Slugger1");
            } else {
                item.noUseGraphic = false;
                item.shoot = 0;
            }
            return base.CanUseItem(player);
        }
        public override void MeleeEffects(Player player, Rectangle hitbox) {
            if(Main.rand.Next(7) == 0) {
                int num780 = Dust.NewDust(new Vector2(player.itemLocation.X - 16f, player.itemLocation.Y - 14f * player.gravDir), 16, 16, 173, 0f, 0f, 100, default(Color), 1f);
                if(Main.rand.Next(3) != 0) {
                    Main.dust[num780].noGravity = true;
                }
                Dust obj23 = Main.dust[num780];
                obj23.velocity *= 0.3f;
                Dust expr_3672_cp_0 = Main.dust[num780];
                expr_3672_cp_0.velocity.Y = expr_3672_cp_0.velocity.Y - 1.5f;
                Main.dust[num780].position = player.RotatedRelativePoint(Main.dust[num780].position, true);
            }
        }
    }
}