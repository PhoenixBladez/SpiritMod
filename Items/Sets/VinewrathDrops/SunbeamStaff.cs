using SpiritMod.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.VinewrathDrops
{
	public class SunbeamStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Photosynthestrike");
			Tooltip.SetDefault("Shoots out a fast moving, homing solar bolt");

			Item.staff[item.type] = true;
		}

		public override void SetDefaults()
		{
			item.damage = 14;
			item.noMelee = true;
			item.magic = true;
			item.width = 64;
			item.height = 64;
			item.useTime = 19;
			item.mana = 8;
			item.useAnimation = 19;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.knockBack = 0f;
			item.value = Item.sellPrice(0, 1, 0, 0);
			item.rare = ItemRarityID.Green;
			item.UseSound = SoundID.Item72;
			item.autoReuse = true;
			item.shootSpeed = 6;
			item.shoot = ModContent.ProjectileType<SolarBeamFriendly>();
		}
	}
}
