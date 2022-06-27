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
			Item.width = 22;
			Item.height = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.rare = ItemRarityID.Blue;
			Item.maxStack = 1;
			Item.UseSound = SoundID.Item79;
			Item.mountType = ModContent.MountType<MessageBottleMount>();
			Item.useTime = Item.useAnimation = 20;
		}

		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			Texture2D tex = ModContent.Request<Texture2D>(Texture + "_World");
			spriteBatch.Draw(tex, Item.Center - Main.screenPosition, null, lightColor, rotation, new Vector2(Item.width, Item.height) / 2, scale, SpriteEffects.None, 0f);
			return false;
		}
	}
}