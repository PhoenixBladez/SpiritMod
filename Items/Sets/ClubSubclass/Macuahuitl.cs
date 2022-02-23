using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

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
            item.channel = true;
            item.damage = 32;
            item.width = 66;
            item.height = 66;
            item.useTime = 320;
            item.useAnimation = 320;
            item.crit = 8;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.melee = true;
            item.noMelee = true;
            item.knockBack = 10;
			item.useTurn = true;
			item.value = Item.buyPrice(0, 8, 0, 0);
            item.rare = ItemRarityID.Green;
            item.autoReuse = false;
            item.shoot = mod.ProjectileType("MacuahuitlProj");
            item.shootSpeed = 6f;
            item.noUseGraphic = true;
        }

		public override Vector2? HoldoutOffset() => new Vector2(-10, 0);
	}
}