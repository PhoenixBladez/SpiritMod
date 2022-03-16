using Microsoft.Xna.Framework;
using SpiritMod.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.FrigidSet
{
	public class HowlingScepter : ModItem
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Frigid Scepter");

		public override void SetDefaults()
		{
			item.damage = 9;
			Item.staff[item.type] = true;
			item.noMelee = true;
			item.magic = true;
			item.width = 64;
			item.height = 64;
			item.useTime = 25;
			item.mana = 9;
			item.useAnimation = 25;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.knockBack = 4;
			item.value = Item.sellPrice(0, 0, 5, 0);
			item.rare = ItemRarityID.Blue;
			item.crit = 6;
			item.autoReuse = false;
			item.shootSpeed = 9;
			item.UseSound = SoundID.Item20;
			item.shoot = ModContent.ProjectileType<HowlingBolt>();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			int proj = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI);
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
			ModRecipe modRecipe = new ModRecipe(mod);
			modRecipe.AddIngredient(ModContent.ItemType<FrigidFragment>(), 9);
			modRecipe.AddTile(TileID.Anvils);
			modRecipe.SetResult(this, 1);
			modRecipe.AddRecipe();
		}
	}
}
