using SpiritMod.Projectiles.Summon;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.BriarChestLoot
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
			Item.damage = 8;
			Item.DamageType = DamageClass.Summon;
			Item.mana = 12;
			Item.width = 32;
			Item.height = 32;
			Item.useTime = 38;
			Item.useAnimation = 38;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.staff[Item.type] = true;
			Item.noMelee = true;
			Item.knockBack = 3;
			Item.value = Terraria.Item.sellPrice(0, 0, 20, 0);
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item20;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<NatureGraspProjectile>();
			Item.shootSpeed = 6f;
		}
	}
}
