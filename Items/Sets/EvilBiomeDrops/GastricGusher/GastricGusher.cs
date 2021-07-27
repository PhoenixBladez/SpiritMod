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
			item.damage = 10;
			item.width = 20;
			item.height = 46;
			item.useTime = 20;
			item.useAnimation = 20;
			item.knockBack = 1;
			item.crit = 8;
			item.shootSpeed = 0f;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.useAmmo = AmmoID.Gel;
			item.rare = ItemRarityID.Blue;
			//item.UseSound = SoundID.None;
			item.shoot = ModContent.ProjectileType<GastricGusherProjectile>();
			item.value = Item.sellPrice(0, 0, 20, 0);
			item.channel = true;
			item.noMelee = true;
			item.useTurn = false;
			item.noUseGraphic = true;
			item.autoReuse = false;
			item.ranged = true;
		}

		public override bool ConsumeAmmo(Player player) => false;
		public override bool CanUseItem(Player player) => player.ownedProjectileCounts[item.shoot] == 0;
	}
}