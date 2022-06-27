using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.EvilBiomeDrops.GastricGusher
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
			Item.damage = 10;
			Item.width = 20;
			Item.height = 46;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.knockBack = 1;
			Item.crit = 8;
			Item.shootSpeed = 0f;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.useAmmo = AmmoID.Gel;
			Item.rare = ItemRarityID.Blue;
			Item.shoot = ModContent.ProjectileType<GastricGusherProjectile>();
			Item.value = Item.sellPrice(0, 0, 20, 0);
			Item.channel = true;
			Item.noMelee = true;
			Item.useTurn = false;
			Item.noUseGraphic = true;
			Item.autoReuse = false;
			Item.DamageType = DamageClass.Ranged;
		}

		public override bool CanConsumeAmmo(Item item, Player player) => false; //So the ammo counter shoots but we don't use double ammo on spam click
		public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] == 0;
	}
}