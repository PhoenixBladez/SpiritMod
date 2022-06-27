using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Clubs;

namespace SpiritMod.Items.Sets.ClubSubclass
{
    public class Macuahuitl : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Macuahuitl");
            Tooltip.SetDefault("Greatly increases armor penetration based on charge time");
        }

        public override void SetDefaults()
        {
            Item.channel = true;
            Item.damage = 32;
            Item.width = 66;
            Item.height = 66;
            Item.useTime = 320;
            Item.useAnimation = 320;
            Item.crit = 8;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.DamageType = DamageClass.Melee;
            Item.noMelee = true;
            Item.knockBack = 10;
			Item.useTurn = true;
			Item.value = Item.buyPrice(0, 8, 0, 0);
            Item.rare = ItemRarityID.Green;
            Item.autoReuse = false;
            Item.shoot = ModContent.ProjectileType<MacuahuitlProj>();
            Item.shootSpeed = 6f;
            Item.noUseGraphic = true;
        }

		public override Vector2? HoldoutOffset() => new Vector2(-10, 0);
	}
}