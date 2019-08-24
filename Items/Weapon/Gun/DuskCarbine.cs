using Terraria;
using System;
using Terraria.ID;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;


namespace SpiritMod.Items.Weapon.Gun
{
    public class DuskCarbine : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dusk Carbine");
			Tooltip.SetDefault("Converts regular bullets into Shadowflame bullets!");
		}


        public override void SetDefaults()
        {
            item.width = 54;
            item.height = 28;
            item.rare = 5;
            item.UseSound = SoundID.Item11;
            item.crit = 4;
            item.damage = 37;
            item.knockBack = 6;
            item.useTurn = false;
            item.value = Terraria.Item.sellPrice(0, 3, 0, 0);

            item.useStyle = 5;
            item.useTime = 9;
            item.useAnimation = 9;
            item.reuseDelay = 16;

            item.ranged = true;
            item.autoReuse = true;
            item.useAmmo = AmmoID.Bullet;

            item.shoot = 10;
            item.shootSpeed = 8;
        }

        public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (type == ProjectileID.Bullet)
                type = mod.ProjectileType("ShadowflameBullet");

            return true;
        }

        public override bool ConsumeAmmo(Player player)
        {
            return player.itemAnimation >= item.useAnimation - 2;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 0);
        }
    }
}
