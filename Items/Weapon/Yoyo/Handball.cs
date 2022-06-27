using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Projectiles.Yoyo;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Yoyo
{
	public class Handball : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Grasp");
			Tooltip.SetDefault("Inflicts 'Blood Corruption'\nCritical hits inflict 'Shadowflame'\n'And so I said, 'Catch these hands''");
			SpiritGlowmask.AddGlowMask(Item.type, "SpiritMod/Items/Weapon/Yoyo/Handball_Glow");
		}


		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.WoodYoyo);
			Item.damage = 16;
			Item.value = Terraria.Item.sellPrice(0, 0, 75, 0);
			Item.rare = ItemRarityID.Green;
			Item.knockBack = 3;
			Item.channel = true;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.useAnimation = 25;
			Item.useTime = 23;
			Item.shoot = ModContent.ProjectileType<GraspProj>();
		}
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Lighting.AddLight(Item.position, 0.46f, .07f, .52f);
			Texture2D texture;
			texture = TextureAssets.Item[Item.type].Value;
			spriteBatch.Draw
			(
				Mod.Assets.Request<Texture2D>("Items/Weapon/Yoyo/Handball_Glow").Value,
				new Vector2
				(
					Item.position.X - Main.screenPosition.X + Item.width * 0.5f,
					Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f + 2f
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
	}
}
