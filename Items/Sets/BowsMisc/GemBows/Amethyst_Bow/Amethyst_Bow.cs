using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace SpiritMod.Items.Sets.BowsMisc.GemBows.Amethyst_Bow
{
	public class Amethyst_Bow : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Amethyst Bow");
			Tooltip.SetDefault("Turns wooden arrows into amethyst arrows\nAmethyst arrows have an increased critical strike chance");
		}

		public override void SetDefaults()
		{
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.useAnimation = 29;
			Item.useTime = 29;
			Item.width = 12;
			Item.height = 28;
			Item.shoot = ProjectileID.WoodenArrowFriendly;
			Item.useAmmo = AmmoID.Arrow;
			Item.UseSound = SoundID.Item5;
			Item.damage = 10;
			Item.shootSpeed = 7f;
			Item.knockBack = 0.5f;
			Item.rare = ItemRarityID.White;
			Item.noMelee = true;
            Item.value = Item.sellPrice(0, 0, 22, 50);
            Item.DamageType = DamageClass.Ranged;
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			Vector2 muzzleOffset = Vector2.Normalize(velocity) * 40f;

			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
				position += muzzleOffset;

			if (type == ProjectileID.WoodenArrowFriendly)
				type = ModContent.ProjectileType<Amethyst_Arrow>();
		}

		public override Vector2? HoldoutOffset() => new Vector2(-1, 0);

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.CopperBow, 1);
			recipe.AddIngredient(ItemID.Amethyst, 8);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			Recipe recipe1 = CreateRecipe();
			recipe1.AddIngredient(ItemID.TinBow, 1);
			recipe1.AddIngredient(ItemID.Amethyst, 8);
			recipe1.AddTile(TileID.Anvils);
			recipe1.Register();
		}
	}
}
