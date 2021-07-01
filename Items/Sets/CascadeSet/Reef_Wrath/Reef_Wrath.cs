using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.CascadeSet.Reef_Wrath
{
	public class Reef_Wrath : ModItem
	{
		public override void SetDefaults()
		{
			item.damage = 18;
			item.noMelee = true;
			item.noUseGraphic = false;
			item.magic = true;
			item.width = 36;
			item.height = 40;
			item.useTime = 36;
			item.useAnimation = 36;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.shoot = mod.ProjectileType("Reef_Wrath_Projectile_Alt");
			item.shootSpeed = 0f;
			item.knockBack = 2.5f;
			item.autoReuse = false;
			item.rare = ItemRarityID.Green;
			item.UseSound = SoundID.Item109;
			item.value = Item.sellPrice(silver: 30);
			item.useTurn = false;
			item.mana = 8;
		}

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Coral, 5);
            recipe.AddIngredient(ItemID.SharkFin, 3);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
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
		
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
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

				while (y < Main.maxTilesY - 10 && Main.tile[x, y] != null && !(WorldGen.SolidTile(x, y) || WorldGen.SolidTile2(x, y) || WorldGen.SolidTile3(x, y))) {
					y++;
				}
				y--;

				Vector2 pos = new Vector2(x, y).ToWorldCoordinates();
				Vector2 vel = Vector2.UnitX.RotatedByRandom(MathHelper.Pi / 8);

				if (Framing.GetTileSafely(x, y + 1).halfBrick())
					pos.Y += 8;

				switch (Framing.GetTileSafely(x, y + 1).slope()) {
					case 1: vel = vel.RotatedBy(MathHelper.Pi / 4);
						break;
					case 2: vel = vel.RotatedBy(-MathHelper.Pi / 4);
						break;
					default:
						break;
				}

				Projectile.NewProjectile(pos, vel, mod.ProjectileType("Reef_Wrath_Projectile_Alt"), 0, 0f, player.whoAmI);
			}
			
			return false;
		}
	}
}
