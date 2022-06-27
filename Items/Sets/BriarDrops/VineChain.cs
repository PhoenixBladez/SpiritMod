using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Flail;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.BriarDrops
{
	public class VineChain : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vine Chain");
			Tooltip.SetDefault("Pulls enemies towards you");
		}

		public override void SetDefaults()
		{
			Item.width = 44;
			Item.height = 44;
			Item.rare = ItemRarityID.Orange;
			Item.noMelee = true;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.useAnimation = 16;
			Item.useTime = 16;
			Item.knockBack = 6;
			Item.value = Item.sellPrice(0, 1, 20, 0);
			Item.damage = 14;
			Item.noUseGraphic = true;
			Item.shoot = ModContent.ProjectileType<VineChainProj>();
			Item.shootSpeed = 18f;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = false;
			Item.DamageType = DamageClass.Melee;
			Item.channel = true;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			Projectile.NewProjectile(Item.GetSource_ItemUse(Item), position, velocity, type, damage, knockback, player.whoAmI, velocity.X, velocity.Y);
			return false;
		}
	}
}