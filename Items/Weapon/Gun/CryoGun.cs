using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Weapon.Gun
{
	public class CryoGun : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Winter's Wake");
			Tooltip.SetDefault("Fires rockets");
			SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Weapon/Gun/CryoGun_Glow");
		}

		public override void SetDefaults()
		{
			item.damage = 32;
			item.ranged = true;
			item.noMelee = true;
			item.rare = ItemRarityID.Orange;
			item.width = 50;
            item.height = 26;
            item.value = Item.sellPrice(0, 0, 70, 0);
            item.useAnimation = 60;
			item.useTime = 60;
			item.knockBack = 2.5f;
			item.crit = 6;
			item.UseSound = SoundID.Item96;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.autoReuse = false;
			item.shoot = ProjectileID.RocketI;
			item.shootSpeed = 10f;
			item.useAmmo = AmmoID.Rocket;
		}
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Lighting.AddLight(item.position, 0.06f, .16f, .22f);
			Texture2D texture;
			texture = Main.itemTexture[item.type];
			spriteBatch.Draw
			(
				mod.GetTexture("Items/Weapon/Gun/CryoGun_Glow"),
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
		public override Vector2? HoldoutOffset() => new Vector2(-8, 0);

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Handgun);
			recipe.AddIngredient(ModContent.ItemType<CryoliteBar>(), 15);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}