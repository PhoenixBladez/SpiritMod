using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Arrow;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace SpiritMod.Items.Sets.BowsMisc.Eyeshot
{
	public class Eyeshot : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Eyeshot");
			Tooltip.SetDefault("Wooden Arrows turn into Eyeballs!");
		}

		public override void SetDefaults()
		{
			Item.damage = 14;
			Item.noMelee = true;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 50;
			Item.height = 30;
			Item.useTime = 25;
			Item.useAnimation = 25;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.shoot = ProjectileID.Shuriken;
			Item.useAmmo = AmmoID.Arrow;
			Item.knockBack = 4;
			Item.useTurn = false;
			Item.value = Item.sellPrice(0, 2, 0, 0);
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item5;
			Item.autoReuse = true;
			Item.shootSpeed = 4.25f;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			if (type == ProjectileID.WoodenArrowFriendly)
				type = ModContent.ProjectileType<EyeArrow>();
			return true;
		}
	}
}
