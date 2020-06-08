using SpiritMod.Projectiles.Thrown;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Thrown
{
    public class EoWDagger : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Putrid Splitter");
            Tooltip.SetDefault("Splits into two smaller, homing eaters");
        }


        public override void SetDefaults() {
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.width = 30;
            item.height = 50;
            item.noUseGraphic = true;
            item.UseSound = SoundID.Item1;
            item.ranged = true;
            item.channel = true;
            item.noMelee = true;
            item.shoot = ModContent.ProjectileType<EoWDaggerProj>();
            item.useAnimation = 25;
            item.consumable = true;
            item.maxStack = 999;
            item.useTime = 25;
            item.shootSpeed = 8.5f;
            item.damage = 15;
            item.knockBack = 3.5f;
            item.value = Item.sellPrice(0, 0, 0, 50);
            item.value = Item.buyPrice(0, 0, 0, 60);
            item.rare = 2;
            item.autoReuse = false;
            item.maxStack = 999;
            item.consumable = true;
        }
    }
}
