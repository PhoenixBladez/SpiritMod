using Microsoft.Xna.Framework;
using SpiritMod.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Gun
{
	public class SoulStinger : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Soul Stinger");
			Tooltip.SetDefault("Shoots out an ethereal sting that phases through walls");
		}


		public override void SetDefaults()
		{
			item.damage = 41;
			item.ranged = true;
			item.width = 68;
			item.height = 24;
			item.useTime = 30;
			item.useAnimation = 30;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.noMelee = true;
			item.knockBack = 0;
			item.useTurn = false;
			item.value = Terraria.Item.sellPrice(0, 6, 0, 0);
			item.rare = ItemRarityID.Pink;
			item.UseSound = SoundID.Item11;
			item.autoReuse = true;
			item.shoot = ModContent.ProjectileType<SoulSting>();
			item.shootSpeed = 10f;
		}
		public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			type = ModContent.ProjectileType<SoulSting>();
			return true;
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-10, 0);
		}
	}
}