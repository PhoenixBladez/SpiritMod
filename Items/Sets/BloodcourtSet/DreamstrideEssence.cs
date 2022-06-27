using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.BloodcourtSet
{
	public class DreamstrideEssence : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dreamstride Essence");
			Tooltip.SetDefault("'The stuff of nightmares'");
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(4, 4));
			ItemID.Sets.ItemNoGravity[Item.type] = true;
			ItemID.Sets.ItemIconPulse[Item.type] = true;
		}

		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 28;
			Item.value = 100;
			Item.rare = ItemRarityID.Blue;
			Item.maxStack = 999;
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI) => Lighting.AddLight(Item.position, 0.92f, .14f, .24f);
		public override Color? GetAlpha(Color lightColor) => Color.White;
	}
}