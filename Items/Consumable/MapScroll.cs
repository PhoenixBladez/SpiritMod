using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable
{
	public class MapScroll : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cartographer's Map");
			Tooltip.SetDefault("Reveals a nearby portion of the minimap");
		}

		public override void SetDefaults()
		{
			item.width = 28;
			item.height = 40;
			item.maxStack = 20;
			item.value = 1000;
			item.rare = ItemRarityID.White;
			item.useAnimation = 30;
			item.useTime = 30;
			item.useStyle = ItemUseStyleID.HoldingUp;
			item.consumable = true;
		}


		public override bool UseItem(Player player)
		{
			CombatText.NewText(new Rectangle((int)player.position.X, (int)player.position.Y - 10, player.width, player.height), new Color(255, 255, 255, 100),
			"Map Revealed");
			{
				Point center = Main.player[Main.myPlayer].Center.ToTileCoordinates();

				int range = 180;

				for (int i = center.X - range / 2; i < center.X + range / 2; i++) {
					for (int j = center.Y - range / 2; j < center.Y + range / 2; j++) {
						if (WorldGen.InWorld(i, j))
							Main.Map.Update(i, j, 255);
					}
				}
				Main.refreshMap = true;
			}
			for (int k = 0; k < 10; k++) {
				int dust = Dust.NewDust(player.Center, player.width, player.height, 263);
				Main.dust[dust].velocity *= -1f;
				Main.dust[dust].noGravity = true;
				Vector2 vector2_1 = new Vector2((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101));
				vector2_1.Normalize();
				Vector2 vector2_2 = vector2_1 * ((float)Main.rand.Next(50, 100) * 0.04f);
				Main.dust[dust].velocity = vector2_2;
				vector2_2.Normalize();
				Vector2 vector2_3 = vector2_2 * 34f;
				Main.dust[dust].position = player.Center - vector2_3;
			}
			return true;
		}

	}
}
