using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Clubs;

namespace SpiritMod.Items.Sets.ClubSubclass
{
    public class BassSlapper : ModItem
    {
		public override void SetStaticDefaults() => DisplayName.SetDefault("Bass Slapper");

		public override void SetDefaults()
        {
            Item.channel = true;
            Item.damage = 16;
            Item.width = 60;
            Item.height = 60;
            Item.useTime = 320;
            Item.useAnimation = 320;
            Item.crit = 4;
            Item.useStyle = ItemUseStyleID.EatFood;
            Item.DamageType = DamageClass.Melee;
            Item.noMelee = true;
            Item.knockBack = 12;
            Item.useTurn = true;
            Item.value = Item.sellPrice(0, 1, 42, 0);
            Item.rare = ItemRarityID.Orange;
            Item.autoReuse = false;
			Item.shoot = ModContent.ProjectileType<BassSlapperProj>();
            Item.shootSpeed = 6f;
            Item.noUseGraphic = true;
        }

		public override Vector2? HoldoutOffset() => new Vector2(-10, 0);
	}
}