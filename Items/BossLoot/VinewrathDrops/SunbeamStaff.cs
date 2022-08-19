using SpiritMod.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.BossLoot.VinewrathDrops
{
	public class SunbeamStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Photosynthestrike");
			Tooltip.SetDefault("Shoots out a fast moving, homing solar bolt");

			Item.staff[Item.type] = true;
		}

		public override void SetDefaults()
		{
			Item.damage = 14;
			Item.noMelee = true;
			Item.DamageType = DamageClass.Magic;
			Item.width = 64;
			Item.height = 64;
			Item.useTime = 19;
			Item.mana = 8;
			Item.useAnimation = 19;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 0f;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item72;
			Item.autoReuse = true;
			Item.shootSpeed = 6;
			Item.shoot = ModContent.ProjectileType<SolarBeamFriendly>();
		}
	}
}
