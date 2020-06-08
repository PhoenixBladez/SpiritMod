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
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Swung
{
    public class MagalaBlade : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Elendskraft");
            Tooltip.SetDefault("Right click to convert Elendskraft into a damaging shield\nEnemies in an aura around the shield receive 'Frenzy Virus' and are knocked back\n'Packs a powerful punch'");
        }


        public override void SetDefaults() {
            item.damage = 50;
            item.melee = true;
            item.width = 54;
            item.height = 44;
            item.useTime = 23;
            item.noUseGraphic = false;
            item.useAnimation = 23;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.knockBack = 5;
            item.value = Terraria.Item.sellPrice(0, 5, 0, 0);
            item.shoot = ModContent.ProjectileType<MagalaShield>();
            item.rare = 5;
            item.shootSpeed = 5f;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
        }
        public override bool AltFunctionUse(Player player) {
            return true;
        }
        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit) {
            if(player.altFunctionUse == 2) {
                target.AddBuff(ModContent.BuffType<FrenzyVirus>(), 580);
            }
        }
        public override bool CanUseItem(Player player) {
            if(player.altFunctionUse == 2) {
                item.damage = 10;
                item.noUseGraphic = true;
                item.shoot = ModContent.ProjectileType<MagalaShield>();
                item.useStyle = 3;
                item.height = 2;
                item.width = 2;
                item.knockBack = 9;
                item.autoReuse = false;
                item.shootSpeed = 2f;
            } else {
                item.damage = 50;
                item.noUseGraphic = false;
                item.useTime = 24;
                item.width = 54;
                item.height = 44;
                item.useAnimation = 24;
                item.shoot = 0;
                item.knockBack = 5;
                item.useStyle = ItemUseStyleID.SwingThrow;
                item.autoReuse = true;
                item.shootSpeed = 0f;
            }
            return base.CanUseItem(player);
        }
        public override void MeleeEffects(Player player, Rectangle hitbox) {
            if(player.altFunctionUse == 2) {

            } else {
                if(Main.rand.Next(2) == 0) {
                    int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 173);
                }
            }
        }
        public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "MagalaScale", 20);
            recipe.AddIngredient(ItemID.DD2SquireDemonSword);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}