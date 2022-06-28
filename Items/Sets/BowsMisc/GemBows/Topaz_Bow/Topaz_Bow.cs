using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace SpiritMod.Items.Sets.BowsMisc.GemBows.Topaz_Bow
{
	public class Topaz_Bow : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Topaz Bow");
			Tooltip.SetDefault("Turns wooden arrows into topaz arrows\nTopaz arrows have high velocity and are resistant to gravity");
		}

		public override void SetDefaults()
		{
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.useAnimation = 28;
			Item.useTime = 28;
			Item.width = 12;
			Item.height = 28;
			Item.shoot = ProjectileID.WoodenArrowFriendly;
			Item.useAmmo = AmmoID.Arrow;
			Item.UseSound = SoundID.Item5;
			Item.damage = 11;
			Item.shootSpeed = 7f;
			Item.knockBack = 0.5f;
			Item.rare = ItemRarityID.White;
			Item.noMelee = true;
            Item.value = Item.sellPrice(0, 0, 45, 0);
            Item.DamageType = DamageClass.Ranged;
		}
		
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			Vector2 muzzleOffset = Vector2.Normalize(velocity) * 40f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
				position += muzzleOffset;
			if (type == ProjectileID.WoodenArrowFriendly)
				type = ModContent.ProjectileType<Topaz_Arrow>();
			return true;
		}

		public override Vector2? HoldoutOffset() => new Vector2(-1, 0);

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.CopperBow, 1);
			recipe.AddIngredient(ItemID.Topaz, 8);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			Recipe recipe1 = CreateRecipe();
			recipe1.AddIngredient(ItemID.TinBow, 1);
			recipe1.AddIngredient(ItemID.Topaz, 8);
			recipe1.AddTile(TileID.Anvils);
			recipe1.Register();
		}
	}
}
