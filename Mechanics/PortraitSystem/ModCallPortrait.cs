using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Mechanics.PortraitSystem
{
	/// <summary>Class used to make a portrait through the Mod.Call system. DO NOT USE WITHIN THE MOD OTHERWISE.</summary>
	public class ModCallPortrait
	{
		/// <summary>The Texture associated with this ModCallPortrait.</summary>
		public readonly Texture2D Texture;
		/// <summary>The frame size of this ModCallPortrait. Defaults to (108, 108).</summary>
		public readonly Point BaseSize;
		/// <summary>The NPCID associated with this ModCallPortrait.</summary>
		public readonly int ID;
		///<summary>Assumes the place of the GetFrame function from BasePortrait.</summary>
		public Func<string, NPC, Rectangle> GetFrame;

		/// <summary>Loads the texture of the portrait by default & sets the size to size. Defaults to (108, 108) by default.</summary>
		public ModCallPortrait(int id, Texture2D t, Func<string, NPC, Rectangle> frame, Point? baseSize = null)
		{
			ID = id;
			Texture = t;
			GetFrame = frame;
			BaseSize = baseSize ?? new Point(108, 108);
		}
	}
}
