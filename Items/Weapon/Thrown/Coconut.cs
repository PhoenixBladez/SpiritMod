using SpiritMod.Projectiles.Thrown;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Thrown
{
    public class Coconut : ModItem
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Hard Coconut");
            Tooltip.SetDefault("Does more damage if dropped from high up\n'You're not brave enough to try eating it'");
        }

        public override void SetDefaults() {
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.width = 9;
            item.height = 15;
            item.noUseGraphic = true;
            item.UseSound = SoundID.Item1;
            item.melee = true;
            item.channel = true;
            item.noMelee = true;
            item.consumable = true;
            item.maxStack = 999;
            item.shoot = ModContent.ProjectileType<CoconutP>();
            item.useAnimation = 60;
            item.useTime = 60;
            item.shootSpeed = 4f;
            item.damage = 16;
            item.knockBack = 3.5f;
            item.value = Terraria.Item.sellPrice(0, 0, 3, 0);
            item.crit = 8;
            item.rare = 2;
            item.autoReuse = true;
            item.maxStack = 999;
            item.consumable = true;
        }
    }
}
