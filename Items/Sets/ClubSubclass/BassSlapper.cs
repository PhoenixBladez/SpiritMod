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
            item.channel = true;
            item.damage = 16;
            item.width = 60;
            item.height = 60;
            item.useTime = 320;
            item.useAnimation = 320;
            item.crit = 4;
            item.useStyle = ItemUseStyleID.EatingUsing;
            item.melee = true;
            item.noMelee = true;
            item.knockBack = 12;
            item.useTurn = false;
            item.value = Item.sellPrice(0, 1, 42, 0);
            item.rare = ItemRarityID.Orange;
            item.autoReuse = false;
			item.shoot = ModContent.ProjectileType<BassSlapperProj>();
            item.shootSpeed = 6f;
            item.noUseGraphic = true;
        }

		public override Vector2? HoldoutOffset() => new Vector2(-10, 0);
	}
}