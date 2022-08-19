using Microsoft.Xna.Framework;
using SpiritMod.Projectiles.Bullet;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.GunsMisc.HeavenFleet
{
	public class HeavenFleet : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Starfleet");
			Tooltip.SetDefault("Uses stars for ammo\nHold down for a bigger blast");
		}

		public override void SetDefaults()
		{
			Item.channel = true;
			Item.damage = 88;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 24;
			Item.height = 24;
			Item.useTime = 24;
			Item.useAnimation = 24;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.knockBack = 6;
			Item.useTurn = false;
			Item.value = Item.sellPrice(0, 5, 0, 0);
			Item.rare = ItemRarityID.Pink;
			Item.autoReuse = true;
			Item.shoot = ModContent.ProjectileType<HeavenFleetProj>();
			Item.shootSpeed = 25f;
			Item.useAmmo = AmmoID.FallenStar;
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			if (type == ProjectileID.Bullet)
				type = ModContent.ProjectileType<ConfluxPellet>();
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<HeavenFleetProj>(), damage, knockback, player.whoAmI, type);
			return false;
		}

		public override Vector2? HoldoutOffset() => new Vector2(-16, 0);

		public override void AddRecipes()
		{
			var recipe = CreateRecipe(1);
			recipe.AddIngredient(ModContent.ItemType<Blaster.Blaster>(), 1);
			recipe.AddIngredient(ItemID.IllegalGunParts, 1);
			recipe.AddIngredient(ModContent.ItemType<BossLoot.StarplateDrops.CosmiliteShard>(), 8);
			recipe.AddIngredient(ItemID.SoulofFlight, 10);
			recipe.AddIngredient(ModContent.ItemType<Placeable.Tiles.ScrapItem>(), 25);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}