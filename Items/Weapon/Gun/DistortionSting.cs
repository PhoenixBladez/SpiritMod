using Microsoft.Xna.Framework;
using SpiritMod.Projectiles;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Gun
{
	public class DistortionSting : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Distortion Sting");
			Tooltip.SetDefault("Shoots out globules of energy that distort enemies' gravity!");

		}


		public override void SetDefaults()
		{
			item.damage = 105;
			item.ranged = true;
			item.width = 65;
			item.height = 21;
			item.useTime = 16;
			item.useAnimation = 16;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.noMelee = true;
			item.knockBack = 4;
			item.useTurn = false;
			item.value = Terraria.Item.sellPrice(0, 6, 0, 0);
			item.rare = ItemRarityID.Cyan;
			item.UseSound = SoundID.Item11;
			item.autoReuse = true;
			item.shoot = ModContent.ProjectileType<AlienSpit>();
			item.shootSpeed = 15f;
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-10, 0);
		}
	}
}