
using SpiritMod.Projectiles.Bullet;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Ammo
{
    public class SepulchreBullet : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Accursed Round");
            Tooltip.SetDefault("Pierces up to two enemies");
        }


        public override void SetDefaults() {
            item.width = 8;
            item.height = 16;
            item.value = 80;
            item.rare = 1;
            item.value = Item.buyPrice(0, 0, 0, 4);
            item.maxStack = 999;

            item.damage = 8;
            item.knockBack = 1f;
            item.ammo = AmmoID.Bullet;

            item.ranged = true;
            item.consumable = true;

            item.shoot = ModContent.ProjectileType<AccursedBullet>();
            item.shootSpeed = 8f;

        }
    }
}