using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.FloatingItems.MessageBottle
{
	public class MessageBottle : FloatingItem
	{
		public override float SpawnWeight => 0.08f;
		public override float Weight => base.Weight * 0.9f;
		public override float Bouyancy => base.Bouyancy * 1.08f;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Message in a Bottle");
			Tooltip.SetDefault("Summons a raft mount\nIncreased fishing power while on the raft");
		}

		public override void SetDefaults()
		{
			item.width = 22;
			item.height = 10;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.value = Item.sellPrice(0, 1, 0, 0);
			item.rare = ItemRarityID.Blue;
			item.maxStack = 1;
			item.UseSound = SoundID.Item79;
			item.mountType = ModContent.MountType<MessageBottleMount>();
			item.useTime = item.useAnimation = 20;
		}

		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			Texture2D tex = ModContent.GetTexture(Texture + "_World");
			spriteBatch.Draw(tex, item.Center - Main.screenPosition, null, lightColor, rotation, new Vector2(item.width, item.height) / 2, scale, SpriteEffects.None, 0f);
			return false;
		}
	}
}