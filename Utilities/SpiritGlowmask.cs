using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Utilities
{
	public class SpiritGlowmask : ModPlayer
	{
		private static readonly Dictionary<int, Texture2D> ItemGlowMask = new Dictionary<int, Texture2D>();

		internal static void Unload()
		{
			ItemGlowMask.Clear();
		}

		public static void AddGlowMask(int itemType, string texturePath)
		{
			ItemGlowMask[itemType]=ModContent.GetTexture(texturePath);
		}

		public override void ModifyDrawLayers(List<PlayerLayer> layers)
		{
			Texture2D textureLegs;
			if (!player.armor[12].IsAir)
			{
				if (player.armor[12].type >= ItemID.Count && ItemGlowMask.TryGetValue(player.armor[12].type, out textureLegs))//Vanity Legs
				{
					InsertAfterVanillaLayer(layers, "Legs", new PlayerLayer(mod.Name, "GlowMaskLegs", delegate (PlayerDrawInfo info)
						{
							GlowmaskUtils.DrawArmorGlowMask(EquipType.Legs, textureLegs, info);
						}));
				}
			}
			else if (player.armor[2].type >= ItemID.Count && ItemGlowMask.TryGetValue(player.armor[2].type, out textureLegs))//Legs
			{
				InsertAfterVanillaLayer(layers, "Legs", new PlayerLayer(mod.Name, "GlowMaskLegs", delegate (PlayerDrawInfo info)
					{
						GlowmaskUtils.DrawArmorGlowMask(EquipType.Legs, textureLegs, info);
					}));
			}
			Texture2D textureBody;
			if (!player.armor[11].IsAir)
			{
				if (player.armor[11].type >= ItemID.Count && ItemGlowMask.TryGetValue(player.armor[11].type, out textureBody))//Vanity Body
				{
					InsertAfterVanillaLayer(layers, "Body", new PlayerLayer(mod.Name, "GlowMaskBody", delegate (PlayerDrawInfo info)
						{
							GlowmaskUtils.DrawArmorGlowMask(EquipType.Body, textureBody, info);
						}));
				}
			}
			else if (player.armor[1].type >= ItemID.Count && ItemGlowMask.TryGetValue(player.armor[1].type, out textureBody))//Body
			{
				InsertAfterVanillaLayer(layers, "Body", new PlayerLayer(mod.Name, "GlowMaskBody", delegate (PlayerDrawInfo info)
					{
						GlowmaskUtils.DrawArmorGlowMask(EquipType.Body, textureBody, info);
					}));
			}
			Texture2D textureHead;
			if (!player.armor[10].IsAir)
			{
				if (player.armor[10].type >= ItemID.Count && ItemGlowMask.TryGetValue(player.armor[10].type, out textureHead))//Vanity Head
				{
					InsertAfterVanillaLayer(layers, "Head", new PlayerLayer(mod.Name, "GlowMaskHead", delegate (PlayerDrawInfo info)
						{
							GlowmaskUtils.DrawArmorGlowMask(EquipType.Head, textureHead, info);
						}));
				}
			}
			else if (player.armor[0].type >= ItemID.Count && ItemGlowMask.TryGetValue(player.armor[0].type, out textureHead))//Head
			{
				InsertAfterVanillaLayer(layers, "Head", new PlayerLayer(mod.Name, "GlowMaskHead", delegate (PlayerDrawInfo info)
					{
						GlowmaskUtils.DrawArmorGlowMask(EquipType.Head, textureHead, info);
					}));
			}
			Texture2D textureItem;
			if (player.HeldItem.type >= ItemID.Count && ItemGlowMask.TryGetValue(player.HeldItem.type, out textureItem))//Held ItemType
			{
				InsertAfterVanillaLayer(layers, "HeldItem", new PlayerLayer(mod.Name, "GlowMaskHeldItem", delegate (PlayerDrawInfo info)
					{
						GlowmaskUtils.DrawItemGlowMask(textureItem, info);
					}));
			}
		}

		public static void InsertAfterVanillaLayer(List<PlayerLayer> layers, string vanillaLayerName, PlayerLayer newPlayerLayer)
		{
			for (int i = 0; i < layers.Count; i++)
			{
				if (layers[i].Name == vanillaLayerName && layers[i].mod == "Terraria")
				{
					layers.Insert(i + 1, newPlayerLayer);
					return;
				}
			}
			layers.Add(newPlayerLayer);
		}
	}
}
