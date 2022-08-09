using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Items.Sets.FloatingItems;
using Terraria.DataStructures;

namespace SpiritMod.Items.Sets.CascadeSet.Reef_Wrath
{
	public class Reef_Wrath : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 18;
			Item.noMelee = true;
			Item.noUseGraphic = false;
			Item.DamageType = DamageClass.Magic;
			Item.width = 36;
			Item.height = 40;
			Item.useTime = 36;
			Item.useAnimation = 36;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.shoot = ModContent.ProjectileType<Reef_Wrath_Projectile_Alt>();
			Item.shootSpeed = 0f;
			Item.knockBack = 2.5f;
			Item.autoReuse = false;
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item109;
			Item.value = Item.sellPrice(silver: 30);
			Item.useTurn = false;
			Item.mana = 8;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<DeepCascadeShard>(), 11);
			recipe.AddIngredient(ModContent.ItemType<Kelp>(), 6);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Reef Wrath");
			Tooltip.SetDefault("Conjures harmful coral spires along the ground");
		}

		private Vector2 CollisionPoint(Player player) {
			float[] scanarray = new float[3];
			float dist = player.Distance(Main.MouseWorld);
			Collision.LaserScan(player.Center, player.DirectionTo(Main.MouseWorld), 0, dist, scanarray);
			dist = 0;
			foreach(float fl in scanarray) {
				dist += fl / scanarray.Length;
			}
			return player.MountedCenter + player.DirectionTo(Main.MouseWorld) * dist;
		}
		
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			for (int i = 0; i < 3; i++)
			{
				Vector2 collisionpoint = CollisionPoint(player);
				int x = (int)(collisionpoint.X) / 16;
				int y = (int)(collisionpoint.Y) / 16;
				int randomiseX = Main.rand.Next(-2, 3);

				if (WorldGen.SolidTile(x + randomiseX, y) || WorldGen.SolidTile3(x + randomiseX, y))
					randomiseX = 0;

				x += randomiseX;

				while (y < Main.maxTilesY - 10 && !(WorldGen.SolidTile(x, y) || WorldGen.SolidTile2(x, y) || WorldGen.SolidTile3(x, y))) {
					y++;
				}
				y--;

				Vector2 pos = new Vector2(x, y).ToWorldCoordinates();
				Vector2 vel = Vector2.UnitX.RotatedByRandom(MathHelper.Pi / 8);

				if (Framing.GetTileSafely(x, y + 1).IsHalfBlock)
					pos.Y += 8;

				switch ((byte)Framing.GetTileSafely(x, y + 1).Slope) {
					case 1: vel = vel.RotatedBy(MathHelper.Pi / 4);
						break;
					case 2: vel = vel.RotatedBy(-MathHelper.Pi / 4);
						break;
					default:
						break;
				}

				Projectile.NewProjectile(Item.GetSource_ItemUse(Item), pos, vel, ModContent.ProjectileType<Reef_Wrath_Projectile_Alt>(), 0, 0f, player.whoAmI);
			}
			
			return false;
		}
	}
}
