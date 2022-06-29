using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod
{
	public class SpiritGlowmask : ModPlayer
	{
		private static readonly Dictionary<int, Texture2D> ItemGlowMask = new Dictionary<int, Texture2D>();

		internal new static void Unload()
		{
			ItemGlowMask.Clear();
		}

		public static void AddGlowMask(int itemType, string texturePath)
		{
			ItemGlowMask[itemType] = ModContent.Request<Texture2D>(texturePath);
		}

		public override void ModifyDrawLayers(List<PlayerDrawLayer> layers)
		{
			Texture2D textureLegs;
			if (!Player.armor[12].IsAir) {
				if (Player.armor[12].type >= ItemID.Count && ItemGlowMask.TryGetValue(Player.armor[12].type, out textureLegs))//Vanity Legs
				{
					InsertAfterVanillaLayer(layers, "Legs", new PlayerDrawLayer(Mod.Name, "GlowMaskLegs", delegate (PlayerDrawSet info) {
						GlowmaskUtils.DrawArmorGlowMask(EquipType.Legs, textureLegs, info);
					}));
				}
			}
			else if (Player.armor[2].type >= ItemID.Count && ItemGlowMask.TryGetValue(Player.armor[2].type, out textureLegs))//Legs
			{
				InsertAfterVanillaLayer(layers, "Legs", new PlayerDrawLayer(Mod.Name, "GlowMaskLegs", delegate (PlayerDrawSet info) {
					GlowmaskUtils.DrawArmorGlowMask(EquipType.Legs, textureLegs, info);
				}));
			}
			Texture2D textureBody;
			if (!Player.armor[11].IsAir) {
				if (Player.armor[11].type >= ItemID.Count && ItemGlowMask.TryGetValue(Player.armor[11].type, out textureBody))//Vanity Body
				{
					InsertAfterVanillaLayer(layers, "Body", new PlayerDrawLayer(Mod.Name, "GlowMaskBody", delegate (PlayerDrawSet info) {
						GlowmaskUtils.DrawArmorGlowMask(EquipType.Body, textureBody, info);
					}));
				}
			}
			else if (Player.armor[1].type >= ItemID.Count && ItemGlowMask.TryGetValue(Player.armor[1].type, out textureBody))//Body
			{
				InsertAfterVanillaLayer(layers, "Body", new PlayerDrawLayer(Mod.Name, "GlowMaskBody", delegate (PlayerDrawSet info) {
					GlowmaskUtils.DrawArmorGlowMask(EquipType.Body, textureBody, info);
				}));
			}
			Texture2D textureHead;
			if (!Player.armor[10].IsAir) {
				if (Player.armor[10].type >= ItemID.Count && ItemGlowMask.TryGetValue(Player.armor[10].type, out textureHead))//Vanity Head
				{
					InsertAfterVanillaLayer(layers, "Head", new PlayerDrawLayer(Mod.Name, "GlowMaskHead", delegate (PlayerDrawSet info) {
						GlowmaskUtils.DrawArmorGlowMask(EquipType.Head, textureHead, info);
					}));
				}
			}
			else if (Player.armor[0].type >= ItemID.Count && ItemGlowMask.TryGetValue(Player.armor[0].type, out textureHead))//Head
			{
				InsertAfterVanillaLayer(layers, "Head", new PlayerDrawLayer(Mod.Name, "GlowMaskHead", delegate (PlayerDrawSet info) {
					GlowmaskUtils.DrawArmorGlowMask(EquipType.Head, textureHead, info);
				}));
			}
			Texture2D textureItem;
			if (Player.HeldItem.type >= ItemID.Count && ItemGlowMask.TryGetValue(Player.HeldItem.type, out textureItem))//Held ItemType
			{
				InsertAfterVanillaLayer(layers, "HeldItem", new PlayerDrawLayer(Mod.Name, "GlowMaskHeldItem", delegate (PlayerDrawSet info) {
					GlowmaskUtils.DrawItemGlowMask(textureItem, info);
				}));
			}
		}

		public static void InsertAfterVanillaLayer(List<PlayerDrawLayer> layers, string vanillaLayerName, PlayerLayer newPlayerLayer)
		{
			for (int i = 0; i < layers.Count; i++) {
				if (layers[i].Name == vanillaLayerName && layers[i].mod == "Terraria") {
					layers.Insert(i + 1, newPlayerLayer);
					return;
				}
			}
			layers.Add(newPlayerLayer);
		}
	}
}
