using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
namespace SpiritMod.Items.Weapon.Gun
{
    public class GhastBeam : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ghast Beam");
            Tooltip.SetDefault("Shoots out Ghast Lasers that may deal multiple ticks of damage");

        }


        private Vector2 newVect;
        int charger;
        public override void SetDefaults()
        {
            item.damage = 41;
            item.ranged = true;
            item.width = 58;
            item.height = 32;
            item.useTime = 9;
            item.useAnimation = 9;
            item.useStyle = 5;
            item.noMelee = true;
            item.knockBack = 3.3f;
            item.useTurn = false;
            item.value = Terraria.Item.sellPrice(0, 8, 0, 0);
            item.rare = 8;
            item.UseSound = SoundID.Item91;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("GhastLaser");
            item.shootSpeed = 14f;
            item.useAmmo = AmmoID.Bullet;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Terraria.Projectile.NewProjectile(position.X, position.Y, speedX, speedY, mod.ProjectileType("GhastLaser"), damage, knockBack, player.whoAmI, 0f, 0f);
            return false;

        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 0);
        }
    }
}