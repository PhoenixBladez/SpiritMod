using SpiritMod.Projectiles.DonatorItems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.DonatorItems.FrostTroll
{
	public class ShiverWind : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shiver Wind");
			Tooltip.SetDefault("Shoots a chilly bolt that morphs into an icy rune");
		}

		public override void SetDefaults()
		{
			Item.damage = 44;
			Item.DamageType = DamageClass.Magic;
			Item.mana = 14;
			Item.width = 52;
			Item.height = 52;
			Item.useTime = 32;
			Item.useAnimation = 32;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.staff[Item.type] = true;
			Item.noMelee = true;
			Item.knockBack = 4;
			Item.value = 80000;
			Item.rare = ItemRarityID.Pink;
			Item.UseSound = SoundID.Item34;
			Item.crit = 11;
			Item.autoReuse = false;
			Item.shoot = ModContent.ProjectileType<ChillBolt>();
			Item.shootSpeed = 7f;
			Item.autoReuse = true;
		}
	}
}
