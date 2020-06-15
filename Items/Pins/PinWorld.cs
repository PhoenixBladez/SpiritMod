using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace SpiritMod.Items.Pins
{
	// This just exists to store the pin locations on the world
	// There's no data management here - please don't flood with unnecessary bloat
	public class PinWorld : ModWorld
	{
		private bool needsPinSync = true;
		public TagCompound pins = new TagCompound();

		public void SetPin(string name, Vector2 pos)
		{
			pins[name] = pos;
			needsPinSync = true;
		}

		public void RemovePin(string name)
		{
			pins.Remove(name);
			needsPinSync = true;
		}

		public override TagCompound Save() => pins;

		public override void Load(TagCompound tag) => pins = tag;

		public override void NetSend(BinaryWriter writer)
		{
			if(needsPinSync) {
				writer.Write(pins.Count);
				foreach(var pair in pins) {
					writer.Write(pair.Key);
					writer.WriteVector2(pins.Get<Vector2>(pair.Key));
				}
				needsPinSync = false;
			}
		}

		public override void NetReceive(BinaryReader reader)
		{
			for(int i = 0; i < reader.ReadInt32(); i++)
				pins[reader.ReadString()] = reader.ReadVector2();
		}
	}
}