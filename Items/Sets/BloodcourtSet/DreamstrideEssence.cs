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
			Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(4, 4));
			ItemID.Sets.ItemNoGravity[item.type] = true;
			ItemID.Sets.ItemIconPulse[item.type] = true;
		}

		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 28;
			item.value = 100;
			item.rare = ItemRarityID.Blue;
			item.maxStack = 999;
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI) => Lighting.AddLight(item.position, 0.92f, .14f, .24f);
		public override Color? GetAlpha(Color lightColor) => Color.White;
	}
}