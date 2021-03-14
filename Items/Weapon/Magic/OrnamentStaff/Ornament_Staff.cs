using Microsoft.Xna.Framework;
using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic.OrnamentStaff
{
	public class Ornament_Staff : ModItem
	{
		public override void SetDefaults()
		{

			item.damage = 15;
			item.noMelee = true;
			item.noUseGraphic = false;
			item.magic = true;
			item.width = 36;
			item.height = 40;
			item.useTime = 48;
			item.useAnimation = 48;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.shoot = ProjectileID.WoodenArrowFriendly;
			item.shootSpeed = 10f;
			item.knockBack = 5f;
			item.autoReuse = true;
			item.rare = ItemRarityID.Orange;
			item.value = Item.sellPrice(gold: 1, silver: 25);
			item.UseSound = new Terraria.Audio.LegacySoundStyle(42, 139);
			item.useTurn = false;
			item.mana = 14;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Staff of Ornaments");
			Tooltip.SetDefault("Casts a bunch of gems that will track your cursor position");
			Item.staff[item.type] = true;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY)) * 80f;
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

				int spread = Main.rand.Next(-20,10);
				spread+=i*5;
				Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.ToRadians(spread));
				Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, Type, damage, knockBack, player.whoAmI);
			}
			return false;
		}
		public override Vector2? HoldoutOffset() => new Vector2(-15, 0);
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddRecipeGroup("SpiritMod:AmethystStaffs", 1);
			recipe.AddRecipeGroup("SpiritMod:SapphireStaffs", 1);
			recipe.AddRecipeGroup("SpiritMod:RubyStaffs", 1);
			recipe.AddIngredient(ModContent.ItemType<GraniteChunk>(), 5);
			recipe.AddIngredient(ModContent.ItemType<MarbleChunk>(), 5);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}

	public class Amethyst_Projectile : BaseOrnamentStaffProj
	{
		public Amethyst_Projectile() : base(86, Color.Purple) { }

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Amethyst Magic");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 14;
			ProjectileID.Sets.TrailingMode[projectile.type] = 2;
		}
	}

	public class Topaz_Projectile : BaseOrnamentStaffProj
	{
		public Topaz_Projectile() : base(87, Color.Yellow) { }
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Topaz Magic");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 14;
			ProjectileID.Sets.TrailingMode[projectile.type] = 2;
		}
	}
	public class Sapphire_Projectile : BaseOrnamentStaffProj
	{
		public Sapphire_Projectile() : base(88, Color.Blue) { }
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sapphire Magic");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 14;
			ProjectileID.Sets.TrailingMode[projectile.type] = 2;
		}
	}

	public class Emerald_Projectile : BaseOrnamentStaffProj
	{
		public Emerald_Projectile() : base(89, Color.Green) { }
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Emerald Magic");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 14;
			ProjectileID.Sets.TrailingMode[projectile.type] = 2;
		}
	}
	public class Ruby_Projectile : BaseOrnamentStaffProj
	{
		public Ruby_Projectile() : base(90, Color.Red) { }
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ruby Magic");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 14;
			ProjectileID.Sets.TrailingMode[projectile.type] = 2;
		}
	}
	public class Diamond_Projectile : BaseOrnamentStaffProj
	{
		public Diamond_Projectile() : base(91, Color.White) { }
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Diamond Magic");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 14;
			ProjectileID.Sets.TrailingMode[projectile.type] = 2;
		}
	}
}
