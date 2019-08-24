using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
namespace SpiritMod.Items.Weapon.Magic
{
    public class SoulWeaver : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Soul Weaver");
            Tooltip.SetDefault("Shoots out multiple Soul Shards in a spread");

        }


        public override void SetDefaults()
        {
            item.width = 24;
            item.height = 24;
            item.useTurn = false;
            item.autoReuse = true;
            item.value = Terraria.Item.sellPrice(0, 1, 60, 0);
            item.value = Item.buyPrice(0, 2, 0, 0);
            item.rare = 5;
            item.damage = 38;
            item.mana = 5;
            item.useStyle = 5;
            item.useTime = 11;
            item.useAnimation = 11;
            item.UseSound = SoundID.Item9;
            item.knockBack = 2;
            item.magic = true;
            item.channel = true;
            item.noMelee = true;
            item.noUseGraphic = true; 
            item.shoot = mod.ProjectileType("SoulShard");
            item.shootSpeed = 12f;
        }

        public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            type = mod.ProjectileType("SoulShard");
            float spread = 30 * 0.0174f;//45 degrees converted to radians
            float baseSpeed = (float)Math.Sqrt(speedX * speedX + speedY * speedY);
            double baseAngle = Math.Atan2(speedX, speedY);
            double randomAngle = baseAngle + (Main.rand.NextFloat() - 0.5f) * spread;
            speedX = baseSpeed * (float)Math.Sin(randomAngle);
            speedY = baseSpeed * (float)Math.Cos(randomAngle);
            return true;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 0);
        }
    }
}