using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Swung
{
    public class ReachBossSword : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bloodthorn Blade");
            Tooltip.SetDefault("Increases in damage and speed as health wanes\nOccasionally shoots out a bloody wave\nFires waves more frequently when below 1/2 health\nMelee critical hits poison enemies and inflict 'Withering Leaf'");
        }


        public override void SetDefaults()
        {
            item.damage = 22;
            item.melee = true;
            item.width = 64;
            item.height = 62;
            item.useTime = 32;
            item.useAnimation = 32;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.knockBack = 6;
            item.value = Terraria.Item.sellPrice(0, 2, 0, 0);
            item.shoot = ModContent.ProjectileType<BloodWave>();
            item.rare = 2;
            item.shootSpeed = 8f;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
        }
        public override bool CanUseItem(Player player)
        {
            if (player.statLife <= player.statLifeMax2 / 2)
            {
                item.damage = 27;
                item.useTime = 22;
                item.useAnimation = 22;
            }
            else if (player.statLife <= player.statLifeMax2 / 3)
            {
                item.damage = 26;
                item.useTime = 27;
                item.useAnimation = 27;
            }
            else if (player.statLife <= player.statLifeMax2 / 4)
            {
                item.damage = 24;
                item.useTime = 29;
                item.useAnimation = 29;
            }
            else
            {
                item.damage = 22;
                item.useTime = 32;
                item.useAnimation = 32;
            }
                return base.CanUseItem(player);
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (Main.rand.Next (4) == 1 && player.statLife >= player.statLifeMax2 / 2)
            {
                Main.PlaySound(2, (int)player.position.X, (int)player.position.Y, 20);
                int proj = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, 0, player.whoAmI);
                return false;
            }
            else if (Main.rand.Next(2) == 1)
            {
                Main.PlaySound(2, (int)player.position.X, (int)player.position.Y, 20);
                int proj = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, 0, player.whoAmI);
                return false;

            }
            return false;
        }
        public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit)
        {
            if (crit)
            {
                target.AddBuff(BuffID.Poisoned, 240);
                target.AddBuff(ModContent.BuffType<WitheringLeaf>(), 120, true);
            }
        }
    }
}