using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace SpiritMod.Items.Sets.ClubSubclass
{
    public class BoneClub : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bone Club");
        }

        public override void SetDefaults()
        {
            item.channel = true;
            item.damage = 27;
            item.width = 60;
            item.height = 60;
            item.useTime = 320;
            item.useAnimation = 320;
            item.crit = 4;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.melee = true;
            item.noMelee = true;
            item.knockBack = 12;
            item.useTurn = true;
            item.value = Item.sellPrice(0, 1, 40, 0);
            item.rare = ItemRarityID.Orange;
            item.autoReuse = false;
            item.shoot = ModContent.ProjectileType<Projectiles.Clubs.BoneClubProj>();
            item.shootSpeed = 6f;
            item.noUseGraphic = true;
        }

		public override Vector2? HoldoutOffset() => new Vector2(-10, 0);
	}
}