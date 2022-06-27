using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.Vulture_Matriarch.Sovereign_Talon
{
	public class Sovereign_Talon : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sovereign Talon");
			Tooltip.SetDefault("Thrusting continuously charges the talon\nAn arcane wave will be cast at full charge");
		}

		public override void SetDefaults()
		{
			Item.damage = 50;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.useAnimation = 32;
			Item.useTime = 32;
			Item.shootSpeed = 2.7f;
			Item.knockBack = 3.5f;
			Item.width = 32;
			Item.height = 32;
			Item.scale = 1f;
			Item.rare = ItemRarityID.LightRed;
			Item.value = Item.sellPrice(gold: 2, silver: 50);
			Item.DamageType = DamageClass.Melee;
			Item.noMelee = true;
			Item.channel = true;
			Item.noUseGraphic = true;
			Item.autoReuse = true;
			Item.UseSound = SoundID.Item1;
			Item.shoot = ModContent.ProjectileType<Sovereign_Talon_Projectile>();
		}

		public override bool CanUseItem(Player player) => player.ownedProjectileCounts[ModContent.ProjectileType<Sovereign_Talon_Projectile>()] < 1;
	}
}
