using Microsoft.Xna.Framework;
using SpiritMod.Items.Sets.BriarDrops;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace SpiritMod.Items.Sets.FloranSet
{
	public class FBow : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Floran Bow");
			Tooltip.SetDefault("'Primitive, yet useful'");
		}

		public override void SetDefaults()
		{
			Item.damage = 11;
			Item.noMelee = true;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 24;
			Item.height = 30;
			Item.useTime = 37;
			Item.useAnimation = 37;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.shoot = ProjectileID.Shuriken;
			Item.useAmmo = AmmoID.Arrow;
			Item.knockBack = 1;
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item5;
			Item.autoReuse = true;
			Item.shootSpeed = 8f;
			Item.value = Item.sellPrice(0, 0, 20, 0);
		}

		public override Vector2? HoldoutOffset() => new Vector2(-3, 0);

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
        {
            for (int I = 0; I < 2; I++)
                Projectile.NewProjectile(source, position.X - 8, position.Y + 8, velocity.X + ((float)Main.rand.Next(-180, 180) / 100), velocity.Y + ((float)Main.rand.Next(-180, 180) / 100), type, damage, knockback, player.whoAmI, 0f, 0f);
            return false;
        }

        public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(1);
			recipe.AddIngredient(ModContent.ItemType<FloranBar>(), 10);
			recipe.AddIngredient(ModContent.ItemType<EnchantedLeaf>(), 4);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}