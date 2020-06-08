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

namespace SpiritMod.Items.Weapon.Magic
{
    public class QuakeFist : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Quake Fist");
            Tooltip.SetDefault("Launches Prismatic fire \nOccasionally inflicts foes with 'Unstable Affliction'");
        }


        private Vector2 newVect;
        int charger;
        public override void SetDefaults() {
            item.damage = 67;
            item.magic = true;
            item.mana = 19;
            item.width = 30;
            item.height = 34;
            item.useTime = 24;
            item.useAnimation = 24;
            item.useStyle = ItemUseStyleID.HoldingOut;//this makes the useStyle animate as a staff instead of as a gun
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 6;
            item.useTurn = false;
            item.value = Terraria.Item.sellPrice(0, 7, 0, 0);
            item.rare = 9;
            item.UseSound = SoundID.Item8;
            item.autoReuse = true;
            item.shoot = ModContent.ProjectileType<PrismaticBolt>();
            item.shootSpeed = 16f;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack) {

            int proj = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, mod.ProjectileType("PrismBolt2"), damage, knockBack, player.whoAmI);
            Projectile newProj = Main.projectile[proj];
            newProj.friendly = true;
            newProj.hostile = false;
            Vector2 origVect = new Vector2(speedX, speedY);
            for(int X = 0; X <= 2; X++) {
                if(Main.rand.Next(2) == 1) {
                    newVect = origVect.RotatedBy(System.Math.PI / (Main.rand.Next(82, 1800) / 10));
                } else {
                    newVect = origVect.RotatedBy(-System.Math.PI / (Main.rand.Next(82, 1800) / 10));
                }
                int proj2 = Projectile.NewProjectile(position.X, position.Y, newVect.X, newVect.Y, type, damage, knockBack, player.whoAmI);
                Projectile newProj2 = Main.projectile[proj2];
            }
            for(int i = 0; i < 3; ++i) {
                if(Main.rand.Next(6) == 0) {
                    if(Main.myPlayer == player.whoAmI) {
                        Vector2 mouse = Main.MouseWorld;
                        Projectile.NewProjectile(mouse.X + Main.rand.Next(-80, 80), player.Center.Y - 1000 + Main.rand.Next(-50, 50), 0, Main.rand.Next(18, 28), ModContent.ProjectileType<AtlasBolt>(), 50, knockBack, player.whoAmI);
                    }
                }
            }
            return false;
        }
    }
}