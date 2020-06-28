using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Projectiles.Yoyo;
using Terraria;
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
			SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Weapon/Yoyo/Handball_Glow");
		}


		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.WoodYoyo);
			item.damage = 16;
			item.value = Terraria.Item.sellPrice(0, 0, 75, 0);
			item.rare = ItemRarityID.Green;
			item.knockBack = 3;
			item.channel = true;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.useAnimation = 25;
			item.useTime = 23;
			item.shoot = ModContent.ProjectileType<GraspProj>();
		}
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Lighting.AddLight(item.position, 0.46f, .07f, .52f);
			Texture2D texture;
			texture = Main.itemTexture[item.type];
			spriteBatch.Draw
			(
				mod.GetTexture("Items/Weapon/Yoyo/Handball_Glow"),
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
	}
}
