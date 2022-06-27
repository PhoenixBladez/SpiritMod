using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
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
			Item.shootSpeed = 10f;
			Item.damage = 15;
			Item.knockBack = 6f;
			Item.DamageType = DamageClass.Magic;
			Item.mana = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.UseSound = SoundID.Item1;
			Item.useAnimation = 30;
			Item.useTime = 30;
			Item.width = 26;
			Item.height = 26;
			Item.noUseGraphic = true;
			Item.noMelee = true;
			Item.autoReuse = true;
			Item.value = Item.buyPrice(gold: 1);
			Item.rare = ItemRarityID.Blue;
			Item.shoot = ModContent.ProjectileType<Punching_Bag_Projectile>();
		}
		
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			Vector2 perturbedSpeed = velocity.RotatedByRandom(MathHelper.ToRadians(6));
			Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockback, player.whoAmI);
			return false;
		}
	}
}
