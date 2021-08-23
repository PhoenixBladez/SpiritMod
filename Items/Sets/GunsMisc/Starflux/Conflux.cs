using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Material;
using SpiritMod.Projectiles.Bullet;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Sets.GunsMisc.Starflux
{
	public class Conflux : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Starflux");
			Tooltip.SetDefault("Converts regular bullets into bouncing stars");
			SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Sets/GunsMisc/Starflux/Conflux_Glow");

		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture;
			texture = Main.itemTexture[item.type];
			spriteBatch.Draw
			(
				ModContent.GetTexture("SpiritMod/Items/Sets/GunsMisc/Starflux/Conflux_Glow"),
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
			item.damage = 36;
			item.ranged = true;
			item.width = 24;
			item.height = 24;
			item.useTime = 17;
			item.useAnimation = 17;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.noMelee = true;
			item.knockBack = 4;
			item.useTurn = false;
			item.value = Terraria.Item.sellPrice(0, 2, 0, 0);
			item.rare = ItemRarityID.LightRed;
			item.UseSound = SoundID.Item36;
			item.autoReuse = true;
			item.shoot = ModContent.ProjectileType<ConfluxPellet>();
			item.shootSpeed = 5.2f;
			item.useAmmo = AmmoID.Bullet;
		}
		public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
            Main.PlaySound(SoundID.Item, player.Center, 12);
			Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY - 1)) * 45f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0)) {
				position += muzzleOffset;
			}
            if (type == ProjectileID.Bullet)
            {
                type = ModContent.ProjectileType<ConfluxPellet>();
            }
            Projectile.NewProjectile(position.X - 8, position.Y + 8, speedX + ((float)Main.rand.Next(-250, 250) / 150), speedY + ((float)Main.rand.Next(-100, 100) / 100), type, damage, knockBack, player.whoAmI, 0f, 0f);
            return false;
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-10, 0);
		}
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.IllegalGunParts, 1);
            recipe.AddIngredient(ModContent.ItemType<Items.Sets.GunsMisc.Scattergun.Scattergun>(), 1);
            recipe.AddIngredient(ItemID.SoulofLight, 11);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
	}
}