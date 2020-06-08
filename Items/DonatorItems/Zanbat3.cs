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
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.DonatorItems
{
    public class Zanbat3 : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Zanbat Blade");
        }
        int charger;
        public override void SetDefaults() {
            item.damage = 61;
            item.useTime = 19;
            item.useAnimation = 19;
            item.melee = true;
            item.width = 48;
            item.height = 48;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.knockBack = 7;
            item.value = 25700;
            item.rare = 4;
            item.crit = 5;
            item.shootSpeed = 11f;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.useTurn = true;
            item.shoot = ModContent.ProjectileType<ZanbatProj>();
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack) {
            if(Main.rand.Next(4) == 1) {
                Main.PlaySound(2, (int)player.position.X, (int)player.position.Y, 20);
                return true;
            }
            return false;
        }
        public override Color? GetAlpha(Color lightColor) {
            return Color.White;
        }
        public override void UseStyle(Player player) {
            float cosRot = (float)Math.Cos(player.itemRotation - 0.98f * player.direction * player.gravDir * 90f);
            float sinRot = (float)Math.Sin(player.itemRotation - 0.98f * player.direction * player.gravDir * 90f);
            for(int i = 0; i < 8; i++) {

                float length = (item.width * 1.2f - i * item.width / 9) * item.scale + 7 + i; //length to base + arm displacement
                int dust = Dust.NewDust(new Vector2((float)(player.itemLocation.X + length * cosRot * player.direction), (float)(player.itemLocation.Y + length * sinRot * player.direction)), 0, 0, 133, player.velocity.X * 0.9f, player.velocity.Y * 0.9f, 100, Color.White, .8f);
                Main.dust[dust].shader = GameShaders.Armor.GetSecondaryShader(58, Main.LocalPlayer);
                Main.dust[dust].velocity *= 0f;
                Main.dust[dust].noGravity = true;
                Main.dust[dust].noLight = true;
            }
        }
        public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "Zanbat2", 1);
            recipe.AddIngredient(ItemID.TitaniumSword, 1);
            recipe.AddIngredient(ItemID.SoulofMight, 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();

            ModRecipe recipe1 = new ModRecipe(mod);
            recipe1.AddIngredient(null, "Zanbat2", 1);
            recipe1.AddIngredient(ItemID.AdamantiteSword, 1);
            recipe1.AddIngredient(ItemID.SoulofMight, 10);
            recipe1.AddTile(TileID.MythrilAnvil);
            recipe1.SetResult(this, 1);
            recipe1.AddRecipe();

        }
    }
}