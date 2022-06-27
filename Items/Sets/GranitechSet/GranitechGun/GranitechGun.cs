using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.GranitechSet.GranitechGun
{
	public class GranitechGun : ModItem
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Vector .109");

		public override void SetDefaults()
		{
			Item.damage = 50;
			Item.width = 28;
			Item.height = 14;
			Item.useTime = Item.useAnimation = 9;
			Item.knockBack = 0f;
			Item.shootSpeed = 9;
			Item.noUseGraphic = true;
			Item.noMelee = true;
			Item.autoReuse = true;
			Item.DamageType = DamageClass.Ranged;
			Item.channel = true;
			Item.rare = ItemRarityID.LightRed;
			Item.value = Item.sellPrice(gold: 2);
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.shoot = ProjectileID.PurificationPowder;
			Item.useAmmo = AmmoID.Bullet;
		}

		public override Vector2? HoldoutOffset() => new Vector2(-6, 0);

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			type = ModContent.ProjectileType<GranitechGunProjectile>();
			Projectile.NewProjectileDirect(source, position, Vector2.Zero, type, 0, 0, player.whoAmI, player.altFunctionUse);
			return false;
		}

		public override void AddRecipes()
		{
			var recipe = CreateRecipe(1);
			recipe.AddIngredient(ModContent.ItemType<GranitechMaterial>(), 12);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}

		public override bool CanConsumeAmmo(Item item, Player player) => false; //Dont consume ammo by myself
	}
}