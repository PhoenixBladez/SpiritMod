using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Magic;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.GunsMisc.PolymorphGun
{
	public class PolymorphGun : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Polymorph Gun");
			Tooltip.SetDefault("Turns enemies into harmless bunnies!\nOnly works on enemies below half health");
		}


		public override void SetDefaults()
		{
			Item.DamageType = DamageClass.Magic;
			Item.damage = 35;
			Item.mana = 18;
			Item.width = 54;
			Item.height = 26;
			Item.useTime = 25;
			Item.useAnimation = 25;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.knockBack = 4;
			Item.value = Terraria.Item.buyPrice(0, 60, 0, 0);
			Item.rare = ItemRarityID.LightPurple;
			Item.UseSound = SoundID.DD2_SonicBoomBladeSlash;
			Item.autoReuse = true;
			Item.shootSpeed = 15f;
			Item.shoot = ModContent.ProjectileType<Polyshot>();
		}

		public override Vector2? HoldoutOffset() => new Vector2(-10, 0);
	}
}