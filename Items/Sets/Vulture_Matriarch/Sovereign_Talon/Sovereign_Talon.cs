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
			item.damage = 50;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.useAnimation = 32;
			item.useTime = 32;
			item.shootSpeed = 2.7f;
			item.knockBack = 3.5f;
			item.width = 32;
			item.height = 32;
			item.scale = 1f;
			item.rare = ItemRarityID.LightRed;
			item.value = Item.sellPrice(gold: 2, silver: 50);
			item.melee = true;
			item.noMelee = true;
			item.channel = true;
			item.noUseGraphic = true;
			item.autoReuse = true;
			item.UseSound = SoundID.Item1;
			item.shoot = ModContent.ProjectileType<Sovereign_Talon_Projectile>();
		}

		public override bool CanUseItem(Player player) => player.ownedProjectileCounts[ModContent.ProjectileType<Sovereign_Talon_Projectile>()] < 1;
	}
}
