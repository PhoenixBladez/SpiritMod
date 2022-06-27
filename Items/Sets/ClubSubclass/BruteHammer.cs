using Microsoft.Xna.Framework;
using SpiritMod.Items.Material;
using SpiritMod.Projectiles.Clubs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.ClubSubclass
{
	public class BruteHammer : ModItem
	{
		public override bool IsLoadingEnabled(Mod mod) => false;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Brute Hammer");
			Tooltip.SetDefault("Hold down and release to slam");
		}

		public override void SetDefaults()
		{
			Item.useStyle = 100;
			Item.width = 40;
			Item.height = 32;
			Item.noUseGraphic = true;
			Item.UseSound = SoundID.Item1;
			Item.DamageType = DamageClass.Melee;
			Item.channel = true;
			Item.noMelee = true;
			Item.useTurn = true;
			Item.useAnimation = 30;
			Item.useTime = 30;
			Item.shootSpeed = 8f;
			Item.knockBack = 5f;
			Item.damage = 12;
			Item.value = Item.sellPrice(0, 0, 60, 0);
			Item.rare = ItemRarityID.Orange;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.shoot = ModContent.ProjectileType<BruteHammerProj>();
		}

		public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] == 0;
	}
}