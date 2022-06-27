using Terraria;
using System.Collections.Generic;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpiritMod.Items.Accessory.Rangefinder
{
	public class Rangefinder : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rangefinder");
			Tooltip.SetDefault("Ranged weapons are now equipped with a laser sight\nWorks while in the inventory");
		}

		public override void SetDefaults()
		{
			Item.width = 36;
			Item.height = 32;
			Item.value = Item.buyPrice(0, 1, 0, 0);
			Item.rare = ItemRarityID.LightRed;
		}

		public override void UpdateInventory(Player player) => player.GetModPlayer<RangefinderPlayer>().active = true;
	}

	public class RangefinderPlayer : ModPlayer
	{
		public bool active = false;

		public override void ResetEffects() => active = false;

		public override void ModifyDrawLayers(List<PlayerLayer> layers)
		{
			int arms = layers.FindIndex(l => l == PlayerLayer.Arms);
			if (arms < 0)
				return;

			layers.Insert(arms - 5, new PlayerLayer(Mod.Name, "HeldItem",
				delegate (PlayerDrawInfo drawInfo)
				{
					Player drawPlayer = drawInfo.drawPlayer;
					DrawData drawData = new DrawData();
					Mod mod = ModLoader.GetMod("SpiritMod");
					Texture2D texture = Main.extraTexture[47];
					Vector2 drawPlayerCenter = drawPlayer.MountedCenter;
					Vector2 distToProj = Main.MouseWorld - drawPlayerCenter;
					float projRotation = distToProj.ToRotation() - 1.57f;
					float distance = distToProj.Length();
					if (active && Player.itemAnimation > 0 && Player.HeldItem.ranged && Player.HeldItem.useAnimation > 0 && !drawPlayer.mount.Active)
					{
						while (distance > 30 && !float.IsNaN(distance))
						{
							distToProj.Normalize();
							distToProj *= 36f;
							drawPlayerCenter += distToProj;
							distToProj = Main.MouseWorld - drawPlayerCenter;
							distance = distToProj.Length();
							drawData = new DrawData(texture,
								new Vector2(drawPlayerCenter.X - Main.screenPosition.X, drawPlayerCenter.Y - Main.screenPosition.Y),
								new Rectangle(0, 0, texture.Width, texture.Height),
								Color.DeepSkyBlue * (distToProj.Length() / 255),
								projRotation,
								new Vector2(texture.Width * 0.5f, texture.Height * 0.5f),
								0.5f,
								SpriteEffects.None,
								0);
							Main.playerDrawData.Add(drawData);
						}
					}
				}));
		}
	}
}