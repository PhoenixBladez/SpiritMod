using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.BowsMisc.GemBows.Ruby_Bow
{
	public class Ruby_Bow : ModItem
	{
		public override void SetDefaults()
		{
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.useAnimation = 26;
			item.useTime = 26;
			item.width = 12;
			item.height = 28;
			item.shoot = ProjectileID.WoodenArrowFriendly;
			item.useAmmo = AmmoID.Arrow;
			item.UseSound = SoundID.Item5;
			item.damage = 14;
			item.shootSpeed = 8f;
			item.knockBack = 0.5f;
			item.rare = ItemRarityID.Blue;
			item.noMelee = true;
            item.value = Terraria.Item.sellPrice(0, 1, 35, 0);
            item.ranged = true;
			item.autoReuse = true;
		}
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ruby Bow");
			Tooltip.SetDefault("Turns wooden arrows into ruby arrows\nRuby arrows bounce once");
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY)) * 40f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
			{
				position += muzzleOffset;
			}
			if (type == ProjectileID.WoodenArrowFriendly)
			{
				type = mod.ProjectileType("Ruby_Arrow");
			}
			Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(8));
			Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
			return false;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-1, 0);
		}
		
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.SilverBow, 1);
			recipe.AddIngredient(ItemID.Ruby, 8);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();

			ModRecipe recipe1 = new ModRecipe(mod);
			recipe1.AddIngredient(ItemID.TungstenBow, 1);
			recipe1.AddIngredient(ItemID.Ruby, 8);
			recipe1.AddTile(TileID.Anvils);
			recipe1.SetResult(this);
			recipe1.AddRecipe();
		}
	}
}
