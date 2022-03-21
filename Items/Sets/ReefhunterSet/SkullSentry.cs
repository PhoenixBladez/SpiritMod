using Microsoft.Xna.Framework;
using SpiritMod.Items.Sets.ReefhunterSet.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.ReefhunterSet
{
	public class SkullSentry : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Maneater");
			Tooltip.SetDefault("Summons a skull which fights nearby enemies");
		}

		public override void SetDefaults()
		{
			item.damage = 24;
			item.width = 28;
			item.height = 14;
			item.useTime = item.useAnimation = 30;
			item.knockBack = 2f;
			item.shootSpeed = 0f;
			item.noUseGraphic = true;
			item.noMelee = true;
			item.autoReuse = true;
			item.sentry = true;
			item.rare = ItemRarityID.LightRed;
			item.value = Item.sellPrice(gold: 2);
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.shoot = ModContent.ProjectileType<SkullSentrySentry>();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			position = Main.MouseWorld;
			return true;
		}
	}
}