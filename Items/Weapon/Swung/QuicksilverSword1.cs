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
    public class QuicksilverSword1 : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Quicksilver Blade");
            Tooltip.SetDefault("Melee hits on foes cause them to explode into a pool of Quicksilver, releasing multiple homing Quicksilver Droplets");

        }


        public override void SetDefaults() {
            item.damage = 92;
            item.useTime = 16;
            item.useAnimation = 16;
            item.melee = true;
            item.width = 74;
            item.height = 84;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.knockBack = 8;
            item.value = Terraria.Item.sellPrice(0, 2, 0, 0);
            item.rare = 8;
            item.UseSound = SoundID.Item69;
            item.autoReuse = true;
            item.useTurn = true;
        }
        public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit) {
            if(!target.chaseable || target.lifeMax <= 5 || target.dontTakeDamage || target.friendly || target.immortal)
                return;

            Vector2 position = target.Center;
            for(int h = 0; h < 2; h++) {
                Vector2 vel = new Vector2(0, -1);
                float rand = Main.rand.NextFloat() * 6.283f;
                vel = vel.RotatedBy(rand);
                vel *= 8f;

                target.friendly = true;
                NPC home = Projectiles.ProjectileExtras.FindRandomNPC(target.Center, 1600f, false);
                target.friendly = false;
                bool homing = home != null;
                Projectile.NewProjectile(position.X, position.Y, vel.X, vel.Y, Projectiles.QuicksilverBolt._type, 45, 1, player.whoAmI, homing ? home.whoAmI : 0, homing ? 30 : 0);

            }
            Main.PlaySound(2, (int)position.X, (int)position.Y, 14);
            //Projectile.NewProjectile(position.X, position.Y, 0f, 0f, ModContent.ProjectileType<Wrath>(), damage, knockback, player.whoAmI, 0f, 0f);

            //Projectiles.ProjectileExtras.Explode()
        }

        public override void MeleeEffects(Player player, Rectangle hitbox) {
            if(Main.rand.Next(2) == 0) {
                int dust2 = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.SilverCoin);
                int dust3 = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.SilverCoin);
                Main.dust[dust2].noGravity = true;
                Main.dust[dust3].noGravity = true;
                Main.dust[dust2].velocity *= 0f;
                Main.dust[dust3].velocity *= 0f;
            }
        }

        public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "Material", 17);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}