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
			item.useStyle = 5;
			item.shoot = 1;
			item.shootSpeed = 10f;
			item.knockBack = 5f;
			item.autoReuse = true;
			item.rare = 3;
			item.value = Item.sellPrice(gold: 1, silver: 25);
			item.UseSound = new Terraria.Audio.LegacySoundStyle(42, 139);
			item.useTurn = false;
			item.mana = 14;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Staff of Ornaments");
			Tooltip.SetDefault("Casts a bunch of gems that will track your mouse");
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
			for (int i = 0; i<6; i++)
			{
				switch (i)
				{
					case 0:
						Type = mod.ProjectileType("Amethyst_Projectile");
						break;
					case 1:
						Type = mod.ProjectileType("Topaz_Projectile");
						break;
					case 2:
						Type = mod.ProjectileType("Sapphire_Projectile");
						break;
					case 3:
						Type = mod.ProjectileType("Emerald_Projectile");
						break;
					case 4:
						Type = mod.ProjectileType("Ruby_Projectile");
						break;
					case 5:
						Type = mod.ProjectileType("Diamond_Projectile");
						break;
					default:
						break;
				}
				int spread = Main.rand.Next(-20,10);
				spread+=i*5;
				Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.ToRadians(spread));
				int p = Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, Type, damage, knockBack, 0, 0.0f, 0.0f);
				Main.projectile[p].ai[0] = (float)Player.tileTargetX;
				Main.projectile[p].ai[1] = (float)Player.tileTargetY;
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
}
