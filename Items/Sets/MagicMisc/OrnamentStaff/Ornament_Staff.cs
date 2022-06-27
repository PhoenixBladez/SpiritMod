using Microsoft.Xna.Framework;
using SpiritMod.Items.Sets.GraniteSet;
using SpiritMod.Items.Sets.MarbleSet;

using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.MagicMisc.OrnamentStaff
{
	public class Ornament_Staff : ModItem
	{
		public override void SetDefaults()
		{

			Item.damage = 16;
			Item.noMelee = true;
			Item.noUseGraphic = false;
			Item.DamageType = DamageClass.Magic;
			Item.width = 36;
			Item.height = 40;
			Item.useTime = 43;
			Item.useAnimation = 43;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.shoot = ProjectileID.WoodenArrowFriendly;
			Item.shootSpeed = 10f;
			Item.knockBack = 5f;
			Item.autoReuse = true;
			Item.rare = ItemRarityID.Orange;
			Item.value = Item.sellPrice(gold: 1, silver: 25);
			Item.UseSound = new Terraria.Audio.LegacySoundStyle(42, 139);
			Item.useTurn = false;
			Item.mana = 14;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Staff of Ornaments");
			Tooltip.SetDefault("Casts a bunch of gems that will track your cursor position");
			Item.staff[Item.type] = true;
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Vector2 muzzleOffset = Vector2.Normalize(velocity) * 80f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
			{
				position += muzzleOffset;
			}

			int Type = 0;
			for (int i = 0; i < 6; i++)
			{
				switch (i)
				{
					case 0:
						Type = ModContent.ProjectileType<Amethyst_Projectile>();
						break;
					case 1:
						Type = ModContent.ProjectileType<Topaz_Projectile>();
						break;
					case 2:
						Type = ModContent.ProjectileType<Sapphire_Projectile>();
						break;
					case 3:
						Type = ModContent.ProjectileType<Emerald_Projectile>();
						break;
					case 4:
						Type = ModContent.ProjectileType<Ruby_Projectile>();
						break;
					case 5:
						Type = ModContent.ProjectileType<Diamond_Projectile>();
						break;
				}

				int spread = Main.rand.Next(-20, 10);
				spread += i * 5;
				Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.ToRadians(spread));
				Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, Type, damage, knockback, player.whoAmI);
			}
			return false;
		}
		public override Vector2? HoldoutOffset() => new Vector2(-15, 0);
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddRecipeGroup("SpiritMod:AmethystStaffs", 1);
			recipe.AddRecipeGroup("SpiritMod:SapphireStaffs", 1);
			recipe.AddRecipeGroup("SpiritMod:RubyStaffs", 1);
			recipe.AddIngredient(ModContent.ItemType<GraniteChunk>(), 5);
			recipe.AddIngredient(ModContent.ItemType<MarbleChunk>(), 5);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}

	public class Amethyst_Projectile : BaseOrnamentStaffProj
	{
		public Amethyst_Projectile() : base(86, Color.Purple) { }

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Amethyst Magic");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 14;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
		}
	}

	public class Topaz_Projectile : BaseOrnamentStaffProj
	{
		public Topaz_Projectile() : base(87, Color.Yellow) { }
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Topaz Magic");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 14;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
		}
	}
	public class Sapphire_Projectile : BaseOrnamentStaffProj
	{
		public Sapphire_Projectile() : base(88, Color.Blue) { }
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sapphire Magic");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 14;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
		}
	}

	public class Emerald_Projectile : BaseOrnamentStaffProj
	{
		public Emerald_Projectile() : base(89, Color.Green) { }
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Emerald Magic");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 14;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
		}
	}
	public class Ruby_Projectile : BaseOrnamentStaffProj
	{
		public Ruby_Projectile() : base(90, Color.Red) { }
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ruby Magic");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 14;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
		}
	}
	public class Diamond_Projectile : BaseOrnamentStaffProj
	{
		public Diamond_Projectile() : base(91, Color.White) { }
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Diamond Magic");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 14;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
		}
	}
}