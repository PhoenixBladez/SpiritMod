using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.DuskingDrops
{
	public class DuskPendant : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dusk Pendant");
			Tooltip.SetDefault("13% increased magic and ranged critical strike chance at night");
			SpiritGlowmask.AddGlowMask(Item.type, "SpiritMod/Items/Sets/DuskingDrops/DuskPendant_Glow");
		}



		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 28;
			Item.rare = ItemRarityID.LightRed;
			Item.value = 80000;
			Item.expert = true;
			Item.DamageType = DamageClass.Melee;
			Item.accessory = true;

			Item.knockBack = 9f;
		}
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture;
			texture = TextureAssets.Item[Item.type].Value;
			spriteBatch.Draw
			(
				ModContent.Request<Texture2D>("SpiritMod/Items/Sets/DuskingDrops/DuskPendant_Glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value,
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

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			if (!Main.dayTime) {
				player.GetCritChance(DamageClass.Ranged) += 13;
				player.GetCritChance(DamageClass.Magic) += 13;
			}
		}
	}
}
