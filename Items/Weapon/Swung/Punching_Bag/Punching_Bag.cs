using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Swung.Punching_Bag
{
	public class Punching_Bag : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Punching Bag");
			Tooltip.SetDefault("Shoots a barrage of fists\n'The number one undead strength training method'");
		}

		public override void SetDefaults()
		{
			item.shootSpeed = 10f;
			item.damage = 15;
			item.knockBack = 6f;
			item.magic = true;
			item.mana = 10;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.UseSound = SoundID.Item1;
			item.useAnimation = 30;
			item.useTime = 30;
			item.width = 26;
			item.height = 26;
			item.noUseGraphic = true;
			item.noMelee = true;
			item.autoReuse = true;
			item.value = Item.buyPrice(gold: 1);
			item.rare = ItemRarityID.Blue;
			item.shoot = ModContent.ProjectileType<Punching_Bag_Projectile>();
		}
		
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(6));
			Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
			return false;
		}
	}
}
