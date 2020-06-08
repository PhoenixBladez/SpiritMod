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
using SpiritMod.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic
{
    public class HowlingScepter : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Frigid Scepter");
        }

        public override void SetDefaults() {
            item.damage = 9;
            Item.staff[item.type] = true;
            item.noMelee = true;
            item.magic = true;
            item.width = 64;
            item.height = 64;
            item.useTime = 25;
            item.mana = 4;
            item.useAnimation = 25;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.knockBack = 4;
            item.value = Terraria.Item.sellPrice(0, 0, 5, 0);
            item.rare = 1;
            item.crit = 6;
            item.autoReuse = false;
            item.shootSpeed = 9;
            item.UseSound = SoundID.Item20;
            item.shoot = ModContent.ProjectileType<HowlingBolt>();
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack) {
            int proj = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI);
            Projectile projectile = Main.projectile[proj];
            for(int k = 0; k < 25; k++) {
                Vector2 mouse = new Vector2(Main.mouseX, Main.mouseY) + Main.screenPosition;
                Vector2 offset = mouse - player.position;
                offset.Normalize();
                offset *= 51f;
                int dust = Dust.NewDust(projectile.Center + offset, projectile.width, projectile.height, 68);

                Main.dust[dust].velocity *= -1f;
                Main.dust[dust].noGravity = true;
                //        Main.dust[dust].scale *= 2f;
                Vector2 vector2_1 = new Vector2((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101));
                vector2_1.Normalize();
                Vector2 vector2_2 = vector2_1 * ((float)Main.rand.Next(50, 100) * 0.04f);
                Main.dust[dust].velocity = vector2_2;
                vector2_2.Normalize();
                Vector2 vector2_3 = vector2_2 * 42f;
                Main.dust[dust].position = (projectile.Center + offset) - vector2_3;
            }
            return false;
        }
        public override void AddRecipes() {

            ModRecipe modRecipe = new ModRecipe(mod);
            modRecipe.AddIngredient(null, "FrigidFragment", 12);
            modRecipe.AddTile(TileID.Anvils);
            modRecipe.SetResult(this, 1);
            modRecipe.AddRecipe();
        }
    }
}
