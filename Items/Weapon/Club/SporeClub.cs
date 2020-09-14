using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace SpiritMod.Items.Weapon.Club
{
    public class SporeClub : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sporebreaker");
            Tooltip.SetDefault("Charged strikes release a field of toxins");
        }

        public override void SetDefaults()
        {
            item.channel = true;
            item.damage = 24;
            item.width = 54;
            item.height = 62;
            item.useTime = 320;
            item.useAnimation = 320;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.melee = true;
            item.noMelee = true;
            item.knockBack = 10;
            item.useTurn = false;
            item.value = Terraria.Item.buyPrice(0, 3, 0, 0);
            item.rare = 2;
            item.autoReuse = false;
            item.shoot = mod.ProjectileType("SporeClubProj");
            item.shootSpeed = 6f;
            item.noUseGraphic = true;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
        }
        public override bool CanUseItem(Player player)
        {
            return base.CanUseItem(player);
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 0);
        }
    }
}