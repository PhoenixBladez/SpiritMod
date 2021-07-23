using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.BloaterDrops
{
	public class GastricGusher : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gastric Gusher");
			Tooltip.SetDefault("Sprays a cone of harmful acid\nCharge to increase range and damage");
		}

		public override void SetDefaults()
		{
			item.damage = 10;
			item.noMelee = true;
			item.noUseGraphic = true;
			item.ranged = true;
			item.width = 20;
			item.height = 46;
			item.useTime = 21;
			item.useAnimation = 21;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.useAmmo = AmmoID.Gel;
			item.knockBack = 1;
			item.useTurn = false;
			item.value = Item.sellPrice(0, 0, 20, 0);
			item.rare = ItemRarityID.Blue;
			item.UseSound = SoundID.Item5;
			item.autoReuse = false;
			item.shoot = ModContent.ProjectileType<GastricGusherProjectile>();
			item.shootSpeed = 0f;
			item.crit = 8;
			item.channel = true;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			return true;
		}

		public override bool CanUseItem(Player player) => player.ownedProjectileCounts[item.shoot] == 0;
	}
}