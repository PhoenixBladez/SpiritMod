using Microsoft.Xna.Framework;
using SpiritMod.Projectiles;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.FrigidSet
{
	public class HowlingScepter : ModItem
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Frigid Scepter");

		public override void SetDefaults()
		{
			Item.damage = 9;
			Item.staff[Item.type] = true;
			Item.noMelee = true;
			Item.DamageType = DamageClass.Magic;
			Item.width = 64;
			Item.height = 64;
			Item.useTime = 25;
			Item.mana = 9;
			Item.useAnimation = 25;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 4;
			Item.value = Item.sellPrice(0, 0, 5, 0);
			Item.rare = ItemRarityID.Blue;
			Item.crit = 6;
			Item.autoReuse = false;
			Item.shootSpeed = 9;
			Item.UseSound = SoundID.Item20;
			Item.shoot = ModContent.ProjectileType<HowlingBolt>();
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			int proj = Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, knockback, player.whoAmI);
			Projectile projectile = Main.projectile[proj];
			Vector2 offset = Vector2.Normalize(Main.MouseWorld - player.position) * 51f;

			for (int k = 0; k < 25; k++)
			{
				int dust = Dust.NewDust(projectile.Center + offset, projectile.width, projectile.height, DustID.BlueCrystalShard);
				Main.dust[dust].velocity *= -1f;
				Main.dust[dust].noGravity = true;
				Vector2 baseSpeed = Vector2.Normalize(new Vector2(Main.rand.Next(-100, 101), Main.rand.Next(-100, 101) + 0.01f)) * (Main.rand.Next(50, 100) * 0.04f);
				Main.dust[dust].velocity = baseSpeed;
				Main.dust[dust].position = (projectile.Center + offset) - Vector2.Normalize(baseSpeed) * 42f;
			}
			return false;
		}

		public override void AddRecipes()
		{
			Recipe modRecipe = CreateRecipe(1);
			modRecipe.AddIngredient(ModContent.ItemType<FrigidFragment>(), 9);
			modRecipe.AddTile(TileID.Anvils);
			modRecipe.Register();
		}
	}
}
