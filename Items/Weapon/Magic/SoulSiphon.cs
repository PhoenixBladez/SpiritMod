using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Weapon.Magic
{
    public class SoulSiphon : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Soul Siphon");
			Tooltip.SetDefault("Saps energy from nearby enemies");
		}


        public override void SetDefaults()
        {
            item.width = 26;
            item.height = 28;
            item.rare = 5;
            item.mana = 9;
            item.damage = 42;
            item.knockBack = 2.5f;
            item.useStyle = 5;
			item.UseSound = SoundID.Item20;
            item.useTime = 11;
            item.useAnimation = 11;
            item.value = Item.sellPrice(0, 12, 0, 0);
            item.magic = true;
            item.noMelee = true;
            item.autoReuse = true;
            Item.staff[item.type] = true;
            item.shoot = mod.ProjectileType("SoulSiphonProjectile");
            item.shootSpeed = 10;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 targetPosition = new Vector2(Main.mouseX + Main.screenPosition.X, Main.mouseY + Main.screenPosition.Y);
            while (Collision.CanHitLine(player.position, player.width, player.height, targetPosition, 1, 1))
            {
                position.X += speedX;
                position.Y += speedY;
                if ((position - targetPosition).Length() < 20f + Math.Abs(speedX) + Math.Abs(speedY))
                {
                    position = targetPosition;
                    break;
                }
            }
            Projectile.NewProjectile(position.X, position.Y, 0f, 0f, type, damage, knockBack, player.whoAmI, 0f, 0f);
            return false;
        }
    }
}