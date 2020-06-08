using SpiritMod.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Swung
{
    public class ScarabSword : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Longhorn Blade");
            Tooltip.SetDefault("Kicks up waves of sand and dust");
        }


        public override void SetDefaults() {
            item.damage = 14;
            item.melee = true;
            item.width = 50;
            item.autoReuse = true;
            item.height = 50;
            item.useTime = 32;
            item.useAnimation = 32;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.knockBack = 3;
            item.value = Item.sellPrice(0, 1, 0, 0);
            item.rare = 1;
            item.UseSound = SoundID.Item1;
            item.shoot = ModContent.ProjectileType<ScarabProjectile>();
            item.shootSpeed = 7; ;
            item.crit = 8;
        }
    }
}