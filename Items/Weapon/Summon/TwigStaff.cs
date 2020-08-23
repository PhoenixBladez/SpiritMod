using SpiritMod.Projectiles.Summon;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Summon
{
	public class TwigStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nature's Grasp");
			Tooltip.SetDefault("Shoots a wave of nature energy\n3 summon tag damage");
		}


		public override void SetDefaults()
		{
			item.damage = 9;
			item.summon = true;
			item.mana = 6;
			item.width = 32;
			item.height = 32;
			item.useTime = 38;
			item.useAnimation = 38;
			item.useStyle = ItemUseStyleID.HoldingOut;
			Item.staff[item.type] = true;
			item.noMelee = true;
			item.knockBack = 3;
			item.value = Terraria.Item.sellPrice(0, 0, 20, 0);
			item.rare = 1;
			item.UseSound = SoundID.Item20;
			item.autoReuse = true;
			item.shoot = ModContent.ProjectileType<NatureGraspProjectile>();
			item.shootSpeed = 6f;
		}
	}
}
