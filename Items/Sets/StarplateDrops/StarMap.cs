using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Projectiles.Magic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.StarplateDrops
{
	public class StarMap : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Astral Map");
			Tooltip.SetDefault("Hold for a second, then release to teleport");
			SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Sets/StarplateDrops/StarMap_Glow");
		}

		public override void SetDefaults()
		{
			item.damage = 0;
			item.noMelee = true;
			item.channel = true; //Channel so that you can held the weapon [Important]
			item.rare = ItemRarityID.Pink;
			item.width = 18;
			item.height = 18;
			item.useTime = 20;
			item.UseSound = SoundID.Item13;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.expert = true;
			item.autoReuse = false;
			item.shoot = ModContent.ProjectileType<StarMapProj>();
			item.shootSpeed = 0f;
			item.noUseGraphic = true;
		}
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Lighting.AddLight(item.position, 0.08f, .28f, .38f);
			Texture2D texture;
			texture = Main.itemTexture[item.type];
			spriteBatch.Draw
			(
				ModContent.GetTexture("SpiritMod/Items/Sets/StarplateDrops/StarMap_Glow"),
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
