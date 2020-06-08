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
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Swung
{
    public class GlowSting : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Seraph's Strike");
            Tooltip.SetDefault("Right-click to release a flurry of strikes");
            SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Weapon/Swung/GlowSting_Glow");
        }

        int currentHit;
        public override void SetDefaults() {
            item.damage = 47;
            item.melee = true;
            item.width = 34;
            item.height = 40;
            item.autoReuse = true;
            item.useTime = 15;
            item.useAnimation = 15;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.knockBack = 6;
            item.value = Item.sellPrice(0, 1, 20, 0);
            item.rare = 4;
            item.UseSound = SoundID.Item1;
            item.shoot = ModContent.ProjectileType<GlowStingSpear>();
            item.shootSpeed = 10f;
            this.currentHit = 0;
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI) {
            Texture2D texture;
            texture = Main.itemTexture[item.type];
            spriteBatch.Draw
            (
                ModContent.GetTexture("SpiritMod/Items/Weapon/Swung/GlowSting_Glow"),
                new Vector2
                (
                    item.position.X - Main.screenPosition.X + item.width * 0.5f,
                    item.position.Y - Main.screenPosition.Y + item.height - texture.Height * 0.5f + 2f
                ),
                new Rectangle(0, 0, texture.Width, texture.Height),
                Color.White,
                rotation,
                texture.Size() * 0.5f,
                scale,
                SpriteEffects.None,
                0f
            );
        }
        public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit) {
            if(Main.rand.Next(3) == 0)
                target.AddBuff(ModContent.BuffType<StarFlame>(), 180);
        }
        public override bool AltFunctionUse(Player player) {
            return true;
        }
        public override bool CanUseItem(Player player) {
            if(player.altFunctionUse == 2) {
                item.noUseGraphic = true;
                item.shoot = ModContent.ProjectileType<GlowStingSpear>();
                item.useStyle = ItemUseStyleID.HoldingOut;
                item.useTime = 12;
                item.useAnimation = 12;
                item.knockBack = 2;
                item.shootSpeed = 8f;
            } else {
                item.noUseGraphic = false;
                item.useTime = 25;
                item.useAnimation = 25;
                item.shoot = 0;
                item.knockBack = 5;
                item.useStyle = ItemUseStyleID.SwingThrow;
                item.shootSpeed = 0f;
            }
            if(player.ownedProjectileCounts[item.shoot] > 0)
                return false;
            return true;
        }
        public override void UseStyle(Player player) {
            if(player.altFunctionUse != 2) {
                for(int projFinder = 0; projFinder < 300; ++projFinder) {
                    if(Main.projectile[projFinder].type == ModContent.ProjectileType<GlowStingSpear>()) {
                        Main.projectile[projFinder].Kill();
                        Main.projectile[projFinder].timeLeft = 2;
                    }
                }
            }
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack) {
            if(player.altFunctionUse == 2) {
                Vector2 origVect = new Vector2(speedX, speedY);
                Vector2 newVect = Vector2.Zero;
                if(Main.rand.Next(2) == 1) {
                    newVect = origVect.RotatedBy(System.Math.PI / (Main.rand.Next(82, 1800) / 10));
                } else {
                    newVect = origVect.RotatedBy(-System.Math.PI / (Main.rand.Next(82, 1800) / 10));
                }
                speedX = newVect.X;
                speedY = newVect.Y;
                this.currentHit++;
                return true;
            }
            return true;
        }
        public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "MoonStone", 8);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}