using Microsoft.Xna.Framework;
using SpiritMod.Items.Material;
using SpiritMod.Projectiles.Clubs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Club
{
	public class BruteHammer : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Brute Hammer");
			Tooltip.SetDefault("Hold down and release to slam");
		}


		private Vector2 newVect;
		public override void SetDefaults()
		{
			item.useStyle = 100;
			item.width = 40;
			item.height = 32;
			item.noUseGraphic = true;
			item.UseSound = SoundID.Item1;
			item.melee = true;
			item.channel = true;
			item.noMelee = true;
			item.useAnimation = 30;
			item.useTime = 30;
			item.shootSpeed = 8f;
			item.knockBack = 5f;
			item.damage = 12;
			item.value = Item.sellPrice(0, 0, 60, 0);
			item.rare = ItemRarityID.Orange;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.shoot = ModContent.ProjectileType<BruteHammerProj>();
		}
		public override bool CanUseItem(Player player) => player.ownedProjectileCounts[item.shoot] == 0;
	}
}