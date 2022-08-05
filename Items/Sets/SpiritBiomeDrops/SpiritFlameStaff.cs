using SpiritMod.Projectiles.Magic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.SpiritBiomeDrops
{
	public class SpiritFlameStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spiritflame Staff");
			Tooltip.SetDefault("Shoots out a spirit bolt that explodes in 4 directions");
		}


		public override void SetDefaults()
		{
			Item.damage = 49;
			Item.DamageType = DamageClass.Magic;
			Item.mana = 14;
			Item.width = 40;
			Item.height = 40;
			Item.useTime = 32;
			Item.useAnimation = 32;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.staff[Item.type] = true;
			Item.noMelee = true;
			Item.knockBack = 0;
			Item.useTurn = false;
			Item.value = Item.sellPrice(0, 4, 0, 0);
			Item.rare = ItemRarityID.LightPurple;
			Item.UseSound = SoundID.Item20;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<SpiritBolt>();
			Item.shootSpeed = 5f;
		}


	}
}
