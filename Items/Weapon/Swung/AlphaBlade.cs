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
    public class AlphaBlade : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Alpha Blade");
            Tooltip.SetDefault("'The power of the universe sides with you'");
        }


        public override void SetDefaults() {
            item.damage = 200;
            item.melee = true;
            item.width = 70;
            item.height = 76;
            item.useTime = 16;
            item.useAnimation = 16;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.knockBack = 6;
            item.value = Terraria.Item.sellPrice(1, 0, 0, 0);
            item.rare = 12;
            item.UseSound = SoundID.Item1;
            item.shoot = ModContent.ProjectileType<PestilentSwordProjectile>();
            item.shootSpeed = 4f;
            item.autoReuse = true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack) {
            Vector2 origVect = new Vector2(speedX, speedY);
            //generate the remaining projectiles

            Vector2 newVect = origVect.RotatedBy(System.Math.PI / (Main.rand.Next(72, 1800) / 10));
            Projectile.NewProjectile(position.X, position.Y, newVect.X, newVect.Y, mod.ProjectileType("AlphaProj1"), damage, knockBack, player.whoAmI, 0f, 0f);
            newVect = origVect.RotatedBy(System.Math.PI / (Main.rand.Next(72, 1800) / 10));
            Projectile.NewProjectile(position.X, position.Y, newVect.X, newVect.Y, mod.ProjectileType("AlphaProj2"), damage, knockBack, player.whoAmI, 0f, 0f);
            newVect = origVect.RotatedBy(System.Math.PI / (Main.rand.Next(72, 1800) / 10));
            Projectile.NewProjectile(position.X, position.Y, newVect.X, newVect.Y, mod.ProjectileType("AlphaProj3"), damage, knockBack, player.whoAmI, 0f, 0f);
            newVect = origVect.RotatedBy(System.Math.PI / (Main.rand.Next(72, 1800) / 10));
            Projectile.NewProjectile(position.X, position.Y, newVect.X, newVect.Y, mod.ProjectileType("AlphaProj4"), damage, knockBack, player.whoAmI, 0f, 0f);
            newVect = origVect.RotatedBy(System.Math.PI / (Main.rand.Next(72, 1800) / 10));
            Projectile.NewProjectile(position.X, position.Y, newVect.X * 1.5f, newVect.Y * 1.5f, mod.ProjectileType("AlphaProj5"), damage, knockBack, player.whoAmI, 0f, 0f);

            return false;
        }
        public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "EternityEssence", 20);
            recipe.AddIngredient(null, "SpiritStar", 1);
            recipe.AddIngredient(3467, 10);
            recipe.AddIngredient(3456, 4);
            recipe.AddIngredient(3457, 4);
            recipe.AddIngredient(3458, 4);
            recipe.AddIngredient(3459, 4);
            recipe.AddTile(412);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}