using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Material;
using SpiritMod.Projectiles.Bullet;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Sets.GunsMisc.Scattergun
{
	public class Scattergun : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Scattergun");
			Tooltip.SetDefault("Converts regular bullets into neon pellets");
			SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Sets/GunsMisc/Scattergun/Scattergun_Glow");

		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture;
			texture = Main.itemTexture[item.type];
			spriteBatch.Draw
			(
				ModContent.GetTexture("SpiritMod/Items/Sets/GunsMisc/Scattergun/Scattergun_Glow"),
				new Vector2
				(
					item.position.X - Main.screenPosition.X + item.width * 0.5f,
					item.position.Y - Main.screenPosition.Y + item.height - texture.Height * 0.5f + 2f
				),
				new Rectangle(0, 0, texture.Width, texture.Height),
				Color.White,
				rotation,
				texture.Size() * 0.5f,
				scale,
				SpriteEffects.None,
				0f
			);
		}
		public override void SetDefaults()
		{
			item.damage = 11;
			item.ranged = true;
			item.width = 24;
			item.height = 24;
			item.useTime = 36;
			item.useAnimation = 36;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.noMelee = true;
			item.knockBack = 4;
			item.useTurn = false;
			item.value = Terraria.Item.sellPrice(0, 2, 0, 0);
			item.rare = ItemRarityID.Orange;
			item.UseSound = SoundID.Item36;
			item.autoReuse = false;
			item.shoot = ModContent.ProjectileType<ScattergunPellet>();
			item.shootSpeed = 3.2f;
			item.useAmmo = AmmoID.Bullet;
		}
		public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
            Main.PlaySound(2, player.Center, 12);
			Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY - 1)) * 45f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0)) {
				position += muzzleOffset;
			}
            if (type == ProjectileID.Bullet)
            {
                type = ModContent.ProjectileType<ScattergunPellet>();
            }
            for (int I = 0; I < 3; I++)
            {
                Projectile.NewProjectile(position.X - 8, position.Y + 8, speedX + ((float)Main.rand.Next(-250, 250) / 150), speedY + ((float)Main.rand.Next(-250, 250) / 100), type, damage, knockBack, player.whoAmI, 0f, 0f);
            }
			return false;
		}

		public override Vector2? HoldoutOffset() => new Vector2(-10, 0);

		public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Sapphire, 1);
            recipe.AddIngredient(ItemID.Amethyst, 1);
            recipe.AddIngredient(ModContent.ItemType<Items.Placeable.Tiles.SpaceJunkItem>(), 35);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
	}
}