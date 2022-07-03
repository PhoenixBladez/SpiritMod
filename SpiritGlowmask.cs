using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod
{
	public class SpiritGlowmask : ModPlayer
	{
		internal static readonly Dictionary<int, Texture2D> ItemGlowMask = new Dictionary<int, Texture2D>();

		internal new static void Unload() => ItemGlowMask.Clear();
		public static void AddGlowMask(int itemType, string texturePath) => ItemGlowMask[itemType] = ModContent.Request<Texture2D>(texturePath, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
	}

	public class SpiritGlowMaskItemLayer : PlayerDrawLayer
	{
		public override Position GetDefaultPosition() => new BeforeParent(PlayerDrawLayers.Head);
		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			if (drawInfo.drawPlayer.HeldItem.type >= ItemID.Count && SpiritGlowmask.ItemGlowMask.TryGetValue(drawInfo.drawPlayer.HeldItem.type, out Texture2D textureItem))//Held ItemType
				GlowmaskUtils.DrawItemGlowMask(textureItem, drawInfo);
		}
	}

	public abstract class SpiritGlowMaskVanityLayer : PlayerDrawLayer
	{
		protected abstract int ID { get; }
		protected abstract EquipType Type { get; }

		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			if (!drawInfo.drawPlayer.armor[ID].IsAir)
				if (drawInfo.drawPlayer.armor[ID].type >= ItemID.Count && SpiritGlowmask.ItemGlowMask.TryGetValue(drawInfo.drawPlayer.armor[ID].type, out Texture2D textureLegs))//Vanity Legs
					GlowmaskUtils.DrawArmorGlowMask(Type, textureLegs, drawInfo);
		}
	}

	public class SpiritGlowMaskVanityLegsLayer : SpiritGlowMaskVanityLayer
	{
		protected override int ID => 12;
		protected override EquipType Type => EquipType.Legs;
		public override Position GetDefaultPosition() => new BeforeParent(PlayerDrawLayers.Leggings);

	}

	public class SpiritGlowMaskVanityBodyLayer : SpiritGlowMaskVanityLayer
	{
		protected override int ID => 11;
		protected override EquipType Type => EquipType.Body;
		public override Position GetDefaultPosition() => new BeforeParent(PlayerDrawLayers.Torso);
	}

	public class SpiritGlowMaskVanityHeadLayer : SpiritGlowMaskVanityLayer
	{
		protected override int ID => 10;
		protected override EquipType Type => EquipType.Head;
		public override Position GetDefaultPosition() => new BeforeParent(PlayerDrawLayers.Head);
	}

	public class SpiritGlowMaskLegsLayer : PlayerDrawLayer
	{
		public override Position GetDefaultPosition() => new BeforeParent(PlayerDrawLayers.Leggings);
		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			if (drawInfo.drawPlayer.armor[2].type >= ItemID.Count && SpiritGlowmask.ItemGlowMask.TryGetValue(drawInfo.drawPlayer.armor[2].type, out Texture2D textureLegs))//Legs
				GlowmaskUtils.DrawArmorGlowMask(EquipType.Legs, textureLegs, drawInfo);
		}
	}

	public class SpiritGlowMaskBodyLayer : PlayerDrawLayer
	{
		public override Position GetDefaultPosition() => new BeforeParent(PlayerDrawLayers.Torso);
		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			if (drawInfo.drawPlayer.armor[1].type >= ItemID.Count && SpiritGlowmask.ItemGlowMask.TryGetValue(drawInfo.drawPlayer.armor[1].type, out Texture2D textureBody))//Body
				GlowmaskUtils.DrawArmorGlowMask(EquipType.Body, textureBody, drawInfo);
		}
	}

	public class SpiritGlowMaskHeadLayer : PlayerDrawLayer
	{
		public override Position GetDefaultPosition() => new BeforeParent(PlayerDrawLayers.Head);
		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			if (drawInfo.drawPlayer.armor[0].type >= ItemID.Count && SpiritGlowmask.ItemGlowMask.TryGetValue(drawInfo.drawPlayer.armor[0].type, out Texture2D textureBody))//Body
				GlowmaskUtils.DrawArmorGlowMask(EquipType.Head, textureBody, drawInfo);
		}
	}
}
