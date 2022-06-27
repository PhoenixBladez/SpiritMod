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
			Item.damage = 13; //This is the amount of damage the item does
			Item.noMelee = true; //This makes sure the bow doesn't do melee damage
			Item.DamageType = DamageClass.Ranged; //This causes your bow to do ranged damage
			Item.width = 24; //Hitbox width
			Item.height = 30; //Hitbox height
			Item.useTime = 37; //How long it takes to use the weapon. If this is shorter than the useAnimation it will fire twice in one click.
			Item.useAnimation = 37;  //The animations time length
			Item.useStyle = ItemUseStyleID.Shoot; //The style in which the item gets used. 5 for bows.
			Item.shoot = ProjectileID.Shuriken; //Makes the bow shoot arrows
			Item.useAmmo = AmmoID.Arrow; //Makes the bow consume arrows
			Item.knockBack = 1; //The amount of knockback the item has
			Item.rare = ItemRarityID.Blue; //The item's name color
			Item.UseSound = SoundID.Item5; //Sound that gets played on use
			Item.autoReuse = true; //if the Bow autoreuses or not
			Item.shootSpeed = 8f; //The arrows speed when shot
			Item.value = Item.sellPrice(0, 0, 20, 0);
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-3, 0);
		}

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