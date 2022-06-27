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
            Item.channel = true;
            Item.damage = 24;
            Item.width = 54;
            Item.height = 62;
            Item.useTime = 320;
            Item.useAnimation = 320;
            Item.crit = 4;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.DamageType = DamageClass.Melee;
            Item.noMelee = true;
            Item.knockBack = 10;
            Item.useTurn = true;
            Item.value = Item.buyPrice(0, 3, 0, 0);
            Item.rare = ItemRarityID.Green;
            Item.autoReuse = false;
            Item.shoot = ModContent.ProjectileType<Projectiles.Clubs.SporeClubProj>();
            Item.shootSpeed = 6f;
            Item.noUseGraphic = true;
        }

		public override Vector2? HoldoutOffset() => new Vector2(-10, 0);
	}
}