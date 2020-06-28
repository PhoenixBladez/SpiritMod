using SpiritMod.Projectiles.Magic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic
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
			item.damage = 49;
			item.magic = true;
			item.mana = 14;
			item.width = 40;
			item.height = 40;
			item.useTime = 32;
			item.useAnimation = 32;
			item.useStyle = ItemUseStyleID.HoldingOut;
			Item.staff[item.type] = true;
			item.noMelee = true;
			item.knockBack = 0;
			item.useTurn = false;
			item.value = Terraria.Item.sellPrice(0, 4, 0, 0);
			item.rare = ItemRarityID.LightPurple;
			item.UseSound = SoundID.Item20;
			item.autoReuse = true;
			item.shoot = ModContent.ProjectileType<SpiritBolt>();
			item.shootSpeed = 5f;
		}


	}
}
