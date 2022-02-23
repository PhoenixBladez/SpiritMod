using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace SpiritMod.Items.Sets.ClubSubclass
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
            item.crit = 4;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.melee = true;
            item.noMelee = true;
            item.knockBack = 10;
            item.useTurn = true;
            item.value = Item.buyPrice(0, 3, 0, 0);
            item.rare = ItemRarityID.Green;
            item.autoReuse = false;
            item.shoot = ModContent.ProjectileType<Projectiles.Clubs.SporeClubProj>();
            item.shootSpeed = 6f;
            item.noUseGraphic = true;
        }

		public override Vector2? HoldoutOffset() => new Vector2(-10, 0);
	}
}